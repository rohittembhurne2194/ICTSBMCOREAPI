using System;
using System.Collections.Generic;
using System.Text;

namespace ICTSBMCOREAPI.SwachhBhart.API.Bll.ViewModels.Models
{
  public  class CollectionDumpSyncResult
    {
        public int tripId { get; set; }
        public string transId { get; set; }
        public string dumpId { get; set; }

        public string status { get; set; }
        public string message { get; set; }
        public string messageMar { get; set; }


        public string bcTransId { get; set; }

        public bool gvstatus { get; set; }
    }
}
