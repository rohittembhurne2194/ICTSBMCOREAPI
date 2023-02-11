using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ICTSBMCOREAPI.Dal.DataContexts.Models.DB.ChildModels
{
    [Table("trail_house")]
    public partial class TrailHouse
    {
       
        [Column("id")]
        [StringLength(255)]
        public string Id { get; set; }
        [Column("start_ts", TypeName = "datetime")]
        public DateTime StartTs { get; set; }
        [Column("end_ts", TypeName = "datetime")]
        public DateTime EndTs { get; set; }
        [Column("create_user")]
        public long CreateUser { get; set; }
        [Column("create_ts", TypeName = "datetime")]
        public DateTime CreateTs { get; set; }
        [Column("update_user")]
        public long? UpdateUser { get; set; }
        [Column("update_ts", TypeName = "datetime")]
        public DateTime? UpdateTs { get; set; }

        //public DbGeography geom { get; set; }
    }
}
