using System;
using System.Collections.Generic;

#nullable disable

namespace ICTSBMCOREAPI.Models.DB.ChildModels
{
    public partial class QrEmployeeMaster
    {
        public int qrEmpId { get; set; }
        public int? appId { get; set; }
        public string qrEmpName { get; set; }
        public string qrEmpNameMar { get; set; }
        public string qrEmpLoginId { get; set; }
        public string qrEmpPassword { get; set; }
        public string qrEmpMobileNumber { get; set; }
        public string qrEmpAddress { get; set; }
        public string type { get; set; }
        public int? typeId { get; set; }
        public string userEmployeeNo { get; set; }
        public string imoNo { get; set; }
        public string bloodGroup { get; set; }
        public bool? isActive { get; set; }
        public string target { get; set; }
        public DateTime? lastModifyDate { get; set; }
    }
}
