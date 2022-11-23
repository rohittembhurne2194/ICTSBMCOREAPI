using System;
using System.Collections.Generic;

#nullable disable

namespace ICTSBMCOREAPI.Dal.DataContexts.Models.DB.ChildModels
{
    public partial class BinLatLong
    {
        public int Id { get; set; }
        public string Lat { get; set; }
        public string Long { get; set; }
        public string Distance { get; set; }
        public string Temp { get; set; }
        public string S1 { get; set; }
        public string S2 { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
