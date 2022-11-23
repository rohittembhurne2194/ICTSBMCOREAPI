using System;
using System.Collections.Generic;

#nullable disable

namespace ICTSBMCOREAPI.Dal.DataContexts.Models.DB.MainModels
{
    public partial class GameDetail
    {
        public int GameDetailsId { get; set; }
        public string ImageUrl { get; set; }
        public int? RightAnswerId { get; set; }
        public int? Point { get; set; }
        public int? GameMasterId { get; set; }
        public int? SloganId { get; set; }
        public string Description { get; set; }
        public DateTime? Created { get; set; }
    }
}
