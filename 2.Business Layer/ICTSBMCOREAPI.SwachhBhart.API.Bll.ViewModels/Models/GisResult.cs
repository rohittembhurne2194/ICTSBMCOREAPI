﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ICTSBMCOREAPI.SwachhBhart.API.Bll.ViewModels.Models
{
    public class GisResult
    {
        [JsonProperty("id")]
        public int id { get; set; }

        [JsonProperty("createUser")]
        public int createUser { get; set; }

        [JsonProperty("createTs")]
        public string createTs { get; set; }
        [JsonProperty("updateUser")]
        public int updateUser { get; set; }
        [JsonProperty("updateTs")]
        public string updateTs { get; set; }
        [JsonProperty("geom")]
        public geom? geom { get; set; }

        //public string ReferanceId { get; set; }

        //public string CreateEmployeeName { get; set; }
        //public string UpdateEmployeeName { get; set; }
        //public string HouseOwnerName { get; set; }
        //public string HouseOwnerMobileNo { get; set; }
        //public string HouseAddress { get; set; }
        [JsonProperty("HouseProperty")]
        public dynamic HouseProperty { get; set; }

        public dynamic geomhouse { get; set; }
    }

    public class geom
    {
        [JsonProperty("type")]
        public string type { get; set; }
        [JsonProperty("coordinates")]
        public string[] coordinates { get; set; }
    }

    public class HouseProperty
    {
        public string? name { get; set; }
        public string? value { get; set; }
        public string? type { get; set; }
        public int? Index { get; set; }
    }

    public class GeomProperty
    {
        public string? type { get; set; }
        public double[] coordinates { get; set; }
    }
}
