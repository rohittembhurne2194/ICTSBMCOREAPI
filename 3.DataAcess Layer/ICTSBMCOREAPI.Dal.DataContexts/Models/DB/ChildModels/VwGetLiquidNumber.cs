using System;
using System.Collections.Generic;

#nullable disable

namespace ICTSBMCOREAPI.Dal.DataContexts.Models.DB.ChildModels
{
    public partial class VwGetLiquidNumber
    {
        public int Lwid { get; set; }
        public string ReferanceId { get; set; }
        public int? AreaId { get; set; }
    }
}
