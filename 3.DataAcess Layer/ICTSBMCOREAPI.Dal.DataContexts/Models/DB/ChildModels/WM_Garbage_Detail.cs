using System;
using System.Collections.Generic;

#nullable disable

namespace ICTSBMCOREAPI.Dal.DataContexts.Models.DB.ChildModels
{
    public partial class WM_Garbage_Detail
    {
        public int GarbageDetailsID { get; set; }
        public int? SubCategoryID { get; set; }
        public decimal? Weight { get; set; }
        public int? UserId { get; set; }
        public int? Source { get; set; }
        public DateTime? gdDate { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
