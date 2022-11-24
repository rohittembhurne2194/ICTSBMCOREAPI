using System;
using System.Collections.Generic;

#nullable disable

namespace ICTSBMCOREAPI.Dal.DataContexts.Models.DB.MainModels
{
    public partial class HSUR_Daily_Attendance
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
        public string batteryStatus { get; set; }
        public string OutbatteryStatus { get; set; }
        public string EmployeeType { get; set; }
        public string ip_address { get; set; }
        public string login_device { get; set; }
        public string HostName { get; set; }
    }
}
