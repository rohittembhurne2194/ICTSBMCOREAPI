using System;
using System.Collections.Generic;

#nullable disable

namespace ICTSBMCOREAPI.Dal.DataContexts.Models.DB.MainModels
{
    public partial class GameMaster
    {
        public int GameId { get; set; }
        public string GameName { get; set; }
        public string GameNameMar { get; set; }
        public string GameNameHindi { get; set; }
    }
}
