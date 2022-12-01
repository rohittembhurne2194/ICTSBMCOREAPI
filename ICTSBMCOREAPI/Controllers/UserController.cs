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
    }
}
