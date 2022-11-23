using System;
using System.Collections.Generic;

#nullable disable

namespace ICTSBMCOREAPI.Dal.DataContexts.Models.DB.ChildModels
{
    public partial class DailyAttendance
    {
        public int DaId { get; set; }
        public int? UserId { get; set; }
        public string StartLat { get; set; }
        public string StartLong { get; set; }
        public string EndLat { get; set; }
        public string EndLong { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public DateTime? DaDate { get; set; }
        public DateTime? DaEndDate { get; set; }
        public string VtId { get; set; }
        public string DaStartNote { get; set; }
        public string DaEndNote { get; set; }
        public string VehicleNumber { get; set; }
        public string BatteryStatus { get; set; }
        public int? TotalKm { get; set; }
        public string OutbatteryStatus { get; set; }
        public string EmployeeType { get; set; }
        public int? Dyid { get; set; }
        public int? Vqrid { get; set; }
        public byte[] BinaryQrCodeImage { get; set; }
    }
}
