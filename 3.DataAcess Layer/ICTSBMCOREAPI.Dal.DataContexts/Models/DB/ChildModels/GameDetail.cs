using System;
using System.Collections.Generic;

#nullable disable

namespace ICTSBMCOREAPI.Dal.DataContexts.Models.DB.ChildModels
{
    public partial class GameDetail
    {
        public int GameDetailsId { get; set; }
        public string ImageUrl { get; set; }
        public int? RightAnswerId { get; set; }
        public int? Point { get; set; }
        public int? GameMasterId { get; set; }
        public int? SloganId { get; set; }
        public DateTime? Created { get; set; }
    }
}
