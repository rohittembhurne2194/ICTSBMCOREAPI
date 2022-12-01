using System;
using System.Collections.Generic;
using System.Text;

namespace ICTSBMCOREAPI.SwachhBhart.API.Bll.ViewModels.Models
{
    public class coordinates
    {
        public double? lat { get; set; }
        public double? lng { get; set; }
    }

    public class AppAreaMapVM
    {
        public int AppId { get; set; }
        public string AppName { get; set; }
        public string AppLat { get; set; }
        public string AppLong { get; set; }
        public Nullable<bool> IsAreaActive { get; set; }
        public List<List<coordinates>> AppAreaLatLong { get; set; }



    }
}
