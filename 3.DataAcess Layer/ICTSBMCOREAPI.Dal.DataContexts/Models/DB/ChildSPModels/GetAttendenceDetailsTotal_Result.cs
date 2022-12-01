using System;
using System.Collections.Generic;
using System.Text;

namespace ICTSBMCOREAPI.Dal.DataContexts.Models.DB.ChildSPModels
{
    public partial class GetAttendenceDetailsTotal_Result
    {
        public Nullable<System.DateTime> day { get; set; }
        public Nullable<int> userid { get; set; }
        public Nullable<int> HouseCollection { get; set; }
        public Nullable<int> PointCollection { get; set; }
        public Nullable<int> DumpYardCollection { get; set; }
    }
}
