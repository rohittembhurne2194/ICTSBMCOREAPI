﻿using Newtonsoft.Json;
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
        public dynamic Housegeom { get; set; }
      
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

        public string? Lat { get; set; }
        public string? Long { get; set; }
    }
}
