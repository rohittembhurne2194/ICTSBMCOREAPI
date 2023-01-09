using System;
using System.Collections.Generic;

#nullable disable

namespace ICTSBMCOREAPI.Models.DB.ChildModels
{
    public partial class DeviceDetail
    {
        public int DeviceDetailId { get; set; }
        public string DeviceID { get; set; }
        public string FCMID { get; set; }
        public string DeviceType { get; set; }
        public string ReferenceID { get; set; }
        public DateTime? InstallDate { get; set; }
    }
}
