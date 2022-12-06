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
                    optionsBuilder.UseSqlServer(SwachhBharatAppConnection.GetConnectionString(AppId),options => options.EnableRetryOnFailure(20));
                }
                else
                {
                    optionsBuilder.UseSqlServer("data source=202.65.157.253;initial catalog=LIVEAdvanceEtapalliGhantaGadi;persist security info=True;user id=appynitty;password=BigV$Telecom;MultipleActiveResultSets=True;App=EntityFramework");

                }

            }
        }
        public DbSet<SP_UserLatLongDetail_Result> SP_UserLatLongDetail_Results { get; set; }
        public DbSet<SP_DistanceCount_Result> SP_DistanceCount_Results { get; set; }
        public DbSet<sp_MsgNotification_Result> sp_MsgNotification_Results { get; set; }
        public DbSet<GetQrWorkHistory_Result> GetQrWorkHistory_Results { get; set; }
        public DbSet<SP_HousePointDumpDetails_Scanify_Result> SP_HousePointDumpDetails_Scanify_Results { get; set; }
        public DbSet<VehicleList_TypeWise_Result> VehicleList_TypeWise_Results { get; set; }
        public DbSet<GetAttendenceDetailsTotal_Result> GetAttendenceDetailsTotal_Results { get; set; }
        public DbSet<GetAttendenceDetailsTotalLiquid_Result> GetAttendenceDetailsTotalLiquid_Results { get; set; }
        public DbSet<GetAttendenceDetailsTotalStreet_Result> GetAttendenceDetailsTotalStreet_Results { get; set; }
        public DbSet<GetAttendenceDetailsTotalDump_Result> GetAttendenceDetailsTotalDump_Results { get; set; }
        public DbSet<CollecctionArea_Result> CollecctionArea_Results { get; set; }
        public DbSet<CollecctionAreaForLiquid_Result> CollecctionAreaForLiquid_Results { get; set; }
        public DbSet<CollecctionAreaForStreet_Result> CollecctionAreaForStreet_Results { get; set; }
        public DbSet<GetMobile_Result> GetMobile_Results { get; set; }
        public DbSet<SP_HouseScanifyDetails_Result> SP_HouseScanifyDetails_Results { get; set; }
        public DbSet<SP_HouseScanify_Result> SP_HouseScanify_Results { get; set; }
        public DbSet<SP_HouseDetailsApp_Result> SP_HouseDetailsApp_Results { get; set; }
        public DbSet<SP_DumpYardDetailsApp_Result> SP_DumpYardDetailsApp_Results { get; set; }
        public DbSet<SP_LiquidDetailsApp_Result> SP_LiquidDetailsApp_Results { get; set; }
        public DbSet<SP_StreetDetailsApp_Result> SP_StreetDetailsApp_Results { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<SP_UserLatLongDetail_Result>().HasNoKey();
            modelBuilder.Entity<SP_DistanceCount_Result>().HasNoKey();
            modelBuilder.Entity<sp_MsgNotification_Result>().HasNoKey();
            modelBuilder.Entity<GetQrWorkHistory_Result>().HasNoKey();
            modelBuilder.Entity<SP_HousePointDumpDetails_Scanify_Result>().HasNoKey();
            modelBuilder.Entity<VehicleList_TypeWise_Result>().HasNoKey();
            modelBuilder.Entity<GetAttendenceDetailsTotal_Result>().HasNoKey();
            modelBuilder.Entity<GetAttendenceDetailsTotalLiquid_Result>().HasNoKey();
            modelBuilder.Entity<GetAttendenceDetailsTotalStreet_Result>().HasNoKey();
            modelBuilder.Entity<GetAttendenceDetailsTotalDump_Result>().HasNoKey();
            modelBuilder.Entity<CollecctionArea_Result>().HasNoKey();
            modelBuilder.Entity<CollecctionAreaForLiquid_Result>().HasNoKey();
            modelBuilder.Entity<CollecctionAreaForStreet_Result>().HasNoKey();
            modelBuilder.Entity<GetMobile_Result>().HasNoKey();
            modelBuilder.Entity<SP_HouseScanifyDetails_Result>().HasNoKey();
            modelBuilder.Entity<SP_HouseScanify_Result>().HasNoKey();
            modelBuilder.Entity<SP_HouseDetailsApp_Result>().HasNoKey();
            modelBuilder.Entity<SP_DumpYardDetailsApp_Result>().HasNoKey();
            modelBuilder.Entity<SP_LiquidDetailsApp_Result>().HasNoKey();
            modelBuilder.Entity<SP_StreetDetailsApp_Result>().HasNoKey();

        }
    }
}
