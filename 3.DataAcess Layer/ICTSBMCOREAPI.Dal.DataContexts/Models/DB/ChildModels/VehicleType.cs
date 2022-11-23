using System;
using System.Collections.Generic;

#nullable disable

namespace ICTSBMCOREAPI.Dal.DataContexts.Models.DB.ChildModels
{
    public partial class VehicleType
    {
        public int VtId { get; set; }
        public string Description { get; set; }
        public string DescriptionMar { get; set; }
        public bool? IsActive { get; set; }
    }
}
