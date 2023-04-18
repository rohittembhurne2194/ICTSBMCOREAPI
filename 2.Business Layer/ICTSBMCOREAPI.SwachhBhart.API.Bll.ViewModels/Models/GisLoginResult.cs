using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICTSBMCOREAPI.SwachhBhart.API.Bll.ViewModels.Models
{
   public class GisLoginResult
    {
       

        public int code { get; set; } 
        public string status { get; set; }
        public string message { get; set; }
        public string timestamp { get; set; } 
        public logindata data { get; set; }

    }

    public class logindata
    {
        public int appid { get; set; }
        public dynamic mapcenter { get; set; }
        public int mapzoom { get; set; }
        public dynamic wmslayers { get; set; }
        public string token { get; set; }
        
        
    }

    public class WMS_LAYERS
    {
        public string? label { get; set; }
        public string? id { get; set; }
    }

}
