using System;
using System.Collections.Generic;

#nullable disable

namespace ICTSBMCOREAPI.Dal.DataContexts.Models.DB.ChildModels
{
    public partial class HouseMaster
    {
        public int HouseId { get; set; }
        public string HouseNumber { get; set; }
        public string HouseOwner { get; set; }
        public string HouseOwnerMar { get; set; }
        public string HouseOwnerMobile { get; set; }
        public string HouseAddress { get; set; }
        public string HouseLat { get; set; }
        public string HouseLong { get; set; }
        public string HouseQrcode { get; set; }
        public int? ZoneId { get; set; }
        public int? AreaId { get; set; }
        public int? WardNo { get; set; }
        public string ReferanceId { get; set; }
        public DateTime? Modified { get; set; }
        public int? UserId { get; set; }
        public string Fcmid { get; set; }
        public DateTime? LastModifiedEntry { get; set; }
        public string RfidtagId { get; set; }
        public string WasteType { get; set; }
        public string QrcodeImage { get; set; }
        public string OccupancyStatus { get; set; }
        public string PropertyType { get; set; }
        public bool? Qrstatus { get; set; }
        public DateTime? QrstatusDate { get; set; }
        public byte[] BinaryQrCodeImage { get; set; }
    }
}
