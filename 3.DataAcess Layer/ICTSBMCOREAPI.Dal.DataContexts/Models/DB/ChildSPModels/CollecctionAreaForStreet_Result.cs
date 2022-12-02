using System;
using System.Collections.Generic;
using System.Text;

namespace ICTSBMCOREAPI.Dal.DataContexts.Models.DB.ChildSPModels
{
    public partial class CollecctionAreaForStreet_Result
    {
        public int Id { get; set; }
        public string Area { get; set; }
        public string AreaMar { get; set; }
        public Nullable<int> wardId { get; set; }
    }
}
