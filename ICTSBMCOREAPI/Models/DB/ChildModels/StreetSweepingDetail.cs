using System;
using System.Collections.Generic;

#nullable disable

namespace ICTSBMCOREAPI.Models.DB.ChildModels
{
    public partial class StreetSweepingDetail
    {
        public int SSId { get; set; }
        public string SSName { get; set; }
        public string SSNameMar { get; set; }
        public string SSLat { get; set; }
        public string SSLong { get; set; }
        public string SSQRCode { get; set; }
        public int? zoneId { get; set; }
        public int? wardId { get; set; }
        public int? areaId { get; set; }
        public string ReferanceId { get; set; }
        public string SSAddress { get; set; }
        public DateTime? lastModifiedDate { get; set; }
        public int? userId { get; set; }
        public string QRCodeImage { get; set; }
        public bool? QRStatus { get; set; }
        public DateTime? QRStatusDate { get; set; }
        public byte[] BinaryQrCodeImage { get; set; }
        public DateTime? DataEntryDate { get; set; }
    }
}
