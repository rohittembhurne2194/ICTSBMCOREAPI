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
        public string Status { get; set; }
        public string Message { get; set; }
        public string timestamp { get; set; } 
        public logindata data { get; set; }

    }

    public class logindata
    {
        public int Appid { get; set; }
        public string token { get; set; }
        public string WMS_LAYERS { get; set; }
        public string MAP_CENTER { get; set; }
    }
}
