using System;
using System.Collections.Generic;

#nullable disable

namespace ICTSBMCOREAPI.Dal.DataContexts.Models.DB.ChildModels
{
    public partial class UserMaster
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string UserNameMar { get; set; }
        public string UserLoginId { get; set; }
        public string UserPassword { get; set; }
        public string UserMobileNumber { get; set; }
        public string UserAddress { get; set; }
        public string UserProfileImage { get; set; }
        public string Type { get; set; }
        public string UserEmployeeNo { get; set; }
        public string UserDesignation { get; set; }
        public string ImoNo { get; set; }
        public string BloodGroup { get; set; }
        public bool? IsActive { get; set; }
        public string GcTarget { get; set; }
        public string EmployeeType { get; set; }
        public string ImoNo2 { get; set; }
        public string ShiftIds { get; set; }
    }
}
