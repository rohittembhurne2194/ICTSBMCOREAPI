using System;
using System.Collections.Generic;

#nullable disable

namespace ICTSBMCOREAPI.Models.DB.ChildModels
{
    public partial class Vw_GetStreetNumber
    {
        public int SSId { get; set; }
        public string ReferanceId { get; set; }
        public int? AreaId { get; set; }
    }
}
