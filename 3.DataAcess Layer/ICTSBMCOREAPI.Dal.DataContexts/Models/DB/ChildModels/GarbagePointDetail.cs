using System;
using System.Collections.Generic;

#nullable disable

namespace ICTSBMCOREAPI.Dal.DataContexts.Models.DB.ChildModels
{
    public partial class GarbagePointDetail
    {
        public int GpId { get; set; }
        public string GpName { get; set; }
        public string GpNameMar { get; set; }
        public string GpLat { get; set; }
        public string GpLong { get; set; }
        public string QrCode { get; set; }
        public int? ZoneId { get; set; }
        public int? WardId { get; set; }
        public int? AreaId { get; set; }
        public string ReferanceId { get; set; }
        public string GpAddress { get; set; }
        public DateTime? Modified { get; set; }
        public int? UserId { get; set; }
    }
}
