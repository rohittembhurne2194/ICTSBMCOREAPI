using System;
using System.Collections.Generic;

#nullable disable

namespace ICTSBMCOREAPI.Models.DB.ChildModels
{
    public partial class WardNumber
    {
        public int Id { get; set; }
        public string WardNo { get; set; }
        public int? zoneId { get; set; }
    }
}
