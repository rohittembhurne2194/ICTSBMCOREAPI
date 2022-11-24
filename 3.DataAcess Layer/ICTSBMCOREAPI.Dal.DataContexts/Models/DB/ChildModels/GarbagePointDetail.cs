using System;
using System.Collections.Generic;

#nullable disable

namespace ICTSBMCOREAPI.Dal.DataContexts.Models.DB.ChildModels
{
    public partial class GarbagePointDetail
    {
        public int gpId { get; set; }
        public string gpName { get; set; }
        public string gpNameMar { get; set; }
        public string gpLat { get; set; }
        public string gpLong { get; set; }
        public string qrCode { get; set; }
        public int? zoneId { get; set; }
        public int? wardId { get; set; }
        public int? areaId { get; set; }
        public string ReferanceId { get; set; }
        public string gpAddress { get; set; }
        public DateTime? modified { get; set; }
        public int? userId { get; set; }
    }
}
