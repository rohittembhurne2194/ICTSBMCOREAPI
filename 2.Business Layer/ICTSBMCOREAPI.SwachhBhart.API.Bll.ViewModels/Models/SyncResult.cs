using System;
using System.Collections.Generic;
using System.Text;

namespace ICTSBMCOREAPI.SwachhBhart.API.Bll.ViewModels.Models
{
    public class SyncResult
    {
        public int OfflineId { get; set; }
        public int ID { get; set; }
        public string status { get; set; }
        public string message { get; set; }
        public string messageMar { get; set; }
        public bool isAttendenceOff { get; set; }
    }


    public class SyncResult1
    {
        public int ID { get; set; }
        public string status { get; set; }
        public string message { get; set; }
        public string messageMar { get; set; }
        public bool isAttendenceOff { get; set; }
        public bool IsInSync { get; set; }
        public bool IsOutSync { get; set; }

        public string EmpType { get; set; }
    }
}
