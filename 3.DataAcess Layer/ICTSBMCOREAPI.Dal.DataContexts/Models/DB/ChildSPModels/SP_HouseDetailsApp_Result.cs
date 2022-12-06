using System;
using System.Collections.Generic;
using System.Text;

namespace ICTSBMCOREAPI.Dal.DataContexts.Models.DB.ChildSPModels
{
    public partial class SP_HouseDetailsApp_Result
    {
        public Nullable<int> userId { get; set; }
        public int houseId { get; set; }
        public string houseLat { get; set; }
        public string houseLong { get; set; }
        public string ReferanceId { get; set; }
        public Nullable<System.DateTime> modified { get; set; }
        public string qrEmpName { get; set; }
        public Nullable<bool> QRStatus { get; set; }
        public string QRCodeImage { get; set; }
    }

}
