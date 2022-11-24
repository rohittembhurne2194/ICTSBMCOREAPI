using System;
using System.Collections.Generic;

#nullable disable

namespace ICTSBMCOREAPI.Dal.DataContexts.Models.DB.MainModels
{
    public partial class Sauchalay_feedback
    {
        public int SauchalayFeedback_ID { get; set; }
        public string ULB { get; set; }
        public string SauchalayID { get; set; }
        public int? AppId { get; set; }
        public string Fullname { get; set; }
        public string MobileNo { get; set; }
        public string que1 { get; set; }
        public string que2 { get; set; }
        public string que3 { get; set; }
        public string Rating { get; set; }
        public string Feedback { get; set; }
        public DateTime? Date { get; set; }
    }
}
