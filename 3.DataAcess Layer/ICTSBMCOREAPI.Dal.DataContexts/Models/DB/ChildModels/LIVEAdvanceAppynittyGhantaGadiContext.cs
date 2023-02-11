using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace ICTSBMCOREAPI.Dal.DataContexts.Models.DB.ChildModels
{
    public partial class LIVEAdvanceAppynittyGhantaGadiContext : DbContext
    {
        public LIVEAdvanceAppynittyGhantaGadiContext()
        {
        }

        public LIVEAdvanceAppynittyGhantaGadiContext(DbContextOptions<LIVEAdvanceAppynittyGhantaGadiContext> options)
            : base(options)
        {
        }

        public virtual DbSet<TrailHouse> TrailHouses { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server= 124.153.94.110;Initial Catalog= LIVEAdvanceAppynittyGhantaGadi;Persist Security Info=False;User ID=appynitty;Password=BigV$Telecom;MultipleActiveResultSets=False;Connection Timeout=30;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<TrailHouse>(entity =>
            {
                entity.Property(e => e.Id).IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
