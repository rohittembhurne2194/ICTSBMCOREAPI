using System;
using System.Collections.Generic;
using System.Text;

namespace ICTSBMCOREAPI.SwachhBhart.API.Bll.ViewModels.Models
{
  public  class GarbageTrailHouseList
    {
        public int houseid { get; set; }
        public int userid { get; set; }
        public string gcDate { get; set; }

        public string houselat { get; set; }
        public string houselong { get; set; }
    }
}
