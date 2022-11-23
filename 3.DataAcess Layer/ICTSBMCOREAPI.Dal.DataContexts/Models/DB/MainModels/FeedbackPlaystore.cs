using System;
using System.Collections.Generic;

#nullable disable

namespace ICTSBMCOREAPI.Dal.DataContexts.Models.DB.MainModels
{
    public partial class FeedbackPlaystore
    {
        public int PlaystoreId { get; set; }
        public int? AppId { get; set; }
        public string Ulblink { get; set; }
    }
}
