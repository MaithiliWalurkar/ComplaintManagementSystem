using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CMS.Controllers.DAL; // Your custom DAL
using CMS.Models;

namespace CMS.Controllers
{
    public class ClientRaiseController : Controller
    {
        // GET: RaiseComplaint page
        public ActionResult RaiseComplaint()
        {
            return View();
        }

        // POST: SubmitComplaint (AJAX)
        [HttpPost]
        public ActionResult SubmitComplaint()
        {
            try
            {
                string title = Request.Form["txtTitle"];
                string category = Request.Form["ddlCategory"];
                string priority = Request.Form["ddlPriority"];
                string description = Request.Form["txtDescription"];
                int clientId = Convert.ToInt32(Session["ClientID"]);

                string filePath = "";
                HttpPostedFileBase file = Request.Files["fileAttachment"];
                if (file != null && file.ContentLength > 0)
                {
                    string fileName = Path.GetFileName(file.FileName);
                    string serverPath = Path.Combine(Server.MapPath("~/Uploads/Complaints"), fileName);
                    file.SaveAs(serverPath);
                    filePath = "/Uploads/Complaints/" + fileName;
                }

                SqlParameter[] param = new SqlParameter[]
                {
                    new SqlParameter("@ComplaintID", 0),
                    new SqlParameter("@Title", title),
                    new SqlParameter("@Description", description),
                    new SqlParameter("@Category", category),
                    new SqlParameter("@Priority", priority),
                    new SqlParameter("@AttachmentPath", filePath),
                    new SqlParameter("@CreatedByClientID", clientId),
                    new SqlParameter("@AssignedAdminID", DBNull.Value),
                    new SqlParameter("@AssignedToUserID", DBNull.Value),
                    new SqlParameter("@ResolvedByUserID", DBNull.Value),
                    new SqlParameter("@ResolvedDate", DBNull.Value),
                    new SqlParameter("@ResolutionStatus", DBNull.Value),
                    new SqlParameter("@ResolutionRemarks", DBNull.Value),
                    new SqlParameter("@Status", DBNull.Value)
                };

                DataSet ds = GlobalClassController.ExecuteDataTable("CMS.UspInsertUpdateComplaint", param);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables[0].Rows[0][0].ToString() == "-1")
                        return Json("Duplicate");
                }

                return Json("Success");
            }
            catch (Exception)
            {
                return Json("Error");
            }
        }

        // GET: Client Complaints Grid
        [HttpGet]
        public JsonResult GetComplaints()
        {
            try
            {
                int clientId = Convert.ToInt32(Session["ClientID"]);

                SqlParameter[] param = new SqlParameter[]
                {
                    new SqlParameter("@ClientID", clientId),
                    new SqlParameter("@ComplaintID", 0),
                    new SqlParameter("@AssignedToUserID", 0),
                    new SqlParameter("@CommandType", "GetByClientID")
                };

                DataSet ds = GlobalClassController.ExecuteDataTable("CMS.UspGetComplaintData", param);
                List<object> complaints = new List<object>();

                if (ds != null && ds.Tables.Count > 0)
                {
                    // Inside complaints = ...
                    complaints = ds.Tables[0].AsEnumerable().Select(row => new
                    {
                        Title = row["Title"].ToString(),
                        Category = row["Category"].ToString(),
                        CPriority = row["CPriority"].ToString(),
                        CStatus = row["CStatus"].ToString(),
                        CreatedDate = Convert.ToDateTime(row["CreatedDate"]).ToString("yyyy-MM-dd HH:mm"),
                        AttachmentPath = row["AttachmentPath"] == DBNull.Value ? "" : row["AttachmentPath"].ToString()
                    }).ToList<object>();

                }

                return Json(complaints, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { error = "Unable to load complaints" }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
