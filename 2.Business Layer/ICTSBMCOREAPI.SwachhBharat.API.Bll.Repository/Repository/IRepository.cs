using ICTSBMCOREAPI.SwachhBhart.API.Bll.ViewModels.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ICTSBMCOREAPI.SwachhBharat.API.Bll.Repository.Repository
{
    public interface IRepository
    {
        Task<SBUser> CheckUserLoginAsync(string userName, string password, string imi, int AppId, string EmpType);
        Task<SBUser> CheckUserLoginForDumpAsync(string userName, string password, string imi, int AppId, string EmpType);
        Task<SBUser> CheckUserLoginForLiquidAsync(string userName, string password, string imi, int AppId, string EmpType);
        Task<SBUser> CheckUserLoginForStreetAsync(string userName, string password, string imi, int AppId, string EmpType);

        //Task<SBUser> CheckUserLoginForNormalAsync(string userName, string password, string imi, int AppId, string EmpType);
        public Task<string> LoginAsync(int AppId);
        Task<Result> SaveUserAttendenceAsync(SBUserAttendence obj, int AppId, int type, string batteryStatus);
        Task<Result> SaveUserAttendenceForDumpAsync(SBUserAttendence obj, int AppId, int type, string batteryStatus);
        Task<Result> SaveUserAttendenceForLiquidAsync(SBUserAttendence obj, int AppId, int type, string batteryStatus);
        Task<Result> SaveUserAttendenceForNormalAsync(SBUserAttendence obj, int AppId, int type, string batteryStatus);
        Task<Result> SaveUserAttendenceForStreetAsync(SBUserAttendence obj, int AppId, int type, string batteryStatus);
    }
}
