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

        public virtual DbSet<AD_USER_MST_LIQUID> AD_USER_MST_LIQUIDs { get; set; }
        public virtual DbSet<AD_USER_MST_STREET> AD_USER_MST_STREETs { get; set; }
        public virtual DbSet<AEmployeeMaster> AEmployeeMasters { get; set; }
        public virtual DbSet<AdminContact> AdminContacts { get; set; }
        public virtual DbSet<AppConnection> AppConnections { get; set; }
        public virtual DbSet<AppDetail> AppDetails { get; set; }
        public virtual DbSet<AspNetRole> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUser> AspNetUsers { get; set; }
        public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUserRole> AspNetUserRoles { get; set; }
        public virtual DbSet<BinMaster> BinMasters { get; set; }
        public virtual DbSet<CheckAppD> CheckAppDs { get; set; }
        public virtual DbSet<EmployeeMaster> EmployeeMasters { get; set; }
        public virtual DbSet<Feedback_playstore> Feedback_playstores { get; set; }
        public virtual DbSet<GIS_AppConnection> GIS_AppConnections { get; set; }
        public virtual DbSet<GameDetail> GameDetails { get; set; }
        public virtual DbSet<GameMaster> GameMasters { get; set; }
        public virtual DbSet<GamePlayerDetail> GamePlayerDetails { get; set; }
        public virtual DbSet<Game_AnswerType> Game_AnswerTypes { get; set; }
        public virtual DbSet<Game_Slogan> Game_Slogans { get; set; }
        public virtual DbSet<GoogelHitDetail> GoogelHitDetails { get; set; }
        public virtual DbSet<GoogleAPIDetail> GoogleAPIDetails { get; set; }
        public virtual DbSet<HSUR_Daily_Attendance> HSUR_Daily_Attendances { get; set; }
        public virtual DbSet<LanguageInfo> LanguageInfos { get; set; }
        public virtual DbSet<RFID_Master> RFID_Masters { get; set; }
        public virtual DbSet<Sauchalay_feedback> Sauchalay_feedbacks { get; set; }
        public virtual DbSet<Subscription> Subscriptions { get; set; }
        public virtual DbSet<UR_Location> UR_Locations { get; set; }
        public virtual DbSet<UserInApp> UserInApps { get; set; }
        public virtual DbSet<country_state> country_states { get; set; }
        public virtual DbSet<state_district> state_districts { get; set; }
        public virtual DbSet<tehsil> tehsils { get; set; }

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

            modelBuilder.Entity<AD_USER_MST_LIQUID>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("AD_USER_MST_LIQUID");

                entity.Property(e => e.ADUM_DESIGNATION)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ADUM_EMAIL)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.ADUM_EMPLOYEE_ID).HasMaxLength(50);

                entity.Property(e => e.ADUM_FRDT).HasColumnType("datetime");

                entity.Property(e => e.ADUM_LOGIN_ID)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.ADUM_MOBILE)
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.ADUM_PASSWORD)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ADUM_TODT).HasColumnType("datetime");

                entity.Property(e => e.ADUM_USER_CODE).ValueGeneratedOnAdd();

                entity.Property(e => e.ADUM_USER_ID)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ADUM_USER_NAME)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.IMO_NO).HasMaxLength(1);

                entity.Property(e => e.LAST_UPDATE).HasColumnType("smalldatetime");

                entity.Property(e => e.LOCAL_USER_NAME)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.MOBILE_ID).HasMaxLength(100);

                entity.Property(e => e.PROFILE_IMAGE).HasMaxLength(100);
            });

            modelBuilder.Entity<AD_USER_MST_STREET>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("AD_USER_MST_STREET");

                entity.Property(e => e.ADUM_DESIGNATION)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ADUM_EMAIL)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.ADUM_EMPLOYEE_ID).HasMaxLength(50);

                entity.Property(e => e.ADUM_FRDT).HasColumnType("datetime");

                entity.Property(e => e.ADUM_LOGIN_ID)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.ADUM_MOBILE)
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.ADUM_PASSWORD)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ADUM_TODT).HasColumnType("datetime");

                entity.Property(e => e.ADUM_USER_CODE).ValueGeneratedOnAdd();

                entity.Property(e => e.ADUM_USER_ID)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ADUM_USER_NAME)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.IMO_NO).HasMaxLength(1);

                entity.Property(e => e.LAST_UPDATE).HasColumnType("smalldatetime");

                entity.Property(e => e.LOCAL_USER_NAME)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.MOBILE_ID).HasMaxLength(100);

                entity.Property(e => e.PROFILE_IMAGE).HasMaxLength(100);
            });

            modelBuilder.Entity<AEmployeeMaster>(entity =>
            {
                entity.HasKey(e => e.EmpId);

                entity.ToTable("AEmployeeMaster");

                entity.Property(e => e.EmpAddress).HasMaxLength(200);

                entity.Property(e => e.EmpMobileNumber).HasMaxLength(20);

                entity.Property(e => e.EmpName).HasMaxLength(500);

                entity.Property(e => e.EmpNameMar).HasMaxLength(200);

                entity.Property(e => e.LoginId).HasMaxLength(200);

                entity.Property(e => e.Password).HasMaxLength(100);

                entity.Property(e => e.lastModifyDateEntry).HasColumnType("datetime");

                entity.Property(e => e.type).HasMaxLength(50);
            });

            modelBuilder.Entity<AdminContact>(entity =>
            {
                entity.ToTable("AdminContact");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.MobileNumber).HasMaxLength(50);

                entity.Property(e => e.Position).HasMaxLength(500);
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

                entity.Property(e => e.AboutThumbnailURL).HasMaxLength(1000);

                entity.Property(e => e.Android_GCM_pushNotification_Key)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.AppLink).HasMaxLength(500);

                entity.Property(e => e.AppName).HasMaxLength(100);

                entity.Property(e => e.AppName_mar).HasMaxLength(100);

                entity.Property(e => e.AppVersion).HasMaxLength(500);

                entity.Property(e => e.CTPTQRCode).HasMaxLength(100);

                entity.Property(e => e.Collection).HasMaxLength(200);

                entity.Property(e => e.CommercialQRCode).HasMaxLength(100);

                entity.Property(e => e.ContactUs).HasMaxLength(50);

                entity.Property(e => e.ContactUsTeamMember).HasMaxLength(50);

                entity.Property(e => e.DumpYardPDF).HasMaxLength(250);

                entity.Property(e => e.DumpYardQRCode).HasMaxLength(250);

                entity.Property(e => e.EmailId).HasMaxLength(100);

                entity.Property(e => e.FAQ).HasMaxLength(50);

                entity.Property(e => e.HomeSplash).HasMaxLength(50);

                entity.Property(e => e.HousePDF).HasMaxLength(550);

                entity.Property(e => e.HouseQRCode).HasMaxLength(220);

                entity.Property(e => e.IsAreaActive).HasDefaultValueSql("((0))");

                entity.Property(e => e.LiquidQRCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.MsgForBroadcast).HasMaxLength(1000);

                entity.Property(e => e.MsgForMixed).HasMaxLength(1000);

                entity.Property(e => e.MsgForNotReceived).HasMaxLength(1000);

                entity.Property(e => e.MsgForNotSpecified).HasMaxLength(1000);

                entity.Property(e => e.MsgForSegregated).HasMaxLength(1000);

                entity.Property(e => e.PointPDF).HasMaxLength(550);

                entity.Property(e => e.PointQRCode).HasMaxLength(220);

                entity.Property(e => e.SWMQRCode).HasMaxLength(100);

                entity.Property(e => e.Status).HasDefaultValueSql("((0))");

                entity.Property(e => e.StreetQRCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Type)
                    .HasMaxLength(20)
                    .IsFixedLength(true);

                entity.Property(e => e.UserProfile).HasMaxLength(200);

                entity.Property(e => e.VehicalQRCode).HasMaxLength(250);

                entity.Property(e => e.YoccDndLink).HasMaxLength(250);

                entity.Property(e => e.YoccFeddbackLink).HasMaxLength(250);

                entity.Property(e => e.baseImageUrl).HasMaxLength(255);

                entity.Property(e => e.baseImageUrlCMS).HasMaxLength(255);

                entity.Property(e => e.basePath).HasMaxLength(40);

                entity.Property(e => e.website).HasMaxLength(100);
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

                entity.Property(e => e.ID).ValueGeneratedNever();

                entity.Property(e => e.AppName).HasMaxLength(100);

                entity.Property(e => e.DeviceId).HasMaxLength(50);
            });

            modelBuilder.Entity<CheckAppD>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("CheckAppD");

                entity.Property(e => e.App_Name)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Id).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<EmployeeMaster>(entity =>
            {
                entity.HasKey(e => e.EmpId);

                entity.ToTable("EmployeeMaster");

                entity.Property(e => e.EmpAddress).HasMaxLength(200);

                entity.Property(e => e.EmpMobileNumber).HasMaxLength(20);

                entity.Property(e => e.EmpName).HasMaxLength(500);

                entity.Property(e => e.EmpNameMar).HasMaxLength(200);

                entity.Property(e => e.LoginId).HasMaxLength(200);

                entity.Property(e => e.Password).HasMaxLength(100);

                entity.Property(e => e.lastModifyDateEntry).HasColumnType("datetime");

                entity.Property(e => e.type).HasMaxLength(50);
            });

            modelBuilder.Entity<Feedback_playstore>(entity =>
            {
                entity.HasKey(e => e.PlaystoreID);

                entity.ToTable("Feedback_playstore");
            });

            modelBuilder.Entity<GIS_AppConnection>(entity =>
            {
                entity.HasKey(e => e.AppConnectionId)
                    .HasName("PK__GIS_AppC__906CF5B097372769");

                entity.ToTable("GIS_AppConnection");

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

            modelBuilder.Entity<GameDetail>(entity =>
            {
                entity.HasKey(e => e.GameDetailsID);

                entity.Property(e => e.Created).HasColumnType("datetime");

                entity.Property(e => e.Description).HasMaxLength(500);

                entity.Property(e => e.ImageUrl).HasMaxLength(500);
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

                entity.Property(e => e.AnswerTypeHindi).HasMaxLength(50);

                entity.Property(e => e.AnswerTypeMar).HasMaxLength(50);
            });

            modelBuilder.Entity<Game_Slogan>(entity =>
            {
                entity.ToTable("Game_Slogan");

                entity.Property(e => e.Slogan).HasMaxLength(500);

                entity.Property(e => e.SloganHindi).HasMaxLength(500);

                entity.Property(e => e.SloganMar).HasMaxLength(500);
            });

            modelBuilder.Entity<GoogelHitDetail>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Date).HasColumnType("date");
            });

            modelBuilder.Entity<GoogleAPIDetail>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();
            });

            modelBuilder.Entity<HSUR_Daily_Attendance>(entity =>
            {
                entity.HasKey(e => e.daID);

                entity.ToTable("HSUR_Daily_Attendance");

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

            modelBuilder.Entity<RFID_Master>(entity =>
            {
                entity.ToTable("RFID_Master");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ReaderID).HasMaxLength(100);

                entity.Property(e => e.TagID).HasMaxLength(100);
            });

            modelBuilder.Entity<Sauchalay_feedback>(entity =>
            {
                entity.HasKey(e => e.SauchalayFeedback_ID)
                    .HasName("PK__Sauchala__4A571DFBA097DBFE");

                entity.ToTable("Sauchalay_feedback");

                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.Property(e => e.Rating).HasMaxLength(50);

                entity.Property(e => e.SauchalayID).HasMaxLength(50);

                entity.Property(e => e.ULB)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.que1).HasMaxLength(50);

                entity.Property(e => e.que10).HasMaxLength(50);

                entity.Property(e => e.que11).HasMaxLength(50);

                entity.Property(e => e.que2).HasMaxLength(50);

                entity.Property(e => e.que3).HasMaxLength(50);

                entity.Property(e => e.que5).HasMaxLength(50);

                entity.Property(e => e.que6).HasMaxLength(50);

                entity.Property(e => e.que7).HasMaxLength(50);

                entity.Property(e => e.que8).HasMaxLength(50);

                entity.Property(e => e.que9).HasMaxLength(50);
            });

            modelBuilder.Entity<Subscription>(entity =>
            {
                entity.Property(e => e.subscriptionName)
                    .IsRequired()
                    .HasMaxLength(30);
            });

            modelBuilder.Entity<UR_Location>(entity =>
            {
                entity.HasKey(e => e.locId)
                    .HasName("PK_Qr_Location");

                entity.ToTable("UR_Location");

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

            modelBuilder.Entity<UserInApp>(entity =>
            {
                entity.HasKey(e => e.UserInAppsID);

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.Property(e => e.subscriptionId).HasDefaultValueSql("((1))");
            });

            modelBuilder.Entity<country_state>(entity =>
            {
                entity.Property(e => e.country_name).HasMaxLength(90);

                entity.Property(e => e.state_2_code).HasMaxLength(2);

                entity.Property(e => e.state_3_code).HasMaxLength(3);

                entity.Property(e => e.state_name).HasMaxLength(250);

                entity.Property(e => e.state_name_mar).HasMaxLength(250);
            });

            modelBuilder.Entity<state_district>(entity =>
            {
                entity.Property(e => e.district_name)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.Property(e => e.district_name_mar).HasMaxLength(450);
            });

            modelBuilder.Entity<tehsil>(entity =>
            {
                entity.ToTable("tehsil");

                entity.Property(e => e.latitude).HasMaxLength(20);

                entity.Property(e => e.longitude).HasMaxLength(20);

                entity.Property(e => e.name).HasMaxLength(250);

                entity.Property(e => e.name_mar).HasMaxLength(250);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
