using System;
using System.Collections.Generic;

#nullable disable

namespace ICTSBMCOREAPI.Models.DB.ChildModels
{
    public partial class EmpShift
    {
        public int shiftId { get; set; }
        public string shiftName { get; set; }
        public string shiftStart { get; set; }
        public string shiftEnd { get; set; }
    }
}
