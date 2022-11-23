using System;
using System.Collections.Generic;

#nullable disable

namespace ICTSBMCOREAPI.Dal.DataContexts.Models.DB.ChildModels
{
    public partial class VehicalQrMaster
    {
        public int VqrId { get; set; }
        public string VehicalNumber { get; set; }
        public string VehicalQrcode { get; set; }
        public string ReferanceId { get; set; }
        public DateTime? Modified { get; set; }
        public string Fcmid { get; set; }
        public DateTime? LastModifiedEntry { get; set; }
        public string RfidtagId { get; set; }
        public string VehicalType { get; set; }
        public string PropertyType { get; set; }
    }
}
