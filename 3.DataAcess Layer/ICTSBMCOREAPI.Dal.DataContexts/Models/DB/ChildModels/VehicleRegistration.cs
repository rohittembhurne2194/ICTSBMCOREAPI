using System;
using System.Collections.Generic;

#nullable disable

namespace ICTSBMCOREAPI.Dal.DataContexts.Models.DB.ChildModels
{
    public partial class VehicleRegistration
    {
        public int vehicleId { get; set; }
        public int? vehicleType { get; set; }
        public string vehicleNo { get; set; }
        public int? areaId { get; set; }
        public bool? isActive { get; set; }
    }
}
