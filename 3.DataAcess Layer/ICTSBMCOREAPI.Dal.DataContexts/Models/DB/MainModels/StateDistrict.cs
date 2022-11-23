using System;
using System.Collections.Generic;

#nullable disable

namespace ICTSBMCOREAPI.Dal.DataContexts.Models.DB.MainModels
{
    public partial class StateDistrict
    {
        public int Id { get; set; }
        public int StateId { get; set; }
        public string DistrictName { get; set; }
        public string DistrictNameMar { get; set; }
    }
}
