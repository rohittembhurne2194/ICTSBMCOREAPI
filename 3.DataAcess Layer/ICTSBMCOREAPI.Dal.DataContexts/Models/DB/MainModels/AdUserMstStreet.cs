using System;
using System.Collections.Generic;

#nullable disable

namespace ICTSBMCOREAPI.Dal.DataContexts.Models.DB.MainModels
{
    public partial class AdUserMstStreet
    {
        public int AdumUserCode { get; set; }
        public byte? ServerId { get; set; }
        public int AppId { get; set; }
        public string AdumUserId { get; set; }
        public string AdumUserName { get; set; }
        public string AdumLoginId { get; set; }
        public string AdumPassword { get; set; }
        public string AdumEmployeeId { get; set; }
        public string AdumDesignation { get; set; }
        public string AdumMobile { get; set; }
        public string AdumEmail { get; set; }
        public string LocalUserName { get; set; }
        public string ProfileImage { get; set; }
        public DateTime? AdumFrdt { get; set; }
        public DateTime? AdumTodt { get; set; }
        public bool? AdumStatus { get; set; }
        public bool? UpdateFlag { get; set; }
        public DateTime? LastUpdate { get; set; }
        public int? AdUserTypeId { get; set; }
        public string MobileId { get; set; }
        public bool? IsActive { get; set; }
        public string ImoNo { get; set; }
    }
}
