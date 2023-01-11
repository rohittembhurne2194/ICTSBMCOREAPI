using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICTSBMCOREAPI.SwachhBhart.API.Bll.ViewModels.Models
{
    public class HouseGisUpdate
    {
        public string id { get; set; }
        public int updateUser { get; set; }
        public string geom { get; set; }

        public string updateTs { get; set; }
    }
}
