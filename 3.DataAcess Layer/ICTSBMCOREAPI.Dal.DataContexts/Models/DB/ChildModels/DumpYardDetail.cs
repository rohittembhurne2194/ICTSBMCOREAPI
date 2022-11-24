using System;
using System.Collections.Generic;

#nullable disable

namespace ICTSBMCOREAPI.Dal.DataContexts.Models.DB.ChildModels
{
    public partial class DumpYardDetail
    {
        public int dyId { get; set; }
        public string dyName { get; set; }
        public string dyNameMar { get; set; }
        public string dyLat { get; set; }
        public string dyLong { get; set; }
        public string dyQRCode { get; set; }
        public int? zoneId { get; set; }
        public int? wardId { get; set; }
        public int? areaId { get; set; }
        public string ReferanceId { get; set; }
        public string dyAddress { get; set; }
        public DateTime? lastModifiedDate { get; set; }
        public int? userId { get; set; }
        public string EmployeeType { get; set; }
        public string QRCodeImage { get; set; }
        public bool? QRStatus { get; set; }
        public DateTime? QRStatusDate { get; set; }
        public byte[] BinaryQrCodeImage { get; set; }
        public DateTime? DataEntryDate { get; set; }
    }
}
