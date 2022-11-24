using System;
using System.Collections.Generic;

#nullable disable

namespace ICTSBMCOREAPI.Dal.DataContexts.Models.DB.MainModels
{
    public partial class AppDetail
    {
        public int AppId { get; set; }
        public string AppName { get; set; }
        public string AppName_mar { get; set; }
        public int? State { get; set; }
        public int? Tehsil { get; set; }
        public int? District { get; set; }
        public string EmailId { get; set; }
        public string website { get; set; }
        public string Android_GCM_pushNotification_Key { get; set; }
        public bool? IsProduction { get; set; }
        public string baseImageUrlCMS { get; set; }
        public string baseImageUrl { get; set; }
        public string AboutThumbnailURL { get; set; }
        public string AboutAppynity { get; set; }
        public string AboutTeamDetail { get; set; }
        public string ContactUsTeamMember { get; set; }
        public string HomeSplash { get; set; }
        public string FAQ { get; set; }
        public string ContactUs { get; set; }
        public string basePath { get; set; }
        public string yoccContact { get; set; }
        public string Type { get; set; }
        public string Logo { get; set; }
        public string Latitude { get; set; }
        public string Logitude { get; set; }
        public string UserProfile { get; set; }
        public string Collection { get; set; }
        public string HouseQRCode { get; set; }
        public string PointQRCode { get; set; }
        public string HousePDF { get; set; }
        public string PointPDF { get; set; }
        public string Grampanchayat_Pro { get; set; }
        public string AppVersion { get; set; }
        public bool? ForceUpdate { get; set; }
        public int? APIHit { get; set; }
        public bool? NewFeatures { get; set; }
        public bool? ReportEnable { get; set; }
        public string DumpYardQRCode { get; set; }
        public string DumpYardPDF { get; set; }
        public int? GramPanchyatAppID { get; set; }
        public int? YoccClientID { get; set; }
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
        public string LiquidQRCode { get; set; }
        public string StreetQRCode { get; set; }
        public string CommercialQRCode { get; set; }
        public string CTPTQRCode { get; set; }
        public string SWMQRCode { get; set; }
        public int? ulb_property { get; set; }
        public int? Add_Ulb_Property { get; set; }
        public bool? Today_Waste_Status { get; set; }
        public bool? Today_Liquid_Status { get; set; }
        public bool? Today_Street_Status { get; set; }
        public bool? Status { get; set; }
        public string VehicalQRCode { get; set; }
        public string AppAreaLatLong { get; set; }
        public bool? IsAreaActive { get; set; }
        public int? Today_HouseScanCount { get; set; }
        public int? Today_LiquidScanCount { get; set; }
        public int? Today_StreetScanCount { get; set; }
        public int? Total_HouseCount { get; set; }
        public int? Total_LiquidCount { get; set; }
        public int? Total_StreetCount { get; set; }
        public int? Today_DumpScanCount { get; set; }
        public int? Total_DumpCount { get; set; }
        public string AppLink { get; set; }
        public int? Today_MixedCount { get; set; }
        public int? Today_SegregatedCount { get; set; }
        public int? Today_GarbageNRCount { get; set; }
        public int? Total_EmployeeActive { get; set; }
        public int? Today_EmployeeDutyOn { get; set; }
        public int? Today_EmployeeDutyOff { get; set; }
    }
}
