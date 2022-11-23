using System;
using System.Collections.Generic;

#nullable disable

namespace ICTSBMCOREAPI.Dal.DataContexts.Models.DB.ChildModels
{
    public partial class DumpYardDetail
    {
        public int DyId { get; set; }
        public string DyName { get; set; }
        public string DyNameMar { get; set; }
        public string DyLat { get; set; }
        public string DyLong { get; set; }
        public string DyQrcode { get; set; }
        public int? ZoneId { get; set; }
        public int? WardId { get; set; }
        public int? AreaId { get; set; }
        public string ReferanceId { get; set; }
        public string DyAddress { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public int? UserId { get; set; }
        public string EmployeeType { get; set; }
        public string QrcodeImage { get; set; }
        public bool? Qrstatus { get; set; }
        public DateTime? QrstatusDate { get; set; }
        public byte[] BinaryQrCodeImage { get; set; }
        public DateTime? DataEntryDate { get; set; }
    }
}
