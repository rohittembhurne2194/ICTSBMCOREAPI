﻿using ICTSBMCOREAPI.Dal.DataContexts.Models.DB.ChildModels;
using ICTSBMCOREAPI.SwachhBhart.API.Bll.ViewModels.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ICTSBMCOREAPI.SwachhBharat.API.Bll.Repository.Repository
{
    public interface IRepository
    {
        Task<Result> CheckAttendenceAsync(int userId, int AppId, int typeId);
        Task<Result> CheckSupervisorAttendenceAsync(BigVQREmployeeAttendenceVM obj);
        Task<SBUser> CheckSupervisorUserLoginAsync(string userName, string password, string EmpType);
        Task<SBUser> CheckUserLoginAsync(string userName, string password, string imi, int AppId, string EmpType);
        Task<SBUser> CheckUserLoginForDumpAsync(string userName, string password, string imi, int AppId, string EmpType);
        Task<SBUser> CheckUserLoginForLiquidAsync(string userName, string password, string imi, int AppId, string EmpType);
        Task<SBUser> CheckUserLoginForStreetAsync(string userName, string password, string imi, int AppId, string EmpType);
        Task<Qr_Location> FillLocationDetailsAsync(BigVQRHPDVM obj, int AppId, bool IsOffline);
        Task<CollectionAppAreaLatLong> GetAppAreaLatLongAsync(int appId);
        Task<List<HouseDetailsVM>> GetAreaHouseAsync(int AppId, int areaId, string EmpType);
        Task<List<HouseDetailsVM>> GetAreaHouseForLiquidAsync(int AppId, int areaId);
        Task<List<HouseDetailsVM>> GetAreaHouseForNormalAsync(int AppId, int areaId);
        Task<List<HouseDetailsVM>> GetAreaHousForStreetAsync(int AppId, int areaId);
        Task<List<CMSBAreaVM>> GetAreaListAsync(int AppId, string SearchString);
        Task<List<GarbagePointDetailsVM>> GetAreaPointAsync(int AppId, int areaId);
        Task<IEnumerable<HSAttendanceGrid>> GetAttendanceDetailsAsync(int userId, DateTime FromDate, DateTime Todate, int appId);
        Task<List<SBArea>> GetCollectionAreaAsync(int AppId, int type, string EmpType);
        Task<List<SBArea>> GetCollectionAreaForLiquidAsync(int AppId, int type);
        Task<List<SBArea>> GetCollectionAreaForNormalAsync(int AppId, int type);
        Task<List<SBArea>> GetCollectionAreaForStreetAsync(int AppId, int type);
        Task<List<HouseDetailsVM>> GetDumpListAsync(int AppId);
        Task<List<DumpYardPointDetailsVM>> GetDumpYardAreaAsync(int AppId, int areaId);
        Task<IEnumerable<HSDumpYardDetailsGrid>> GetDumpYardDetailsAsync(int userId, DateTime FromDate, DateTime Todate, int appId);
        Task<List<HSHouseDetailsGrid>> GetDumpYardDetailsByIdAsync(int appId, string ReferanceId);
        Task<List<HSHouseDetailsGrid>> GetHDSLListAsync(int appId, string EmpType, string ReferanceId);
        Task<IEnumerable<HSHouseDetailsGrid>> GetHouseDetailsAsync(int userId, DateTime FromDate, DateTime Todate, int appId, string ReferanceId);
        Task<List<HouseDetailsVM>> GetHouseForNormalAsync(int AppId, string EmpType);
        Task<List<HouseDetailsVM>> GetHouseListAsync(int appId, string EmpType);
        Task<IEnumerable<HouseScanifyDetailsGridRow>> GetHouseScanifyDetailsAsync(int qrEmpId, DateTime FromDate, DateTime Todate, int appId);
        Task<IEnumerable<HSLiquidDetailsGrid>> GetLiquidDetailsAsync(int userId, DateTime FromDate, DateTime Todate, int appId);
        Task<List<HSHouseDetailsGrid>> GetLiquidDetailsByIdAsync(int appId, string ReferanceId);
        Task<List<HouseScanifyEmployeeDetails>> GetQREmployeeDetailsListAsync(int userId, string EmpType, int appId, int QrEmpID, bool status);
        Task<List<HSEmployee>> GetQREmployeeListAsync(int userId, string EmpType, int appId);
        Task<List<SBWorkDetails>> GetQrWorkHistoryAsync(int userId, int year, int month, int appId);
        Task<List<BigVQrworkhistorydetails>> GetQrWorkHistoryDetailsAsync(DateTime date, int AppId, int userId);
        Task<BigVQRHPDVM2> GetScanifyHouseDetailsDataAsync(int appId, string ReferenceId, int gcType);
        Task<HSDashboard> GetSelectedUlbDataAsync(int userId, string EmpType, int appId);
        Task<IEnumerable<HSStreetDetailsGrid>> GetStreetDetailsAsync(int userId, DateTime FromDate, DateTime Todate, int appId);
        Task<List<HSHouseDetailsGrid>> GetStreetDetailsByIdAsync(int appId, string ReferanceId);
        Task<List<NameULB>> GetUlbAsync(int userId, string Status);
        Task<SBUserView> GetUserAsync(int AppId, int userId, int typeId);
        Task<SyncResult2> GetUserMobileIdentificationAsync(int appId, int userId, bool isSync, int batteryStatus, string imeinos);
        Task<List<SBWorkDetails>> GetUserWorkAsync(int userId, int year, int month, int appId, string EmpType);

        Task<List<LatLongD>> GetLatLong(int appId, int userid, DateTime date);

        Task<List<SBWorkDetailsHistory>> GetUserWorkDetailsAsync(DateTime date, int appId, int userId, int languageId);
        Task<List<SBWorkDetails>> GetUserWorkForDumpAsync(int userId, int year, int month, int appId);
        Task<List<SBWorkDetails>> GetUserWorkForLiquidAsync(int userId, int year, int month, int appId);
        Task<List<SBWorkDetails>> GetUserWorkForNormalAsync(int userId, int year, int month, int appId);
        Task<List<SBWorkDetails>> GetUserWorkForStreetAsync(int userId, int year, int month, int appId);
        Task<List<SBVehicleType>> GetVehicleAsync(int appId);
        Task<List<VehicleList>> GetVehicleListAsync(int appId, int VehicleTypeId);
        Task<List<PropertyTypeList>> GetPropertyTypeListAsync(int appId);
        Task<List<HouseDetailsVM>> GetVehicleListAsync(int AppId);
        Task<Result> GetVersionUpdateAsync(string version, int AppId);
        Task<List<CMSBWardZoneVM>> GetWardZoneListAsync(int AppId);
        Task<List<CMSBZoneVM>> GetZoneAsync(int AppId, string SearchString);

        //Task<SBUser> CheckUserLoginForNormalAsync(string userName, string password, string imi, int AppId, string EmpType);
        public Task<string> LoginAsync(int AppId);
        public Task<GisLoginResult> GisLoginAsync(string userLoginId, string userPassword);
        Task<CollectionSyncResult> SaveAddEmployeeAsync(HouseScanifyEmployeeDetails obj, int AppId);
        Task<CollectionSyncResult> SaveAddUserRoleAsync(UserRoleDetails obj);
        Task<List<SBAAttendenceSettingsGridRow>> SaveAttendenceSettingsDetailAsync(int AppId, string hour);
        Task<CollectionSyncResult> SaveDumpCollectionSyncForLiquidAsync(SBGarbageCollectionView obj, int AppId, int type);
        Task<CollectionSyncResult> SaveDumpCollectionSyncForNormalAsync(SBGarbageCollectionView obj, int AppId, int type);
        Task<CollectionSyncResult> SaveDumpCollectionSyncForStreetAsync(SBGarbageCollectionView obj, int AppId, int type);
        Task<CollectionSyncResult> SaveGarbageCollectionOfflineAsync(SBGarbageCollectionView obj, int AppId, int type);
        Task<CollectionSyncResult> SaveHouseCollectionSyncAsync(SBGarbageCollectionView obj, int AppId, int type);
        Task<CollectionSyncResult> SaveLiquidCollectionSyncAsync(SBGarbageCollectionView obj, int AppId, int type);
        Task<CollectionSyncResult> SavePointCollectionSyncAsync(SBGarbageCollectionView obj, int AppId, int type);
        Task<Result> SaveQrEmployeeAttendenceAsync(BigVQREmployeeAttendenceVM obj, int AppId, int type);
        Task<Result> SaveQrHPDCollectionsAsync(BigVQRHPDVM obj, int AppId, int gcType);
        Task<Result> SaveHouseTrail(Trial obj, int AppId);
        Task<Result> SaveGarbageTrail(Trial obj, int AppId);
        Task<List<CollectionSyncResult>> SaveQrHPDCollectionsOfflineAsync(List<BigVQRHPDVM> obj, int AppId);
        Task<CollectionSyncResult> SaveStreetCollectionSyncAsync(SBGarbageCollectionView obj, int AppId, int type);
        Task<Result> SaveSupervisorAttendenceAsync(BigVQREmployeeAttendenceVM obj, int type);
        Task<Result> SaveUserAttendenceAsync(SBUserAttendence obj, int AppId, int type, string batteryStatus);

        Task<CollectionDumpSyncResult> SaveDumpyardTripCollection(DumpTripVM obj);
        Task<Result> SaveUserAttendenceForDumpAsync(SBUserAttendence obj, int AppId, int type, string batteryStatus);
        Task<Result> SaveUserAttendenceForLiquidAsync(SBUserAttendence obj, int AppId, int type, string batteryStatus);
        Task<Result> SaveUserAttendenceForNormalAsync(SBUserAttendence obj, int AppId, int type, string batteryStatus);
        Task<Result> SaveUserAttendenceForStreetAsync(SBUserAttendence obj, int AppId, int type, string batteryStatus);
        Task<List<SyncResult1>> SaveUserAttendenceOfflineAsync(List<SBUserAttendence> obj, int AppId, string cdate, string EmpType);
        Task<List<SyncResult1>> SaveUserAttendenceOfflineForDumpAsync(List<SBUserAttendence> obj, int AppId, string cdate, string EmpType);
        Task<List<SyncResult1>> SaveUserAttendenceOfflineForLiquidAsync(List<SBUserAttendence> obj, int AppId, string cdate, string EmpType);
        Task<List<SyncResult1>> SaveUserAttendenceOfflineForNormalAsync(List<SBUserAttendence> obj, int AppId, string cdate, string EmpType);
        Task<List<SyncResult1>> SaveUserAttendenceOfflineForStreetAsync(List<SBUserAttendence> obj, int AppId, string cdate, string EmpType);
        Task<List<SyncResult>> SaveUserLocationAsync(List<SBUserLocation> obj, int AppId, string batteryStatus, int typeId, string EmpType);
        Task<List<SyncResult>> SaveUserLocationNSLAsync(List<SBUserLocation> obj, int AppId, string batteryStatus, int typeId, string EmpType);
        Task<CollectionSyncResult> SaveUserLocationOfflineSyncAsync(SBGarbageCollectionView obj, int AppId, int typeId);
        Task<Result> SendSMSToHOuseAsync(int area, int AppId);
        Task<SBUser> SupervisorLoginAsync(string userName, string password, string EmpType);
        Task<CollectionQRStatusResult> UpdateQRstatusAsync(HSHouseDetailsGrid obj, int AppId);
        Task<IEnumerable<UREmployeeAttendence>> UserRoleAttendanceAsync(int userid, DateTime FromDate, DateTime Todate, bool type);
        Task<List<UserRoleDetails>> UserRoleListAsync(int userId, string EmpType, bool status, int EmpId);
        Task<CollectionSyncResult> SaveSurveyDetails(SurveyFormDetails svDetail, int appId);
        Task<List<SurveyFormDetail>> GetSurveyDetailsById(int AppId, string ReferanceId);
    }
}
