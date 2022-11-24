using System;
using System.Collections.Generic;

#nullable disable

namespace ICTSBMCOREAPI.Dal.DataContexts.Models.DB.ChildModels
{
    public partial class Daily_Attendance
    {
        public int daID { get; set; }
        public int? userId { get; set; }
        public string startLat { get; set; }
        public string startLong { get; set; }
        public string endLat { get; set; }
        public string endLong { get; set; }
        public string startTime { get; set; }
        public string endTime { get; set; }
        public DateTime? daDate { get; set; }
        public DateTime? daEndDate { get; set; }
        public string vtId { get; set; }
        public string daStartNote { get; set; }
        public string daEndNote { get; set; }
        public string vehicleNumber { get; set; }
        public string batteryStatus { get; set; }
        public int? totalKm { get; set; }
        public string OutbatteryStatus { get; set; }
        public string EmployeeType { get; set; }
        public int? dyid { get; set; }
        public int? VQRID { get; set; }
        public byte[] BinaryQrCodeImage { get; set; }
    }
}
