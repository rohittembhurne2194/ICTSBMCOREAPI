using System;
using System.Collections.Generic;

#nullable disable

namespace ICTSBMCOREAPI.Dal.DataContexts.Models.DB.ChildModels
{
    public partial class GarbageCollectionDetail
    {
        public int GcId { get; set; }
        public int? UserId { get; set; }
        public int? HouseId { get; set; }
        public int? GpId { get; set; }
        public DateTime? GcDate { get; set; }
        public string Lat { get; set; }
        public string Long { get; set; }
        public string GcImage { get; set; }
        public string GcQrcode { get; set; }
        public string GpBeforImage { get; set; }
        public string GpAfterImage { get; set; }
        public int? GcType { get; set; }
        public string VehicleNumber { get; set; }
        public string Note { get; set; }
        public string LocAddresss { get; set; }
        public int? GarbageType { get; set; }
        public DateTime? Modified { get; set; }
        public int? DyId { get; set; }
        public decimal? TotalGcWeight { get; set; }
        public decimal? TotalDryWeight { get; set; }
        public decimal? TotalWetWeight { get; set; }
        public string BatteryStatus { get; set; }
        public double? Distance { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string RfidtagId { get; set; }
        public string RfidreaderId { get; set; }
        public int? SourceId { get; set; }
        public string OutbatteryStatus { get; set; }
        public string WasteType { get; set; }
        public string EmployeeType { get; set; }
        public int? Lwid { get; set; }
        public int? Ssid { get; set; }
        public int? AreaId { get; set; }
        public DateTime? GpBeforImageTime { get; set; }
        public int? Vqrid { get; set; }
    }
}
