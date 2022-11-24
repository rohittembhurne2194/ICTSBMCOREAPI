using System;
using System.Collections.Generic;

#nullable disable

namespace ICTSBMCOREAPI.Dal.DataContexts.Models.DB.MainModels
{
    public partial class CheckAppD
    {
        public int Id { get; set; }
        public string App_Name { get; set; }
        public bool IsCheked { get; set; }
        public int? AppId { get; set; }
        public bool? IsActive { get; set; }
    }
}
