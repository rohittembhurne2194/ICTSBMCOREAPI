using ICTSBMCOREAPI.Dal.DataContexts.Models.DB;
using ICTSBMCOREAPI.SwachhBharat.API.Bll.Repository.Repository;
using ICTSBMCOREAPI.SwachhBhart.API.Bll.ViewModels.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
        public async Task<Result> SaveQrEmployeeAttendence([FromHeader] int AppId, [FromBody] BigVQREmployeeAttendenceVM obj)
        {

            Result objDetail = new Result();
            objDetail = await objRep.SaveQrEmployeeAttendenceAsync(obj, AppId, 0);
            return objDetail;
        }
        [HttpPost]
        [Route("Save/QrEmployeeAttendenceOut")]
        public async Task<Result> SaveQrEmployeeAttendenceOut([FromHeader] int AppId, [FromBody] BigVQREmployeeAttendenceVM obj)
        {
            Result objDetail = new Result();
            objDetail = await objRep.SaveQrEmployeeAttendenceAsync(obj, AppId, 1);
            return objDetail;
        }

        [HttpPost]
        [Route("Save/QrHPDCollections")]
        public async Task<Result> SaveQrHPDCollections([FromHeader] int AppId, [FromHeader] int gcType, [FromBody] BigVQRHPDVM obj)
        {

           // string referanceid = (string.IsNullOrEmpty(referanceId) ? "" : referanceId);

            string houseid1 = (string.IsNullOrEmpty(obj.ReferanceId) ? "" : obj.ReferanceId);
            string[] houseList = houseid1.Split(',');

            if (houseList.Length > 1)
            {
                obj.ReferanceId = houseList[0];
                obj.wastetype = houseList[1];

            }

            //string[] referancList = referanceId.Split(',');

            //if (referancList.Length > 1)
            //{
            //    referanceid = referancList[0];

            //}
            Result objDetail1 = new Result();
            objDetail1 = await objRep.SaveQrHPDCollectionsAsync(obj, AppId, gcType);
            if (objDetail1.status == "success")
            {
                var message = "";

                using DevICTSBMMainEntities dbMain = new DevICTSBMMainEntities();
                //TimeSpan timespan = new TimeSpan(00, 00, 00);
                //DateTime time = DateTime.Now.Add(timespan);

                Trial tn = new Trial();
                List<DumpTripStatusResult> objDetail = new List<DumpTripStatusResult>();

                try
                {
                    var GIS_CON = dbMain.GIS_AppConnections.Where(c => c.AppId == AppId).FirstOrDefault();

                    if (GIS_CON != null)
                    {
                        var gis_url = GIS_CON.DataSource;
                        var gis_DBName = GIS_CON.InitialCatalog;
                        var gis_username = GIS_CON.UserId;
                        var gis_password = GIS_CON.Password;

                        //foreach (var item in obj)
                        //{
                        tn.startTs = obj.startTs;
                        tn.endTs = obj.endTs;
                        tn.geom = obj.geom;

                        tn.houseId = Convert.ToInt32(objDetail1.houseid);
                        if (objDetail1.IsExixts == true)
                        {
                            tn.updateTs = obj.createTs;
                            tn.updateUser = obj.userId;
                        }
                        else
                        {
                            tn.createUser = obj.userId;
                            tn.createTs = obj.createTs;
                        }


                        HttpClient client = new HttpClient();
                        var json = JsonConvert.SerializeObject(tn, Formatting.Indented);
                        var stringContent = new StringContent(json);
                        stringContent.Headers.ContentType.MediaType = "application/json";
                        stringContent.Headers.Add("url", gis_url + "/" + gis_DBName);
                        stringContent.Headers.Add("username", gis_username);
                        stringContent.Headers.Add("password", gis_password);
                        var response = await client.PostAsync("http://114.143.244.130:9091/house", stringContent);
                        var responseString = await response.Content.ReadAsStringAsync();
                        var dynamicobject = JsonConvert.DeserializeObject<dynamic>(responseString);
                        objDetail.Add(new DumpTripStatusResult()
                        {
                            code = dynamicobject.code.ToString(),
                            status = dynamicobject.status.ToString(),
                            message = dynamicobject.message.ToString(),
                            errorMessages = dynamicobject.errorMessages.ToString(),
                            timestamp = dynamicobject.timestamp.ToString(),
                            data = dynamicobject.data.ToString()
                        });
                        objDetail1.gismessage = dynamicobject.message.ToString();
                        objDetail1.giserrorMessages = dynamicobject.errorMessages.ToString();
                    }
                    else
                    {

                        objDetail.Add(new DumpTripStatusResult()
                        {
                            code = "",
                            status = "",
                            message = "",
                            errorMessages = "GIS Connection Are Not Available",
                            timestamp = "",
                            data = ""
                        });
                         objDetail1.gismessage= "GIS Connection Are Not Available";
                    }
                }
                catch (Exception ex)
                {
                    objDetail.Add(new DumpTripStatusResult()
                    {
                        code = "",
                        status = "",
                        message = "",
                        errorMessages = ex.Message.ToString(),
                        timestamp = "",
                        data = ""
                    });
                     objDetail1.giserrorMessages = ex.Message.ToString();
                }
                
                // }
                //   return objDetail;
            }
            return objDetail1;
        }

        [HttpPost]
        [Route("Save/QrHPDCollectionsOffline")]
        public async Task<List<CollectionSyncResult>> SaveQrHPDCollectionsOffline([FromHeader] int AppId, [FromBody] List<BigVQRHPDVM> obj)
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
        public async Task<List<BigVQrworkhistorydetails>> GetQRWorkDetails([FromHeader] int AppId, [FromHeader] int userId, [FromHeader] string date)
        {

            DateTime Date = Convert.ToDateTime(date);

            List<BigVQrworkhistorydetails> objDetail = new List<BigVQrworkhistorydetails>();
            objDetail = await objRep.GetQrWorkHistoryDetailsAsync(Date, AppId, userId);
            return objDetail;
        }
        [HttpGet]
        [Route("Get/ScanifyHouse")]
        public async Task<BigVQRHPDVM2> GetScanifyHouseDetailsData([FromHeader] int AppId, [FromHeader] string ReferenceId, [FromHeader] int gcType)
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
