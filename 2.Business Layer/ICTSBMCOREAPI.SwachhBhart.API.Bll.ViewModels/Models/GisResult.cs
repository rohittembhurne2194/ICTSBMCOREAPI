using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ICTSBMCOREAPI.SwachhBhart.API.Bll.ViewModels.Models
{
    public class GisResult
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
        public geom? geom { get; set; }

        public string ReferanceId { get; set; }

        public string userid { get; set; }
        public string EmployeeName { get; set; }
        public string HouseOwnerName { get; set; }
        public string HouseOwnerMobileNo { get; set; }
        public string HouseAddress { get; set; }
       
    }

    public class geom
    {
        [JsonProperty("type")]
        public string type { get; set; }
        [JsonProperty("coordinates")]
        public string[] coordinates { get; set; }
    }

}
