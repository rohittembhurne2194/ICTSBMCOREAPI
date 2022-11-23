using System;
using System.Collections.Generic;

#nullable disable

namespace ICTSBMCOREAPI.Dal.DataContexts.Models.DB.ChildModels
{
    public partial class VwGetHouseNumber
    {
        public int HouseId { get; set; }
        public string HouseNumber { get; set; }
        public string ReferanceId { get; set; }
        public int? AreaId { get; set; }
    }
}
