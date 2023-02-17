using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ICTSBMCOREAPI.Dal.DataContexts.Models.DB.MainModels
{
    public partial class AspNetGisUser
    {
        [Key]
        public int Id { get; set; }
        public int AppId { get; set; }
        [Required]
        [StringLength(256)]
        public string UserName { get; set; }
        [Required]
        [StringLength(256)]
        public string Password { get; set; }
    }
}
