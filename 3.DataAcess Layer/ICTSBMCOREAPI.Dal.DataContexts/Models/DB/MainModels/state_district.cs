using System;
using System.Collections.Generic;

#nullable disable

namespace ICTSBMCOREAPI.Dal.DataContexts.Models.DB.MainModels
{
    public partial class state_district
    {
        public int id { get; set; }
        public int state_id { get; set; }
        public string district_name { get; set; }
        public string district_name_mar { get; set; }
    }
}
