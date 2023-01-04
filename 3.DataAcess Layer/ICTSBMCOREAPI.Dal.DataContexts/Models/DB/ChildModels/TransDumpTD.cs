using System;
using System.Collections.Generic;

#nullable disable

namespace ICTSBMCOREAPI.Dal.DataContexts.Models.DB.ChildModels
{
    public partial class TransDumpTD
    {
        public int TransBcId { get; set; }
        public string transId { get; set; }
        public DateTime? startDateTime { get; set; }
        public DateTime? endDateTime { get; set; }
        public int? userId { get; set; }
        public string dyId { get; set; }
        public string houseList { get; set; }
        public int? tripNo { get; set; }
        public string vehicleNumber { get; set; }
        public decimal? totalGcWeight { get; set; }
        public decimal? totalDryWeight { get; set; }
        public decimal? totalWetWeight { get; set; }
        public string bcTransId { get; set; }
        public bool? TStatus { get; set; }
        public long? bcStartDateTime { get; set; }
        public long? bcEndDateTime { get; set; }
        public long? bcTotalGcWeight { get; set; }
        public long? bcTotalDryWeight { get; set; }
        public long? bcTotalWetWeight { get; set; }
        public TimeSpan? tHr { get; set; }
        public int? tNh { get; set; }
        public long? bcThr { get; set; }
        public decimal? UsTotalGcWeight { get; set; }
        public decimal? UsTotalDryWeight { get; set; }
        public decimal? UsTotalWetWeight { get; set; }
    }
}
