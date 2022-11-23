using System;
using System.Collections.Generic;

#nullable disable

namespace ICTSBMCOREAPI.Dal.DataContexts.Models.DB.MainModels
{
    public partial class AppDetail
    {
        public int AppId { get; set; }
        public string AppName { get; set; }
        public string AppNameMar { get; set; }
        public int? State { get; set; }
        public int? Tehsil { get; set; }
        public int? District { get; set; }
        public string EmailId { get; set; }
        public string Website { get; set; }
        public string AndroidGcmPushNotificationKey { get; set; }
        public bool? IsProduction { get; set; }
        public string BaseImageUrlCms { get; set; }
        public string BaseImageUrl { get; set; }
        public string AboutThumbnailUrl { get; set; }
        public string AboutAppynity { get; set; }
        public string AboutTeamDetail { get; set; }
        public string ContactUsTeamMember { get; set; }
        public string HomeSplash { get; set; }
        public string Faq { get; set; }
        public string ContactUs { get; set; }
        public string BasePath { get; set; }
        public string YoccContact { get; set; }
        public string Type { get; set; }
        public string Logo { get; set; }
        public string Latitude { get; set; }
        public string Logitude { get; set; }
        public string UserProfile { get; set; }
        public string Collection { get; set; }
        public string HouseQrcode { get; set; }
        public string PointQrcode { get; set; }
        public string HousePdf { get; set; }
        public string PointPdf { get; set; }
        public string GrampanchayatPro { get; set; }
        public string AppVersion { get; set; }
        public bool? ForceUpdate { get; set; }
        public int? Apihit { get; set; }
        public bool? NewFeatures { get; set; }
        public bool? ReportEnable { get; set; }
        public string DumpYardQrcode { get; set; }
        public string DumpYardPdf { get; set; }
        public int? GramPanchyatAppId { get; set; }
        public int? YoccClientId { get; set; }
        public string YoccFeddbackLink { get; set; }
        public string YoccDndLink { get; set; }
        public bool? IsActive { get; set; }
        public int? LanguageId { get; set; }
        public string MsgForBroadcast { get; set; }
        public string MsgForNotSpecified { get; set; }
        public string MsgForMixed { get; set; }
        public string MsgForNotReceived { get; set; }
        public string MsgForSegregated { get; set; }
        public bool? IsScanNear { get; set; }
        public string LiquidQrcode { get; set; }
        public string StreetQrcode { get; set; }
        public string CommercialQrcode { get; set; }
        public string Ctptqrcode { get; set; }
        public string Swmqrcode { get; set; }
        public int? UlbProperty { get; set; }
        public int? AddUlbProperty { get; set; }
        public bool? TodayWasteStatus { get; set; }
        public bool? TodayLiquidStatus { get; set; }
        public bool? TodayStreetStatus { get; set; }
        public bool? Status { get; set; }
        public string VehicalQrcode { get; set; }
        public string AppAreaLatLong { get; set; }
        public bool? IsAreaActive { get; set; }
        public int? TodayHouseScanCount { get; set; }
        public int? TodayLiquidScanCount { get; set; }
        public int? TodayStreetScanCount { get; set; }
        public int? TotalHouseCount { get; set; }
        public int? TotalLiquidCount { get; set; }
        public int? TotalStreetCount { get; set; }
        public int? TodayDumpScanCount { get; set; }
        public int? TotalDumpCount { get; set; }
        public string AppLink { get; set; }
        public int? TodayMixedCount { get; set; }
        public int? TodaySegregatedCount { get; set; }
        public int? TodayGarbageNrcount { get; set; }
        public int? TotalEmployeeActive { get; set; }
        public int? TodayEmployeeDutyOn { get; set; }
        public int? TodayEmployeeDutyOff { get; set; }
    }
}
