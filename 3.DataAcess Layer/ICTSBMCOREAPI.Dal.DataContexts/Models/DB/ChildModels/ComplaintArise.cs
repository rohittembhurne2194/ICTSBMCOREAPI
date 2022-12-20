using System;
using System.Collections.Generic;

#nullable disable

namespace ICTSBMCOREAPI.Dal.DataContexts.Models.DB.ChildModels
{
    public partial class ComplaintArise
    {
        public int CAId { get; set; }
        public int? userid { get; set; }
        public int? Cid { get; set; }
        public DateTime? PauseDate { get; set; }
        public DateTime? ResumeDate { get; set; }
        public string EmployeeType { get; set; }
    }
}
