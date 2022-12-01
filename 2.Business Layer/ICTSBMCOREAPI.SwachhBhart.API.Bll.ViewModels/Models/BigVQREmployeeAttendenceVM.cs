using System;
using System.Collections.Generic;
using System.Text;

namespace ICTSBMCOREAPI.SwachhBhart.API.Bll.ViewModels.Models
{
    public class BigVQREmployeeAttendenceVM
    {
        public int qrEmpDaId { get; set; }
        public Nullable<int> qrEmpId { get; set; }
        public string startLat { get; set; }
        public string startLong { get; set; }
        public string endLat { get; set; }
        public string endLong { get; set; }
        public string startTime { get; set; }
        public string endTime { get; set; }
        public Nullable<System.DateTime> startDate { get; set; }
        public Nullable<System.DateTime> endDate { get; set; }
        public string startNote { get; set; }
        public string endNote { get; set; }
        public string batteryStatus { get; set; }
        public int OfflineId { get; set; }
        public string EmployeeType { get; set; }
    }
}
