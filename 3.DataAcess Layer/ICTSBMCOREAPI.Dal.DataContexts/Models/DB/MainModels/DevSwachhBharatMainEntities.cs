using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace ICTSBMCOREAPI.Dal.DataContexts.Models.DB.MainModels
{
    public partial class DevSwachhBharatMainEntities : DbContext
    {
        public DevSwachhBharatMainEntities()
        {
        }

        public DevSwachhBharatMainEntities(DbContextOptions<DevSwachhBharatMainEntities> options)
            : base(options)
        {
        }

        public virtual DbSet<AdUserMstLiquid> AdUserMstLiquids { get; set; }
        public virtual DbSet<AdUserMstStreet> AdUserMstStreets { get; set; }
        public virtual DbSet<AdminContact> AdminContacts { get; set; }
        public virtual DbSet<AemployeeMaster> AemployeeMasters { get; set; }
        public virtual DbSet<AppConnection> AppConnections { get; set; }
        public virtual DbSet<AppDetail> AppDetails { get; set; }
        public virtual DbSet<AspNetRole> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUser> AspNetUsers { get; set; }
        public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUserRole> AspNetUserRoles { get; set; }
        public virtual DbSet<BinMaster> BinMasters { get; set; }
        public virtual DbSet<CheckAppD> CheckAppDs { get; set; }
        public virtual DbSet<CountryState> CountryStates { get; set; }
        public virtual DbSet<EmployeeMaster> EmployeeMasters { get; set; }
        public virtual DbSet<FeedbackPlaystore> FeedbackPlaystores { get; set; }
        public virtual DbSet<GameAnswerType> GameAnswerTypes { get; set; }
        public virtual DbSet<GameDetail> GameDetails { get; set; }
        public virtual DbSet<GameMaster> GameMasters { get; set; }
        public virtual DbSet<GamePlayerDetail> GamePlayerDetails { get; set; }
        public virtual DbSet<GameSlogan> GameSlogans { get; set; }
        public virtual DbSet<GoogelHitDetail> GoogelHitDetails { get; set; }
        public virtual DbSet<GoogleApidetail> GoogleApidetails { get; set; }
        public virtual DbSet<HsurDailyAttendance> HsurDailyAttendances { get; set; }
        public virtual DbSet<LanguageInfo> LanguageInfos { get; set; }
        public virtual DbSet<RfidMaster> RfidMasters { get; set; }
        public virtual DbSet<SauchalayFeedback> SauchalayFeedbacks { get; set; }
        public virtual DbSet<StateDistrict> StateDistricts { get; set; }
        public virtual DbSet<Subscription> Subscriptions { get; set; }
        public virtual DbSet<Tehsil> Tehsils { get; set; }
        public virtual DbSet<UrLocation> UrLocations { get; set; }
        public virtual DbSet<UserInApp> UserInApps { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("data source=202.65.157.253;initial catalog=LIVEAdvanceDevSwachhBharatMain;persist security info=True;user id=appynitty;password=BigV$Telecom;MultipleActiveResultSets=True;App=EntityFramework");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<AdUserMstLiquid>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("AD_USER_MST_LIQUID");

                entity.Property(e => e.AdUserTypeId).HasColumnName("AD_USER_TYPE_ID");

                entity.Property(e => e.AdumDesignation)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("ADUM_DESIGNATION");

                entity.Property(e => e.AdumEmail)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("ADUM_EMAIL");

                entity.Property(e => e.AdumEmployeeId)
                    .HasMaxLength(50)
                    .HasColumnName("ADUM_EMPLOYEE_ID");

                entity.Property(e => e.AdumFrdt)
                    .HasColumnType("datetime")
                    .HasColumnName("ADUM_FRDT");

                entity.Property(e => e.AdumLoginId)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("ADUM_LOGIN_ID");

                entity.Property(e => e.AdumMobile)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("ADUM_MOBILE");

                entity.Property(e => e.AdumPassword)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("ADUM_PASSWORD");

                entity.Property(e => e.AdumStatus).HasColumnName("ADUM_STATUS");

                entity.Property(e => e.AdumTodt)
                    .HasColumnType("datetime")
                    .HasColumnName("ADUM_TODT");

                entity.Property(e => e.AdumUserCode)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ADUM_USER_CODE");

                entity.Property(e => e.AdumUserId)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("ADUM_USER_ID");

                entity.Property(e => e.AdumUserName)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("ADUM_USER_NAME");

                entity.Property(e => e.AppId).HasColumnName("APP_ID");

                entity.Property(e => e.ImoNo)
                    .HasMaxLength(1)
                    .HasColumnName("IMO_NO");

                entity.Property(e => e.IsActive).HasColumnName("IS_ACTIVE");

                entity.Property(e => e.LastUpdate)
                    .HasColumnType("smalldatetime")
                    .HasColumnName("LAST_UPDATE");

                entity.Property(e => e.LocalUserName)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("LOCAL_USER_NAME");

                entity.Property(e => e.MobileId)
                    .HasMaxLength(100)
                    .HasColumnName("MOBILE_ID");

                entity.Property(e => e.ProfileImage)
                    .HasMaxLength(100)
                    .HasColumnName("PROFILE_IMAGE");

                entity.Property(e => e.ServerId).HasColumnName("SERVER_ID");

                entity.Property(e => e.UpdateFlag).HasColumnName("UPDATE_FLAG");
            });

            modelBuilder.Entity<AdUserMstStreet>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("AD_USER_MST_STREET");

                entity.Property(e => e.AdUserTypeId).HasColumnName("AD_USER_TYPE_ID");

                entity.Property(e => e.AdumDesignation)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("ADUM_DESIGNATION");

                entity.Property(e => e.AdumEmail)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("ADUM_EMAIL");

                entity.Property(e => e.AdumEmployeeId)
                    .HasMaxLength(50)
                    .HasColumnName("ADUM_EMPLOYEE_ID");

                entity.Property(e => e.AdumFrdt)
                    .HasColumnType("datetime")
                    .HasColumnName("ADUM_FRDT");

                entity.Property(e => e.AdumLoginId)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("ADUM_LOGIN_ID");

                entity.Property(e => e.AdumMobile)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("ADUM_MOBILE");

                entity.Property(e => e.AdumPassword)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("ADUM_PASSWORD");

                entity.Property(e => e.AdumStatus).HasColumnName("ADUM_STATUS");

                entity.Property(e => e.AdumTodt)
                    .HasColumnType("datetime")
                    .HasColumnName("ADUM_TODT");

                entity.Property(e => e.AdumUserCode)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ADUM_USER_CODE");

                entity.Property(e => e.AdumUserId)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("ADUM_USER_ID");

                entity.Property(e => e.AdumUserName)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("ADUM_USER_NAME");

                entity.Property(e => e.AppId).HasColumnName("APP_ID");

                entity.Property(e => e.ImoNo)
                    .HasMaxLength(1)
                    .HasColumnName("IMO_NO");

                entity.Property(e => e.IsActive).HasColumnName("IS_ACTIVE");

                entity.Property(e => e.LastUpdate)
                    .HasColumnType("smalldatetime")
                    .HasColumnName("LAST_UPDATE");

                entity.Property(e => e.LocalUserName)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("LOCAL_USER_NAME");

                entity.Property(e => e.MobileId)
                    .HasMaxLength(100)
                    .HasColumnName("MOBILE_ID");

                entity.Property(e => e.ProfileImage)
                    .HasMaxLength(100)
                    .HasColumnName("PROFILE_IMAGE");

                entity.Property(e => e.ServerId).HasColumnName("SERVER_ID");

                entity.Property(e => e.UpdateFlag).HasColumnName("UPDATE_FLAG");
            });

            modelBuilder.Entity<AdminContact>(entity =>
            {
                entity.ToTable("AdminContact");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.MobileNumber).HasMaxLength(50);

                entity.Property(e => e.Position).HasMaxLength(500);
            });

            modelBuilder.Entity<AemployeeMaster>(entity =>
            {
                entity.HasKey(e => e.EmpId);

                entity.ToTable("AEmployeeMaster");

                entity.Property(e => e.EmpAddress).HasMaxLength(200);

                entity.Property(e => e.EmpMobileNumber).HasMaxLength(20);

                entity.Property(e => e.EmpName).HasMaxLength(500);

                entity.Property(e => e.EmpNameMar).HasMaxLength(200);

                entity.Property(e => e.IsActive).HasColumnName("isActive");

                entity.Property(e => e.IsActiveUlb).HasColumnName("isActiveULB");

                entity.Property(e => e.LastModifyDateEntry)
                    .HasColumnType("datetime")
                    .HasColumnName("lastModifyDateEntry");

                entity.Property(e => e.LoginId).HasMaxLength(200);

                entity.Property(e => e.Password).HasMaxLength(100);

                entity.Property(e => e.Type)
                    .HasMaxLength(50)
                    .HasColumnName("type");
            });

            modelBuilder.Entity<AppConnection>(entity =>
            {
                entity.ToTable("AppConnection");

                entity.Property(e => e.DataSource)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.InitialCatalog)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<AppDetail>(entity =>
            {
                entity.HasKey(e => e.AppId);

                entity.Property(e => e.AboutAppynity).HasMaxLength(50);

                entity.Property(e => e.AboutTeamDetail).HasMaxLength(50);

                entity.Property(e => e.AboutThumbnailUrl)
                    .HasMaxLength(1000)
                    .HasColumnName("AboutThumbnailURL");

                entity.Property(e => e.AddUlbProperty).HasColumnName("Add_Ulb_Property");

                entity.Property(e => e.AndroidGcmPushNotificationKey)
                    .HasMaxLength(150)
                    .IsUnicode(false)
                    .HasColumnName("Android_GCM_pushNotification_Key");

                entity.Property(e => e.Apihit).HasColumnName("APIHit");

                entity.Property(e => e.AppLink).HasMaxLength(500);

                entity.Property(e => e.AppName).HasMaxLength(100);

                entity.Property(e => e.AppNameMar)
                    .HasMaxLength(100)
                    .HasColumnName("AppName_mar");

                entity.Property(e => e.AppVersion).HasMaxLength(500);

                entity.Property(e => e.BaseImageUrl)
                    .HasMaxLength(255)
                    .HasColumnName("baseImageUrl");

                entity.Property(e => e.BaseImageUrlCms)
                    .HasMaxLength(255)
                    .HasColumnName("baseImageUrlCMS");

                entity.Property(e => e.BasePath)
                    .HasMaxLength(40)
                    .HasColumnName("basePath");

                entity.Property(e => e.Collection).HasMaxLength(200);

                entity.Property(e => e.CommercialQrcode)
                    .HasMaxLength(100)
                    .HasColumnName("CommercialQRCode");

                entity.Property(e => e.ContactUs).HasMaxLength(50);

                entity.Property(e => e.ContactUsTeamMember).HasMaxLength(50);

                entity.Property(e => e.Ctptqrcode)
                    .HasMaxLength(100)
                    .HasColumnName("CTPTQRCode");

                entity.Property(e => e.DumpYardPdf)
                    .HasMaxLength(250)
                    .HasColumnName("DumpYardPDF");

                entity.Property(e => e.DumpYardQrcode)
                    .HasMaxLength(250)
                    .HasColumnName("DumpYardQRCode");

                entity.Property(e => e.EmailId).HasMaxLength(100);

                entity.Property(e => e.Faq)
                    .HasMaxLength(50)
                    .HasColumnName("FAQ");

                entity.Property(e => e.GramPanchyatAppId).HasColumnName("GramPanchyatAppID");

                entity.Property(e => e.GrampanchayatPro).HasColumnName("Grampanchayat_Pro");

                entity.Property(e => e.HomeSplash).HasMaxLength(50);

                entity.Property(e => e.HousePdf)
                    .HasMaxLength(550)
                    .HasColumnName("HousePDF");

                entity.Property(e => e.HouseQrcode)
                    .HasMaxLength(220)
                    .HasColumnName("HouseQRCode");

                entity.Property(e => e.IsAreaActive).HasDefaultValueSql("((0))");

                entity.Property(e => e.LiquidQrcode)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("LiquidQRCode");

                entity.Property(e => e.MsgForBroadcast).HasMaxLength(1000);

                entity.Property(e => e.MsgForMixed).HasMaxLength(1000);

                entity.Property(e => e.MsgForNotReceived).HasMaxLength(1000);

                entity.Property(e => e.MsgForNotSpecified).HasMaxLength(1000);

                entity.Property(e => e.MsgForSegregated).HasMaxLength(1000);

                entity.Property(e => e.PointPdf)
                    .HasMaxLength(550)
                    .HasColumnName("PointPDF");

                entity.Property(e => e.PointQrcode)
                    .HasMaxLength(220)
                    .HasColumnName("PointQRCode");

                entity.Property(e => e.Status).HasDefaultValueSql("((0))");

                entity.Property(e => e.StreetQrcode)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("StreetQRCode");

                entity.Property(e => e.Swmqrcode)
                    .HasMaxLength(100)
                    .HasColumnName("SWMQRCode");

                entity.Property(e => e.TodayDumpScanCount).HasColumnName("Today_DumpScanCount");

                entity.Property(e => e.TodayEmployeeDutyOff).HasColumnName("Today_EmployeeDutyOff");

                entity.Property(e => e.TodayEmployeeDutyOn).HasColumnName("Today_EmployeeDutyOn");

                entity.Property(e => e.TodayGarbageNrcount).HasColumnName("Today_GarbageNRCount");

                entity.Property(e => e.TodayHouseScanCount).HasColumnName("Today_HouseScanCount");

                entity.Property(e => e.TodayLiquidScanCount).HasColumnName("Today_LiquidScanCount");

                entity.Property(e => e.TodayLiquidStatus).HasColumnName("Today_Liquid_Status");

                entity.Property(e => e.TodayMixedCount).HasColumnName("Today_MixedCount");

                entity.Property(e => e.TodaySegregatedCount).HasColumnName("Today_SegregatedCount");

                entity.Property(e => e.TodayStreetScanCount).HasColumnName("Today_StreetScanCount");

                entity.Property(e => e.TodayStreetStatus).HasColumnName("Today_Street_Status");

                entity.Property(e => e.TodayWasteStatus).HasColumnName("Today_Waste_Status");

                entity.Property(e => e.TotalDumpCount).HasColumnName("Total_DumpCount");

                entity.Property(e => e.TotalEmployeeActive).HasColumnName("Total_EmployeeActive");

                entity.Property(e => e.TotalHouseCount).HasColumnName("Total_HouseCount");

                entity.Property(e => e.TotalLiquidCount).HasColumnName("Total_LiquidCount");

                entity.Property(e => e.TotalStreetCount).HasColumnName("Total_StreetCount");

                entity.Property(e => e.Type)
                    .HasMaxLength(20)
                    .IsFixedLength(true);

                entity.Property(e => e.UlbProperty).HasColumnName("ulb_property");

                entity.Property(e => e.UserProfile).HasMaxLength(200);

                entity.Property(e => e.VehicalQrcode)
                    .HasMaxLength(250)
                    .HasColumnName("VehicalQRCode");

                entity.Property(e => e.Website)
                    .HasMaxLength(100)
                    .HasColumnName("website");

                entity.Property(e => e.YoccClientId).HasColumnName("YoccClientID");

                entity.Property(e => e.YoccContact).HasColumnName("yoccContact");

                entity.Property(e => e.YoccDndLink).HasMaxLength(250);

                entity.Property(e => e.YoccFeddbackLink).HasMaxLength(250);
            });

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

                entity.Property(e => e.Loginuser).HasMaxLength(256);

                entity.Property(e => e.PasswordString).HasMaxLength(50);

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

            modelBuilder.Entity<BinMaster>(entity =>
            {
                entity.ToTable("BinMaster");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("ID");

                entity.Property(e => e.AppName).HasMaxLength(100);

                entity.Property(e => e.DeviceId).HasMaxLength(50);
            });

            modelBuilder.Entity<CheckAppD>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("CheckAppD");

                entity.Property(e => e.AppName)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("App_Name");

                entity.Property(e => e.Id).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<CountryState>(entity =>
            {
                entity.ToTable("country_states");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CountryName)
                    .HasMaxLength(90)
                    .HasColumnName("country_name");

                entity.Property(e => e.State2Code)
                    .HasMaxLength(2)
                    .HasColumnName("state_2_code");

                entity.Property(e => e.State3Code)
                    .HasMaxLength(3)
                    .HasColumnName("state_3_code");

                entity.Property(e => e.StateName)
                    .HasMaxLength(250)
                    .HasColumnName("state_name");

                entity.Property(e => e.StateNameMar)
                    .HasMaxLength(250)
                    .HasColumnName("state_name_mar");
            });

            modelBuilder.Entity<EmployeeMaster>(entity =>
            {
                entity.HasKey(e => e.EmpId);

                entity.ToTable("EmployeeMaster");

                entity.Property(e => e.EmpAddress).HasMaxLength(200);

                entity.Property(e => e.EmpMobileNumber).HasMaxLength(20);

                entity.Property(e => e.EmpName).HasMaxLength(500);

                entity.Property(e => e.EmpNameMar).HasMaxLength(200);

                entity.Property(e => e.IsActive).HasColumnName("isActive");

                entity.Property(e => e.IsActiveUlb).HasColumnName("isActiveULB");

                entity.Property(e => e.LastModifyDateEntry)
                    .HasColumnType("datetime")
                    .HasColumnName("lastModifyDateEntry");

                entity.Property(e => e.LoginId).HasMaxLength(200);

                entity.Property(e => e.Password).HasMaxLength(100);

                entity.Property(e => e.Type)
                    .HasMaxLength(50)
                    .HasColumnName("type");
            });

            modelBuilder.Entity<FeedbackPlaystore>(entity =>
            {
                entity.HasKey(e => e.PlaystoreId);

                entity.ToTable("Feedback_playstore");

                entity.Property(e => e.PlaystoreId).HasColumnName("PlaystoreID");

                entity.Property(e => e.AppId).HasColumnName("AppID");

                entity.Property(e => e.Ulblink).HasColumnName("ULBlink");
            });

            modelBuilder.Entity<GameAnswerType>(entity =>
            {
                entity.HasKey(e => e.AnswerTypeId)
                    .HasName("PK_AnswerType");

                entity.ToTable("Game_AnswerType");

                entity.Property(e => e.AnswerType).HasMaxLength(50);

                entity.Property(e => e.AnswerTypeHindi).HasMaxLength(50);

                entity.Property(e => e.AnswerTypeMar).HasMaxLength(50);

                entity.Property(e => e.GameMasterId).HasColumnName("GameMasterID");
            });

            modelBuilder.Entity<GameDetail>(entity =>
            {
                entity.HasKey(e => e.GameDetailsId);

                entity.Property(e => e.GameDetailsId).HasColumnName("GameDetailsID");

                entity.Property(e => e.Created).HasColumnType("datetime");

                entity.Property(e => e.Description).HasMaxLength(500);

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

                entity.Property(e => e.GameNameHindi).HasMaxLength(100);

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

                entity.Property(e => e.SloganHindi).HasMaxLength(500);

                entity.Property(e => e.SloganMar).HasMaxLength(500);
            });

            modelBuilder.Entity<GoogelHitDetail>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Api).HasColumnName("API");

                entity.Property(e => e.Date).HasColumnType("date");

                entity.Property(e => e.Hit).HasColumnName("hit");
            });

            modelBuilder.Entity<GoogleApidetail>(entity =>
            {
                entity.ToTable("GoogleAPIDetails");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.GoogleApi).HasColumnName("GoogleAPI");
            });

            modelBuilder.Entity<HsurDailyAttendance>(entity =>
            {
                entity.HasKey(e => e.DaId);

                entity.ToTable("HSUR_Daily_Attendance");

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

                entity.Property(e => e.IpAddress).HasColumnName("ip_address");

                entity.Property(e => e.LoginDevice).HasColumnName("login_device");

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

                entity.Property(e => e.UserId).HasColumnName("userId");
            });

            modelBuilder.Entity<LanguageInfo>(entity =>
            {
                entity.ToTable("LanguageInfo");

                entity.Property(e => e.Language)
                    .IsRequired()
                    .HasMaxLength(30);

                entity.Property(e => e.LanguageCode)
                    .IsRequired()
                    .HasMaxLength(30);
            });

            modelBuilder.Entity<RfidMaster>(entity =>
            {
                entity.ToTable("RFID_Master");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AppId).HasColumnName("AppID");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ReaderId)
                    .HasMaxLength(100)
                    .HasColumnName("ReaderID");

                entity.Property(e => e.TagId)
                    .HasMaxLength(100)
                    .HasColumnName("TagID");
            });

            modelBuilder.Entity<SauchalayFeedback>(entity =>
            {
                entity.ToTable("Sauchalay_feedback");

                entity.Property(e => e.SauchalayFeedbackId).HasColumnName("SauchalayFeedback_ID");

                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.Property(e => e.Que1)
                    .HasMaxLength(50)
                    .HasColumnName("que1");

                entity.Property(e => e.Que2)
                    .HasMaxLength(50)
                    .HasColumnName("que2");

                entity.Property(e => e.Que3)
                    .HasMaxLength(50)
                    .HasColumnName("que3");

                entity.Property(e => e.Rating).HasMaxLength(50);

                entity.Property(e => e.SauchalayId)
                    .HasMaxLength(50)
                    .HasColumnName("SauchalayID");

                entity.Property(e => e.Ulb)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("ULB");
            });

            modelBuilder.Entity<StateDistrict>(entity =>
            {
                entity.ToTable("state_districts");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.DistrictName)
                    .IsRequired()
                    .HasMaxLength(450)
                    .HasColumnName("district_name");

                entity.Property(e => e.DistrictNameMar)
                    .HasMaxLength(450)
                    .HasColumnName("district_name_mar");

                entity.Property(e => e.StateId).HasColumnName("state_id");
            });

            modelBuilder.Entity<Subscription>(entity =>
            {
                entity.Property(e => e.SubscriptionId).HasColumnName("subscriptionId");

                entity.Property(e => e.SubscriptionName)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("subscriptionName");
            });

            modelBuilder.Entity<Tehsil>(entity =>
            {
                entity.ToTable("tehsil");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.District).HasColumnName("district");

                entity.Property(e => e.Latitude)
                    .HasMaxLength(20)
                    .HasColumnName("latitude");

                entity.Property(e => e.Longitude)
                    .HasMaxLength(20)
                    .HasColumnName("longitude");

                entity.Property(e => e.Name)
                    .HasMaxLength(250)
                    .HasColumnName("name");

                entity.Property(e => e.NameMar)
                    .HasMaxLength(250)
                    .HasColumnName("name_mar");

                entity.Property(e => e.State).HasColumnName("state");
            });

            modelBuilder.Entity<UrLocation>(entity =>
            {
                entity.HasKey(e => e.LocId)
                    .HasName("PK_Qr_Location");

                entity.ToTable("UR_Location");

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

            modelBuilder.Entity<UserInApp>(entity =>
            {
                entity.HasKey(e => e.UserInAppsId);

                entity.Property(e => e.UserInAppsId).HasColumnName("UserInAppsID");

                entity.Property(e => e.SubscriptionId)
                    .HasColumnName("subscriptionId")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasMaxLength(128);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
