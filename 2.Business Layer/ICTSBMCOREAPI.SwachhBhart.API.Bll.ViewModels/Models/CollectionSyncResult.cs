using System;
using System.Collections.Generic;
using System.Text;

namespace ICTSBMCOREAPI.SwachhBhart.API.Bll.ViewModels.Models
{
    public class CollectionSyncResult
    {
        public int ID { get; set; }
        public string status { get; set; }
        public string message { get; set; }
        public string messageMar { get; set; }
        public bool isAttendenceOff { get; set; }
        public string houseId { get; set; }
        public bool IsExist { get; set; }
        public string giserrorMessages { get; set; }
        public string referenceID { get; set; }



    }
}
