using CMS.Controllers.DAL;
using CMS.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CMS.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult LoginUser(VMLogin login)
        {
            SqlParameter[] param = new SqlParameter[]
            {
        new SqlParameter("@UserEmail", login.UserEmail),
        new SqlParameter("@UserPassword", login.UserPassword)
            };

            DataSet ds = GlobalClassController.ExecuteDataTable("CMS.LoginClientUser", param);

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                var dr = ds.Tables[0].Rows[0];

                if (dr["Status"].ToString() == "-1")
                    return Json("Invalid");

                Session["UserEmail"] = dr["Email"];

                if (Convert.ToBoolean(dr["IsAdmin"]))
                {
                    Session["AdminID"] = dr["ClientID"];
                    Session["Role"] = "Admin";
                    return Json("Admin");
                }
                else if (Convert.ToBoolean(dr["IsStaff"]))
                {
                    Session["StaffID"] = dr["ClientID"];
                    Session["Role"] = "Staff";
                    return Json("Staff");
                }
                else
                {
                    Session["ClientID"] = dr["ClientID"];
                    Session["Role"] = "Client";
                    return Json("Client");
                }
            }

            return Json("Error");
        }
    }
}