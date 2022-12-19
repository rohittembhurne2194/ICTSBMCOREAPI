using System;
using System.Collections.Generic;
using System.Text;

namespace ICTSBMCOREAPI.SwachhBhart.API.Bll.ViewModels.Models
{
   public class TrailsDetails
    {
        public string code { get; set; }
        public string status { get; set; }
        public string message { get; set; }
        public string errorMessages { get; set; }

        public string timestamp { get; set; }
        public dynamic data { get; set; }

        public  List<NewData> newdata { get; set; }

    }

    public class NewData
    {
        public string id { get; set; }
        public string createUser { get; set; }
        public string createTs { get; set; }
        public string updateUser { get; set; }
        public string updateTs { get; set; }

        public dynamic geom { get; set; }

        public List<Geom> newgeom { get; set; }
    }

    public class Geom
    {
        public string type { get; set; }
        public string coordinates { get; set; }
    }
}
