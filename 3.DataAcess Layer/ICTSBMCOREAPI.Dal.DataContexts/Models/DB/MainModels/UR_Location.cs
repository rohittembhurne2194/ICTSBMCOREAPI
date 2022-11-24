using System;
using System.Collections.Generic;

#nullable disable

namespace ICTSBMCOREAPI.Dal.DataContexts.Models.DB.MainModels
{
    public partial class UR_Location
    {
        public int locId { get; set; }
        public int? empId { get; set; }
        public DateTime? datetime { get; set; }
        public string lat { get; set; }
        public string _long { get; set; }
        public string address { get; set; }
        public string area { get; set; }
        public int? type { get; set; }
        public string batteryStatus { get; set; }
        public decimal? Distnace { get; set; }
        public bool? IsOffline { get; set; }
        public string ReferanceID { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
