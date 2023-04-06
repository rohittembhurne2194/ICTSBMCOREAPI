using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICTSBMCOREAPI.Dal.DataContexts.Models.DB.ChildSPModels
{
    public class GetQrWorkHistoryDetails_Result
    {
        public int type { get; set; }
        public string HouseNo { get; set; }
        public string LiquidNo { get; set; }
        public string StreetNo { get; set; }
        public string DumpYardNo { get; set; }
        public DateTime? Date { get; set; }
        //public DateTime? time { get; set; }

        //public string Date { get; set; }
        public string time { get; set; }
    }
}
