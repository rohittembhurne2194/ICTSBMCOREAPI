using System;
using System.Collections.Generic;

#nullable disable

namespace ICTSBMCOREAPI.Dal.DataContexts.Models.DB.ChildModels
{
    public partial class DeviceDetail
    {
        public int DeviceDetailId { get; set; }
        public string DeviceId { get; set; }
        public string Fcmid { get; set; }
        public string DeviceType { get; set; }
        public string ReferenceId { get; set; }
        public DateTime? InstallDate { get; set; }
    }
}
