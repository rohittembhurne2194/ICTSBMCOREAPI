using System;
using System.Collections.Generic;

#nullable disable

namespace ICTSBMCOREAPI.Dal.DataContexts.Models.DB.ChildModels
{
    public partial class GramCleaningComplient
    {
        public int CleaningComplientId { get; set; }
        public string Place { get; set; }
        public string WardNo { get; set; }
        public string Details { get; set; }
        public string Name { get; set; }
        public decimal? Number { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Tip { get; set; }
        public string Image { get; set; }
        public string LatLog { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public int? LanguageId { get; set; }
        public string Status { get; set; }
        public string UserId { get; set; }
        public string StatusDescription { get; set; }
        public string StatusImageUrl { get; set; }
        public string RefId { get; set; }
        public int? Type { get; set; }
    }
}
