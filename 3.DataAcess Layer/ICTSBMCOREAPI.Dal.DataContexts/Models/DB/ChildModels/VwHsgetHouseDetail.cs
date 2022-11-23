using System;
using System.Collections.Generic;

#nullable disable

namespace ICTSBMCOREAPI.Dal.DataContexts.Models.DB.ChildModels
{
    public partial class VwHsgetHouseDetail
    {
        public int? UserId { get; set; }
        public int HouseId { get; set; }
        public string HouseLat { get; set; }
        public string HouseLong { get; set; }
        public string ReferanceId { get; set; }
        public bool? Qrstatus { get; set; }
        public DateTime? QrstatusDate { get; set; }
        public DateTime? Modified { get; set; }
        public string QrEmpName { get; set; }
        public string HouseOwner { get; set; }
        public string BinaryQrCodeImage { get; set; }
        public int? FilterTotalCount { get; set; }
    }
}
