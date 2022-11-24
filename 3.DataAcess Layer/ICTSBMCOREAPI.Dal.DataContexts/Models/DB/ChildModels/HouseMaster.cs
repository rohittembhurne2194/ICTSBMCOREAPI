using System;
using System.Collections.Generic;

#nullable disable

namespace ICTSBMCOREAPI.Dal.DataContexts.Models.DB.ChildModels
{
    public partial class HouseMaster
    {
        public int houseId { get; set; }
        public string houseNumber { get; set; }
        public string houseOwner { get; set; }
        public string houseOwnerMar { get; set; }
        public string houseOwnerMobile { get; set; }
        public string houseAddress { get; set; }
        public string houseLat { get; set; }
        public string houseLong { get; set; }
        public string houseQRCode { get; set; }
        public int? ZoneId { get; set; }
        public int? AreaId { get; set; }
        public int? WardNo { get; set; }
        public string ReferanceId { get; set; }
        public DateTime? modified { get; set; }
        public int? userId { get; set; }
        public string FCMID { get; set; }
        public DateTime? lastModifiedEntry { get; set; }
        public string RFIDTagId { get; set; }
        public string WasteType { get; set; }
        public string QRCodeImage { get; set; }
        public string OccupancyStatus { get; set; }
        public string Property_Type { get; set; }
        public bool? QRStatus { get; set; }
        public DateTime? QRStatusDate { get; set; }
        public byte[] BinaryQrCodeImage { get; set; }
    }
}
