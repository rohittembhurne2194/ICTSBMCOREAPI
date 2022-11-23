using System;
using System.Collections.Generic;

#nullable disable

namespace ICTSBMCOREAPI.Dal.DataContexts.Models.DB.ChildModels
{
    public partial class Ss17Answer
    {
        public int Trno { get; set; }
        public int Id { get; set; }
        public int? NoWaterBodies { get; set; }
        public int? NoDrainNallas { get; set; }
        public int? NoLocations { get; set; }
        public int? NoOutlets { get; set; }
        public int? InsertId { get; set; }
        public DateTime? InsertDate { get; set; }
    }
}
