using ICTSBMCOREAPI.SwachhBharat.API.Bll.Repository.Repository;
using ICTSBMCOREAPI.SwachhBhart.API.Bll.ViewModels.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ICTSBMCOREAPI.Controllers
{
    [Authorize]
    [Route("api")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IRepository objRep;
        public UserController(ILogger<UserController> logger, IRepository repository)
        {
            _logger = logger;
            objRep = repository;

        }

        [HttpPost]
        [Route("Save/UserAttendenceIn")]

        public async Task<Result> SaveUserAttendence([FromHeader] int AppId, [FromHeader] string batteryStatus, SBUserAttendence obj)
        {
            Result objDetail = new Result();
            objDetail = await objRep.SaveUserAttendenceAsync(obj, AppId, 0, batteryStatus);
            return objDetail;
        }

        [HttpPost]
        [Route("Save/UserAttendenceOut")]

        public async Task<Result> SaveUserAttendenceOut([FromHeader] int AppId, [FromHeader] string batteryStatus, SBUserAttendence obj)
        {
            Result objDetail = new Result();
            objDetail = await objRep.SaveUserAttendenceAsync(obj, AppId, 1, batteryStatus);
            return objDetail;
        }

        [HttpPost]
        [Route("Save/UserLocation")]
        public async Task<List<SyncResult>> SaveUserLocation([FromHeader] int AppId, [FromHeader] string batteryStatus, [FromHeader] int typeId, [FromHeader] string EmpType, [FromBody] List<SBUserLocation> obj)
        {

            List<SyncResult> objDetail = new List<SyncResult>();
            objDetail = await objRep.SaveUserLocationAsync(obj, AppId, batteryStatus, typeId, EmpType);
            return objDetail;
        }

        [HttpGet]
        [Route("Get/User")]
        public async Task<SBUserView> GetUser([FromHeader] int AppId, [FromHeader] int userId, [FromHeader] int typeId)
        {
            
            SBUserView objDetail = new SBUserView();
            objDetail = await objRep.GetUserAsync(AppId, userId, typeId);
            return objDetail;
        }
        [HttpGet]
        [Route("Get/VersionUpdate")]
        public async Task<Result> GetVersionUpdate([FromHeader] int AppId, [FromHeader] string version)
        {
            Result objDetail = new Result();
            objDetail = await objRep.GetVersionUpdateAsync(version, AppId);
            return objDetail;

        }

        [HttpGet]
        [Route("Get/IsAttendence")]
        public async Task<Result> CheckAttendence([FromHeader] int AppId, [FromHeader] int userId, [FromHeader] int typeId)
        {
            
            Result objDetail = new Result();
            objDetail = await objRep.CheckAttendenceAsync(userId, AppId, typeId);
            return objDetail;

        }
        [HttpPost]
        [Route("Save/AttendenceOffline")]
        public async Task<List<SyncResult1>> SaveUserAttendenceOffline([FromHeader] int AppId, [FromHeader] string date, [FromHeader] string EmpType, [FromBody]List<SBUserAttendence> obj)
        {
            List<SyncResult1> objDetail = new List<SyncResult1>();
            objDetail = await objRep.SaveUserAttendenceOfflineAsync(obj, AppId, date, EmpType);
            return objDetail;
        }
        [HttpGet]
        [Route("Get/WorkHistory")]
        //api/BookATable/GetBookAtableList
        public async Task<List<SBWorkDetails>> GetWork([FromHeader] int AppId, [FromHeader] int userId, [FromHeader] int year, [FromHeader] int month, [FromHeader] string EmpType)
        {
            
            List<SBWorkDetails> objDetail = new List<SBWorkDetails>();
            objDetail = await objRep.GetUserWorkAsync(userId, year, month, AppId, EmpType);
            return objDetail.OrderByDescending(c => c.date).ToList();
        }

        [HttpGet]
        [Route("Get/WorkHistory/Details")]
        //api/BookATable/GetBookAtableList
        public async Task<List<SBWorkDetailsHistory>> GetWorkDetails([FromHeader] int AppId, [FromHeader] int userId, [FromHeader] int languageId, [FromHeader] DateTime fdate)
        {
            
           
            List<SBWorkDetailsHistory> objDetail = new List<SBWorkDetailsHistory>();
            objDetail = await objRep.GetUserWorkDetailsAsync(fdate, AppId, userId, languageId);
            return objDetail;
        }

    }
}
