using System;
using System.Collections.Generic;

#nullable disable

namespace ICTSBMCOREAPI.Models.DB.ChildModels
{
    public partial class TeritoryMaster
    {
        public int Id { get; set; }
        public string Area { get; set; }
        public string AreaMar { get; set; }
        public int? wardId { get; set; }
    }
}
