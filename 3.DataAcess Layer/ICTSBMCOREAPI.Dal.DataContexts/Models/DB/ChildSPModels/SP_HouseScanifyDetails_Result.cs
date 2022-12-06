using System;
using System.Collections.Generic;
using System.Text;

namespace ICTSBMCOREAPI.Dal.DataContexts.Models.DB.ChildSPModels
{
    public partial class SP_HouseScanifyDetails_Result
    {
        public Nullable<int> TotalHouse { get; set; }
        public Nullable<int> TotalHouseUpdated { get; set; }
        public Nullable<int> TotalHouseUpdated_CurrentDay { get; set; }
        public Nullable<int> TotalPoint { get; set; }
        public Nullable<int> TotalPointUpdated { get; set; }
        public Nullable<int> TotalPointUpdated_CurrentDay { get; set; }
        public Nullable<int> TotalDump { get; set; }
        public Nullable<int> TotalDumpUpdated { get; set; }
        public Nullable<int> TotalDumpUpdated_CurrentDay { get; set; }
        public Nullable<int> TotalLiquid { get; set; }
        public Nullable<int> TotalLiquidUpdated { get; set; }
        public Nullable<int> TotalLiquidUpdated_CurrentDay { get; set; }
        public Nullable<int> TotalStreet { get; set; }
        public Nullable<int> TotalStreetUpdated { get; set; }
        public Nullable<int> TotalStreetUpdated_CurrentDay { get; set; }
    }

}
