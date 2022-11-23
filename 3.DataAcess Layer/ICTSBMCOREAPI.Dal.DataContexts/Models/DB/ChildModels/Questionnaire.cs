using System;
using System.Collections.Generic;

#nullable disable

namespace ICTSBMCOREAPI.Dal.DataContexts.Models.DB.ChildModels
{
    public partial class Questionnaire
    {
        public int? QNumber { get; set; }
        public string QText { get; set; }
        public string QResponse { get; set; }
    }
}
