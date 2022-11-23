using System;
using System.Collections.Generic;

#nullable disable

namespace ICTSBMCOREAPI.Dal.DataContexts.Models.DB.ChildModels
{
    public partial class WmGarbageDetail
    {
        public int GarbageDetailsId { get; set; }
        public int? SubCategoryId { get; set; }
        public decimal? Weight { get; set; }
        public int? UserId { get; set; }
        public int? Source { get; set; }
        public DateTime? GdDate { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
