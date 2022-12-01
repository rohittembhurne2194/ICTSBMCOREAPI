using ICTSBMCOREAPI.Dal.DataContexts.Models.DB.ChildModels;
using ICTSBMCOREAPI.Dal.DataContexts.Models.DB.ChildSPModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ICTSBMCOREAPI.Dal.DataContexts.Models.DB
{
    public class DevICTSBMChildEntities : DevSwachhBharatNagpurEntities
    {
        private int AppId;
        public DevICTSBMChildEntities()
        {

        }
        public DevICTSBMChildEntities(int AppId)
        {
            this.AppId = AppId;
        }
        public DevICTSBMChildEntities(DbContextOptions<DevSwachhBharatNagpurEntities> options)
            : base(options)
        {
           
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                //optionsBuilder.UseSqlServer("data source=202.65.157.253;initial catalog=LIVEAdvanceEtapalliGhantaGadi;persist security info=True;user id=appynitty;password=BigV$Telecom;MultipleActiveResultSets=True;App=EntityFramework");
                if (AppId > 0)
                {
                    optionsBuilder.UseSqlServer(SwachhBharatAppConnection.GetConnectionString(AppId));
                }
                else
                {
                    optionsBuilder.UseSqlServer("data source=202.65.157.253;initial catalog=LIVEAdvanceEtapalliGhantaGadi;persist security info=True;user id=appynitty;password=BigV$Telecom;MultipleActiveResultSets=True;App=EntityFramework");

                }

            }
        }
        public DbSet<SP_UserLatLongDetail_Result> SP_UserLatLongDetail_Results { get; set; }
        public DbSet<SP_DistanceCount_Result> SP_DistanceCount_Results { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<SP_UserLatLongDetail_Result>().HasNoKey();
            modelBuilder.Entity<SP_DistanceCount_Result>().HasNoKey();
           
        }
    }
}
