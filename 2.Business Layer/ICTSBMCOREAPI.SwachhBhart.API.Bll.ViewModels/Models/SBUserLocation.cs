using System;
using System.Collections.Generic;
using System.Text;

namespace ICTSBMCOREAPI.SwachhBhart.API.Bll.ViewModels.Models
{
    public class SBUserLocation
    {
        public int locId { get; set; }
        public int OfflineId { get; set; }
        public int userId { get; set; }
        public DateTime datetime { get; set; }
        public string lat { get; set; }
        public string @long { get; set; }
        public string address { get; set; }
        public string area { get; set; }
        public int typeId { get; set; }
    }
}
