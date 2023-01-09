using System;
using System.Collections.Generic;

#nullable disable

namespace ICTSBMCOREAPI.Models.DB.ChildModels
{
    public partial class Vehical_QR_Master
    {
        public int vqrId { get; set; }
        public string VehicalNumber { get; set; }
        public string VehicalQRCode { get; set; }
        public string ReferanceId { get; set; }
        public DateTime? modified { get; set; }
        public string FCMID { get; set; }
        public DateTime? lastModifiedEntry { get; set; }
        public string RFIDTagId { get; set; }
        public string VehicalType { get; set; }
        public string Property_Type { get; set; }
    }
}
