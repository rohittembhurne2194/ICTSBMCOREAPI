﻿using System;
using System.Collections.Generic;

#nullable disable

namespace ICTSBMCOREAPI.Models.DB.ChildModels
{
    public partial class UserMaster
    {
        public int userId { get; set; }
        public string userName { get; set; }
        public string userNameMar { get; set; }
        public string userLoginId { get; set; }
        public string userPassword { get; set; }
        public string userMobileNumber { get; set; }
        public string userAddress { get; set; }
        public string userProfileImage { get; set; }
        public string Type { get; set; }
        public string userEmployeeNo { get; set; }
        public string userDesignation { get; set; }
        public string imoNo { get; set; }
        public string bloodGroup { get; set; }
        public bool? isActive { get; set; }
        public string gcTarget { get; set; }
        public string EmployeeType { get; set; }
        public string imoNo2 { get; set; }
        public string shiftIds { get; set; }
    }
}
