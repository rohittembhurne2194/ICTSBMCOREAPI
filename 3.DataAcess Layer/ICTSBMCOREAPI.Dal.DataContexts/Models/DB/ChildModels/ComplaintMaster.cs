using System;
using System.Collections.Generic;

#nullable disable

namespace ICTSBMCOREAPI.Dal.DataContexts.Models.DB.ChildModels
{
    public partial class ComplaintMaster
    {
        public int Cid { get; set; }
        public string Cname { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? CreationDate { get; set; }
    }
}
