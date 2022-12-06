using System;
using System.Collections.Generic;
using System.Text;

namespace ICTSBMCOREAPI.Dal.DataContexts.Models.DB.ChildSPModels
{
    public partial class SP_HouseScanify_Result
    {
        public int qrEMpId { get; set; }
        public string qrEmpName { get; set; }
        public string qrEmpNameMar { get; set; }
        public string qrEmpLoginId { get; set; }
        public string qrEmpPassword { get; set; }
        public string qrEmpMobileNumber { get; set; }
        public string qrEmpAddress { get; set; }
        public Nullable<bool> isActive { get; set; }
        public string bloodGroup { get; set; }
        public Nullable<System.DateTime> lastModifyDate { get; set; }
        public Nullable<int> HouseCount { get; set; }
        public Nullable<int> LiquidCount { get; set; }
        public Nullable<int> StreetCount { get; set; }
        public Nullable<int> PointCount { get; set; }
        public Nullable<int> DumpCount { get; set; }
    }

}
