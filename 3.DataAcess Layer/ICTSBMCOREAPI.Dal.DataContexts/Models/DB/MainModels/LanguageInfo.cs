using System;
using System.Collections.Generic;

#nullable disable

namespace ICTSBMCOREAPI.Dal.DataContexts.Models.DB.MainModels
{
    public partial class LanguageInfo
    {
        public int Id { get; set; }
        public string Language { get; set; }
        public string LanguageCode { get; set; }
    }
}
