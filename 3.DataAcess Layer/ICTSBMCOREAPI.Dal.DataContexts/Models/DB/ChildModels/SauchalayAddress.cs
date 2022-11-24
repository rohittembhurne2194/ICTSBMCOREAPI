using System;
using System.Collections.Generic;

#nullable disable

namespace ICTSBMCOREAPI.Dal.DataContexts.Models.DB.ChildModels
{
    public partial class SauchalayAddress
    {
        public int Id { get; set; }
        public string SauchalayID { get; set; }
        public string Address { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string Lat { get; set; }
        public string Long { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public string QrImageUrl { get; set; }
        public string Mobile { get; set; }
    }
}
