using ICTSBMCOREAPI.SwachhBharat.API.Bll.Repository.Repository;
using ICTSBMCOREAPI.SwachhBhart.API.Bll.ViewModels.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
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
        [HttpGet]
        [Route("Get/Zone")]
        public async Task<List<CMSBZoneVM>> GetZone(int AppId, string SearchString)
        {
            List<CMSBZoneVM> objDetail = new List<CMSBZoneVM>();
           
            objDetail = await objRep.GetZoneAsync(AppId, SearchString);
            return objDetail;
        }
        [HttpGet]
        [Route("Get/WardZoneList")]
        public async Task<List<CMSBWardZoneVM>> GetWardZoneList(int AppId)
        {
            List<CMSBWardZoneVM> objDetail = new List<CMSBWardZoneVM>();
            objDetail = await objRep.GetWardZoneListAsync(AppId);
            return objDetail;
        }

        [HttpGet]
        [Route("Get/AreaList")]
        public async Task<List<CMSBAreaVM>> GetAreaList(int AppId, string SearchString)
        {

            List<CMSBAreaVM> objDetail = new List<CMSBAreaVM>();
            objDetail = await objRep.GetAreaListAsync(AppId, SearchString);
            return objDetail;

        }
        [HttpGet]
        [Route("Get/CollectionArea")]
        public async Task<List<SBArea>> GetColectionArea([FromHeader] int AppId, [FromHeader] int type, [FromHeader] string EmpType)
        {

            List<SBArea> objDetail = new List<SBArea>();

            objDetail = await objRep.GetCollectionAreaAsync(AppId, type, EmpType);
            return objDetail;

        }


        [HttpGet]
        [Route("Get/AreaHouse")]
        public async Task<List<HouseDetailsVM>> GetHouseAreaWise([FromHeader] int AppId, [FromHeader] int areaId, [FromHeader] string EmpType)
        {

            List<HouseDetailsVM> objDetail = new List<HouseDetailsVM>();
            objDetail = await objRep.GetAreaHouseAsync(AppId, areaId, EmpType);
            return objDetail;

        }


        [HttpGet]
        [Route("Get/AppAreaLatLong")]
        //api/BookATable/GetBookAtableList
        public async Task<CollectionAppAreaLatLong> GetAppAreaLatLong([FromHeader] int AppId)
        {
            
            CollectionAppAreaLatLong objDetail = new CollectionAppAreaLatLong();
            objDetail = await objRep.GetAppAreaLatLongAsync(AppId);
            return objDetail;
        }


        [HttpGet]
        [Route("Get/SendSms")]
        public async Task<Result> SendNotificationt([FromHeader] int AppId, [FromHeader] int areaId)
        {
            Result objDetail = new Result();
            objDetail = await objRep.SendSMSToHOuseAsync(areaId, AppId);
            return objDetail;

        }



        [HttpGet]
        [Route("Get/AreaPoint")]
        public async Task<List<GarbagePointDetailsVM>> GetPointAreaWise([FromHeader] int AppId, [FromHeader] int areaId)
        {

            List<GarbagePointDetailsVM> objDetail = new List<GarbagePointDetailsVM>();
            objDetail = await objRep.GetAreaPointAsync(AppId, areaId);
            return objDetail;

        }


        [HttpGet]
        [Route("Get/DumpYardPoint")]
        public async Task<List<DumpYardPointDetailsVM>> GetDumpYardAreaWise([FromHeader] int AppId, [FromHeader] int areaId)
        {

            List<DumpYardPointDetailsVM> objDetail = new List<DumpYardPointDetailsVM>();
            objDetail = await objRep.GetDumpYardAreaAsync(AppId, areaId);
            return objDetail;

        }

        [HttpGet]
        [Route("Get/GetUserMobileIdentification")]
        public async Task<SyncResult2> GetUserMobileIdentification([FromHeader] int AppId, [FromHeader] int userId, [FromHeader] bool isSync, [FromHeader] int batteryStatus, [FromHeader] string imeinos)
        {

            SyncResult2 objDetail = new SyncResult2();

            objDetail = await objRep.GetUserMobileIdentificationAsync(AppId, userId, isSync, batteryStatus, imeinos);
            return objDetail;
        }


        [HttpPost]
        [Route("Save/HouseMapTrail")]
        public async Task<List<DumpTripStatusResult>>HouseMapTrail([FromBody] List<Trial> obj,[FromHeader] string url, [FromHeader] string username, [FromHeader] string password)
        {
            Trial tn = new Trial();
            List<DumpTripStatusResult> objDetail = new List<DumpTripStatusResult>();
            foreach (var item in obj)
            {
                //tn.startTs = item.startTs;
                //tn.endTs = item.endTs;
                tn.createUser = item.createUser;
                tn.geom = item.geom;
                tn.createTs = item.createTs;
                tn.houseId = item.houseId;
                tn.updateTs = item.updateTs;
                tn.updateUser = item.updateUser;

                HttpClient client = new HttpClient();
                var json = JsonConvert.SerializeObject(tn, Formatting.Indented);
                var stringContent = new StringContent(json);
                stringContent.Headers.ContentType.MediaType = "application/json";
                stringContent.Headers.Add("url",url);
                stringContent.Headers.Add("username", username);
                stringContent.Headers.Add("password", password);


                //HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "http://114.143.244.130:9091/house");
                //request.Content = new StringContent("{\"url\":\"jdbc:postgresql://192.168.100.51:5434/sbmgis\",\"username\":\"sbmgis\",\"password\":\"sbmgis\"}", Encoding.UTF8,"application/json");//CONTENT-TYPE header
                //request.Content.Headers.Add("url", url);
                //request.Content.Headers.Add("username", username);
                //request.Content.Headers.Add("password", password);
                //await client.SendAsync(request)
                //        .ContinueWith(async responseTask =>
                //        {
                //            Console.WriteLine("Response: {0}", responseTask.Result);
                //            var Content = await responseTask.Result.Content.ReadAsStringAsync();
                //        });
                
                var response = await client.PostAsync("http://114.143.244.130:9091/house", stringContent);

                


                var responseString = await response.Content.ReadAsStringAsync();

                objDetail.Add(new DumpTripStatusResult()
                {
                    code = responseString.ToString(),
                    status = responseString.ToString(),
                    message = responseString.ToString(),
                    errorMessages = responseString.ToString(),
                    timestamp = responseString.ToString(),
                    data = responseString.ToString()
                });
            }
            return objDetail;

            //foreach (var item in obj)
            //{
            //    tn.startTs = item.startTs;
            //    tn.endTs = item.endTs;
            //    tn.createUser = item.createUser;
            //    tn.geom = item.geom;
            //    tn = await objRep.SaveHouseMapTrail(tn);

            //}
            //return objDetail;

        }
    }
    
}

