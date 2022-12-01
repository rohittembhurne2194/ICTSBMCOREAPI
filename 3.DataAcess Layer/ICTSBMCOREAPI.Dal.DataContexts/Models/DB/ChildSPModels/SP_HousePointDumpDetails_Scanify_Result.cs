using System;
using System.Collections.Generic;
using System.Text;

namespace ICTSBMCOREAPI.Dal.DataContexts.Models.DB.ChildSPModels
{
    public partial class SP_HousePointDumpDetails_Scanify_Result
    {
        public int houseId { get; set; }
        public string ReferanceId { get; set; }
        public string Name { get; set; }
        public string NameMar { get; set; }
        public string Images { get; set; }
        public string Zone { get; set; }
        public int zoneId { get; set; }
        public string Ward { get; set; }
        public int WardNoId { get; set; }
        public string Area { get; set; }
        public int AreaId { get; set; }
        public string HouseNumber { get; set; }
        public string MobileNumber { get; set; }
        public string Address { get; set; }
        public string Lat { get; set; }
        public string Long { get; set; }
    }

}
