using System;
using System.Collections.Generic;
using System.Text;

namespace ICTSBMCOREAPI.Dal.DataContexts.Models.DB.ChildSPModels
{
    public partial class SP_LiquidDetailsApp_Result
    {
        public Nullable<int> userId { get; set; }
        public int LWId { get; set; }
        public string LWLat { get; set; }
        public string LWLong { get; set; }
        public string ReferanceId { get; set; }
        public Nullable<System.DateTime> lastModifiedDate { get; set; }
        public string qrEmpName { get; set; }
        public Nullable<bool> QRStatus { get; set; }
        public string QRCodeImage { get; set; }
    }

}
