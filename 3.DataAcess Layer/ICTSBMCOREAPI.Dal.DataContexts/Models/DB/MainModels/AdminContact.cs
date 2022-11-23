using System;
using System.Collections.Generic;

#nullable disable

namespace ICTSBMCOREAPI.Dal.DataContexts.Models.DB.MainModels
{
    public partial class AdminContact
    {
        public int Id { get; set; }
        public string Position { get; set; }
        public string MobileNumber { get; set; }
    }
}
