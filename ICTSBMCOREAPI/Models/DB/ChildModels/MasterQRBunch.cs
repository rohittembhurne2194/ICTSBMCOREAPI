using System;
using System.Collections.Generic;

#nullable disable

namespace ICTSBMCOREAPI.Models.DB.ChildModels
{
    public partial class MasterQRBunch
    {
        public int MasterId { get; set; }
        public int? HouseBunchId { get; set; }
        public string QRList { get; set; }
        public bool? ISActive { get; set; }
    }
}
