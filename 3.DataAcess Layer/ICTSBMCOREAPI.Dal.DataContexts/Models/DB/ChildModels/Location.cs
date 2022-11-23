using System;
using System.Collections.Generic;

#nullable disable

namespace ICTSBMCOREAPI.Dal.DataContexts.Models.DB.ChildModels
{
    public partial class Location
    {
        public int LocId { get; set; }
        public int? UserId { get; set; }
        public DateTime? Datetime { get; set; }
        public string Lat { get; set; }
        public string Long { get; set; }
        public string Address { get; set; }
        public string Area { get; set; }
        public int? Type { get; set; }
        public string BatteryStatus { get; set; }
        public decimal? Distnace { get; set; }
        public DateTime? CreatedDate { get; set; }
        public bool? IsOffline { get; set; }
        public string ReferanceId { get; set; }
        public string RfidtagId { get; set; }
        public string RfidreaderId { get; set; }
        public int? SourceId { get; set; }
        public string EmployeeType { get; set; }
    }
}
