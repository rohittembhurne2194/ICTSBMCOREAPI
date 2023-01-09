using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ICTSBMCOREAPI.SwachhBhart.API.Bll.ViewModels.Models
{
   public class DumpTripVM
    {
        public int tripId { get; set; }

        public int offlineId { get; set; }

        public String transId { get; set; }
        public string startDateTime { get; set; }
        public string endDateTime { get; set; } /// UserTypeID i.e. 0 = Ghanta Gadi, 1 = Scanify , 2 = Waste Management
        public int userId { get; set; }
        public String dyId { get; set; }
        public String[] houseList { get; set; }
        public int tripNo { get; set; }

        public int totalNumberOfHouses { get; set; }
        public string vehicleNumber { get; set; }
        public decimal totalGcWeight { get; set; }
        public decimal totalDryWeight { get; set; }
        public decimal totalWetWeight { get; set; }

        public string bcTransId { get; set; }

        public bool TStatus { get; set; }


        public Nullable<long> bcStartDateTime { get; set; }
        public Nullable<long> bcEndDateTime { get; set; }
        public Nullable<long> bcTotalGcWeight { get; set; }
        public Nullable<long> bcTotalDryWeight { get; set; }
        public Nullable<long> bcTotalWetWeight { get; set; }
        [JsonIgnore]
        public TimeSpan? totalHours { get; set; }

        public Nullable<long> bcThr { get; set; }

        public decimal USTotalGcWeight { get; set; }
        public decimal USTotalDryWeight { get; set; }
        public decimal USTotalWetWeight { get; set; }

        public decimal totalGcWeightkg { get; set; }
        public decimal totalDryWeightkg { get; set; }
        public decimal totalWetWeightkg { get; set; }
    }
}
