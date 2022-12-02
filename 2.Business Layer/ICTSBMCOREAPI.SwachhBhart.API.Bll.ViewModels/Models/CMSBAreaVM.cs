using System;
using System.Collections.Generic;
using System.Text;

namespace ICTSBMCOREAPI.SwachhBhart.API.Bll.ViewModels.Models
{
    public class CMSBAreaVM
    {
        public int id { get; set; }
        public string areaMar { get; set; }
        public string area { get; set; }

        public Nullable<int> wardId { get; set; }

        public string Wardno { get; set; }
    }

}
