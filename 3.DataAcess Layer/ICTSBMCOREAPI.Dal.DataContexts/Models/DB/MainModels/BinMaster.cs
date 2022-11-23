using System;
using System.Collections.Generic;

#nullable disable

namespace ICTSBMCOREAPI.Dal.DataContexts.Models.DB.MainModels
{
    public partial class BinMaster
    {
        public int Id { get; set; }
        public string DeviceId { get; set; }
        public int? AppId { get; set; }
        public string AppName { get; set; }
    }
}
