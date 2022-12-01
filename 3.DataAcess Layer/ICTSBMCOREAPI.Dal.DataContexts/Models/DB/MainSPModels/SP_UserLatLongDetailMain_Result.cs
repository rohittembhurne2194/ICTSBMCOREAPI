using System;
using System.Collections.Generic;
using System.Text;

namespace ICTSBMCOREAPI.Dal.DataContexts.Models.DB.MainSPModels
{
    public partial class SP_UserLatLongDetailMain_Result
    {
        public int locId { get; set; }
        public Nullable<int> empId { get; set; }
        public Nullable<System.DateTime> datetime { get; set; }
        public string lat { get; set; }
        public string @long { get; set; }
        public string address { get; set; }
        public string area { get; set; }
        public Nullable<int> type { get; set; }
        public string batteryStatus { get; set; }
        public Nullable<decimal> Distnace { get; set; }
    }
}
