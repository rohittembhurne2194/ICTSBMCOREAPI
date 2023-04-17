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
        public int createUser { get; set; }

        [JsonProperty("createTs")]
        public string createTs { get; set; }
        [JsonProperty("updateUser")]
        public int? updateUser { get; set; }
        [JsonProperty("updateTs")]
        public string? updateTs { get; set; }
        [JsonProperty("startTs")]
        public string startTs { get; set; }
        [JsonProperty("endTs")]
        public string endTs { get; set; }
        public string EmpName { get; set; }
        [JsonProperty("geom")]
        public geomt? geom { get; set; }
       

        [JsonProperty("Housegeom")]
        public  dynamic HouseList { get; set; }
        public  dynamic DumpList { get; set; }
      
      
   
    }

    public class geomt
    {
        [JsonProperty("type")]
        public string type { get; set; }
        [JsonProperty("coordinates")]
        public double[][] coordinates { get; set; }
    }

    public class GisHouseList
    {
        public int? houseid { get; set; }

        //public string? houselat { get; set; }
        //public string? houselong { get; set; }
        public string? ReferanceId { get; set; }
        public string? houseOwner { get; set; }
        public string? houseOwnerMobile { get; set; }
        public string? houseAddress { get; set; }
        public string? employeename { get; set; }
    }

    public class GisDumpList
    {
        public int? dyid { get; set; }

        //public string? houselat { get; set; }
        //public string? houselong { get; set; }
        public string? ReferanceId { get; set; }
        public string? dyname { get; set; }
        public string? dyAddress { get; set; }
        public string? employeename { get; set; }
    }
}
