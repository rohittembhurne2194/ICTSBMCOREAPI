using System;
using System.Collections.Generic;
using System.Text;

namespace ICTSBMCOREAPI.SwachhBhart.API.Bll.ViewModels.Models
{
  public  class DumpTripBCVM
    {
        public int tripId { get; set; }
        public String transId { get; set; }
        public string startDateTime { get; set; }
        public string endDateTime { get; set; } /// UserTypeID i.e. 0 = Ghanta Gadi, 1 = Scanify , 2 = Waste Management
        public int userId { get; set; }
        public String dyId { get; set; }
        public String[] houseList { get; set; }
        public int tripNo { get; set; }

        public Nullable<System.TimeSpan> totalHours { get; set; }
        public int totalNumberOfHouses { get; set; }
        public string vehicleNumber { get; set; }
        public decimal totalGcWeight { get; set; }
        public decimal totalDryWeight { get; set; }
        public decimal totalWetWeight { get; set; }
    }
}
