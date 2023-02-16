using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ICTSBMCOREAPI.Dal.DataContexts.Models.DB.MainModels
{
    [Table("GIS_AppConnection")]
    public partial class GisAppConnection
    {
        [Key]
        public int AppConnectionId { get; set; }
        public int AppId { get; set; }
        [Required]
        [StringLength(255)]
        public string DataSource { get; set; }
        [Required]
        [StringLength(255)]
        public string InitialCatalog { get; set; }
        [Required]
        [StringLength(255)]
        public string UserId { get; set; }
        [Required]
        [StringLength(255)]
        public string Password { get; set; }
        [Column("url")]
        [StringLength(255)]
        public string Url { get; set; }
    }
}
