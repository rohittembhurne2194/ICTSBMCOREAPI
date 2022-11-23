using System;
using System.Collections.Generic;

#nullable disable

namespace ICTSBMCOREAPI.Dal.DataContexts.Models.DB.MainModels
{
    public partial class GameAnswerType
    {
        public int AnswerTypeId { get; set; }
        public string AnswerType { get; set; }
        public string AnswerTypeMar { get; set; }
        public string AnswerTypeHindi { get; set; }
        public int? GameMasterId { get; set; }
    }
}
