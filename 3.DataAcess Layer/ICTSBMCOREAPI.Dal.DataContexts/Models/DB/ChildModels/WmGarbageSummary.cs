using System;
using System.Collections.Generic;

#nullable disable

namespace ICTSBMCOREAPI.Dal.DataContexts.Models.DB.ChildModels
{
    public partial class WmGarbageSummary
    {
        public int Id { get; set; }
        public int? SubCategoryId { get; set; }
        public decimal? TotalWeight { get; set; }
        public decimal? Price { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
