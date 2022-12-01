using System;
using System.Collections.Generic;
using System.Text;

namespace ICTSBMCOREAPI.Dal.DataContexts.Models.DB.ChildSPModels
{
    public partial class GetAttendenceDetailsTotalDump_Result
    {
        public Nullable<System.DateTime> day { get; set; }
        public Nullable<int> userid { get; set; }
        public Nullable<int> DumpYardPlantCollection { get; set; }
    }
}
