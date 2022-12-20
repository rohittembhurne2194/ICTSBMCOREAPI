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
    }
}
