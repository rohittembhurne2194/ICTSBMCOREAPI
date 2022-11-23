using System;
using System.Collections.Generic;

#nullable disable

namespace ICTSBMCOREAPI.Dal.DataContexts.Models.DB.ChildModels
{
    public partial class VwHsgetDumpyardDetail
    {
        public int? UserId { get; set; }
        public int DyId { get; set; }
        public string DyLat { get; set; }
        public string DyLong { get; set; }
        public string ReferanceId { get; set; }
        public bool? Qrstatus { get; set; }
        public DateTime? QrstatusDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public string QrEmpName { get; set; }
        public string DyName { get; set; }
        public string BinaryQrCodeImage { get; set; }
        public int? FilterTotalCount { get; set; }
    }
}
