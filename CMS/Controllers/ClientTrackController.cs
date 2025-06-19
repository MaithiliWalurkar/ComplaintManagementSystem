using CMS.Controllers.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CMS.Controllers
{
    public class ClientTrackController : Controller
    {
        // GET: ClientTrack
        public ActionResult ComplaintStatus()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetComplaintTrackBasic()
        {
            int clientId = Convert.ToInt32(Session["ClientID"]);

            SqlParameter[] param = new SqlParameter[]
            {
        new SqlParameter("@ClientID", clientId)
            };

            DataSet ds = GlobalClassController.ExecuteDataTable("CMS.UspGetComplaintTrackBasic", param);

            var result = ds.Tables[0].AsEnumerable().Select(row => new
            {
                Title = row["Title"].ToString(),
                Category = row["Category"].ToString(),
                CPriority = row["CPriority"].ToString(),
                CStatus = row["CStatus"].ToString(),
                ResolutionStatus = row["ResolutionStatus"].ToString(),
                ResolutionRemarks = row["ResolutionRemarks"].ToString(),
                AttachmentPath = row["AttachmentPath"].ToString(),
                CreatedDate = Convert.ToDateTime(row["CreatedDate"]).ToString("yyyy-MM-dd"),
                AssignedAdmin = row["AssignedAdmin"].ToString(),
                AssignedTechnician = row["AssignedTechnician"].ToString(),

                // Safely handle null dates
                CompleteDate = row["CompleteDate"] != DBNull.Value
                    ? Convert.ToDateTime(row["CompleteDate"]).ToString("yyyy-MM-dd")
                    : "",

                AssignDate = row["AssignDate"] != DBNull.Value
                    ? Convert.ToDateTime(row["AssignDate"]).ToString("yyyy-MM-dd")
                    : ""
            }).ToList();

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}