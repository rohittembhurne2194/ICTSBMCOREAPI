using System;
using System.Collections.Generic;
using System.Text;

namespace ICTSBMCOREAPI.Dal.DataContexts.Models.DB.ChildSPModels
{
    public partial class GetAttendenceDetailsTotalStreet_Result
    {
        public Nullable<System.DateTime> day { get; set; }
        public Nullable<int> userid { get; set; }
        public Nullable<int> StreetCollection { get; set; }
        public Nullable<int> DumpYardCollection { get; set; }
    }
}
