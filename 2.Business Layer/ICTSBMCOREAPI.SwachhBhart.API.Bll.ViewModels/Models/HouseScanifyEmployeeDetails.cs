using System;
using System.Collections.Generic;
using System.Text;

namespace ICTSBMCOREAPI.SwachhBhart.API.Bll.ViewModels.Models
{
    public class HouseScanifyEmployeeDetails
    {
        public int qrEmpId { get; set; }
        public Nullable<int> appId { get; set; }
        public string qrEmpName { get; set; }
        public string qrEmpNameMar { get; set; }
        public string qrEmpPassword { get; set; }
        public string qrEmpMobileNumber { get; set; }
        public string qrEmpAddress { get; set; }
        public string type { get; set; }
        public Nullable<int> typeId { get; set; }
        public string userEmployeeNo { get; set; }
        public string imoNo { get; set; }
        public string bloodGroup { get; set; }
        public Nullable<bool> isActive { get; set; }
        public string target { get; set; }
        public Nullable<System.DateTime> lastModifyDate { get; set; }
        public string qrEmpLoginId { get; set; }
        public string LoginId { get; set; }
        public string Password { get; set; }
    }

}
