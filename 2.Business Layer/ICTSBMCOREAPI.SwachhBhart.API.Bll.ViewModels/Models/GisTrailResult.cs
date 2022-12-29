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

        public housegeom? Housegeom { get; set; }
        public int? houseid { get; set; }
    }

    public class geomt
    {
        [JsonProperty("type")]
        public string type { get; set; }
        [JsonProperty("coordinates")]
        public string[][] coordinates { get; set; }
    }

    public class housegeom
    {
        [JsonProperty("houseid")]
        public int? houseid { get; set; }

        [JsonProperty("housecoordinates")]
        public string[] housecoordinates { get; set; }
    }
}
