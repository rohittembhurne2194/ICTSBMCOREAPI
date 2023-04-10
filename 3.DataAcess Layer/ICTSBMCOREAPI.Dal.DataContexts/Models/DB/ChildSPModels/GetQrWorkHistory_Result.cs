using System;
using System.Collections.Generic;
using System.Text;

namespace ICTSBMCOREAPI.Dal.DataContexts.Models.DB.ChildSPModels
{
    public partial class GetQrWorkHistory_Result
    {
        public string Day { get; set; }
        public Nullable<int> UserId { get; set; }
        public Nullable<int> HouseCollection { get; set; }
        public Nullable<int> PointCollection { get; set; }
        public Nullable<int> DumpYardCollection { get; set; }
        public Nullable<int> LiquidCollection { get; set; }
        public Nullable<int> StreetCollection { get; set; }
        public Nullable<int> SurveyCollection { get; set; }

    }
}
