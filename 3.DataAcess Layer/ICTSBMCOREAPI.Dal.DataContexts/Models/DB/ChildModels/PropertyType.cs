using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ICTSBMCOREAPI.Dal.DataContexts.Models.DB.ChildModels
{
    [Table("Property_Type")]
    public partial class PropertyType
    {
        [Key]
        public int Id { get; set; }
        public int PropertyId { get; set; }
        [Column("Property_Type")]
        public string PropertyType1 { get; set; }
    }
}
