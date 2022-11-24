using System;
using System.Collections.Generic;

#nullable disable

namespace ICTSBMCOREAPI.Dal.DataContexts.Models.DB.ChildModels
{
    public partial class view_Location
    {
        public DateTime? LocDate { get; set; }
        public int? USERID { get; set; }
        public decimal? Distnace { get; set; }
    }
}
