using System;
using System.Collections.Generic;
using System.Text;

namespace ICTSBMCOREAPI.Dal.DataContexts.Models.DB.ChildSPModels
{
    public partial class calculateDistance_Result
    {
        public int ID { get; set; }
        public Nullable<decimal> Lattitude { get; set; }
        public Nullable<decimal> Longitude { get; set; }
        public Nullable<decimal> DISTANCE { get; set; }
    }
}
