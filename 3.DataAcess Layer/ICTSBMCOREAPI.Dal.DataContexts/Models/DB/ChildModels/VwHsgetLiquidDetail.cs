using System;
using System.Collections.Generic;

#nullable disable

namespace ICTSBMCOREAPI.Dal.DataContexts.Models.DB.ChildModels
{
    public partial class VwHsgetLiquidDetail
    {
        public int? UserId { get; set; }
        public int Lwid { get; set; }
        public string Lwlat { get; set; }
        public string Lwlong { get; set; }
        public string ReferanceId { get; set; }
        public bool? Qrstatus { get; set; }
        public DateTime? QrstatusDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public string QrEmpName { get; set; }
        public string Lwname { get; set; }
        public string BinaryQrCodeImage { get; set; }
        public int? FilterTotalCount { get; set; }
    }
}
