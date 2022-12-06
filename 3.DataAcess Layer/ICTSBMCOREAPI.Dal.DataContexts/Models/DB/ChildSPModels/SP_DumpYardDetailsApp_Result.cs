using System;
using System.Collections.Generic;
using System.Text;

namespace ICTSBMCOREAPI.Dal.DataContexts.Models.DB.ChildSPModels
{
    public partial class SP_DumpYardDetailsApp_Result
    {
        public Nullable<int> userId { get; set; }
        public int dyId { get; set; }
        public string dyLat { get; set; }
        public string dyLong { get; set; }
        public string ReferanceId { get; set; }
        public Nullable<System.DateTime> lastModifiedDate { get; set; }
        public string qrEmpName { get; set; }
        public Nullable<bool> QRStatus { get; set; }
        public string QRCodeImage { get; set; }
    }

}
