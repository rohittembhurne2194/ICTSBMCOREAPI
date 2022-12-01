using System;
using System.Collections.Generic;
using System.Text;

namespace ICTSBMCOREAPI.SwachhBhart.API.Bll.ViewModels.Models
{
    public class SBGarbageCollectionView : SBGarbageCollectionChildView
    {
        public int gcId { get; set; }
        public int userId { get; set; }
        public int typeId { get; set; } /// UserTypeID i.e. 0 = Ghanta Gadi, 1 = Scanify , 2 = Waste Management
        public string houseId { get; set; }
        public string gpId { get; set; }
        public string gcDate { get; set; }
        public string Lat { get; set; }
        public string Long { get; set; }
        public string gpBeforImage { get; set; }
        public string gpAfterImage { get; set; }
        public int gcType { get; set; }
        public string note { get; set; }
        public string vehicleNumber { get; set; }
        public int garbageType { get; set; }
        public string dyId { get; set; }

        public string vqrId { get; set; }
        public string LWId { get; set; }

        public string SSId { get; set; }
        //public string totalGcWeight { get; set; }
        public Nullable<decimal> totalGcWeight { get; set; }
        //public string totalDryWeight { get; set; }
        public Nullable<decimal> totalDryWeight { get; set; }
        //public string totalWetWeight { get; set; }
        public Nullable<decimal> totalWetWeight { get; set; }
        public string batteryStatus { get; set; }
        public decimal Distance { get; set; }
        public bool IsOffline { get; set; }

        public string wastetype { get; set; }

        public string EmpType { get; set; }

        public bool IsIn { get; set; }

    }

    public class SBGarbageCollectionChildView
    {
        public string ReferenceID { get; set; }
        public int OfflineID { get; set; }
        //public bool IsOffline { get; set; }
        public bool IsLocation { get; set; }
    }
}
