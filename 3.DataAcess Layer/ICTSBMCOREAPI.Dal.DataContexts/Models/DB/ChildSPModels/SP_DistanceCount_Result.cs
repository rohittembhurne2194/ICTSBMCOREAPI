using System;
using System.Collections.Generic;
using System.Text;

namespace ICTSBMCOREAPI.Dal.DataContexts.Models.DB.ChildSPModels
{
    public partial class SP_DistanceCount_Result
    {
        public Nullable<decimal> Distance_in_KM { get; set; }
        public Nullable<decimal> Distance_in_meters { get; set; }
    }
}
