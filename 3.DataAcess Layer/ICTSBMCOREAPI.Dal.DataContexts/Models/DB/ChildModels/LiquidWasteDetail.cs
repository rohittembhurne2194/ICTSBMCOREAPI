using System;
using System.Collections.Generic;

#nullable disable

namespace ICTSBMCOREAPI.Dal.DataContexts.Models.DB.ChildModels
{
    public partial class LiquidWasteDetail
    {
        public int Lwid { get; set; }
        public string Lwname { get; set; }
        public string LwnameMar { get; set; }
        public string Lwlat { get; set; }
        public string Lwlong { get; set; }
        public string Lwqrcode { get; set; }
        public int? ZoneId { get; set; }
        public int? WardId { get; set; }
        public int? AreaId { get; set; }
        public string ReferanceId { get; set; }
        public string LwaddreLw { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public int? UserId { get; set; }
        public string QrcodeImage { get; set; }
        public bool? Qrstatus { get; set; }
        public DateTime? QrstatusDate { get; set; }
        public byte[] BinaryQrCodeImage { get; set; }
        public DateTime? DataEntryDate { get; set; }
    }
}
