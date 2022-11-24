using System;
using System.Collections.Generic;

#nullable disable

namespace ICTSBMCOREAPI.Dal.DataContexts.Models.DB.ChildModels
{
    public partial class GramCleaningComplient
    {
        public int CleaningComplientId { get; set; }
        public string Place { get; set; }
        public string Ward_No_ { get; set; }
        public string Details { get; set; }
        public string Name { get; set; }
        public decimal? Number { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Tip { get; set; }
        public string Image { get; set; }
        public string Lat_Log { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public int? languageId { get; set; }
        public string status { get; set; }
        public string userId { get; set; }
        public string status_description { get; set; }
        public string status_image_url { get; set; }
        public string ref_id { get; set; }
        public int? Type { get; set; }
    }
}
