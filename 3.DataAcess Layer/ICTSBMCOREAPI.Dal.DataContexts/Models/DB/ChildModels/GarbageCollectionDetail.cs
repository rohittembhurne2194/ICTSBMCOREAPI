using System;
using System.Collections.Generic;

#nullable disable

namespace ICTSBMCOREAPI.Dal.DataContexts.Models.DB.ChildModels
{
    public partial class GarbageCollectionDetail
    {
        public int gcId { get; set; }
        public int? userId { get; set; }
        public int? houseId { get; set; }
        public int? gpId { get; set; }
        public DateTime? gcDate { get; set; }
        public string Lat { get; set; }
        public string Long { get; set; }
        public string gcImage { get; set; }
        public string gcQRcode { get; set; }
        public string gpBeforImage { get; set; }
        public string gpAfterImage { get; set; }
        public int? gcType { get; set; }
        public string vehicleNumber { get; set; }
        public string note { get; set; }
        public string locAddresss { get; set; }
        public int? garbageType { get; set; }
        public DateTime? modified { get; set; }
        public int? dyId { get; set; }
        public decimal? totalGcWeight { get; set; }
        public decimal? totalDryWeight { get; set; }
        public decimal? totalWetWeight { get; set; }
        public string batteryStatus { get; set; }
        public double? Distance { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string RFIDTagId { get; set; }
        public string RFIDReaderId { get; set; }
        public int? SourceId { get; set; }
        public string OutbatteryStatus { get; set; }
        public string WasteType { get; set; }
        public string EmployeeType { get; set; }
        public int? LWId { get; set; }
        public int? SSId { get; set; }
        public int? AreaId { get; set; }
        public DateTime? gpBeforImageTime { get; set; }
        public int? vqrid { get; set; }
    }
}
