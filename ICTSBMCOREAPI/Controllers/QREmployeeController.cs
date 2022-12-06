using ICTSBMCOREAPI.Dal.DataContexts.Models.DB;
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
    public class QREmployeeController : ControllerBase
    {
        private readonly ILogger<QREmployeeController> _logger;
        private readonly IRepository objRep;
        private readonly DevICTSBMMainEntities dbMain;
        public QREmployeeController(ILogger<QREmployeeController> logger, IRepository repository, DevICTSBMMainEntities dbMain)
        {
            _logger = logger;
            objRep = repository;
            this.dbMain = dbMain;

        }


        [HttpPost]
        [Route("Save/QrEmployeeAttendenceIn")]
        public async Task<Result> SaveQrEmployeeAttendence([FromHeader] int AppId, [FromBody]BigVQREmployeeAttendenceVM obj)
        {
            
            Result objDetail = new Result();
            objDetail = await objRep.SaveQrEmployeeAttendenceAsync(obj, AppId, 0);
            return objDetail;
        }
        [HttpPost]
        [Route("Save/QrEmployeeAttendenceOut")]
        public async Task<Result> SaveQrEmployeeAttendenceOut([FromHeader] int AppId, [FromBody]BigVQREmployeeAttendenceVM obj)
        {
            Result objDetail = new Result();
            objDetail = await objRep.SaveQrEmployeeAttendenceAsync(obj, AppId, 1);
            return objDetail;
        }

        [HttpPost]
        [Route("Save/QrHPDCollections")]
        public async Task<Result> SaveQrHPDCollections([FromHeader] int AppId, [FromHeader] string referanceId, [FromHeader] int gcType, [FromBody]BigVQRHPDVM obj)
        {
            
            string referanceid = (string.IsNullOrEmpty(referanceId) ? "" : referanceId);
            
            string houseid1 = (string.IsNullOrEmpty(obj.ReferanceId) ? "" : obj.ReferanceId);
            string[] houseList = houseid1.Split(',');

            if (houseList.Length > 1)
            {
                obj.ReferanceId = houseList[0];
                obj.wastetype = houseList[1];

            }

            string[] referancList = referanceId.Split(',');

            if (referancList.Length > 1)
            {
                referanceid = referancList[0];

            }
            Result objDetail = new Result();
            objDetail = await objRep.SaveQrHPDCollectionsAsync(obj, AppId, referanceid, gcType);
            return objDetail;
        }

        [HttpPost]
        [Route("Save/QrHPDCollectionsOffline")]
        public async Task<List<CollectionSyncResult>> SaveQrHPDCollectionsOffline([FromHeader] int AppId,[FromBody]List<BigVQRHPDVM> obj)
        {

            List<CollectionSyncResult> objDetail = new List<CollectionSyncResult>();
            objDetail = await objRep.SaveQrHPDCollectionsOfflineAsync(obj, AppId);
            return objDetail;
        }


        [HttpGet]
        [Route("Get/QrWorkHistory")]
        //api/BookATable/GetBookAtableList
        public async Task<List<SBWorkDetails>> GetWork([FromHeader] int AppId, [FromHeader] int userId, [FromHeader] int year, [FromHeader] int month)
        {
            
            List<SBWorkDetails> objDetail = new List<SBWorkDetails>();
            objDetail = await objRep.GetQrWorkHistoryAsync(userId, year, month, AppId);
            return objDetail.OrderByDescending(c => c.date).ToList();
        }

        [HttpGet]
        [Route("Get/QrWorkHistoryDetails")]
        //api/BookATable/GetBookAtableList
        public async Task<List<BigVQrworkhistorydetails>> GetQRWorkDetails([FromHeader] int AppId, [FromHeader] int userId,[FromHeader] string date)
        {
           
            DateTime Date = Convert.ToDateTime(date);

            List<BigVQrworkhistorydetails> objDetail = new List<BigVQrworkhistorydetails>();
            objDetail = await objRep.GetQrWorkHistoryDetailsAsync(Date, AppId, userId);
            return objDetail;
        }
        [HttpGet]
        [Route("Get/ScanifyHouse")]
        public async Task<BigVQRHPDVM2> GetScanifyHouseDetailsData([FromHeader] int AppId, [FromHeader] string ReferenceId,[FromHeader] int gcType)
        {

            BigVQRHPDVM2 objDetail = new BigVQRHPDVM2();
            objDetail = await objRep.GetScanifyHouseDetailsDataAsync(AppId, ReferenceId, gcType);
            return objDetail;

        }


        [HttpGet]
        [Route("Get/Vehicles")]
        public async Task<List<VehicleList>> VehicleList([FromHeader] int AppId, [FromHeader] int VehicleTypeId)
        {
            
            List<VehicleList> objDetail = new List<VehicleList>();
            objDetail = await objRep.GetVehicleListAsync(AppId, VehicleTypeId);
            return objDetail;
        }
    }
}
