using System;
using System.Collections.Generic;
using System.Text;

namespace ICTSBMCOREAPI.SwachhBhart.API.Bll.ViewModels.Models
{
    public class CMSBWardZoneVM
    {
        public int WardID { get; set; }
        public string WardNo { get; set; }
        public Nullable<int> zoneId { get; set; }
        public string Zone { get; set; }
    }

}
