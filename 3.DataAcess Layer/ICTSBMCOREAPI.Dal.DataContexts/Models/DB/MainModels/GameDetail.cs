using System;
using System.Collections.Generic;

#nullable disable

namespace ICTSBMCOREAPI.Dal.DataContexts.Models.DB.MainModels
{
    public partial class GameDetail
    {
        public int GameDetailsID { get; set; }
        public string ImageUrl { get; set; }
        public int? RightAnswerID { get; set; }
        public int? Point { get; set; }
        public int? GameMasterID { get; set; }
        public int? SloganID { get; set; }
        public string Description { get; set; }
        public DateTime? Created { get; set; }
    }
}
