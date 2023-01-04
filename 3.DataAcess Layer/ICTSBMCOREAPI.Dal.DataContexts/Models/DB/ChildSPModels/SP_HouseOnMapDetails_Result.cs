using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICTSBMCOREAPI.Dal.DataContexts.Models.DB.ChildSPModels
{
    [Keyless]
    public partial class SP_HouseOnMapDetails_Result
    {
        public Nullable<int> userId { get; set; }
        public Nullable<int> houseId { get; set; }
        public Nullable<int> lwid { get; set; }
        public Nullable<int> ssid { get; set; }
        public Nullable<int> dyid { get; set; }
        public string ReferanceId { get; set; }
        public string houseNumber { get; set; }
        public string houseOwner { get; set; }
        public string houseOwnerMobile { get; set; }
        public string houseAddress { get; set; }
        public Nullable<int> garbageType { get; set; }
        public Nullable<System.DateTime> gcDate { get; set; }


        public string houseLat { get; set; }
        public string houseLong { get; set; }
        


        //public string userName { get; set; }
        //public string date { get; set; }
        //public string time { get; set; }
        //public string lat { get; set; }
        //public string log { get; set; }
        //public string address { get; set; }
        //public string vehcileNumber { get; set; }
        //public string userMobile { get; set; }
        //public string gcTime { get; set; }
        //public Nullable<int> gcType { get; set; }

        //public int areaId { get; set; }
        //public int BeatId { get; set; }
        //public bool IsIn { get; set; }
    }
}
