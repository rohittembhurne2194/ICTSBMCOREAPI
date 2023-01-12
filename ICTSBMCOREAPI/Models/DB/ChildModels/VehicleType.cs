using System;
using System.Collections.Generic;

#nullable disable

namespace ICTSBMCOREAPI.Models.DB.ChildModels
{
    public partial class VehicleType
    {
        public int vtId { get; set; }
        public string description { get; set; }
        public string descriptionMar { get; set; }
        public bool? isActive { get; set; }
    }
}
