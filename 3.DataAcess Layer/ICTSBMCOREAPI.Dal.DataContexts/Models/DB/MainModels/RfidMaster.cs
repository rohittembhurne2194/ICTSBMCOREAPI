using System;
using System.Collections.Generic;

#nullable disable

namespace ICTSBMCOREAPI.Dal.DataContexts.Models.DB.MainModels
{
    public partial class RfidMaster
    {
        public int Id { get; set; }
        public string ReaderId { get; set; }
        public string TagId { get; set; }
        public int? AppId { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
