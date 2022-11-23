using System;
using System.Collections.Generic;

#nullable disable

namespace ICTSBMCOREAPI.Dal.DataContexts.Models.DB.MainModels
{
    public partial class SauchalayFeedback
    {
        public int SauchalayFeedbackId { get; set; }
        public string Ulb { get; set; }
        public string SauchalayId { get; set; }
        public int? AppId { get; set; }
        public string Fullname { get; set; }
        public string MobileNo { get; set; }
        public string Que1 { get; set; }
        public string Que2 { get; set; }
        public string Que3 { get; set; }
        public string Rating { get; set; }
        public string Feedback { get; set; }
        public DateTime? Date { get; set; }
    }
}
