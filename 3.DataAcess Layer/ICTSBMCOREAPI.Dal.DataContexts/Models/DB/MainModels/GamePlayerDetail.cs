using System;
using System.Collections.Generic;

#nullable disable

namespace ICTSBMCOREAPI.Dal.DataContexts.Models.DB.MainModels
{
    public partial class GamePlayerDetail
    {
        public int Id { get; set; }
        public string PlayerId { get; set; }
        public int? GameId { get; set; }
        public string Name { get; set; }
        public string Mobile { get; set; }
        public int? Score { get; set; }
        public string DeviceId { get; set; }
        public DateTime? Created { get; set; }
    }
}
