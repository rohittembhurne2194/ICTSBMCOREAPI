using System;
using System.Collections.Generic;
using System.Text;

namespace ICTSBMCOREAPI.SwachhBhart.API.Bll.ViewModels.Models
{
    public class SBVehicleType
    {
        public int vtId { get; set; }
        public string description { get; set; }
        public string descriptionMar { get; set; }
        public bool? isActive { get; set; }
    }
}
