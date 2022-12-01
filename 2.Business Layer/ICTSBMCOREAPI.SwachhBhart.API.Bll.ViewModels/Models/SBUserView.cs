using System;
using System.Collections.Generic;
using System.Text;

namespace ICTSBMCOREAPI.SwachhBhart.API.Bll.ViewModels.Models
{
    public class SBUserView
    {
        public string userId { get; set; }
        public string name { get; set; }
        public string nameMar { get; set; }
        public string mobileNumber { get; set; }
        public string address { get; set; }
        public string profileImage { get; set; }
        public string type { get; set; }
        public int typeId { get; set; }
        public string bloodGroup { get; set; }
    }

}
