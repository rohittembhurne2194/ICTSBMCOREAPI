using System;
using System.Collections.Generic;

#nullable disable

namespace ICTSBMCOREAPI.Dal.DataContexts.Models.DB.ChildModels
{
    public partial class LanguageInfo
    {
        public int Id { get; set; }
        public string LanguageType { get; set; }
        public string LanguageCode { get; set; }
    }
}
