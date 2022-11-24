using System;
using System.Collections.Generic;

#nullable disable

namespace ICTSBMCOREAPI.Dal.DataContexts.Models.DB.ChildModels
{
    public partial class VW_HSGetLiquidDetail
    {
        public int? userId { get; set; }
        public int LWId { get; set; }
        public string LWLat { get; set; }
        public string LWLong { get; set; }
        public string ReferanceId { get; set; }
        public bool? QRStatus { get; set; }
        public DateTime? QRStatusDate { get; set; }
        public DateTime? lastModifiedDate { get; set; }
        public string qrEmpName { get; set; }
        public string LWName { get; set; }
        public string BinaryQrCodeImage { get; set; }
        public int? FilterTotalCount { get; set; }
    }
}
