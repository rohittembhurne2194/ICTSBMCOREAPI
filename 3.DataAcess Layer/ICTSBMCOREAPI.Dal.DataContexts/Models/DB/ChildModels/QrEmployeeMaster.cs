using System;
using System.Collections.Generic;

#nullable disable

namespace ICTSBMCOREAPI.Dal.DataContexts.Models.DB.ChildModels
{
    public partial class QrEmployeeMaster
    {
        public int QrEmpId { get; set; }
        public int? AppId { get; set; }
        public string QrEmpName { get; set; }
        public string QrEmpNameMar { get; set; }
        public string QrEmpLoginId { get; set; }
        public string QrEmpPassword { get; set; }
        public string QrEmpMobileNumber { get; set; }
        public string QrEmpAddress { get; set; }
        public string Type { get; set; }
        public int? TypeId { get; set; }
        public string UserEmployeeNo { get; set; }
        public string ImoNo { get; set; }
        public string BloodGroup { get; set; }
        public bool? IsActive { get; set; }
        public string Target { get; set; }
        public DateTime? LastModifyDate { get; set; }
    }
}
