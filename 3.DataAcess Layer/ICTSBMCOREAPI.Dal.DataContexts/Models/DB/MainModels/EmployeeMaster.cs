using System;
using System.Collections.Generic;

#nullable disable

namespace ICTSBMCOREAPI.Dal.DataContexts.Models.DB.MainModels
{
    public partial class EmployeeMaster
    {
        public int EmpId { get; set; }
        public string EmpName { get; set; }
        public string EmpNameMar { get; set; }
        public string LoginId { get; set; }
        public string Password { get; set; }
        public string EmpMobileNumber { get; set; }
        public string EmpAddress { get; set; }
        public string type { get; set; }
        public bool? isActive { get; set; }
        public string isActiveULB { get; set; }
        public DateTime? lastModifyDateEntry { get; set; }
    }
}
