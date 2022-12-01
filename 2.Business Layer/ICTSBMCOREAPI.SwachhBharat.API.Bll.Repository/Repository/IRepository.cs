using ICTSBMCOREAPI.Dal.DataContexts.Models.DB.ChildModels;
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
        Task<SBUser> CheckUserLoginAsync(string userName, string password, string imi, int AppId, string EmpType);
        Task<SBUser> CheckUserLoginForDumpAsync(string userName, string password, string imi, int AppId, string EmpType);
        Task<SBUser> CheckUserLoginForLiquidAsync(string userName, string password, string imi, int AppId, string EmpType);
        Task<SBUser> CheckUserLoginForStreetAsync(string userName, string password, string imi, int AppId, string EmpType);
        Task<Qr_Location> FillLocationDetailsAsync(BigVQRHPDVM obj, int AppId, bool IsOffline);
        Task<List<SBWorkDetails>> GetQrWorkHistoryAsync(int userId, int year, int month, int appId);
        Task<List<BigVQrworkhistorydetails>> GetQrWorkHistoryDetailsAsync(DateTime date, int AppId, int userId);
        Task<BigVQRHPDVM2> GetScanifyHouseDetailsDataAsync(int appId, string ReferenceId, int gcType);
        Task<SBUserView> GetUserAsync(int AppId, int userId, int typeId);
        Task<List<SBWorkDetails>> GetUserWorkAsync(int userId, int year, int month, int appId, string EmpType);
        Task<List<SBWorkDetailsHistory>> GetUserWorkDetailsAsync(DateTime date, int appId, int userId, int languageId);
        Task<List<SBWorkDetails>> GetUserWorkForDumpAsync(int userId, int year, int month, int appId);
        Task<List<SBWorkDetails>> GetUserWorkForLiquidAsync(int userId, int year, int month, int appId);
        Task<List<SBWorkDetails>> GetUserWorkForNormalAsync(int userId, int year, int month, int appId);
        Task<List<SBWorkDetails>> GetUserWorkForStreetAsync(int userId, int year, int month, int appId);
        Task<List<SBVehicleType>> GetVehicleAsync(int appId);
        Task<List<VehicleList>> GetVehicleListAsync(int appId, int VehicleTypeId);
        Task<Result> GetVersionUpdateAsync(string version, int AppId);

        //Task<SBUser> CheckUserLoginForNormalAsync(string userName, string password, string imi, int AppId, string EmpType);
        public Task<string> LoginAsync(int AppId);
        Task<List<SBAAttendenceSettingsGridRow>> SaveAttendenceSettingsDetailAsync(int AppId, string hour);
        Task<CollectionSyncResult> SaveDumpCollectionSyncForLiquidAsync(SBGarbageCollectionView obj, int AppId, int type);
        Task<CollectionSyncResult> SaveDumpCollectionSyncForNormalAsync(SBGarbageCollectionView obj, int AppId, int type);
        Task<CollectionSyncResult> SaveDumpCollectionSyncForStreetAsync(SBGarbageCollectionView obj, int AppId, int type);
        Task<CollectionSyncResult> SaveGarbageCollectionOfflineAsync(SBGarbageCollectionView obj, int AppId, int type);
        Task<CollectionSyncResult> SaveHouseCollectionSyncAsync(SBGarbageCollectionView obj, int AppId, int type);
        Task<CollectionSyncResult> SaveLiquidCollectionSyncAsync(SBGarbageCollectionView obj, int AppId, int type);
        Task<CollectionSyncResult> SavePointCollectionSyncAsync(SBGarbageCollectionView obj, int AppId, int type);
        Task<Result> SaveQrEmployeeAttendenceAsync(BigVQREmployeeAttendenceVM obj, int AppId, int type);
        Task<Result> SaveQrHPDCollectionsAsync(BigVQRHPDVM obj, int AppId, string referanceid, int gcType);
        Task<List<CollectionSyncResult>> SaveQrHPDCollectionsOfflineAsync(List<BigVQRHPDVM> obj, int AppId);
        Task<CollectionSyncResult> SaveStreetCollectionSyncAsync(SBGarbageCollectionView obj, int AppId, int type);
        Task<Result> SaveUserAttendenceAsync(SBUserAttendence obj, int AppId, int type, string batteryStatus);
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
    }
}
