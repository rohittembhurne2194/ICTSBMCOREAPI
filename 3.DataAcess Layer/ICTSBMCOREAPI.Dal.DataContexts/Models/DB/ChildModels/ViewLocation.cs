using System;
using System.Collections.Generic;

#nullable disable

namespace ICTSBMCOREAPI.Dal.DataContexts.Models.DB.ChildModels
{
    public partial class ViewLocation
    {
        public DateTime? LocDate { get; set; }
        public int? Userid { get; set; }
        public decimal? Distnace { get; set; }
    }
}
