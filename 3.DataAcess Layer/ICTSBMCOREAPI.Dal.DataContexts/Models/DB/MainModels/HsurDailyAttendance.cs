using System;
using System.Collections.Generic;

#nullable disable

namespace ICTSBMCOREAPI.Dal.DataContexts.Models.DB.MainModels
{
    public partial class HsurDailyAttendance
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
        public string BatteryStatus { get; set; }
        public string OutbatteryStatus { get; set; }
        public string EmployeeType { get; set; }
        public string IpAddress { get; set; }
        public string LoginDevice { get; set; }
        public string HostName { get; set; }
    }
}
