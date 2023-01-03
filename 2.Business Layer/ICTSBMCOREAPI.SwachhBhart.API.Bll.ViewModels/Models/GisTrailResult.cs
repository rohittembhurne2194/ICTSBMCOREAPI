using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ICTSBMCOREAPI.SwachhBhart.API.Bll.ViewModels.Models
{
  public  class GisTrailResult
    {
        [JsonProperty("id")]
        public string id { get; set; }

        [JsonProperty("createUser")]
        public string createUser { get; set; }

        [JsonProperty("createTs")]
        public string createTs { get; set; }
        [JsonProperty("updateUser")]
        public string updateUser { get; set; }
        [JsonProperty("updateTs")]
        public string updateTs { get; set; }
        [JsonProperty("geom")]
        public geomt? geom { get; set; }
        [JsonProperty("Housegeom")]

         public  dynamic HouseList { get; set; }
        //public int? houseid { get; set; }
        //public IList<string> Categories { get; set; }
    }

    public class geomt
    {
        [JsonProperty("type")]
        public string type { get; set; }
        [JsonProperty("coordinates")]
        public string[][] coordinates { get; set; }
    }

    public class GisHouseList
    {
        public int? houseid { get; set; }

        public string? houselat { get; set; }
        public string? houselong { get; set; }
        public string? ReferanceId { get; set; }
        public string? houseOwner { get; set; }
        public string? houseOwnerMobile { get; set; }
        public string? houseAddress { get; set; }
        public string? employeename { get; set; }
    }
}
