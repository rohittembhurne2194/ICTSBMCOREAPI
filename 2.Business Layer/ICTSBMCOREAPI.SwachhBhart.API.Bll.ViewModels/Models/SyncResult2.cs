using System;
using System.Collections.Generic;
using System.Text;

namespace ICTSBMCOREAPI.SwachhBhart.API.Bll.ViewModels.Models
{
    public class SyncResult2
    {
        public int UserId { get; set; }
        public bool IsInSync { get; set; }
        public int batterystatus { get; set; }
        public string imei { get; set; }


    }
}
