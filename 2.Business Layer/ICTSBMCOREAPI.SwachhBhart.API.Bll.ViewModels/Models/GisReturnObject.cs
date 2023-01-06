using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICTSBMCOREAPI.SwachhBhart.API.Bll.ViewModels.Models
{
   public class GisReturnObject
    {
        public int code { get; set; }

        public string status { get; set; }

        public string message { get; set; }
        public string timestamp { get; set; }
        public dynamic data { get; set; }
    }
}
