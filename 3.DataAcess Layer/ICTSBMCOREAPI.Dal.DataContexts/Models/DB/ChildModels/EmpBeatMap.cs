using System;
using System.Collections.Generic;

#nullable disable

namespace ICTSBMCOREAPI.Dal.DataContexts.Models.DB.ChildModels
{
    public partial class EmpBeatMap
    {
        public int EbmId { get; set; }
        public int? UserId { get; set; }
        public string EbmLatLong { get; set; }
        public string Type { get; set; }
    }
}
