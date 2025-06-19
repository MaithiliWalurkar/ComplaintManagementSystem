using CMS.Controllers.DAL;
using CMS.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace CMS.Controllers
{
    public class ClientController : Controller
    {
        // GET: Client
        public ActionResult ClientRegister()
        {
            return View();
        }

        [HttpPost]
        public ActionResult RegisterUser(VMClientRegister client)
        {
            string otp = new Random().Next(100000, 999999).ToString();

            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@CompanyName", client.CompanyName),
                new SqlParameter("@ContactPerson", client.ContactPerson),
                new SqlParameter("@Email", client.Email),
                new SqlParameter("@PasswordHash", client.Password),
                new SqlParameter("@OTP", otp)
            };

            object result = GlobalClassController.ExecuteScalar("CMS.InsertClientWithOTP", param);

            if ((int)result == 1)
            {
                VMEmailDataToSent vMEmailDataToSent = new VMEmailDataToSent();
                vMEmailDataToSent.otp = otp;
                SendVerificationEmail(client.Email, vMEmailDataToSent, 1);
                return Json(1);
            }
            else if ((int)result == 2)
            {
                return Json(2);
            }

            return Json(0); // Error
        }

        [HttpPost]
        public ActionResult VerifyOTP(string email, string otp)
        {
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@Email", email),
                new SqlParameter("@OTP", otp)
            };

            object result = GlobalClassController.ExecuteScalar("CMS.VerifyClientOTP", param);

            return Json(result);
        }

        private void SendVerificationEmail(string email, VMEmailDataToSent vMEmailDataToSent, int bodyType)
        {
            try
            {
                SqlParameter[] param1 = new SqlParameter[]
                {
            new SqlParameter("@BodyType", bodyType)
                };

                DataSet ds = GlobalClassController.ExecuteDataTable("CMS.UspGetEMailSMSDetails", param1);

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];

                    string subject = dr["EmialSmsSubject"].ToString();
                    string bodyTemplate = dr["EmialSmsBody"].ToString();
                    string finalBody = " ";

                    if (bodyType == 1)
                    {
                        finalBody = bodyTemplate.Contains("{otp}")
                        ? bodyTemplate.Replace("{otp}", vMEmailDataToSent.otp)
                        : bodyTemplate;
                    }
                    else if (bodyType == 2)
                    {
                        finalBody = bodyTemplate + vMEmailDataToSent.Password;
                    }

                    string fromMail = ConfigurationManager.AppSettings["FromMailAddress"];
                    string password = ConfigurationManager.AppSettings["AppEmailPassword"];

                    MailMessage mail = new MailMessage
                    {
                        From = new MailAddress(fromMail),
                        Subject = subject,
                        Body = finalBody,
                        IsBodyHtml = true // if template includes HTML, otherwise false
                    };

                    mail.To.Add(email);

                    using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                    {
                        smtp.Credentials = new NetworkCredential(fromMail, password);
                        smtp.EnableSsl = true;
                        smtp.Send(mail);
                    }
                }
                else
                {
                    throw new Exception($"No email template found for BodyType = {bodyType}.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error while sending email: " + ex.Message);
            }
        }

        public ActionResult ClientDashboard()
        {
            return View();
        }

    }
}