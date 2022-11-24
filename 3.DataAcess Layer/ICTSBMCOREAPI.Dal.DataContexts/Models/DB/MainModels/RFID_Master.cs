using System;
using System.Collections.Generic;

#nullable disable

namespace ICTSBMCOREAPI.Dal.DataContexts.Models.DB.MainModels
{
    public partial class RFID_Master
    {
        public int ID { get; set; }
        public string ReaderID { get; set; }
        public string TagID { get; set; }
        public int? AppID { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
