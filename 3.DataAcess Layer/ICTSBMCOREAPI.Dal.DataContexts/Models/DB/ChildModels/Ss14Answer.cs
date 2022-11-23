using System;
using System.Collections.Generic;

#nullable disable

namespace ICTSBMCOREAPI.Dal.DataContexts.Models.DB.ChildModels
{
    public partial class Ss14Answer
    {
        public int AnsId { get; set; }
        public int QId { get; set; }
        public string TotalCount { get; set; }
        public DateTime? InsertDate { get; set; }
        public int? InsertId { get; set; }
        public string Marks { get; set; }
    }
}
