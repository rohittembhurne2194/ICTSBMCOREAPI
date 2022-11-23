using System;
using System.Collections.Generic;

#nullable disable

namespace ICTSBMCOREAPI.Dal.DataContexts.Models.DB.ChildModels
{
    public partial class WmGarbageSubCategory
    {
        public int SubCategoryId { get; set; }
        public string SubCategory { get; set; }
        public int? CategoryId { get; set; }
    }
}
