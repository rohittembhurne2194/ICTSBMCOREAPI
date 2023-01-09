using System;
using System.Collections.Generic;

#nullable disable

namespace ICTSBMCOREAPI.Models.DB.ChildModels
{
    public partial class HouseList
    {
        public int Id { get; set; }
        public string ReferanceId { get; set; }
        public bool IsCheked { get; set; }
        public int? HouseId { get; set; }
        public bool? IsActive { get; set; }
        public int? AreaId { get; set; }
    }
}
