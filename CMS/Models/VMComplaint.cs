using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CMS.Models
{
    public class VMComplaint
    {
        public int ComplaintID { get; set; } = 0;

        public string Title { get; set; }

        public string Category { get; set; }

        public string Priority { get; set; }

        public string Description { get; set; }

        public HttpPostedFileBase AttachmentFile { get; set; } // For file upload

        public string AttachmentPath { get; set; } // Optional, after save

        public int CreatedByClientID { get; set; } // Filled from session

        public string Status { get; set; } = "Pending"; // Default

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        // Optional fields (used in update)
        public int? AssignedAdminID { get; set; }

        public int? AssignedToUserID { get; set; }

        public int? ResolvedByUserID { get; set; }

        public DateTime? ResolvedDate { get; set; }

        public string ResolutionStatus { get; set; }

        public string ResolutionRemarks { get; set; }

        public DateTime? ModifiedDate { get; set; }
    }
}