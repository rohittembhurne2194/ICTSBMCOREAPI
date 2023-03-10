using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICTSBMCOREAPI.SwachhBhart.API.Bll.ViewModels.Models
{
  public  class HouseOnMapVM
    {
        //public int userId { get; set; }
        public int houseId { get; set; }
        //public int lwid { get; set; }
        //public int ssid { get; set; }
        public int dyid { get; set; }
        //public string ReferanceId { get; set; }
        //public string houseNumber { get; set; }
        //public string houseOwner { get; set; }
        //public string houseOwnerMobile { get; set; }
        //public string houseAddress { get; set; }
        public Nullable<int> garbageType { get; set; }
        public string gcDate { get; set; }

        public string gcTime { get; set; }
        //public double houseLat { get; set; }
        //public double houseLong { get; set; }
        public dynamic geom { get; set; }
        public dynamic HouseProperty { get; set; }
    }

 
}
