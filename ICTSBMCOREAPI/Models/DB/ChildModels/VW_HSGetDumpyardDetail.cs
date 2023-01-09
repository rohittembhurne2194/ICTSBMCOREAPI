using System;
using System.Collections.Generic;

#nullable disable

namespace ICTSBMCOREAPI.Models.DB.ChildModels
{
    public partial class VW_HSGetDumpyardDetail
    {
        public int? userId { get; set; }
        public int dyId { get; set; }
        public string dyLat { get; set; }
        public string dyLong { get; set; }
        public string ReferanceId { get; set; }
        public bool? QRStatus { get; set; }
        public DateTime? QRStatusDate { get; set; }
        public DateTime? lastModifiedDate { get; set; }
        public string qrEmpName { get; set; }
        public string dyName { get; set; }
        public string BinaryQrCodeImage { get; set; }
        public int? FilterTotalCount { get; set; }
    }
}
