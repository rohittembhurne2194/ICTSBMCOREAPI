using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace ICTSBMCOREAPI.Dal.DataContexts.Models.DB.ChildModels
{
    public partial class DevSwachhBharatNagpurEntities : DbContext
    {
        public DevSwachhBharatNagpurEntities()
        {
        }

        public DevSwachhBharatNagpurEntities(DbContextOptions<DevSwachhBharatNagpurEntities> options)
            : base(options)
        {
        }

        public virtual DbSet<AspNetRole> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUser> AspNetUsers { get; set; }
        public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUserRole> AspNetUserRoles { get; set; }
        public virtual DbSet<BinLatLong> BinLatLongs { get; set; }
        public virtual DbSet<BuildingShp> BuildingShps { get; set; }
        public virtual DbSet<Buildings_CSV> Buildings_CSVs { get; set; }
        public virtual DbSet<ComplaintArise> ComplaintArises { get; set; }
        public virtual DbSet<ComplaintMaster> ComplaintMasters { get; set; }
        public virtual DbSet<Daily_Attendance> Daily_Attendances { get; set; }
        public virtual DbSet<DeviceDetail> DeviceDetails { get; set; }
        public virtual DbSet<DumpYardDetail> DumpYardDetails { get; set; }
        public virtual DbSet<EmpBeatMap> EmpBeatMaps { get; set; }
        public virtual DbSet<EmpShift> EmpShifts { get; set; }
        public virtual DbSet<GameDetail> GameDetails { get; set; }
        public virtual DbSet<GameMaster> GameMasters { get; set; }
        public virtual DbSet<GamePlayerDetail> GamePlayerDetails { get; set; }
        public virtual DbSet<Game_AnswerType> Game_AnswerTypes { get; set; }
        public virtual DbSet<Game_Slogan> Game_Slogans { get; set; }
        public virtual DbSet<GarbageCollectionDetail> GarbageCollectionDetails { get; set; }
        public virtual DbSet<GarbagePointDetail> GarbagePointDetails { get; set; }
        public virtual DbSet<GramCleaningComplient> GramCleaningComplients { get; set; }
        public virtual DbSet<HouseBunch> HouseBunches { get; set; }
        public virtual DbSet<HouseList> HouseLists { get; set; }
        public virtual DbSet<HouseMaster> HouseMasters { get; set; }
        public virtual DbSet<LanguageInfo> LanguageInfos { get; set; }
        public virtual DbSet<LiquidWasteDetail> LiquidWasteDetails { get; set; }
        public virtual DbSet<Location> Locations { get; set; }
        public virtual DbSet<MasterQR> MasterQRs { get; set; }
        public virtual DbSet<MasterQRBunch> MasterQRBunches { get; set; }
        public virtual DbSet<MonthlyAttedance> MonthlyAttedances { get; set; }
        public virtual DbSet<QrEmployeeMaster> QrEmployeeMasters { get; set; }
        public virtual DbSet<Qr_Employee_Daily_Attendance> Qr_Employee_Daily_Attendances { get; set; }
        public virtual DbSet<Qr_Location> Qr_Locations { get; set; }
        public virtual DbSet<SS_1_4_ANSWER> SS_1_4_ANSWERs { get; set; }
        public virtual DbSet<SS_1_4_QUESTION> SS_1_4_QUESTIONs { get; set; }
        public virtual DbSet<SS_1_7_ANSWER> SS_1_7_ANSWERs { get; set; }
        public virtual DbSet<SauchalayAddress> SauchalayAddresses { get; set; }
        public virtual DbSet<StreetSweepingBeat> StreetSweepingBeats { get; set; }
        public virtual DbSet<StreetSweepingDetail> StreetSweepingDetails { get; set; }
        public virtual DbSet<TeritoryMaster> TeritoryMasters { get; set; }
        public virtual DbSet<TransDumpTD> TransDumpTDs { get; set; }
        public virtual DbSet<UserMaster> UserMasters { get; set; }
        public virtual DbSet<VW_HSGetDumpyardDetail> VW_HSGetDumpyardDetails { get; set; }
        public virtual DbSet<VW_HSGetHouseDetail> VW_HSGetHouseDetails { get; set; }
        public virtual DbSet<VW_HSGetLiquidDetail> VW_HSGetLiquidDetails { get; set; }
        public virtual DbSet<VW_HSGetStreetDetail> VW_HSGetStreetDetails { get; set; }
        public virtual DbSet<Vehical_QR_Master> Vehical_QR_Masters { get; set; }
        public virtual DbSet<VehicleRegistration> VehicleRegistrations { get; set; }
        public virtual DbSet<VehicleType> VehicleTypes { get; set; }
        public virtual DbSet<Vw_BitCount> Vw_BitCounts { get; set; }
        public virtual DbSet<Vw_GetHouseNumber> Vw_GetHouseNumbers { get; set; }
        public virtual DbSet<Vw_GetLiquidNumber> Vw_GetLiquidNumbers { get; set; }
        public virtual DbSet<Vw_GetStreetNumber> Vw_GetStreetNumbers { get; set; }
        public virtual DbSet<WM_GarbageCategory> WM_GarbageCategories { get; set; }
        public virtual DbSet<WM_GarbageSubCategory> WM_GarbageSubCategories { get; set; }
        public virtual DbSet<WM_Garbage_Detail> WM_Garbage_Details { get; set; }
        public virtual DbSet<WM_Garbage_Sale> WM_Garbage_Sales { get; set; }
        public virtual DbSet<WM_Garbage_Summary> WM_Garbage_Summaries { get; set; }
        public virtual DbSet<WardNumber> WardNumbers { get; set; }
        public virtual DbSet<ZoneMaster> ZoneMasters { get; set; }
        public virtual DbSet<__MigrationHistory> __MigrationHistories { get; set; }
        public virtual DbSet<questionnaire> questionnaires { get; set; }
        public virtual DbSet<view_LLocation> view_LLocations { get; set; }
        public virtual DbSet<view_Location> view_Locations { get; set; }
        public virtual DbSet<view_SLocation> view_SLocations { get; set; }

        public virtual DbSet<TrailHouse> TrailHouses { get; set; }
        public virtual DbSet<SurveyFormDetail> SurveyFormDetails { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("data source=124.153.94.110;initial catalog=LIVEAdvanceAppynittyGhantaGadi;persist security info=True;user id=sa;password=sa@123;MultipleActiveResultSets=True;App=EntityFramework");
            }
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<TrailHouse>().HasNoKey();

            modelBuilder.Entity<AspNetRole>(entity =>
            {
                entity.Property(e => e.Id).HasMaxLength(128);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(256);
            });

            modelBuilder.Entity<AspNetUser>(entity =>
            {
                entity.Property(e => e.Id).HasMaxLength(128);

                entity.Property(e => e.Email).HasMaxLength(256);

                entity.Property(e => e.LockoutEndDateUtc).HasColumnType("datetime");

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(256);
            });

            modelBuilder.Entity<AspNetUserClaim>(entity =>
            {
                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserClaims)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_dbo.AspNetUserClaims_dbo.AspNetUsers_UserId");
            });

            modelBuilder.Entity<AspNetUserLogin>(entity =>
            {
                entity.HasKey(e => new { e.LoginProvider, e.ProviderKey, e.UserId })
                    .HasName("PK_dbo.AspNetUserLogins");

                entity.Property(e => e.LoginProvider).HasMaxLength(128);

                entity.Property(e => e.ProviderKey).HasMaxLength(128);

                entity.Property(e => e.UserId).HasMaxLength(128);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserLogins)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_dbo.AspNetUserLogins_dbo.AspNetUsers_UserId");
            });

            modelBuilder.Entity<AspNetUserRole>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.RoleId })
                    .HasName("PK_dbo.AspNetUserRoles");

                entity.Property(e => e.UserId).HasMaxLength(128);

                entity.Property(e => e.RoleId).HasMaxLength(128);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AspNetUserRoles)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("FK_dbo.AspNetUserRoles_dbo.AspNetRoles_RoleId");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserRoles)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_dbo.AspNetUserRoles_dbo.AspNetUsers_UserId");
            });

            modelBuilder.Entity<BinLatLong>(entity =>
            {
                entity.ToTable("BinLatLong");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.S1)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.S2)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Temp).HasMaxLength(500);

                entity.Property(e => e._long)
                    .HasMaxLength(500)
                    .HasColumnName("long");

                entity.Property(e => e.lat).HasMaxLength(500);
            });

            modelBuilder.Entity<BuildingShp>(entity =>
            {
                entity.ToTable("BuildingShp");

                entity.Property(e => e._long)
                    .HasMaxLength(500)
                    .HasColumnName("long");

                entity.Property(e => e.lat).HasMaxLength(500);
            });

            modelBuilder.Entity<Buildings_CSV>(entity =>
            {
                entity.ToTable("Buildings_CSV");
            });

            modelBuilder.Entity<ComplaintArise>(entity =>
            {
                entity.HasKey(e => e.CAId);

                entity.ToTable("ComplaintArise");

                entity.Property(e => e.EmployeeType).HasMaxLength(50);

                entity.Property(e => e.PauseDate).HasColumnType("datetime");

                entity.Property(e => e.ResumeDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<ComplaintMaster>(entity =>
            {
                entity.HasKey(e => e.Cid);

                entity.ToTable("ComplaintMaster");

                entity.Property(e => e.Cname).HasMaxLength(500);

                entity.Property(e => e.CreationDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<Daily_Attendance>(entity =>
            {
                entity.HasKey(e => e.daID);

                entity.ToTable("Daily_Attendance");

                entity.Property(e => e.EmployeeType)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.OutbatteryStatus)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.batteryStatus)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.daDate).HasColumnType("date");

                entity.Property(e => e.daEndDate).HasColumnType("date");

                entity.Property(e => e.endLat)
                    .HasMaxLength(10)
                    .IsFixedLength(true);

                entity.Property(e => e.endLong)
                    .HasMaxLength(10)
                    .IsFixedLength(true);

                entity.Property(e => e.endTime).HasMaxLength(200);

                entity.Property(e => e.startLat).HasMaxLength(500);

                entity.Property(e => e.startLong).HasMaxLength(500);

                entity.Property(e => e.startTime).HasMaxLength(200);

                entity.Property(e => e.vehicleNumber).HasMaxLength(50);

                entity.Property(e => e.vtId)
                    .HasMaxLength(10)
                    .IsFixedLength(true);
            });

            modelBuilder.Entity<DeviceDetail>(entity =>
            {
                entity.Property(e => e.DeviceType).HasMaxLength(50);

                entity.Property(e => e.InstallDate).HasColumnType("datetime");

                entity.Property(e => e.ReferenceID).HasMaxLength(50);
            });

            modelBuilder.Entity<DumpYardDetail>(entity =>
            {
                entity.HasKey(e => e.dyId);

                entity.Property(e => e.DataEntryDate).HasColumnType("datetime");

                entity.Property(e => e.EmployeeType)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.QRStatusDate).HasColumnType("datetime");

                entity.Property(e => e.ReferanceId).HasMaxLength(200);

                entity.Property(e => e.dyLat).HasMaxLength(500);

                entity.Property(e => e.dyLong).HasMaxLength(500);

                entity.Property(e => e.dyName).HasMaxLength(200);

                entity.Property(e => e.dyNameMar).HasMaxLength(200);

                entity.Property(e => e.lastModifiedDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<EmpBeatMap>(entity =>
            {
                entity.HasKey(e => e.ebmId)
                    .HasName("PK__EmpBeatM__AF992DDAF73F6286");

                entity.ToTable("EmpBeatMap");

                entity.Property(e => e.Type)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<EmpShift>(entity =>
            {
                entity.HasKey(e => e.shiftId)
                    .HasName("PK__EmpShift__F2F06B02B6870FA7");

                entity.ToTable("EmpShift");

                entity.Property(e => e.shiftEnd).HasMaxLength(50);

                entity.Property(e => e.shiftName).HasMaxLength(50);

                entity.Property(e => e.shiftStart).HasMaxLength(50);
            });

            modelBuilder.Entity<GameDetail>(entity =>
            {
                entity.HasKey(e => e.GameDetailsID);

                entity.Property(e => e.Created).HasColumnType("datetime");

                entity.Property(e => e.ImageUrl).HasMaxLength(500);
            });

            modelBuilder.Entity<GameMaster>(entity =>
            {
                entity.HasKey(e => e.GameId);

                entity.ToTable("GameMaster");

                entity.Property(e => e.GameName).HasMaxLength(100);

                entity.Property(e => e.GameNameMar).HasMaxLength(100);
            });

            modelBuilder.Entity<GamePlayerDetail>(entity =>
            {
                entity.Property(e => e.Created).HasColumnType("datetime");

                entity.Property(e => e.DeviceId).HasMaxLength(500);

                entity.Property(e => e.Mobile).HasMaxLength(50);

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.Property(e => e.PlayerId).HasMaxLength(50);
            });

            modelBuilder.Entity<Game_AnswerType>(entity =>
            {
                entity.HasKey(e => e.AnswerTypeId)
                    .HasName("PK_AnswerType");

                entity.ToTable("Game_AnswerType");

                entity.Property(e => e.AnswerType).HasMaxLength(50);

                entity.Property(e => e.AnswerTypeMar).HasMaxLength(50);
            });

            modelBuilder.Entity<Game_Slogan>(entity =>
            {
                entity.ToTable("Game_Slogan");

                entity.Property(e => e.Slogan).HasMaxLength(500);

                entity.Property(e => e.SloganMar).HasMaxLength(500);
            });

            modelBuilder.Entity<GarbageCollectionDetail>(entity =>
            {
                entity.HasKey(e => e.gcId);

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.EmployeeType)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Lat).HasMaxLength(500);

                entity.Property(e => e.Long).HasMaxLength(500);

                entity.Property(e => e.OutbatteryStatus)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.RFIDReaderId).HasMaxLength(255);

                entity.Property(e => e.RFIDTagId).HasMaxLength(255);

                entity.Property(e => e.WasteType)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.batteryStatus)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.gcDate).HasColumnType("datetime");

                entity.Property(e => e.gpBeforImageTime).HasColumnType("datetime");

                entity.Property(e => e.modified).HasColumnType("datetime");

                entity.Property(e => e.totalDryWeight).HasColumnType("decimal(18, 4)");

                entity.Property(e => e.totalGcWeight).HasColumnType("decimal(18, 4)");

                entity.Property(e => e.totalWetWeight).HasColumnType("decimal(18, 4)");

                entity.Property(e => e.vehicleNumber).HasMaxLength(50);
            });

            modelBuilder.Entity<GarbagePointDetail>(entity =>
            {
                entity.HasKey(e => e.gpId)
                    .HasName("PK_GrabagePointDetails");

                entity.Property(e => e.ReferanceId).HasMaxLength(200);

                entity.Property(e => e.gpLat).HasMaxLength(500);

                entity.Property(e => e.gpLong).HasMaxLength(500);

                entity.Property(e => e.gpName).HasMaxLength(200);

                entity.Property(e => e.gpNameMar).HasMaxLength(200);

                entity.Property(e => e.modified).HasColumnType("datetime");
            });

            modelBuilder.Entity<GramCleaningComplient>(entity =>
            {
                entity.HasKey(e => e.CleaningComplientId)
                    .HasName("PK__DealsOfD__1BC5A871742CCFC0");

                entity.ToTable("GramCleaningComplient");

                entity.Property(e => e.Address).HasMaxLength(550);

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Email).HasMaxLength(200);

                entity.Property(e => e.Image).HasMaxLength(200);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.Number).HasColumnType("numeric(13, 0)");

                entity.Property(e => e.Place).HasMaxLength(50);

                entity.Property(e => e.Tip)
                    .HasMaxLength(200)
                    .IsFixedLength(true);

                entity.Property(e => e.Ward_No_)
                    .HasMaxLength(100)
                    .HasColumnName("Ward_No.");

                entity.Property(e => e.ref_id).HasMaxLength(200);

                entity.Property(e => e.status).HasMaxLength(200);
            });

            modelBuilder.Entity<HouseBunch>(entity =>
            {
                entity.HasKey(e => e.bunchId);

                entity.ToTable("HouseBunch");

                entity.Property(e => e.bunchname).HasMaxLength(500);
            });

            modelBuilder.Entity<HouseList>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("HouseList");

                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.ReferanceId)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<HouseMaster>(entity =>
            {
                entity.HasKey(e => e.houseId)
                    .HasName("PK_housemaster");

                entity.ToTable("HouseMaster");

                entity.Property(e => e.FCMID)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.QRStatusDate).HasColumnType("datetime");

                entity.Property(e => e.RFIDTagId).HasMaxLength(255);

                entity.Property(e => e.ReferanceId).HasMaxLength(350);

                entity.Property(e => e.WasteType)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.houseAddress).HasMaxLength(500);

                entity.Property(e => e.houseLat).HasMaxLength(500);

                entity.Property(e => e.houseLong).HasMaxLength(500);

                entity.Property(e => e.houseNumber).HasMaxLength(500);

                entity.Property(e => e.houseOwner)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.houseOwnerMar).HasMaxLength(200);

                entity.Property(e => e.houseOwnerMobile).HasMaxLength(200);

                entity.Property(e => e.lastModifiedEntry).HasColumnType("datetime");

                entity.Property(e => e.modified).HasColumnType("datetime");
            });

            modelBuilder.Entity<LanguageInfo>(entity =>
            {
                entity.ToTable("LanguageInfo");

                entity.Property(e => e.languageCode)
                    .IsRequired()
                    .HasMaxLength(30);

                entity.Property(e => e.languageType)
                    .IsRequired()
                    .HasMaxLength(30);
            });

            modelBuilder.Entity<LiquidWasteDetail>(entity =>
            {
                entity.HasKey(e => e.LWId);

                entity.Property(e => e.DataEntryDate).HasColumnType("datetime");

                entity.Property(e => e.LWLat).HasMaxLength(500);

                entity.Property(e => e.LWLong).HasMaxLength(500);

                entity.Property(e => e.LWName).HasMaxLength(200);

                entity.Property(e => e.LWNameMar).HasMaxLength(200);

                entity.Property(e => e.QRStatusDate).HasColumnType("datetime");

                entity.Property(e => e.ReferanceId).HasMaxLength(200);

                entity.Property(e => e.lastModifiedDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<Location>(entity =>
            {
                entity.HasKey(e => e.locId);

                entity.ToTable("Location");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Distnace).HasColumnType("decimal(6, 2)");

                entity.Property(e => e.EmployeeType)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.RFIDReaderId).HasMaxLength(255);

                entity.Property(e => e.RFIDTagId).HasMaxLength(255);

                entity.Property(e => e.ReferanceID).HasMaxLength(50);

                entity.Property(e => e._long)
                    .HasMaxLength(500)
                    .HasColumnName("long");

                entity.Property(e => e.area).HasMaxLength(500);

                entity.Property(e => e.batteryStatus)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.datetime).HasColumnType("datetime");

                entity.Property(e => e.lat).HasMaxLength(500);
            });

            modelBuilder.Entity<MasterQR>(entity =>
            {
                entity.HasKey(e => e.MasterId);

                entity.ToTable("MasterQR");

                entity.Property(e => e.ReferanceId).HasMaxLength(350);
            });

            modelBuilder.Entity<MasterQRBunch>(entity =>
            {
                entity.HasKey(e => e.MasterId);

                entity.ToTable("MasterQRBunch");
            });

            modelBuilder.Entity<MonthlyAttedance>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("MonthlyAttedance");

                entity.Property(e => e.EmployeeType)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.ID).ValueGeneratedOnAdd();

                entity.Property(e => e.TOTAL_MONTH_DAYS).HasMaxLength(10);

                entity.Property(e => e.UserName).HasMaxLength(200);
            });

            modelBuilder.Entity<QrEmployeeMaster>(entity =>
            {
                entity.HasKey(e => e.qrEmpId);

                entity.ToTable("QrEmployeeMaster");

                entity.Property(e => e.bloodGroup).HasMaxLength(500);

                entity.Property(e => e.lastModifyDate).HasColumnType("datetime");

                entity.Property(e => e.qrEmpAddress).HasMaxLength(200);

                entity.Property(e => e.qrEmpLoginId).HasMaxLength(200);

                entity.Property(e => e.qrEmpMobileNumber).HasMaxLength(20);

                entity.Property(e => e.qrEmpName).HasMaxLength(500);

                entity.Property(e => e.qrEmpNameMar).HasMaxLength(200);

                entity.Property(e => e.qrEmpPassword).HasMaxLength(100);

                entity.Property(e => e.target)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.type).HasMaxLength(50);

                entity.Property(e => e.userEmployeeNo).HasMaxLength(250);
            });

            modelBuilder.Entity<Qr_Employee_Daily_Attendance>(entity =>
            {
                entity.HasKey(e => e.qrEmpDaId)
                    .HasName("PK_QrEmployeeDailyAttendance");

                entity.ToTable("Qr_Employee_Daily_Attendance");

                entity.Property(e => e.batteryStatus)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.endDate).HasColumnType("date");

                entity.Property(e => e.endLat)
                    .HasMaxLength(10)
                    .IsFixedLength(true);

                entity.Property(e => e.endLong)
                    .HasMaxLength(10)
                    .IsFixedLength(true);

                entity.Property(e => e.endTime).HasMaxLength(200);

                entity.Property(e => e.startDate).HasColumnType("date");

                entity.Property(e => e.startLat).HasMaxLength(500);

                entity.Property(e => e.startLong).HasMaxLength(500);

                entity.Property(e => e.startTime).HasMaxLength(200);
            });

            modelBuilder.Entity<Qr_Location>(entity =>
            {
                entity.HasKey(e => e.locId);

                entity.ToTable("Qr_Location");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Distnace).HasColumnType("decimal(6, 2)");

                entity.Property(e => e.ReferanceID).HasMaxLength(50);

                entity.Property(e => e._long)
                    .HasMaxLength(500)
                    .HasColumnName("long");

                entity.Property(e => e.area).HasMaxLength(500);

                entity.Property(e => e.batteryStatus)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.datetime).HasColumnType("datetime");

                entity.Property(e => e.lat).HasMaxLength(500);
            });

            modelBuilder.Entity<SS_1_4_ANSWER>(entity =>
            {
                entity.HasKey(e => e.ANS_ID);

                entity.ToTable("SS_1.4_ANSWER");

                entity.Property(e => e.INSERT_DATE).HasColumnType("datetime");

                entity.Property(e => e.MARKS)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TOTAL_COUNT)
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<SS_1_4_QUESTION>(entity =>
            {
                entity.HasKey(e => e.Q_ID);

                entity.ToTable("SS_1.4_QUESTION");

                entity.Property(e => e.NOTE)
                    .HasMaxLength(199)
                    .IsUnicode(false);

                entity.Property(e => e.QUESTIOND)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Q_NO)
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<SS_1_7_ANSWER>(entity =>
            {
                entity.HasKey(e => e.Trno);

                entity.ToTable("SS_1.7_ANSWER");

                entity.Property(e => e.INSERT_DATE).HasColumnType("datetime");
            });

            modelBuilder.Entity<SauchalayAddress>(entity =>
            {
                entity.ToTable("SauchalayAddress");

                entity.Property(e => e.Address).HasMaxLength(500);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ImageUrl).HasMaxLength(1000);

                entity.Property(e => e.Lat).HasMaxLength(500);

                entity.Property(e => e.Long).HasMaxLength(500);

                entity.Property(e => e.Mobile).HasMaxLength(200);

                entity.Property(e => e.Name).HasMaxLength(200);

                entity.Property(e => e.QrImageUrl).HasMaxLength(1000);

                entity.Property(e => e.SauchalayID).HasMaxLength(100);
            });

            modelBuilder.Entity<StreetSweepingBeat>(entity =>
            {
                entity.HasKey(e => e.BeatId);

                entity.ToTable("StreetSweepingBeat");

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.ReferanceId1).HasMaxLength(200);

                entity.Property(e => e.ReferanceId2).HasMaxLength(200);

                entity.Property(e => e.ReferanceId3).HasMaxLength(200);

                entity.Property(e => e.ReferanceId4).HasMaxLength(200);

                entity.Property(e => e.ReferanceId5).HasMaxLength(200);
            });

            modelBuilder.Entity<StreetSweepingDetail>(entity =>
            {
                entity.HasKey(e => e.SSId);

                entity.Property(e => e.DataEntryDate).HasColumnType("datetime");

                entity.Property(e => e.QRStatusDate).HasColumnType("datetime");

                entity.Property(e => e.ReferanceId).HasMaxLength(200);

                entity.Property(e => e.SSLat).HasMaxLength(500);

                entity.Property(e => e.SSLong).HasMaxLength(500);

                entity.Property(e => e.SSName).HasMaxLength(200);

                entity.Property(e => e.SSNameMar).HasMaxLength(200);

                entity.Property(e => e.lastModifiedDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<TeritoryMaster>(entity =>
            {
                entity.ToTable("TeritoryMaster");

                entity.Property(e => e.Area).HasMaxLength(250);

                entity.Property(e => e.AreaMar).HasMaxLength(50);
            });

            modelBuilder.Entity<TransDumpTD>(entity =>
            {
                entity.HasKey(e => e.TransBcId)
                    .HasName("PK__TransDum__46195E11E052C142");

                entity.ToTable("TransDumpTD");

                entity.Property(e => e.TotalDryWeightKg).HasColumnType("decimal(18, 10)");

                entity.Property(e => e.TotalGcWeightKg).HasColumnType("decimal(18, 10)");

                entity.Property(e => e.TotalWetWeightKg).HasColumnType("decimal(18, 10)");

                entity.Property(e => e.UsTotalDryWeight).HasColumnType("decimal(18, 4)");

                entity.Property(e => e.UsTotalGcWeight).HasColumnType("decimal(18, 4)");

                entity.Property(e => e.UsTotalWetWeight).HasColumnType("decimal(18, 4)");

                entity.Property(e => e.bcTransId).HasMaxLength(500);

                entity.Property(e => e.dyId).HasMaxLength(100);

                entity.Property(e => e.endDateTime).HasColumnType("datetime");

                entity.Property(e => e.startDateTime).HasColumnType("datetime");

                entity.Property(e => e.totalDryWeight).HasColumnType("decimal(18, 4)");

                entity.Property(e => e.totalGcWeight).HasColumnType("decimal(18, 4)");

                entity.Property(e => e.totalWetWeight).HasColumnType("decimal(18, 4)");

                entity.Property(e => e.transId).HasMaxLength(500);

                entity.Property(e => e.vehicleNumber).HasMaxLength(100);
            });

            modelBuilder.Entity<UserMaster>(entity =>
            {
                entity.HasKey(e => e.userId);

                entity.ToTable("UserMaster");

                entity.Property(e => e.EmployeeType)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Type).HasMaxLength(50);

                entity.Property(e => e.bloodGroup).HasMaxLength(250);

                entity.Property(e => e.gcTarget)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.shiftIds).HasMaxLength(500);

                entity.Property(e => e.userAddress).HasMaxLength(200);

                entity.Property(e => e.userEmployeeNo).HasMaxLength(250);

                entity.Property(e => e.userLoginId).HasMaxLength(200);

                entity.Property(e => e.userMobileNumber).HasMaxLength(20);

                entity.Property(e => e.userName).HasMaxLength(500);

                entity.Property(e => e.userNameMar).HasMaxLength(200);

                entity.Property(e => e.userPassword).HasMaxLength(100);

                entity.Property(e => e.userProfileImage).HasMaxLength(500);
            });

            modelBuilder.Entity<VW_HSGetDumpyardDetail>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("VW_HSGetDumpyardDetails");

                entity.Property(e => e.QRStatusDate).HasColumnType("datetime");

                entity.Property(e => e.ReferanceId).HasMaxLength(200);

                entity.Property(e => e.dyLat).HasMaxLength(500);

                entity.Property(e => e.dyLong).HasMaxLength(500);

                entity.Property(e => e.dyName).HasMaxLength(200);

                entity.Property(e => e.lastModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.qrEmpName).HasMaxLength(500);
            });

            modelBuilder.Entity<VW_HSGetHouseDetail>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("VW_HSGetHouseDetails");

                entity.Property(e => e.QRStatusDate).HasColumnType("datetime");

                entity.Property(e => e.ReferanceId).HasMaxLength(350);

                entity.Property(e => e.houseLat).HasMaxLength(500);

                entity.Property(e => e.houseLong).HasMaxLength(500);

                entity.Property(e => e.houseOwner)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.modified).HasColumnType("datetime");

                entity.Property(e => e.qrEmpName).HasMaxLength(500);
            });

            modelBuilder.Entity<VW_HSGetLiquidDetail>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("VW_HSGetLiquidDetails");

                entity.Property(e => e.LWLat).HasMaxLength(500);

                entity.Property(e => e.LWLong).HasMaxLength(500);

                entity.Property(e => e.LWName).HasMaxLength(200);

                entity.Property(e => e.QRStatusDate).HasColumnType("datetime");

                entity.Property(e => e.ReferanceId).HasMaxLength(200);

                entity.Property(e => e.lastModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.qrEmpName).HasMaxLength(500);
            });

            modelBuilder.Entity<VW_HSGetStreetDetail>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("VW_HSGetStreetDetails");

                entity.Property(e => e.QRStatusDate).HasColumnType("datetime");

                entity.Property(e => e.ReferanceId).HasMaxLength(200);

                entity.Property(e => e.SSLat).HasMaxLength(500);

                entity.Property(e => e.SSLong).HasMaxLength(500);

                entity.Property(e => e.SSName).HasMaxLength(200);

                entity.Property(e => e.lastModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.qrEmpName).HasMaxLength(500);
            });

            modelBuilder.Entity<Vehical_QR_Master>(entity =>
            {
                entity.HasKey(e => e.vqrId);

                entity.ToTable("Vehical_QR_Master");

                entity.Property(e => e.FCMID)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.RFIDTagId).HasMaxLength(255);

                entity.Property(e => e.ReferanceId).HasMaxLength(350);

                entity.Property(e => e.VehicalNumber).HasMaxLength(500);

                entity.Property(e => e.VehicalType)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.lastModifiedEntry).HasColumnType("datetime");

                entity.Property(e => e.modified).HasColumnType("datetime");
            });

            modelBuilder.Entity<VehicleRegistration>(entity =>
            {
                entity.HasKey(e => e.vehicleId)
                    .HasName("PK__VehicleR__5B9D25F22229CD81");

                entity.ToTable("VehicleRegistration");

                entity.Property(e => e.vehicleNo).HasMaxLength(300);
            });

            modelBuilder.Entity<VehicleType>(entity =>
            {
                entity.HasKey(e => e.vtId);

                entity.ToTable("VehicleType");

                entity.Property(e => e.description).HasMaxLength(300);

                entity.Property(e => e.descriptionMar).HasMaxLength(300);
            });

            modelBuilder.Entity<Vw_BitCount>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("Vw_BitCount");

                entity.Property(e => e.BeatId).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<Vw_GetHouseNumber>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("Vw_GetHouseNumber");

                entity.Property(e => e.ReferanceId).HasMaxLength(350);

                entity.Property(e => e.houseNumber).HasMaxLength(500);
            });

            modelBuilder.Entity<Vw_GetLiquidNumber>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("Vw_GetLiquidNumber");

                entity.Property(e => e.ReferanceId).HasMaxLength(200);
            });

            modelBuilder.Entity<Vw_GetStreetNumber>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("Vw_GetStreetNumber");

                entity.Property(e => e.ReferanceId).HasMaxLength(200);
            });

            modelBuilder.Entity<WM_GarbageCategory>(entity =>
            {
                entity.HasKey(e => e.CategoryID);

                entity.ToTable("WM_GarbageCategory");

                entity.Property(e => e.Category).HasMaxLength(50);
            });

            modelBuilder.Entity<WM_GarbageSubCategory>(entity =>
            {
                entity.HasKey(e => e.SubCategoryID);

                entity.ToTable("WM_GarbageSubCategory");

                entity.Property(e => e.SubCategory).HasMaxLength(50);
            });

            modelBuilder.Entity<WM_Garbage_Detail>(entity =>
            {
                entity.HasKey(e => e.GarbageDetailsID);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Weight).HasColumnType("decimal(18, 4)");

                entity.Property(e => e.gdDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<WM_Garbage_Sale>(entity =>
            {
                entity.Property(e => e.Amount).HasColumnType("decimal(18, 4)");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.PartyName).HasMaxLength(500);

                entity.Property(e => e.SalesWeight).HasColumnType("decimal(18, 4)");
            });

            modelBuilder.Entity<WM_Garbage_Summary>(entity =>
            {
                entity.ToTable("WM_Garbage_Summary");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Price).HasColumnType("decimal(18, 4)");

                entity.Property(e => e.TotalWeight).HasColumnType("decimal(18, 4)");
            });

            modelBuilder.Entity<WardNumber>(entity =>
            {
                entity.ToTable("WardNumber");

                entity.Property(e => e.WardNo).HasMaxLength(50);
            });

            modelBuilder.Entity<ZoneMaster>(entity =>
            {
                entity.HasKey(e => e.zoneId);

                entity.ToTable("ZoneMaster");

                entity.Property(e => e.name).HasMaxLength(500);
            });

            modelBuilder.Entity<__MigrationHistory>(entity =>
            {
                entity.HasKey(e => new { e.MigrationId, e.ContextKey })
                    .HasName("PK_dbo.__MigrationHistory");

                entity.ToTable("__MigrationHistory");

                entity.Property(e => e.MigrationId).HasMaxLength(150);

                entity.Property(e => e.ContextKey).HasMaxLength(300);

                entity.Property(e => e.Model).IsRequired();

                entity.Property(e => e.ProductVersion)
                    .IsRequired()
                    .HasMaxLength(32);
            });

            modelBuilder.Entity<questionnaire>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("questionnaire");

                entity.Property(e => e.Q_Number).HasColumnName("Q Number");

                entity.Property(e => e.Q_Response)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("Q Response");

                entity.Property(e => e.Q_Text)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("Q Text");
            });

            modelBuilder.Entity<view_LLocation>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("view_LLocation");

                entity.Property(e => e.Distnace).HasColumnType("decimal(6, 2)");

                entity.Property(e => e.LocDate).HasColumnType("date");
            });

            modelBuilder.Entity<view_Location>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("view_Location");

                entity.Property(e => e.Distnace).HasColumnType("decimal(6, 2)");

                entity.Property(e => e.LocDate).HasColumnType("date");
            });

            modelBuilder.Entity<view_SLocation>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("view_SLocation");

                entity.Property(e => e.Distnace).HasColumnType("decimal(6, 2)");

                entity.Property(e => e.LocDate).HasColumnType("date");
            });

            modelBuilder.Entity<SurveyFormDetail>(entity =>
            {
                entity.Property(e => e.HouseId).ValueGeneratedNever();

                entity.Property(e => e.SvId).ValueGeneratedOnAdd();
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
