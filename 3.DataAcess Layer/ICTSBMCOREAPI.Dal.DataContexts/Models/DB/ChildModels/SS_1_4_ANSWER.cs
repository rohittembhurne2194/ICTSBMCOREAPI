using System;
using System.Collections.Generic;

#nullable disable

namespace ICTSBMCOREAPI.Dal.DataContexts.Models.DB.ChildModels
{
    public partial class SS_1_4_ANSWER
    {
        public int ANS_ID { get; set; }
        public int Q_ID { get; set; }
        public string TOTAL_COUNT { get; set; }
        public DateTime? INSERT_DATE { get; set; }
        public int? INSERT_ID { get; set; }
        public string MARKS { get; set; }
    }
}
