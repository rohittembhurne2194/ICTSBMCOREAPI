using System;
using System.Collections.Generic;

#nullable disable

namespace ICTSBMCOREAPI.Dal.DataContexts.Models.DB.MainModels
{
    public partial class GoogelHitDetail
    {
        public int Id { get; set; }
        public string API { get; set; }
        public string EmailId { get; set; }
        public DateTime? Date { get; set; }
        public int? hit { get; set; }
    }
}
