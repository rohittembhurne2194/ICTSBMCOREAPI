using System;
using System.Collections.Generic;

#nullable disable

namespace ICTSBMCOREAPI.Dal.DataContexts.Models.DB.ChildModels
{
    public partial class VW_HSGetHouseDetail
    {
        public int? userId { get; set; }
        public int houseId { get; set; }
        public string houseLat { get; set; }
        public string houseLong { get; set; }
        public string ReferanceId { get; set; }
        public bool? QRStatus { get; set; }
        public DateTime? QRStatusDate { get; set; }
        public DateTime? modified { get; set; }
        public string qrEmpName { get; set; }
        public string houseOwner { get; set; }
        public string BinaryQrCodeImage { get; set; }
        public int? FilterTotalCount { get; set; }
    }
}
