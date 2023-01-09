using System;
using System.Collections.Generic;

#nullable disable

namespace ICTSBMCOREAPI.Models.DB.ChildModels
{
    public partial class WM_Garbage_Sale
    {
        public int ID { get; set; }
        public int? SubCategoryID { get; set; }
        public string PartyName { get; set; }
        public decimal? SalesWeight { get; set; }
        public decimal? Amount { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
