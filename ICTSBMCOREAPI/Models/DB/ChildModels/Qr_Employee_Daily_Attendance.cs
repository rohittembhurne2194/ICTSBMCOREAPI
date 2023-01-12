using System;
using System.Collections.Generic;

#nullable disable

namespace ICTSBMCOREAPI.Models.DB.ChildModels
{
    public partial class Qr_Employee_Daily_Attendance
    {
        public int qrEmpDaId { get; set; }
        public int? qrEmpId { get; set; }
        public string startLat { get; set; }
        public string startLong { get; set; }
        public string endLat { get; set; }
        public string endLong { get; set; }
        public string startTime { get; set; }
        public string endTime { get; set; }
        public DateTime? startDate { get; set; }
        public DateTime? endDate { get; set; }
        public string startNote { get; set; }
        public string endNote { get; set; }
        public string batteryStatus { get; set; }
    }
}
