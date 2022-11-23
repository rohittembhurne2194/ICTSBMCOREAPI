using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace ICTSBMCOREAPI.Dal.DataContexts.Models.DB.ChildModels
{
    public partial class DevSwachhBharatNagpurEntities : DbContext
    {
        private int AppId;
        public DevSwachhBharatNagpurEntities(int AppId)
        {
            this.AppId = AppId;
        }
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
        public virtual DbSet<DailyAttendance> DailyAttendances { get; set; }
        public virtual DbSet<DeviceDetail> DeviceDetails { get; set; }
        public virtual DbSet<DumpYardDetail> DumpYardDetails { get; set; }
        public virtual DbSet<EmpBeatMap> EmpBeatMaps { get; set; }
        public virtual DbSet<EmpShift> EmpShifts { get; set; }
        public virtual DbSet<GameAnswerType> GameAnswerTypes { get; set; }
        public virtual DbSet<GameDetail> GameDetails { get; set; }
        public virtual DbSet<GameMaster> GameMasters { get; set; }
        public virtual DbSet<GamePlayerDetail> GamePlayerDetails { get; set; }
        public virtual DbSet<GameSlogan> GameSlogans { get; set; }
        public virtual DbSet<GarbageCollectionDetail> GarbageCollectionDetails { get; set; }
        public virtual DbSet<GarbagePointDetail> GarbagePointDetails { get; set; }
        public virtual DbSet<GramCleaningComplient> GramCleaningComplients { get; set; }
        public virtual DbSet<HouseBunch> HouseBunches { get; set; }
        public virtual DbSet<HouseList> HouseLists { get; set; }
        public virtual DbSet<HouseMaster> HouseMasters { get; set; }
        public virtual DbSet<LanguageInfo> LanguageInfos { get; set; }
        public virtual DbSet<LiquidWasteDetail> LiquidWasteDetails { get; set; }
        public virtual DbSet<Location> Locations { get; set; }
        public virtual DbSet<MasterQrbunch> MasterQrbunches { get; set; }
        public virtual DbSet<MigrationHistory> MigrationHistories { get; set; }
        public virtual DbSet<MonthlyAttedance> MonthlyAttedances { get; set; }
        public virtual DbSet<QrEmployeeDailyAttendance> QrEmployeeDailyAttendances { get; set; }
        public virtual DbSet<QrEmployeeMaster> QrEmployeeMasters { get; set; }
        public virtual DbSet<QrLocation> QrLocations { get; set; }
        public virtual DbSet<Questionnaire> Questionnaires { get; set; }
        public virtual DbSet<SauchalayAddress> SauchalayAddresses { get; set; }
        public virtual DbSet<Ss14Answer> Ss14Answers { get; set; }
        public virtual DbSet<Ss14Question> Ss14Questions { get; set; }
        public virtual DbSet<Ss17Answer> Ss17Answers { get; set; }
        public virtual DbSet<StreetSweepingBeat> StreetSweepingBeats { get; set; }
        public virtual DbSet<StreetSweepingDetail> StreetSweepingDetails { get; set; }
        public virtual DbSet<TeritoryMaster> TeritoryMasters { get; set; }
        public virtual DbSet<UserMaster> UserMasters { get; set; }
        public virtual DbSet<VehicalQrMaster> VehicalQrMasters { get; set; }
        public virtual DbSet<VehicleType> VehicleTypes { get; set; }
        public virtual DbSet<ViewLlocation> ViewLlocations { get; set; }
        public virtual DbSet<ViewLocation> ViewLocations { get; set; }
        public virtual DbSet<ViewSlocation> ViewSlocations { get; set; }
        public virtual DbSet<VwBitCount> VwBitCounts { get; set; }
        public virtual DbSet<VwGetHouseNumber> VwGetHouseNumbers { get; set; }
        public virtual DbSet<VwGetLiquidNumber> VwGetLiquidNumbers { get; set; }
        public virtual DbSet<VwGetStreetNumber> VwGetStreetNumbers { get; set; }
        public virtual DbSet<VwHsgetDumpyardDetail> VwHsgetDumpyardDetails { get; set; }
        public virtual DbSet<VwHsgetHouseDetail> VwHsgetHouseDetails { get; set; }
        public virtual DbSet<VwHsgetLiquidDetail> VwHsgetLiquidDetails { get; set; }
        public virtual DbSet<VwHsgetStreetDetail> VwHsgetStreetDetails { get; set; }
        public virtual DbSet<WardNumber> WardNumbers { get; set; }
        public virtual DbSet<WmGarbageCategory> WmGarbageCategories { get; set; }
        public virtual DbSet<WmGarbageDetail> WmGarbageDetails { get; set; }
        public virtual DbSet<WmGarbageSale> WmGarbageSales { get; set; }
        public virtual DbSet<WmGarbageSubCategory> WmGarbageSubCategories { get; set; }
        public virtual DbSet<WmGarbageSummary> WmGarbageSummaries { get; set; }
        public virtual DbSet<ZoneMaster> ZoneMasters { get; set; }

        //        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //        {
        //            if (!optionsBuilder.IsConfigured)
        //            {
        //#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        //                optionsBuilder.UseSqlServer("data source=202.65.157.253;initial catalog=LIVEAdvanceEtapalliGhantaGadi;persist security info=True;user id=appynitty;password=BigV$Telecom;MultipleActiveResultSets=True;App=EntityFramework");
        //            }
        //        }


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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

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

                entity.Property(e => e.Lat)
                    .HasMaxLength(500)
                    .HasColumnName("lat");

                entity.Property(e => e.Long)
                    .HasMaxLength(500)
                    .HasColumnName("long");

                entity.Property(e => e.S1)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.S2)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Temp).HasMaxLength(500);
            });

            modelBuilder.Entity<DailyAttendance>(entity =>
            {
                entity.HasKey(e => e.DaId);

                entity.ToTable("Daily_Attendance");

                entity.Property(e => e.DaId).HasColumnName("daID");

                entity.Property(e => e.BatteryStatus)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("batteryStatus");

                entity.Property(e => e.DaDate)
                    .HasColumnType("date")
                    .HasColumnName("daDate");

                entity.Property(e => e.DaEndDate)
                    .HasColumnType("date")
                    .HasColumnName("daEndDate");

                entity.Property(e => e.DaEndNote).HasColumnName("daEndNote");

                entity.Property(e => e.DaStartNote).HasColumnName("daStartNote");

                entity.Property(e => e.Dyid).HasColumnName("dyid");

                entity.Property(e => e.EmployeeType)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.EndLat)
                    .HasMaxLength(10)
                    .HasColumnName("endLat")
                    .IsFixedLength(true);

                entity.Property(e => e.EndLong)
                    .HasMaxLength(10)
                    .HasColumnName("endLong")
                    .IsFixedLength(true);

                entity.Property(e => e.EndTime)
                    .HasMaxLength(200)
                    .HasColumnName("endTime");

                entity.Property(e => e.OutbatteryStatus)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.StartLat)
                    .HasMaxLength(500)
                    .HasColumnName("startLat");

                entity.Property(e => e.StartLong)
                    .HasMaxLength(500)
                    .HasColumnName("startLong");

                entity.Property(e => e.StartTime)
                    .HasMaxLength(200)
                    .HasColumnName("startTime");

                entity.Property(e => e.TotalKm).HasColumnName("totalKm");

                entity.Property(e => e.UserId).HasColumnName("userId");

                entity.Property(e => e.VehicleNumber)
                    .HasMaxLength(50)
                    .HasColumnName("vehicleNumber");

                entity.Property(e => e.Vqrid).HasColumnName("VQRID");

                entity.Property(e => e.VtId)
                    .HasMaxLength(10)
                    .HasColumnName("vtId")
                    .IsFixedLength(true);
            });

            modelBuilder.Entity<DeviceDetail>(entity =>
            {
                entity.Property(e => e.DeviceId).HasColumnName("DeviceID");

                entity.Property(e => e.DeviceType).HasMaxLength(50);

                entity.Property(e => e.Fcmid).HasColumnName("FCMID");

                entity.Property(e => e.InstallDate).HasColumnType("datetime");

                entity.Property(e => e.ReferenceId)
                    .HasMaxLength(50)
                    .HasColumnName("ReferenceID");
            });

            modelBuilder.Entity<DumpYardDetail>(entity =>
            {
                entity.HasKey(e => e.DyId);

                entity.Property(e => e.DyId).HasColumnName("dyId");

                entity.Property(e => e.AreaId).HasColumnName("areaId");

                entity.Property(e => e.DataEntryDate).HasColumnType("datetime");

                entity.Property(e => e.DyAddress).HasColumnName("dyAddress");

                entity.Property(e => e.DyLat)
                    .HasMaxLength(500)
                    .HasColumnName("dyLat");

                entity.Property(e => e.DyLong)
                    .HasMaxLength(500)
                    .HasColumnName("dyLong");

                entity.Property(e => e.DyName)
                    .HasMaxLength(200)
                    .HasColumnName("dyName");

                entity.Property(e => e.DyNameMar)
                    .HasMaxLength(200)
                    .HasColumnName("dyNameMar");

                entity.Property(e => e.DyQrcode).HasColumnName("dyQRCode");

                entity.Property(e => e.EmployeeType)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.LastModifiedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("lastModifiedDate");

                entity.Property(e => e.QrcodeImage).HasColumnName("QRCodeImage");

                entity.Property(e => e.Qrstatus).HasColumnName("QRStatus");

                entity.Property(e => e.QrstatusDate)
                    .HasColumnType("datetime")
                    .HasColumnName("QRStatusDate");

                entity.Property(e => e.ReferanceId).HasMaxLength(200);

                entity.Property(e => e.UserId).HasColumnName("userId");

                entity.Property(e => e.WardId).HasColumnName("wardId");

                entity.Property(e => e.ZoneId).HasColumnName("zoneId");
            });

            modelBuilder.Entity<EmpBeatMap>(entity =>
            {
                entity.HasKey(e => e.EbmId)
                    .HasName("PK__EmpBeatM__AF992DDA35B515D5");

                entity.ToTable("EmpBeatMap");

                entity.Property(e => e.EbmId).HasColumnName("ebmId");

                entity.Property(e => e.EbmLatLong).HasColumnName("ebmLatLong");

                entity.Property(e => e.Type)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UserId).HasColumnName("userId");
            });

            modelBuilder.Entity<EmpShift>(entity =>
            {
                entity.HasKey(e => e.ShiftId)
                    .HasName("PK__EmpShift__F2F06B02445EEC64");

                entity.ToTable("EmpShift");

                entity.Property(e => e.ShiftId).HasColumnName("shiftId");

                entity.Property(e => e.ShiftEnd)
                    .HasMaxLength(50)
                    .HasColumnName("shiftEnd");

                entity.Property(e => e.ShiftName)
                    .HasMaxLength(50)
                    .HasColumnName("shiftName");

                entity.Property(e => e.ShiftStart)
                    .HasMaxLength(50)
                    .HasColumnName("shiftStart");
            });

            modelBuilder.Entity<GameAnswerType>(entity =>
            {
                entity.HasKey(e => e.AnswerTypeId)
                    .HasName("PK_AnswerType");

                entity.ToTable("Game_AnswerType");

                entity.Property(e => e.AnswerType).HasMaxLength(50);

                entity.Property(e => e.AnswerTypeMar).HasMaxLength(50);
            });

            modelBuilder.Entity<GameDetail>(entity =>
            {
                entity.HasKey(e => e.GameDetailsId);

                entity.Property(e => e.GameDetailsId).HasColumnName("GameDetailsID");

                entity.Property(e => e.Created).HasColumnType("datetime");

                entity.Property(e => e.GameMasterId).HasColumnName("GameMasterID");

                entity.Property(e => e.ImageUrl).HasMaxLength(500);

                entity.Property(e => e.RightAnswerId).HasColumnName("RightAnswerID");

                entity.Property(e => e.SloganId).HasColumnName("SloganID");
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
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Created).HasColumnType("datetime");

                entity.Property(e => e.DeviceId).HasMaxLength(500);

                entity.Property(e => e.Mobile).HasMaxLength(50);

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.Property(e => e.PlayerId).HasMaxLength(50);
            });

            modelBuilder.Entity<GameSlogan>(entity =>
            {
                entity.ToTable("Game_Slogan");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Slogan).HasMaxLength(500);

                entity.Property(e => e.SloganMar).HasMaxLength(500);
            });

            modelBuilder.Entity<GarbageCollectionDetail>(entity =>
            {
                entity.HasKey(e => e.GcId);

                entity.Property(e => e.GcId).HasColumnName("gcId");

                entity.Property(e => e.BatteryStatus)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("batteryStatus");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DyId).HasColumnName("dyId");

                entity.Property(e => e.EmployeeType)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.GarbageType).HasColumnName("garbageType");

                entity.Property(e => e.GcDate)
                    .HasColumnType("datetime")
                    .HasColumnName("gcDate");

                entity.Property(e => e.GcImage).HasColumnName("gcImage");

                entity.Property(e => e.GcQrcode).HasColumnName("gcQRcode");

                entity.Property(e => e.GcType).HasColumnName("gcType");

                entity.Property(e => e.GpAfterImage).HasColumnName("gpAfterImage");

                entity.Property(e => e.GpBeforImage).HasColumnName("gpBeforImage");

                entity.Property(e => e.GpBeforImageTime)
                    .HasColumnType("datetime")
                    .HasColumnName("gpBeforImageTime");

                entity.Property(e => e.GpId).HasColumnName("gpId");

                entity.Property(e => e.HouseId).HasColumnName("houseId");

                entity.Property(e => e.Lat).HasMaxLength(500);

                entity.Property(e => e.LocAddresss).HasColumnName("locAddresss");

                entity.Property(e => e.Long).HasMaxLength(500);

                entity.Property(e => e.Lwid).HasColumnName("LWId");

                entity.Property(e => e.Modified)
                    .HasColumnType("datetime")
                    .HasColumnName("modified");

                entity.Property(e => e.Note).HasColumnName("note");

                entity.Property(e => e.OutbatteryStatus)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.RfidreaderId)
                    .HasMaxLength(255)
                    .HasColumnName("RFIDReaderId");

                entity.Property(e => e.RfidtagId)
                    .HasMaxLength(255)
                    .HasColumnName("RFIDTagId");

                entity.Property(e => e.Ssid).HasColumnName("SSId");

                entity.Property(e => e.TotalDryWeight)
                    .HasColumnType("decimal(18, 4)")
                    .HasColumnName("totalDryWeight");

                entity.Property(e => e.TotalGcWeight)
                    .HasColumnType("decimal(18, 4)")
                    .HasColumnName("totalGcWeight");

                entity.Property(e => e.TotalWetWeight)
                    .HasColumnType("decimal(18, 4)")
                    .HasColumnName("totalWetWeight");

                entity.Property(e => e.UserId).HasColumnName("userId");

                entity.Property(e => e.VehicleNumber)
                    .HasMaxLength(50)
                    .HasColumnName("vehicleNumber");

                entity.Property(e => e.Vqrid).HasColumnName("vqrid");

                entity.Property(e => e.WasteType)
                    .HasMaxLength(10)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<GarbagePointDetail>(entity =>
            {
                entity.HasKey(e => e.GpId)
                    .HasName("PK_GrabagePointDetails");

                entity.Property(e => e.GpId).HasColumnName("gpId");

                entity.Property(e => e.AreaId).HasColumnName("areaId");

                entity.Property(e => e.GpAddress).HasColumnName("gpAddress");

                entity.Property(e => e.GpLat)
                    .HasMaxLength(500)
                    .HasColumnName("gpLat");

                entity.Property(e => e.GpLong)
                    .HasMaxLength(500)
                    .HasColumnName("gpLong");

                entity.Property(e => e.GpName)
                    .HasMaxLength(200)
                    .HasColumnName("gpName");

                entity.Property(e => e.GpNameMar)
                    .HasMaxLength(200)
                    .HasColumnName("gpNameMar");

                entity.Property(e => e.Modified)
                    .HasColumnType("datetime")
                    .HasColumnName("modified");

                entity.Property(e => e.QrCode).HasColumnName("qrCode");

                entity.Property(e => e.ReferanceId).HasMaxLength(200);

                entity.Property(e => e.UserId).HasColumnName("userId");

                entity.Property(e => e.WardId).HasColumnName("wardId");

                entity.Property(e => e.ZoneId).HasColumnName("zoneId");
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

                entity.Property(e => e.LanguageId).HasColumnName("languageId");

                entity.Property(e => e.LatLog).HasColumnName("Lat_Log");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.Number).HasColumnType("numeric(13, 0)");

                entity.Property(e => e.Place).HasMaxLength(50);

                entity.Property(e => e.RefId)
                    .HasMaxLength(200)
                    .HasColumnName("ref_id");

                entity.Property(e => e.Status)
                    .HasMaxLength(200)
                    .HasColumnName("status");

                entity.Property(e => e.StatusDescription).HasColumnName("status_description");

                entity.Property(e => e.StatusImageUrl).HasColumnName("status_image_url");

                entity.Property(e => e.Tip)
                    .HasMaxLength(200)
                    .IsFixedLength(true);

                entity.Property(e => e.UserId).HasColumnName("userId");

                entity.Property(e => e.WardNo)
                    .HasMaxLength(100)
                    .HasColumnName("Ward_No.");
            });

            modelBuilder.Entity<HouseBunch>(entity =>
            {
                entity.HasKey(e => e.BunchId);

                entity.ToTable("HouseBunch");

                entity.Property(e => e.BunchId).HasColumnName("bunchId");

                entity.Property(e => e.Bunchname)
                    .HasMaxLength(500)
                    .HasColumnName("bunchname");
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
                entity.HasKey(e => e.HouseId);

                entity.ToTable("HouseMaster");

                entity.Property(e => e.HouseId).HasColumnName("houseId");

                entity.Property(e => e.Fcmid)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("FCMID");

                entity.Property(e => e.HouseAddress)
                    .HasMaxLength(500)
                    .HasColumnName("houseAddress");

                entity.Property(e => e.HouseLat)
                    .HasMaxLength(500)
                    .HasColumnName("houseLat");

                entity.Property(e => e.HouseLong)
                    .HasMaxLength(500)
                    .HasColumnName("houseLong");

                entity.Property(e => e.HouseNumber)
                    .HasMaxLength(500)
                    .HasColumnName("houseNumber");

                entity.Property(e => e.HouseOwner)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("houseOwner");

                entity.Property(e => e.HouseOwnerMar)
                    .HasMaxLength(200)
                    .HasColumnName("houseOwnerMar");

                entity.Property(e => e.HouseOwnerMobile)
                    .HasMaxLength(200)
                    .HasColumnName("houseOwnerMobile");

                entity.Property(e => e.HouseQrcode).HasColumnName("houseQRCode");

                entity.Property(e => e.LastModifiedEntry)
                    .HasColumnType("datetime")
                    .HasColumnName("lastModifiedEntry");

                entity.Property(e => e.Modified)
                    .HasColumnType("datetime")
                    .HasColumnName("modified");

                entity.Property(e => e.PropertyType).HasColumnName("Property_Type");

                entity.Property(e => e.QrcodeImage).HasColumnName("QRCodeImage");

                entity.Property(e => e.Qrstatus).HasColumnName("QRStatus");

                entity.Property(e => e.QrstatusDate)
                    .HasColumnType("datetime")
                    .HasColumnName("QRStatusDate");

                entity.Property(e => e.ReferanceId).HasMaxLength(350);

                entity.Property(e => e.RfidtagId)
                    .HasMaxLength(255)
                    .HasColumnName("RFIDTagId");

                entity.Property(e => e.UserId).HasColumnName("userId");

                entity.Property(e => e.WasteType)
                    .HasMaxLength(10)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<LanguageInfo>(entity =>
            {
                entity.ToTable("LanguageInfo");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.LanguageCode)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("languageCode");

                entity.Property(e => e.LanguageType)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("languageType");
            });

            modelBuilder.Entity<LiquidWasteDetail>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.AreaId).HasColumnName("areaId");

                entity.Property(e => e.DataEntryDate).HasColumnType("datetime");

                entity.Property(e => e.LastModifiedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("lastModifiedDate");

                entity.Property(e => e.LwaddreLw).HasColumnName("LWAddreLW");

                entity.Property(e => e.Lwid)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("LWId");

                entity.Property(e => e.Lwlat)
                    .HasMaxLength(500)
                    .HasColumnName("LWLat");

                entity.Property(e => e.Lwlong)
                    .HasMaxLength(500)
                    .HasColumnName("LWLong");

                entity.Property(e => e.Lwname)
                    .HasMaxLength(200)
                    .HasColumnName("LWName");

                entity.Property(e => e.LwnameMar)
                    .HasMaxLength(200)
                    .HasColumnName("LWNameMar");

                entity.Property(e => e.Lwqrcode).HasColumnName("LWQRCode");

                entity.Property(e => e.QrcodeImage).HasColumnName("QRCodeImage");

                entity.Property(e => e.Qrstatus).HasColumnName("QRStatus");

                entity.Property(e => e.QrstatusDate)
                    .HasColumnType("datetime")
                    .HasColumnName("QRStatusDate");

                entity.Property(e => e.ReferanceId).HasMaxLength(200);

                entity.Property(e => e.UserId).HasColumnName("userId");

                entity.Property(e => e.WardId).HasColumnName("wardId");

                entity.Property(e => e.ZoneId).HasColumnName("zoneId");
            });

            modelBuilder.Entity<Location>(entity =>
            {
                entity.HasKey(e => e.LocId);

                entity.ToTable("Location");

                entity.Property(e => e.LocId).HasColumnName("locId");

                entity.Property(e => e.Address).HasColumnName("address");

                entity.Property(e => e.Area)
                    .HasMaxLength(500)
                    .HasColumnName("area");

                entity.Property(e => e.BatteryStatus)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("batteryStatus");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Datetime)
                    .HasColumnType("datetime")
                    .HasColumnName("datetime");

                entity.Property(e => e.Distnace).HasColumnType("decimal(6, 2)");

                entity.Property(e => e.EmployeeType)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Lat)
                    .HasMaxLength(500)
                    .HasColumnName("lat");

                entity.Property(e => e.Long)
                    .HasMaxLength(500)
                    .HasColumnName("long");

                entity.Property(e => e.ReferanceId)
                    .HasMaxLength(50)
                    .HasColumnName("ReferanceID");

                entity.Property(e => e.RfidreaderId)
                    .HasMaxLength(255)
                    .HasColumnName("RFIDReaderId");

                entity.Property(e => e.RfidtagId)
                    .HasMaxLength(255)
                    .HasColumnName("RFIDTagId");

                entity.Property(e => e.Type).HasColumnName("type");

                entity.Property(e => e.UserId).HasColumnName("userId");
            });

            modelBuilder.Entity<MasterQrbunch>(entity =>
            {
                entity.HasKey(e => e.MasterId);

                entity.ToTable("MasterQRBunch");

                entity.Property(e => e.Isactive).HasColumnName("ISActive");

                entity.Property(e => e.Qrlist).HasColumnName("QRList");
            });

            modelBuilder.Entity<MigrationHistory>(entity =>
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

            modelBuilder.Entity<MonthlyAttedance>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("MonthlyAttedance");

                entity.Property(e => e.ACount).HasColumnName("A_Count");

                entity.Property(e => e.EmployeeType)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.HCount).HasColumnName("H_Count");

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");

                entity.Property(e => e.MonthName).HasColumnName("Month_name");

                entity.Property(e => e.PCount).HasColumnName("P_Count");

                entity.Property(e => e.TotalMonthDays)
                    .HasMaxLength(10)
                    .HasColumnName("TOTAL_MONTH_DAYS");

                entity.Property(e => e.UserId).HasColumnName("userId");

                entity.Property(e => e.UserName).HasMaxLength(200);

                entity.Property(e => e.YearName).HasColumnName("YEAR_NAME");
            });

            modelBuilder.Entity<QrEmployeeDailyAttendance>(entity =>
            {
                entity.HasKey(e => e.QrEmpDaId)
                    .HasName("PK_QrEmployeeDailyAttendance");

                entity.ToTable("Qr_Employee_Daily_Attendance");

                entity.Property(e => e.QrEmpDaId).HasColumnName("qrEmpDaId");

                entity.Property(e => e.BatteryStatus)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("batteryStatus");

                entity.Property(e => e.EndDate)
                    .HasColumnType("date")
                    .HasColumnName("endDate");

                entity.Property(e => e.EndLat)
                    .HasMaxLength(10)
                    .HasColumnName("endLat")
                    .IsFixedLength(true);

                entity.Property(e => e.EndLong)
                    .HasMaxLength(10)
                    .HasColumnName("endLong")
                    .IsFixedLength(true);

                entity.Property(e => e.EndNote).HasColumnName("endNote");

                entity.Property(e => e.EndTime)
                    .HasMaxLength(200)
                    .HasColumnName("endTime");

                entity.Property(e => e.QrEmpId).HasColumnName("qrEmpId");

                entity.Property(e => e.StartDate)
                    .HasColumnType("date")
                    .HasColumnName("startDate");

                entity.Property(e => e.StartLat)
                    .HasMaxLength(500)
                    .HasColumnName("startLat");

                entity.Property(e => e.StartLong)
                    .HasMaxLength(500)
                    .HasColumnName("startLong");

                entity.Property(e => e.StartNote).HasColumnName("startNote");

                entity.Property(e => e.StartTime)
                    .HasMaxLength(200)
                    .HasColumnName("startTime");
            });

            modelBuilder.Entity<QrEmployeeMaster>(entity =>
            {
                entity.HasKey(e => e.QrEmpId);

                entity.ToTable("QrEmployeeMaster");

                entity.Property(e => e.QrEmpId).HasColumnName("qrEmpId");

                entity.Property(e => e.AppId).HasColumnName("appId");

                entity.Property(e => e.BloodGroup)
                    .HasMaxLength(500)
                    .HasColumnName("bloodGroup");

                entity.Property(e => e.ImoNo).HasColumnName("imoNo");

                entity.Property(e => e.IsActive).HasColumnName("isActive");

                entity.Property(e => e.LastModifyDate)
                    .HasColumnType("datetime")
                    .HasColumnName("lastModifyDate");

                entity.Property(e => e.QrEmpAddress)
                    .HasMaxLength(200)
                    .HasColumnName("qrEmpAddress");

                entity.Property(e => e.QrEmpLoginId)
                    .HasMaxLength(200)
                    .HasColumnName("qrEmpLoginId");

                entity.Property(e => e.QrEmpMobileNumber)
                    .HasMaxLength(20)
                    .HasColumnName("qrEmpMobileNumber");

                entity.Property(e => e.QrEmpName)
                    .HasMaxLength(500)
                    .HasColumnName("qrEmpName");

                entity.Property(e => e.QrEmpNameMar)
                    .HasMaxLength(200)
                    .HasColumnName("qrEmpNameMar");

                entity.Property(e => e.QrEmpPassword)
                    .HasMaxLength(100)
                    .HasColumnName("qrEmpPassword");

                entity.Property(e => e.Target)
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("target");

                entity.Property(e => e.Type)
                    .HasMaxLength(50)
                    .HasColumnName("type");

                entity.Property(e => e.TypeId).HasColumnName("typeId");

                entity.Property(e => e.UserEmployeeNo)
                    .HasMaxLength(250)
                    .HasColumnName("userEmployeeNo");
            });

            modelBuilder.Entity<QrLocation>(entity =>
            {
                entity.HasKey(e => e.LocId);

                entity.ToTable("Qr_Location");

                entity.Property(e => e.LocId).HasColumnName("locId");

                entity.Property(e => e.Address).HasColumnName("address");

                entity.Property(e => e.Area)
                    .HasMaxLength(500)
                    .HasColumnName("area");

                entity.Property(e => e.BatteryStatus)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("batteryStatus");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Datetime)
                    .HasColumnType("datetime")
                    .HasColumnName("datetime");

                entity.Property(e => e.Distnace).HasColumnType("decimal(6, 2)");

                entity.Property(e => e.EmpId).HasColumnName("empId");

                entity.Property(e => e.Lat)
                    .HasMaxLength(500)
                    .HasColumnName("lat");

                entity.Property(e => e.Long)
                    .HasMaxLength(500)
                    .HasColumnName("long");

                entity.Property(e => e.ReferanceId)
                    .HasMaxLength(50)
                    .HasColumnName("ReferanceID");

                entity.Property(e => e.Type).HasColumnName("type");
            });

            modelBuilder.Entity<Questionnaire>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("questionnaire");

                entity.Property(e => e.QNumber).HasColumnName("Q Number");

                entity.Property(e => e.QResponse)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("Q Response");

                entity.Property(e => e.QText)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("Q Text");
            });

            modelBuilder.Entity<SauchalayAddress>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("SauchalayAddress");

                entity.Property(e => e.Address).HasMaxLength(500);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.ImageUrl).HasMaxLength(1000);

                entity.Property(e => e.Lat).HasMaxLength(500);

                entity.Property(e => e.Long).HasMaxLength(500);

                entity.Property(e => e.Mobile).HasMaxLength(200);

                entity.Property(e => e.Name).HasMaxLength(200);

                entity.Property(e => e.QrImageUrl).HasMaxLength(1000);

                entity.Property(e => e.SauchalayId)
                    .HasMaxLength(100)
                    .HasColumnName("SauchalayID");
            });

            modelBuilder.Entity<Ss14Answer>(entity =>
            {
                entity.HasKey(e => e.AnsId);

                entity.ToTable("SS_1.4_ANSWER");

                entity.Property(e => e.AnsId).HasColumnName("ANS_ID");

                entity.Property(e => e.InsertDate)
                    .HasColumnType("datetime")
                    .HasColumnName("INSERT_DATE");

                entity.Property(e => e.InsertId).HasColumnName("INSERT_ID");

                entity.Property(e => e.Marks)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("MARKS");

                entity.Property(e => e.QId).HasColumnName("Q_ID");

                entity.Property(e => e.TotalCount)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("TOTAL_COUNT");
            });

            modelBuilder.Entity<Ss14Question>(entity =>
            {
                entity.HasKey(e => e.QId);

                entity.ToTable("SS_1.4_QUESTION");

                entity.Property(e => e.QId).HasColumnName("Q_ID");

                entity.Property(e => e.Note)
                    .HasMaxLength(199)
                    .IsUnicode(false)
                    .HasColumnName("NOTE");

                entity.Property(e => e.QNo)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("Q_NO");

                entity.Property(e => e.Questiond)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("QUESTIOND");
            });

            modelBuilder.Entity<Ss17Answer>(entity =>
            {
                entity.HasKey(e => e.Trno);

                entity.ToTable("SS_1.7_ANSWER");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.InsertDate)
                    .HasColumnType("datetime")
                    .HasColumnName("INSERT_DATE");

                entity.Property(e => e.InsertId).HasColumnName("INSERT_ID");

                entity.Property(e => e.NoDrainNallas).HasColumnName("No_drain_nallas");

                entity.Property(e => e.NoLocations).HasColumnName("No_locations");

                entity.Property(e => e.NoOutlets).HasColumnName("No_outlets");

                entity.Property(e => e.NoWaterBodies).HasColumnName("No_water_bodies");
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
                entity.HasNoKey();

                entity.Property(e => e.AreaId).HasColumnName("areaId");

                entity.Property(e => e.DataEntryDate).HasColumnType("datetime");

                entity.Property(e => e.LastModifiedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("lastModifiedDate");

                entity.Property(e => e.QrcodeImage).HasColumnName("QRCodeImage");

                entity.Property(e => e.Qrstatus).HasColumnName("QRStatus");

                entity.Property(e => e.QrstatusDate)
                    .HasColumnType("datetime")
                    .HasColumnName("QRStatusDate");

                entity.Property(e => e.ReferanceId).HasMaxLength(200);

                entity.Property(e => e.Ssaddress).HasColumnName("SSAddress");

                entity.Property(e => e.Ssid)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("SSId");

                entity.Property(e => e.Sslat)
                    .HasMaxLength(500)
                    .HasColumnName("SSLat");

                entity.Property(e => e.Sslong)
                    .HasMaxLength(500)
                    .HasColumnName("SSLong");

                entity.Property(e => e.Ssname)
                    .HasMaxLength(200)
                    .HasColumnName("SSName");

                entity.Property(e => e.SsnameMar)
                    .HasMaxLength(200)
                    .HasColumnName("SSNameMar");

                entity.Property(e => e.Ssqrcode).HasColumnName("SSQRCode");

                entity.Property(e => e.UserId).HasColumnName("userId");

                entity.Property(e => e.WardId).HasColumnName("wardId");

                entity.Property(e => e.ZoneId).HasColumnName("zoneId");
            });

            modelBuilder.Entity<TeritoryMaster>(entity =>
            {
                entity.ToTable("TeritoryMaster");

                entity.Property(e => e.Area).HasMaxLength(250);

                entity.Property(e => e.AreaMar).HasMaxLength(50);

                entity.Property(e => e.WardId).HasColumnName("wardId");
            });

            modelBuilder.Entity<UserMaster>(entity =>
            {
                entity.HasKey(e => e.UserId);

                entity.ToTable("UserMaster");

                entity.Property(e => e.UserId).HasColumnName("userId");

                entity.Property(e => e.BloodGroup)
                    .HasMaxLength(250)
                    .HasColumnName("bloodGroup");

                entity.Property(e => e.EmployeeType)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.GcTarget)
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("gcTarget");

                entity.Property(e => e.ImoNo).HasColumnName("imoNo");

                entity.Property(e => e.ImoNo2).HasColumnName("imoNo2");

                entity.Property(e => e.IsActive).HasColumnName("isActive");

                entity.Property(e => e.ShiftIds)
                    .HasMaxLength(500)
                    .HasColumnName("shiftIds");

                entity.Property(e => e.Type).HasMaxLength(50);

                entity.Property(e => e.UserAddress)
                    .HasMaxLength(200)
                    .HasColumnName("userAddress");

                entity.Property(e => e.UserDesignation).HasColumnName("userDesignation");

                entity.Property(e => e.UserEmployeeNo)
                    .HasMaxLength(250)
                    .HasColumnName("userEmployeeNo");

                entity.Property(e => e.UserLoginId)
                    .HasMaxLength(200)
                    .HasColumnName("userLoginId");

                entity.Property(e => e.UserMobileNumber)
                    .HasMaxLength(20)
                    .HasColumnName("userMobileNumber");

                entity.Property(e => e.UserName)
                    .HasMaxLength(500)
                    .HasColumnName("userName");

                entity.Property(e => e.UserNameMar)
                    .HasMaxLength(200)
                    .HasColumnName("userNameMar");

                entity.Property(e => e.UserPassword)
                    .HasMaxLength(100)
                    .HasColumnName("userPassword");

                entity.Property(e => e.UserProfileImage)
                    .HasMaxLength(500)
                    .HasColumnName("userProfileImage");
            });

            modelBuilder.Entity<VehicalQrMaster>(entity =>
            {
                entity.HasKey(e => e.VqrId);

                entity.ToTable("Vehical_QR_Master");

                entity.Property(e => e.VqrId).HasColumnName("vqrId");

                entity.Property(e => e.Fcmid)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("FCMID");

                entity.Property(e => e.LastModifiedEntry)
                    .HasColumnType("datetime")
                    .HasColumnName("lastModifiedEntry");

                entity.Property(e => e.Modified)
                    .HasColumnType("datetime")
                    .HasColumnName("modified");

                entity.Property(e => e.PropertyType).HasColumnName("Property_Type");

                entity.Property(e => e.ReferanceId).HasMaxLength(350);

                entity.Property(e => e.RfidtagId)
                    .HasMaxLength(255)
                    .HasColumnName("RFIDTagId");

                entity.Property(e => e.VehicalNumber).HasMaxLength(500);

                entity.Property(e => e.VehicalQrcode).HasColumnName("VehicalQRCode");

                entity.Property(e => e.VehicalType)
                    .HasMaxLength(10)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<VehicleType>(entity =>
            {
                entity.HasKey(e => e.VtId);

                entity.ToTable("VehicleType");

                entity.Property(e => e.VtId).HasColumnName("vtId");

                entity.Property(e => e.Description)
                    .HasMaxLength(300)
                    .HasColumnName("description");

                entity.Property(e => e.DescriptionMar)
                    .HasMaxLength(300)
                    .HasColumnName("descriptionMar");

                entity.Property(e => e.IsActive).HasColumnName("isActive");
            });

            modelBuilder.Entity<ViewLlocation>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("view_LLocation");

                entity.Property(e => e.Distnace).HasColumnType("decimal(6, 2)");

                entity.Property(e => e.LocDate).HasColumnType("date");

                entity.Property(e => e.Userid).HasColumnName("USERID");
            });

            modelBuilder.Entity<ViewLocation>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("view_Location");

                entity.Property(e => e.Distnace).HasColumnType("decimal(6, 2)");

                entity.Property(e => e.LocDate).HasColumnType("date");

                entity.Property(e => e.Userid).HasColumnName("USERID");
            });

            modelBuilder.Entity<ViewSlocation>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("view_SLocation");

                entity.Property(e => e.Distnace).HasColumnType("decimal(6, 2)");

                entity.Property(e => e.LocDate).HasColumnType("date");

                entity.Property(e => e.Userid).HasColumnName("USERID");
            });

            modelBuilder.Entity<VwBitCount>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("Vw_BitCount");

                entity.Property(e => e.BeatId).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<VwGetHouseNumber>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("Vw_GetHouseNumber");

                entity.Property(e => e.HouseId).HasColumnName("houseId");

                entity.Property(e => e.HouseNumber)
                    .HasMaxLength(500)
                    .HasColumnName("houseNumber");

                entity.Property(e => e.ReferanceId).HasMaxLength(350);
            });

            modelBuilder.Entity<VwGetLiquidNumber>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("Vw_GetLiquidNumber");

                entity.Property(e => e.Lwid).HasColumnName("LWId");

                entity.Property(e => e.ReferanceId).HasMaxLength(200);
            });

            modelBuilder.Entity<VwGetStreetNumber>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("Vw_GetStreetNumber");

                entity.Property(e => e.ReferanceId).HasMaxLength(200);

                entity.Property(e => e.Ssid).HasColumnName("SSId");
            });

            modelBuilder.Entity<VwHsgetDumpyardDetail>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("VW_HSGetDumpyardDetails");

                entity.Property(e => e.DyId).HasColumnName("dyId");

                entity.Property(e => e.DyLat)
                    .HasMaxLength(500)
                    .HasColumnName("dyLat");

                entity.Property(e => e.DyLong)
                    .HasMaxLength(500)
                    .HasColumnName("dyLong");

                entity.Property(e => e.DyName)
                    .HasMaxLength(200)
                    .HasColumnName("dyName");

                entity.Property(e => e.LastModifiedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("lastModifiedDate");

                entity.Property(e => e.QrEmpName)
                    .HasMaxLength(500)
                    .HasColumnName("qrEmpName");

                entity.Property(e => e.Qrstatus).HasColumnName("QRStatus");

                entity.Property(e => e.QrstatusDate)
                    .HasColumnType("datetime")
                    .HasColumnName("QRStatusDate");

                entity.Property(e => e.ReferanceId).HasMaxLength(200);

                entity.Property(e => e.UserId).HasColumnName("userId");
            });

            modelBuilder.Entity<VwHsgetHouseDetail>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("VW_HSGetHouseDetails");

                entity.Property(e => e.BinaryQrCodeImage).IsRequired();

                entity.Property(e => e.HouseId).HasColumnName("houseId");

                entity.Property(e => e.HouseLat)
                    .HasMaxLength(500)
                    .HasColumnName("houseLat");

                entity.Property(e => e.HouseLong)
                    .HasMaxLength(500)
                    .HasColumnName("houseLong");

                entity.Property(e => e.HouseOwner)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("houseOwner");

                entity.Property(e => e.Modified)
                    .HasColumnType("datetime")
                    .HasColumnName("modified");

                entity.Property(e => e.QrEmpName)
                    .HasMaxLength(500)
                    .HasColumnName("qrEmpName");

                entity.Property(e => e.Qrstatus).HasColumnName("QRStatus");

                entity.Property(e => e.QrstatusDate)
                    .HasColumnType("datetime")
                    .HasColumnName("QRStatusDate");

                entity.Property(e => e.ReferanceId).HasMaxLength(350);

                entity.Property(e => e.UserId).HasColumnName("userId");
            });

            modelBuilder.Entity<VwHsgetLiquidDetail>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("VW_HSGetLiquidDetails");

                entity.Property(e => e.LastModifiedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("lastModifiedDate");

                entity.Property(e => e.Lwid).HasColumnName("LWId");

                entity.Property(e => e.Lwlat)
                    .HasMaxLength(500)
                    .HasColumnName("LWLat");

                entity.Property(e => e.Lwlong)
                    .HasMaxLength(500)
                    .HasColumnName("LWLong");

                entity.Property(e => e.Lwname)
                    .HasMaxLength(200)
                    .HasColumnName("LWName");

                entity.Property(e => e.QrEmpName)
                    .HasMaxLength(500)
                    .HasColumnName("qrEmpName");

                entity.Property(e => e.Qrstatus).HasColumnName("QRStatus");

                entity.Property(e => e.QrstatusDate)
                    .HasColumnType("datetime")
                    .HasColumnName("QRStatusDate");

                entity.Property(e => e.ReferanceId).HasMaxLength(200);

                entity.Property(e => e.UserId).HasColumnName("userId");
            });

            modelBuilder.Entity<VwHsgetStreetDetail>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("VW_HSGetStreetDetails");

                entity.Property(e => e.LastModifiedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("lastModifiedDate");

                entity.Property(e => e.QrEmpName)
                    .HasMaxLength(500)
                    .HasColumnName("qrEmpName");

                entity.Property(e => e.Qrstatus).HasColumnName("QRStatus");

                entity.Property(e => e.QrstatusDate)
                    .HasColumnType("datetime")
                    .HasColumnName("QRStatusDate");

                entity.Property(e => e.ReferanceId).HasMaxLength(200);

                entity.Property(e => e.Ssid).HasColumnName("SSId");

                entity.Property(e => e.Sslat)
                    .HasMaxLength(500)
                    .HasColumnName("SSLat");

                entity.Property(e => e.Sslong)
                    .HasMaxLength(500)
                    .HasColumnName("SSLong");

                entity.Property(e => e.Ssname)
                    .HasMaxLength(200)
                    .HasColumnName("SSName");

                entity.Property(e => e.UserId).HasColumnName("userId");
            });

            modelBuilder.Entity<WardNumber>(entity =>
            {
                entity.ToTable("WardNumber");

                entity.Property(e => e.WardNo).HasMaxLength(50);

                entity.Property(e => e.ZoneId).HasColumnName("zoneId");
            });

            modelBuilder.Entity<WmGarbageCategory>(entity =>
            {
                entity.HasKey(e => e.CategoryId);

                entity.ToTable("WM_GarbageCategory");

                entity.Property(e => e.CategoryId).HasColumnName("CategoryID");

                entity.Property(e => e.Category).HasMaxLength(50);
            });

            modelBuilder.Entity<WmGarbageDetail>(entity =>
            {
                entity.HasKey(e => e.GarbageDetailsId);

                entity.ToTable("WM_Garbage_Details");

                entity.Property(e => e.GarbageDetailsId).HasColumnName("GarbageDetailsID");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.GdDate)
                    .HasColumnType("datetime")
                    .HasColumnName("gdDate");

                entity.Property(e => e.SubCategoryId).HasColumnName("SubCategoryID");

                entity.Property(e => e.Weight).HasColumnType("decimal(18, 4)");
            });

            modelBuilder.Entity<WmGarbageSale>(entity =>
            {
                entity.ToTable("WM_Garbage_Sales");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Amount).HasColumnType("decimal(18, 4)");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.PartyName).HasMaxLength(500);

                entity.Property(e => e.SalesWeight).HasColumnType("decimal(18, 4)");

                entity.Property(e => e.SubCategoryId).HasColumnName("SubCategoryID");
            });

            modelBuilder.Entity<WmGarbageSubCategory>(entity =>
            {
                entity.HasKey(e => e.SubCategoryId);

                entity.ToTable("WM_GarbageSubCategory");

                entity.Property(e => e.SubCategoryId).HasColumnName("SubCategoryID");

                entity.Property(e => e.CategoryId).HasColumnName("CategoryID");

                entity.Property(e => e.SubCategory).HasMaxLength(50);
            });

            modelBuilder.Entity<WmGarbageSummary>(entity =>
            {
                entity.ToTable("WM_Garbage_Summary");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Price).HasColumnType("decimal(18, 4)");

                entity.Property(e => e.SubCategoryId).HasColumnName("SubCategoryID");

                entity.Property(e => e.TotalWeight).HasColumnType("decimal(18, 4)");
            });

            modelBuilder.Entity<ZoneMaster>(entity =>
            {
                entity.HasKey(e => e.ZoneId);

                entity.ToTable("ZoneMaster");

                entity.Property(e => e.ZoneId).HasColumnName("zoneId");

                entity.Property(e => e.Name)
                    .HasMaxLength(500)
                    .HasColumnName("name");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
