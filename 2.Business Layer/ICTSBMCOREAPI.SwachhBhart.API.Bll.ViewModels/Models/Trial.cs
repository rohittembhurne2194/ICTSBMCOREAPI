using System;
using System.Collections.Generic;
using System.Text;

namespace ICTSBMCOREAPI.SwachhBhart.API.Bll.ViewModels.Models
{
    public class Trial
    {
        public string id { get; set; }
        public int houseid { get; set; }
        public string startDate { get; set; }
        public string endDate { get; set; }
        public int createUser { get; set; }

        public int updateUser { get; set; }
        public string geom { get; set; }

        public string createTs { get; set; }
        public string updateTs { get; set; }
    }
}
