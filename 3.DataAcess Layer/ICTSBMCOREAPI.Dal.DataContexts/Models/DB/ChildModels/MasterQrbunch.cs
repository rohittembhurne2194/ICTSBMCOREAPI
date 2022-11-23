using System;
using System.Collections.Generic;

#nullable disable

namespace ICTSBMCOREAPI.Dal.DataContexts.Models.DB.ChildModels
{
    public partial class MasterQrbunch
    {
        public int MasterId { get; set; }
        public int? HouseBunchId { get; set; }
        public string Qrlist { get; set; }
        public bool? Isactive { get; set; }
    }
}
