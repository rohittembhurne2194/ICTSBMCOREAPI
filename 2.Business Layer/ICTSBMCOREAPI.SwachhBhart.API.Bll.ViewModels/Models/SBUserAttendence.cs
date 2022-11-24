using System;
using System.Collections.Generic;
using System.Text;

namespace ICTSBMCOREAPI.SwachhBhart.API.Bll.ViewModels.Models
{
    public class SBUserAttendence
    {
        public int daID { get; set; }
        public int userId { get; set; }
        public string startLat { get; set; }
        public string startLong { get; set; }
        public string endLat { get; set; }
        public string endLong { get; set; }
        public string startTime { get; set; }
        public string endTime { get; set; }
        public DateTime daDate { get; set; }
        public DateTime daEndDate { get; set; }
        public string vtId { get; set; }
        public string vehicleNumber { get; set; }
        public string daStartNote { get; set; }
        public string daEndNote { get; set; }
        public string batteryStatus { get; set; }
        public Nullable<int> totalKm { get; set; }
        public int OfflineID { get; set; }

        public string EmpType { get; set; }

        //public int type { get; set; }

        public string ReferanceId { get; set; }


        public string BinaryQrCodeImage { get; set; }

        public string QrCodeImage { get; set; }




    }

}
