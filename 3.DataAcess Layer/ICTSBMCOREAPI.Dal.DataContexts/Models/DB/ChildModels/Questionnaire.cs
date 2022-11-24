using System;
using System.Collections.Generic;

#nullable disable

namespace ICTSBMCOREAPI.Dal.DataContexts.Models.DB.ChildModels
{
    public partial class questionnaire
    {
        public int? Q_Number { get; set; }
        public string Q_Text { get; set; }
        public string Q_Response { get; set; }
    }
}
