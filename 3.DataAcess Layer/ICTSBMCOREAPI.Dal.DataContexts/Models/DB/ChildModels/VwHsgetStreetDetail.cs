using System;
using System.Collections.Generic;

#nullable disable

namespace ICTSBMCOREAPI.Dal.DataContexts.Models.DB.ChildModels
{
    public partial class VwHsgetStreetDetail
    {
        public int? UserId { get; set; }
        public int Ssid { get; set; }
        public string Sslat { get; set; }
        public string Sslong { get; set; }
        public string ReferanceId { get; set; }
        public bool? Qrstatus { get; set; }
        public DateTime? QrstatusDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public string QrEmpName { get; set; }
        public string Ssname { get; set; }
        public string BinaryQrCodeImage { get; set; }
        public int? FilterTotalCount { get; set; }
    }
}
