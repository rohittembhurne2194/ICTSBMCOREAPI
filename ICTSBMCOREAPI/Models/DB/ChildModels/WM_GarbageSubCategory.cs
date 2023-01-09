using System;
using System.Collections.Generic;

#nullable disable

namespace ICTSBMCOREAPI.Models.DB.ChildModels
{
    public partial class WM_GarbageSubCategory
    {
        public int SubCategoryID { get; set; }
        public string SubCategory { get; set; }
        public int? CategoryID { get; set; }
    }
}
