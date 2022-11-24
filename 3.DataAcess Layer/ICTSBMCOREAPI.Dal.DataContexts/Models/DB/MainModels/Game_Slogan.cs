using System;
using System.Collections.Generic;

#nullable disable

namespace ICTSBMCOREAPI.Dal.DataContexts.Models.DB.MainModels
{
    public partial class Game_Slogan
    {
        public int ID { get; set; }
        public string Slogan { get; set; }
        public string SloganMar { get; set; }
        public string SloganHindi { get; set; }
    }
}
