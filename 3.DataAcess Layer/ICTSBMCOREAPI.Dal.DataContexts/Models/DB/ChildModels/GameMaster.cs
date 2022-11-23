using System;
using System.Collections.Generic;

#nullable disable

namespace ICTSBMCOREAPI.Dal.DataContexts.Models.DB.ChildModels
{
    public partial class GameMaster
    {
        public int GameId { get; set; }
        public string GameName { get; set; }
        public string GameNameMar { get; set; }
    }
}
