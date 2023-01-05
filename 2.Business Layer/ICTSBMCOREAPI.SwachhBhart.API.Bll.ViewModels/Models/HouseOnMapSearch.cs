using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICTSBMCOREAPI.SwachhBhart.API.Bll.ViewModels.Models
{
   public class HouseOnMapSearch
    {
        public string date { get; set; }
        public int? userid { get; set; }
        public int? areaId { get; set; }
        public int? wardNo { get; set; }
        //public string SearchString { get; set; }
        public int? garbageType { get; set; }
        public int? filterType { get; set; }
    }
}
