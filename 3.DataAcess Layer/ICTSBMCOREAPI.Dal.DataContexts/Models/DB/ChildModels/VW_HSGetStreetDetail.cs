using System;
using System.Collections.Generic;

#nullable disable

namespace ICTSBMCOREAPI.Dal.DataContexts.Models.DB.ChildModels
{
    public partial class VW_HSGetStreetDetail
    {
        public int? userId { get; set; }
        public int SSId { get; set; }
        public string SSLat { get; set; }
        public string SSLong { get; set; }
        public string ReferanceId { get; set; }
        public bool? QRStatus { get; set; }
        public DateTime? QRStatusDate { get; set; }
        public DateTime? lastModifiedDate { get; set; }
        public string qrEmpName { get; set; }
        public string SSName { get; set; }
        public string BinaryQrCodeImage { get; set; }
        public int? FilterTotalCount { get; set; }
    }
}
