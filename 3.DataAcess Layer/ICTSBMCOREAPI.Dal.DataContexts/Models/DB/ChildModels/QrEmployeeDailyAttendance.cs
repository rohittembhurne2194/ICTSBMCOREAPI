using System;
using System.Collections.Generic;

#nullable disable

namespace ICTSBMCOREAPI.Dal.DataContexts.Models.DB.ChildModels
{
    public partial class QrEmployeeDailyAttendance
    {
        public int QrEmpDaId { get; set; }
        public int? QrEmpId { get; set; }
        public string StartLat { get; set; }
        public string StartLong { get; set; }
        public string EndLat { get; set; }
        public string EndLong { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string StartNote { get; set; }
        public string EndNote { get; set; }
        public string BatteryStatus { get; set; }
    }
}
