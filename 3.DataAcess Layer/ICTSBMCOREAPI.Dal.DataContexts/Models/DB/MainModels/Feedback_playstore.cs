using System;
using System.Collections.Generic;

#nullable disable

namespace ICTSBMCOREAPI.Dal.DataContexts.Models.DB.MainModels
{
    public partial class Feedback_playstore
    {
        public int PlaystoreID { get; set; }
        public int? AppID { get; set; }
        public string ULBlink { get; set; }
    }
}
