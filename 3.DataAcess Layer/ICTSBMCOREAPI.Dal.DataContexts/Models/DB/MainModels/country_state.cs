using System;
using System.Collections.Generic;

#nullable disable

namespace ICTSBMCOREAPI.Dal.DataContexts.Models.DB.MainModels
{
    public partial class country_state
    {
        public int id { get; set; }
        public string country_name { get; set; }
        public string state_name { get; set; }
        public string state_name_mar { get; set; }
        public string state_3_code { get; set; }
        public string state_2_code { get; set; }
    }
}
