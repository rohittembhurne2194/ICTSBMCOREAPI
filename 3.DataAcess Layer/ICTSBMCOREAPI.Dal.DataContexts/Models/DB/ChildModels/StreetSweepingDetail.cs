using System;
using System.Collections.Generic;

#nullable disable

namespace ICTSBMCOREAPI.Dal.DataContexts.Models.DB.ChildModels
{
    public partial class StreetSweepingDetail
    {
        public int Ssid { get; set; }
        public string Ssname { get; set; }
        public string SsnameMar { get; set; }
        public string Sslat { get; set; }
        public string Sslong { get; set; }
        public string Ssqrcode { get; set; }
        public int? ZoneId { get; set; }
        public int? WardId { get; set; }
        public int? AreaId { get; set; }
        public string ReferanceId { get; set; }
        public string Ssaddress { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public int? UserId { get; set; }
        public string QrcodeImage { get; set; }
        public bool? Qrstatus { get; set; }
        public DateTime? QrstatusDate { get; set; }
        public byte[] BinaryQrCodeImage { get; set; }
        public DateTime? DataEntryDate { get; set; }
    }
}
