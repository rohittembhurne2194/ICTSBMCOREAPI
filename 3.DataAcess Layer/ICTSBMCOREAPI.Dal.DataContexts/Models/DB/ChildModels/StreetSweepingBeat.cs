using System;
using System.Collections.Generic;

#nullable disable

namespace ICTSBMCOREAPI.Dal.DataContexts.Models.DB.ChildModels
{
    public partial class StreetSweepingBeat
    {
        public int BeatId { get; set; }
        public DateTime? CreateDate { get; set; }
        public string ReferanceId1 { get; set; }
        public string ReferanceId2 { get; set; }
        public string ReferanceId3 { get; set; }
        public string ReferanceId4 { get; set; }
        public string ReferanceId5 { get; set; }
    }
}
