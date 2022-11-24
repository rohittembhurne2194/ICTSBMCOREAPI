using System;
using System.Collections.Generic;

#nullable disable

namespace ICTSBMCOREAPI.Dal.DataContexts.Models.DB.MainModels
{
    public partial class tehsil
    {
        public int id { get; set; }
        public string name { get; set; }
        public string name_mar { get; set; }
        public string latitude { get; set; }
        public string longitude { get; set; }
        public int? state { get; set; }
        public int? district { get; set; }
    }
}
