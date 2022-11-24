using System;
using System.Collections.Generic;

#nullable disable

namespace ICTSBMCOREAPI.Dal.DataContexts.Models.DB.ChildModels
{
    public partial class LanguageInfo
    {
        public int id { get; set; }
        public string languageType { get; set; }
        public string languageCode { get; set; }
    }
}
