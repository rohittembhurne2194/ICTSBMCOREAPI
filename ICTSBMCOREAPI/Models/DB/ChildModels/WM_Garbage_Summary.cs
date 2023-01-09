using System;
using System.Collections.Generic;

#nullable disable

namespace ICTSBMCOREAPI.Models.DB.ChildModels
{
    public partial class WM_Garbage_Summary
    {
        public int ID { get; set; }
        public int? SubCategoryID { get; set; }
        public decimal? TotalWeight { get; set; }
        public decimal? Price { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
