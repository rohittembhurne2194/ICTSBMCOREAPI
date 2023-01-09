using System;
using System.Collections.Generic;

#nullable disable

namespace ICTSBMCOREAPI.Models.DB.ChildModels
{
    public partial class MasterQR
    {
        public int MasterId { get; set; }
        public string ReferanceId { get; set; }
        public string QRList { get; set; }
        public bool? ISActive { get; set; }
    }
}
