using System;
using System.Collections.Generic;
using System.Text;

namespace ICTSBMCOREAPI.SwachhBhart.API.Bll.ViewModels.Models
{
    public class BigVQRHPDVM
    {
        public int gcId { get; set; }
        public int userId { get; set; }
        public string houseId { get; set; }
        public int gpId { get; set; }
        public string dyId { get; set; }
        public string name { get; set; }
        public string namemar { get; set; }
        public string Address { get; set; }
        public string Lat { get; set; }
        public string Long { get; set; }
        public string ReferanceId { get; set; }
        public int gcType { get; set; }
        public string houseNumber { get; set; }
        public int areaId { get; set; }
        public int wardId { get; set; }
        public int zoneId { get; set; }
        public int wardNo { get; set; }
        public string mobileno { get; set; }
        public string OfflineId { get; set; }

        public string wastetype { get; set; }

        public string QRCodeImage { get; set; }
        public DateTime date { get; set; }

        public bool IsIn { get; set; }
    }

}
