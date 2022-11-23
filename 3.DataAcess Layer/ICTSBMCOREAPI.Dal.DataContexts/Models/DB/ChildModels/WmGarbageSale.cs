using System;
using System.Collections.Generic;

#nullable disable

namespace ICTSBMCOREAPI.Dal.DataContexts.Models.DB.ChildModels
{
    public partial class WmGarbageSale
    {
        public int Id { get; set; }
        public int? SubCategoryId { get; set; }
        public string PartyName { get; set; }
        public decimal? SalesWeight { get; set; }
        public decimal? Amount { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
