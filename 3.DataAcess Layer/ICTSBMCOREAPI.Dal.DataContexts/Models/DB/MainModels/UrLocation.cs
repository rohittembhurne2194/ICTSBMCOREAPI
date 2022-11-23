using System;
using System.Collections.Generic;

#nullable disable

namespace ICTSBMCOREAPI.Dal.DataContexts.Models.DB.MainModels
{
    public partial class UrLocation
    {
        public int LocId { get; set; }
        public int? EmpId { get; set; }
        public DateTime? Datetime { get; set; }
        public string Lat { get; set; }
        public string Long { get; set; }
        public string Address { get; set; }
        public string Area { get; set; }
        public int? Type { get; set; }
        public string BatteryStatus { get; set; }
        public decimal? Distnace { get; set; }
        public bool? IsOffline { get; set; }
        public string ReferanceId { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
