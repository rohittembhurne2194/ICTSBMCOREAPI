using System;
using System.Collections.Generic;
using System.Text;

namespace ICTSBMCOREAPI.SwachhBhart.API.Bll.ViewModels.Models
{
    public class UREmployeeAttendence
    {
        public int qrEmpDaId { get; set; }
        public Nullable<int> qrEmpId { get; set; }
        public string startLat { get; set; }
        public string startLong { get; set; }
        public string endLat { get; set; }
        public string endLong { get; set; }
        public string startTime { get; set; }
        public string endTime { get; set; }
        public string startDate { get; set; }
        public string endDate { get; set; }
        public string startNote { get; set; }
        public string endNote { get; set; }
        public string batteryStatus { get; set; }
        public string OutbatteryStatus { get; set; }
        public int OfflineId { get; set; }
        public string EmployeeType { get; set; }

        public string IpAddress { get; set; }
        public string LoginDevice { get; set; }
        public string HostName { get; set; }
        public string EmployeeName { get; set; }


    }
}
