using System;
using System.Collections.Generic;

#nullable disable

namespace ICTSBMCOREAPI.Dal.DataContexts.Models.DB.ChildModels
{
    public partial class EmpBeatMap
    {
        public int ebmId { get; set; }
        public int? userId { get; set; }
        public string ebmLatLong { get; set; }
        public string Type { get; set; }
    }
}
