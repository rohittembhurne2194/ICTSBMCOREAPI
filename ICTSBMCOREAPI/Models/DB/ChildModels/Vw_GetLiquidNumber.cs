using System;
using System.Collections.Generic;

#nullable disable

namespace ICTSBMCOREAPI.Models.DB.ChildModels
{
    public partial class Vw_GetLiquidNumber
    {
        public int LWId { get; set; }
        public string ReferanceId { get; set; }
        public int? AreaId { get; set; }
    }
}
