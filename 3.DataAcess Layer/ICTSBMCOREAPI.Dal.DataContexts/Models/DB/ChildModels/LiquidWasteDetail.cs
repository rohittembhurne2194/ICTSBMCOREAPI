using System;
using System.Collections.Generic;

#nullable disable

namespace ICTSBMCOREAPI.Dal.DataContexts.Models.DB.ChildModels
{
    public partial class LiquidWasteDetail
    {
        public int LWId { get; set; }
        public string LWName { get; set; }
        public string LWNameMar { get; set; }
        public string LWLat { get; set; }
        public string LWLong { get; set; }
        public string LWQRCode { get; set; }
        public int? zoneId { get; set; }
        public int? wardId { get; set; }
        public int? areaId { get; set; }
        public string ReferanceId { get; set; }
        public string LWAddreLW { get; set; }
        public DateTime? lastModifiedDate { get; set; }
        public int? userId { get; set; }
        public string QRCodeImage { get; set; }
        public bool? QRStatus { get; set; }
        public DateTime? QRStatusDate { get; set; }
        public byte[] BinaryQrCodeImage { get; set; }
        public DateTime? DataEntryDate { get; set; }
    }
}
