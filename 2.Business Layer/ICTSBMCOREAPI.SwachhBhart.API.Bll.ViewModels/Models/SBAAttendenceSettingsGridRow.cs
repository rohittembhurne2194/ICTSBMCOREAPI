using System;
using System.Collections.Generic;
using System.Text;

namespace ICTSBMCOREAPI.SwachhBhart.API.Bll.ViewModels.Models
{
    public class SBAAttendenceSettingsGridRow
    {
        public string userName { get; set; }
        public string InTime { get; set; }

        public string NotificationTime { get; set; }

        public string NotificationMobileNumber { get; set; }

        public int daID { get; set; }
        public string daDate { get; set; }
        public string daEndDate { get; set; }
        public string startLat { get; set; }
        public string startLong { get; set; }
        public string endLat { get; set; }
        public string endLong { get; set; }
        public string startTime { get; set; }
        public string endTime { get; set; }
        public string vtId { get; set; }
        public string vehicleNumber { get; set; }
        public int userId { get; set; }
        public Nullable<DateTime> CompareDate { get; set; }
        public string Date { get; set; }
        public string daDateTIme { get; set; }

    }
}
