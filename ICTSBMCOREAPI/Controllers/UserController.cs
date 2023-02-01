using ICTSBMCOREAPI.Dal.DataContexts.Models.DB;
using ICTSBMCOREAPI.Dal.DataContexts.Models.DB.ChildSPModels;
using ICTSBMCOREAPI.SwachhBharat.API.Bll.Repository.Repository;
using ICTSBMCOREAPI.SwachhBhart.API.Bll.ViewModels.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Nancy.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace ICTSBMCOREAPI.Controllers
{
    [Authorize]
    [Route("api")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IRepository objRep;
        private object result;

        public UserController(ILogger<UserController> logger, IRepository repository)
        {
            _logger = logger;
            objRep = repository;

        }

        [HttpPost]
        [Route("Save/UserAttendenceIn")]

        public async Task<ActionResult<Result>> SaveUserAttendence([FromHeader] string authorization, [FromHeader] int AppId, [FromHeader] string batteryStatus, SBUserAttendence obj)
        {
            var stream = authorization.Replace("Bearer ", string.Empty);
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(stream);
            var tokenS = jsonToken as JwtSecurityToken;

            var jti = tokenS.Claims.First(claim => claim.Type == "AppId").Value;


            var Auth_AppId = Convert.ToInt32(jti);

            if (Auth_AppId == AppId)
            {
                Result objDetail = new Result();
                objDetail = await objRep.SaveUserAttendenceAsync(obj, AppId, 0, batteryStatus);
                return objDetail;
            }
            else
            {
                return Unauthorized();
            }

        }

        [HttpPost]
        [Route("Save/UserAttendenceOut")]

        public async Task<ActionResult<Result>> SaveUserAttendenceOut([FromHeader] string authorization, [FromHeader] int AppId, [FromHeader] string batteryStatus, SBUserAttendence obj)
        {


            var stream = authorization.Replace("Bearer ", string.Empty);
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(stream);
            var tokenS = jsonToken as JwtSecurityToken;

            var jti = tokenS.Claims.First(claim => claim.Type == "AppId").Value;


            var Auth_AppId = Convert.ToInt32(jti);

            if (Auth_AppId == AppId)
            {
                Result objDetail = new Result();
                objDetail = await objRep.SaveUserAttendenceAsync(obj, AppId, 1, batteryStatus);
                return objDetail;
            }
            else
            {
                return Unauthorized();
            }
        }

        [HttpPost]
        [Route("Save/UserLocation")]
        public async Task<ActionResult<List<SyncResult>>> SaveUserLocation([FromHeader] string authorization, [FromHeader] int AppId, [FromHeader] string batteryStatus, [FromHeader] int typeId, [FromHeader] string EmpType, [FromBody] List<SBUserLocation> obj)
        {


            var stream = authorization.Replace("Bearer ", string.Empty);
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(stream);
            var tokenS = jsonToken as JwtSecurityToken;

            var jti = tokenS.Claims.First(claim => claim.Type == "AppId").Value;


            var Auth_AppId = Convert.ToInt32(jti);

            if (Auth_AppId == AppId)
            {
                List<SyncResult> objDetail = new List<SyncResult>();
                objDetail = await objRep.SaveUserLocationAsync(obj, AppId, batteryStatus, typeId, EmpType);
                return objDetail;

            }
            else
            {
                return Unauthorized();
            }
        }

        [HttpGet]
        [Route("Get/User")]
        public async Task<ActionResult<SBUserView>> GetUser([FromHeader] string authorization, [FromHeader] int AppId, [FromHeader] int userId, [FromHeader] int typeId)
        {

            var stream = authorization.Replace("Bearer ", string.Empty);
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(stream);
            var tokenS = jsonToken as JwtSecurityToken;

            var jti = tokenS.Claims.First(claim => claim.Type == "AppId").Value;


            var Auth_AppId = Convert.ToInt32(jti);

            if (Auth_AppId == AppId)
            {
                SBUserView objDetail = new SBUserView();
                objDetail = await objRep.GetUserAsync(AppId, userId, typeId);
                return objDetail;

            }
            else
            {
                return Unauthorized();
            }
        }
        [HttpGet]
        [Route("Get/VersionUpdate")]
        public async Task<ActionResult<Result>> GetVersionUpdate([FromHeader] string authorization, [FromHeader] int AppId, [FromHeader] string version)
        {

            var stream = authorization.Replace("Bearer ", string.Empty);
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(stream);
            var tokenS = jsonToken as JwtSecurityToken;

            var jti = tokenS.Claims.First(claim => claim.Type == "AppId").Value;


            var Auth_AppId = Convert.ToInt32(jti);

            if (Auth_AppId == AppId)
            {
                Result objDetail = new Result();
                objDetail = await objRep.GetVersionUpdateAsync(version, AppId);
                return objDetail;

            }
            else
            {
                return Unauthorized();
            }
        }

        [HttpGet]
        [Route("Get/IsAttendence")]
        public async Task<ActionResult<Result>> CheckAttendence([FromHeader] string authorization, [FromHeader] int AppId, [FromHeader] int userId, [FromHeader] int typeId)
        {
            var stream = authorization.Replace("Bearer ", string.Empty);
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(stream);
            var tokenS = jsonToken as JwtSecurityToken;

            var jti = tokenS.Claims.First(claim => claim.Type == "AppId").Value;


            var Auth_AppId = Convert.ToInt32(jti);

            if (Auth_AppId == AppId)
            {
                Result objDetail = new Result();
                objDetail = await objRep.CheckAttendenceAsync(userId, AppId, typeId);
                return objDetail;

            }
            else
            {
                return Unauthorized();
            }

        }
        [HttpPost]
        [Route("Save/AttendenceOffline")]
        public async Task<ActionResult<List<SyncResult1>>> SaveUserAttendenceOffline([FromHeader] string authorization, [FromHeader] int AppId, [FromHeader] string date, [FromHeader] string EmpType, [FromBody] List<SBUserAttendence> obj)
        {
            var stream = authorization.Replace("Bearer ", string.Empty);
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(stream);
            var tokenS = jsonToken as JwtSecurityToken;

            var jti = tokenS.Claims.First(claim => claim.Type == "AppId").Value;


            var Auth_AppId = Convert.ToInt32(jti);

            if (Auth_AppId == AppId)
            {
                List<SyncResult1> objDetail = new List<SyncResult1>();
                objDetail = await objRep.SaveUserAttendenceOfflineAsync(obj, AppId, date, EmpType);
                return objDetail;

            }
            else
            {
                return Unauthorized();
            }
        }
        [HttpGet]
        [Route("Get/WorkHistory")]
        //api/BookATable/GetBookAtableList
        public async Task<ActionResult<List<SBWorkDetails>>> GetWork([FromHeader] string authorization, [FromHeader] int AppId, [FromHeader] int userId, [FromHeader] int year, [FromHeader] int month, [FromHeader] string EmpType)
        {
            var stream = authorization.Replace("Bearer ", string.Empty);
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(stream);
            var tokenS = jsonToken as JwtSecurityToken;

            var jti = tokenS.Claims.First(claim => claim.Type == "AppId").Value;


            var Auth_AppId = Convert.ToInt32(jti);

            if (Auth_AppId == AppId)
            {
                List<SBWorkDetails> objDetail = new List<SBWorkDetails>();
                objDetail = await objRep.GetUserWorkAsync(userId, year, month, AppId, EmpType);
                return objDetail.OrderByDescending(c => c.date).ToList();

            }
            else
            {
                return Unauthorized();
            }
        }

        [HttpGet]
        [Route("Get/GetLatLongD")]
        public async Task<ActionResult<List<LatLongD>>> GetLatLong([FromHeader] string authorization, [FromHeader] int AppId, [FromHeader] int userId, [FromHeader] DateTime fdate)
        {
            var stream = authorization.Replace("Bearer ", string.Empty);
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(stream);
            var tokenS = jsonToken as JwtSecurityToken;

            var jti = tokenS.Claims.First(claim => claim.Type == "AppId").Value;


            var Auth_AppId = Convert.ToInt32(jti);

            if (Auth_AppId == AppId)
            {
                List<LatLongD> objDetail = new List<LatLongD>();
                objDetail = await objRep.GetLatLong(AppId, userId, fdate);
                return objDetail.OrderByDescending(c => c.RefferenceId).ToList(); ;
            }
            else
            {
                return Unauthorized();
            }
        }


        [HttpGet]
        [Route("Get/WorkHistory/Details")]
        //api/BookATable/GetBookAtableList
        public async Task<ActionResult<List<SBWorkDetailsHistory>>> GetWorkDetails([FromHeader] string authorization, [FromHeader] int AppId, [FromHeader] int userId, [FromHeader] int languageId, [FromHeader] DateTime fdate)
        {
            var stream = authorization.Replace("Bearer ", string.Empty);
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(stream);
            var tokenS = jsonToken as JwtSecurityToken;

            var jti = tokenS.Claims.First(claim => claim.Type == "AppId").Value;


            var Auth_AppId = Convert.ToInt32(jti);

            if (Auth_AppId == AppId)
            {

                List<SBWorkDetailsHistory> objDetail = new List<SBWorkDetailsHistory>();
                objDetail = await objRep.GetUserWorkDetailsAsync(fdate, AppId, userId, languageId);
                return objDetail;

            }
            else
            {
                return Unauthorized();
            }
        }
        [HttpGet]
        [Route("Get/Zone")]
        public async Task<ActionResult<List<CMSBZoneVM>>> GetZone([FromHeader] string authorization, int AppId, string SearchString)
        {
            var stream = authorization.Replace("Bearer ", string.Empty);
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(stream);
            var tokenS = jsonToken as JwtSecurityToken;

            var jti = tokenS.Claims.First(claim => claim.Type == "AppId").Value;


            var Auth_AppId = Convert.ToInt32(jti);

            if (Auth_AppId == AppId)
            {
                List<CMSBZoneVM> objDetail = new List<CMSBZoneVM>();

                objDetail = await objRep.GetZoneAsync(AppId, SearchString);
                return objDetail;

            }
            else
            {
                return Unauthorized();
            }
        }
        [HttpGet]
        [Route("Get/WardZoneList")]
        public async Task<ActionResult<List<CMSBWardZoneVM>>> GetWardZoneList([FromHeader] string authorization, int AppId)
        {
            var stream = authorization.Replace("Bearer ", string.Empty);
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(stream);
            var tokenS = jsonToken as JwtSecurityToken;

            var jti = tokenS.Claims.First(claim => claim.Type == "AppId").Value;


            var Auth_AppId = Convert.ToInt32(jti);

            if (Auth_AppId == AppId)
            {

                List<CMSBWardZoneVM> objDetail = new List<CMSBWardZoneVM>();
                objDetail = await objRep.GetWardZoneListAsync(AppId);
                return objDetail;

            }
            else
            {
                return Unauthorized();
            }
        }

        [HttpGet]
        [Route("Get/AreaList")]
        public async Task<ActionResult<List<CMSBAreaVM>>> GetAreaList([FromHeader] string authorization, int AppId, string SearchString)
        {
            var stream = authorization.Replace("Bearer ", string.Empty);
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(stream);
            var tokenS = jsonToken as JwtSecurityToken;

            var jti = tokenS.Claims.First(claim => claim.Type == "AppId").Value;


            var Auth_AppId = Convert.ToInt32(jti);

            if (Auth_AppId == AppId)
            {
                List<CMSBAreaVM> objDetail = new List<CMSBAreaVM>();
                objDetail = await objRep.GetAreaListAsync(AppId, SearchString);
                return objDetail;

            }
            else
            {
                return Unauthorized();
            }

        }
        [HttpGet]
        [Route("Get/CollectionArea")]
        public async Task<ActionResult<List<SBArea>>> GetColectionArea([FromHeader] string authorization, [FromHeader] int AppId, [FromHeader] int type, [FromHeader] string EmpType)
        {
            var stream = authorization.Replace("Bearer ", string.Empty);
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(stream);
            var tokenS = jsonToken as JwtSecurityToken;

            var jti = tokenS.Claims.First(claim => claim.Type == "AppId").Value;


            var Auth_AppId = Convert.ToInt32(jti);

            if (Auth_AppId == AppId)
            {

                List<SBArea> objDetail = new List<SBArea>();

                objDetail = await objRep.GetCollectionAreaAsync(AppId, type, EmpType);
                return objDetail;

            }
            else
            {
                return Unauthorized();
            }

        }


        [HttpGet]
        [Route("Get/AreaHouse")]
        public async Task<ActionResult<List<HouseDetailsVM>>> GetHouseAreaWise([FromHeader] string authorization, [FromHeader] int AppId, [FromHeader] int areaId, [FromHeader] string EmpType)
        {
            var stream = authorization.Replace("Bearer ", string.Empty);
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(stream);
            var tokenS = jsonToken as JwtSecurityToken;

            var jti = tokenS.Claims.First(claim => claim.Type == "AppId").Value;


            var Auth_AppId = Convert.ToInt32(jti);

            if (Auth_AppId == AppId)
            {
                List<HouseDetailsVM> objDetail = new List<HouseDetailsVM>();
                objDetail = await objRep.GetAreaHouseAsync(AppId, areaId, EmpType);
                return objDetail;

            }
            else
            {
                return Unauthorized();
            }

        }


        [HttpGet]
        [Route("Get/AppAreaLatLong")]
        //api/BookATable/GetBookAtableList
        public async Task<ActionResult<CollectionAppAreaLatLong>> GetAppAreaLatLong([FromHeader] string authorization, [FromHeader] int AppId)
        {
            var stream = authorization.Replace("Bearer ", string.Empty);
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(stream);
            var tokenS = jsonToken as JwtSecurityToken;

            var jti = tokenS.Claims.First(claim => claim.Type == "AppId").Value;


            var Auth_AppId = Convert.ToInt32(jti);

            if (Auth_AppId == AppId)
            {
                CollectionAppAreaLatLong objDetail = new CollectionAppAreaLatLong();
                objDetail = await objRep.GetAppAreaLatLongAsync(AppId);
                return objDetail;

            }
            else
            {
                return Unauthorized();
            }
        }


        [HttpGet]
        [Route("Get/SendSms")]
        public async Task<ActionResult<Result>> SendNotificationt([FromHeader] string authorization, [FromHeader] int AppId, [FromHeader] int areaId)
        {
            var stream = authorization.Replace("Bearer ", string.Empty);
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(stream);
            var tokenS = jsonToken as JwtSecurityToken;

            var jti = tokenS.Claims.First(claim => claim.Type == "AppId").Value;


            var Auth_AppId = Convert.ToInt32(jti);

            if (Auth_AppId == AppId)
            {
                Result objDetail = new Result();
                objDetail = await objRep.SendSMSToHOuseAsync(areaId, AppId);
                return objDetail;

            }
            else
            {
                return Unauthorized();
            }

        }



        [HttpGet]
        [Route("Get/AreaPoint")]
        public async Task<ActionResult<List<GarbagePointDetailsVM>>> GetPointAreaWise([FromHeader] string authorization, [FromHeader] int AppId, [FromHeader] int areaId)
        {
            var stream = authorization.Replace("Bearer ", string.Empty);
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(stream);
            var tokenS = jsonToken as JwtSecurityToken;

            var jti = tokenS.Claims.First(claim => claim.Type == "AppId").Value;


            var Auth_AppId = Convert.ToInt32(jti);

            if (Auth_AppId == AppId)
            {
                List<GarbagePointDetailsVM> objDetail = new List<GarbagePointDetailsVM>();
                objDetail = await objRep.GetAreaPointAsync(AppId, areaId);
                return objDetail;

            }
            else
            {
                return Unauthorized();
            }


        }


        [HttpGet]
        [Route("Get/DumpYardPoint")]
        public async Task<ActionResult<List<DumpYardPointDetailsVM>>> GetDumpYardAreaWise([FromHeader] string authorization, [FromHeader] int AppId, [FromHeader] int areaId)
        {
            var stream = authorization.Replace("Bearer ", string.Empty);
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(stream);
            var tokenS = jsonToken as JwtSecurityToken;

            var jti = tokenS.Claims.First(claim => claim.Type == "AppId").Value;


            var Auth_AppId = Convert.ToInt32(jti);

            if (Auth_AppId == AppId)
            {
                List<DumpYardPointDetailsVM> objDetail = new List<DumpYardPointDetailsVM>();
                objDetail = await objRep.GetDumpYardAreaAsync(AppId, areaId);
                return objDetail;

            }
            else
            {
                return Unauthorized();
            }


        }

        [HttpGet]
        [Route("Get/GetUserMobileIdentification")]
        public async Task<ActionResult<SyncResult2>> GetUserMobileIdentification([FromHeader] string authorization, [FromHeader] int AppId, [FromHeader] int userId, [FromHeader] bool isSync, [FromHeader] int batteryStatus, [FromHeader] string imeinos)
        {

            var stream = authorization.Replace("Bearer ", string.Empty);
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(stream);
            var tokenS = jsonToken as JwtSecurityToken;

            var jti = tokenS.Claims.First(claim => claim.Type == "AppId").Value;


            var Auth_AppId = Convert.ToInt32(jti);

            if (Auth_AppId == AppId)
            {
                SyncResult2 objDetail = new SyncResult2();

                objDetail = await objRep.GetUserMobileIdentificationAsync(AppId, userId, isSync, batteryStatus, imeinos);
                return objDetail;

            }
            else
            {
                return Unauthorized();
            }
        }


        [HttpPost]
        [Route("Save/GarbageMapTrail")]
        public async Task<ActionResult<List<DumpTripStatusResult>>> GarbageMapTrail([FromHeader] string authorization, [FromBody] Trial obj, [FromHeader] int AppId)
        {
            var message = "";

            using DevICTSBMMainEntities dbMain = new DevICTSBMMainEntities();
            List<DumpTripStatusResult> objDetail = new List<DumpTripStatusResult>();

            var stream = authorization.Replace("Bearer ", string.Empty);
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(stream);
            var tokenS = jsonToken as JwtSecurityToken;

            var jti = tokenS.Claims.First(claim => claim.Type == "AppId").Value;


            var Auth_AppId = Convert.ToInt32(jti);

            if (Auth_AppId == AppId)
            {
                try
                {
                    var GIS_CON = dbMain.GIS_AppConnections.Where(c => c.AppId == AppId).FirstOrDefault();

                    if (GIS_CON != null)
                    {
                        var gis_url = GIS_CON.DataSource;
                        var gis_DBName = GIS_CON.InitialCatalog;
                        var gis_username = GIS_CON.UserId;
                        var gis_password = GIS_CON.Password;

                        HttpClient client = new HttpClient();
                        Trial tn = new Trial();

                        //foreach (var item in obj)
                        //{
                        //    tn.id = item.id;
                        //    tn.startTs = item.startTs;
                        //    tn.endTs = item.endTs;
                        //    tn.createUser = item.createUser;
                        //    tn.geom = item.geom;
                        //    tn.createTs = item.createTs;
                        //    tn.updateTs = item.updateTs;
                        //    tn.updateUser = item.updateUser;


                        //var json = JsonConvert.SerializeObject(tn, Formatting.Indented);
                        //var stringContent = new StringContent(json);
                        //stringContent.Headers.ContentType.MediaType = "application/json";
                        //stringContent.Headers.Add("url", gis_url + "/" + gis_DBName);
                        //stringContent.Headers.Add("username", gis_username);
                        //stringContent.Headers.Add("password", gis_password);
                        //var response = await client.PostAsync("http://114.143.244.130:9091/garbage-trail", stringContent);


                        client.BaseAddress = new Uri("http://114.143.244.130:9091/");
                        //client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        client.DefaultRequestHeaders.Add("url", gis_url + "/" + gis_DBName);
                        client.DefaultRequestHeaders.Add("username", gis_username);
                        client.DefaultRequestHeaders.Add("password", gis_password);
                        var response = await client.PostAsJsonAsync("garbage-trail", obj);

                        if (response.IsSuccessStatusCode)
                        {
                            var responseString = await response.Content.ReadAsStringAsync();
                            var dynamicobject = JsonConvert.DeserializeObject<dynamic>(responseString);
                            objDetail.Add(new DumpTripStatusResult()
                            {
                                code = (int)response.StatusCode,
                                status = dynamicobject.status.ToString(),
                                message = dynamicobject.message.ToString(),
                                errorMessages = dynamicobject.errorMessages.ToString(),
                                timestamp = DateTime.Now.ToString(),
                                data = dynamicobject.data.ToString()
                            });
                        }
                        else
                        {
                            objDetail.Add(new DumpTripStatusResult()
                            {
                                code = (int)response.StatusCode,
                                status = "Failed",
                                message = "Not Found",
                                timestamp = DateTime.Now.ToString()
                            });
                        }
                        //}
                        result = objDetail;
                        return Ok(result);
                    }
                    else
                    {

                        objDetail.Add(new DumpTripStatusResult()
                        {
                            code = 404,
                            status = "Failed",
                            message = "GIS Connection Are Not Available",
                            timestamp = DateTime.Now.ToString(),
                            data = ""
                        });
                        result = objDetail;

                        return NotFound(result);


                    }
                }

                catch (Exception ex)
                {
                    //_logger.LogError(ex.ToString(), ex);

                    objDetail.Add(new DumpTripStatusResult()
                    {
                        code = 400,
                        status = "Failed",
                        message = ex.Message.ToString(),
                        errorMessages = ex.Message.ToString(),
                        timestamp = DateTime.Now.ToString(),
                        data = ""
                    });
                    result = objDetail;

                }
                return BadRequest(result);
            }
            else
            {
                objDetail.Add(new DumpTripStatusResult()
                {
                    code = 401,
                    status = "Failed",
                    message = "Unauthorized",
                    timestamp = DateTime.Now.ToString(),
                });

                result = objDetail;
                return Unauthorized(result);
            }

        }
        [HttpPost]
        [Route("Save/HouseMapTrail")]
        public async Task<ActionResult<List<DumpTripStatusResult>>> HouseMapTrail([FromHeader] string authorization, [FromBody] Trial obj, [FromHeader] int AppId)
        {
            var message = "";

            using DevICTSBMMainEntities dbMain = new DevICTSBMMainEntities();
            List<DumpTripStatusResult> objDetail = new List<DumpTripStatusResult>();

            var stream = authorization.Replace("Bearer ", string.Empty);
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(stream);
            var tokenS = jsonToken as JwtSecurityToken;

            var jti = tokenS.Claims.First(claim => claim.Type == "AppId").Value;


            var Auth_AppId = Convert.ToInt32(jti);

            if (Auth_AppId == AppId)
            {
                try
                {
                    var GIS_CON = dbMain.GIS_AppConnections.Where(c => c.AppId == AppId).FirstOrDefault();

                    if (GIS_CON != null)
                    {
                        var gis_url = GIS_CON.DataSource;
                        var gis_DBName = GIS_CON.InitialCatalog;
                        var gis_username = GIS_CON.UserId;
                        var gis_password = GIS_CON.Password;


                        HttpClient client = new HttpClient();
                        Trial tn = new Trial();

                        //foreach (var item in obj)
                        //{
                        //    tn.id = item.id;
                        //    tn.startTs = item.startTs;
                        //    tn.endTs = item.endTs;
                        //    tn.createUser = item.createUser;
                        //    tn.geom = item.geom;
                        //    tn.createTs = item.createTs;
                        //    tn.updateTs = item.updateTs;
                        //    tn.updateUser = item.updateUser;

                        //var json = JsonConvert.SerializeObject(tn, Formatting.Indented);
                        //var stringContent = new StringContent(json);
                        //stringContent.Headers.ContentType.MediaType = "application/json";
                        //stringContent.Headers.Add("url", gis_url + "/" + gis_DBName);
                        //stringContent.Headers.Add("username", gis_username);
                        //stringContent.Headers.Add("password", gis_password);

                        //var response = await client.PostAsync("http://114.143.244.130:9091/house-trail", stringContent);



                        client.BaseAddress = new Uri("http://114.143.244.130:9091/");
                        //client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        client.DefaultRequestHeaders.Add("url", gis_url + "/" + gis_DBName);
                        client.DefaultRequestHeaders.Add("username", gis_username);
                        client.DefaultRequestHeaders.Add("password", gis_password);
                        var response = await client.PostAsJsonAsync("house-trail", obj);

                        if (response.IsSuccessStatusCode)
                        {
                            var responseString = await response.Content.ReadAsStringAsync();
                            var dynamicobject = JsonConvert.DeserializeObject<dynamic>(responseString);
                            objDetail.Add(new DumpTripStatusResult()
                            {
                                code = (int)response.StatusCode,
                                status = dynamicobject.status.ToString(),
                                message = dynamicobject.message.ToString(),
                                errorMessages = dynamicobject.errorMessages.ToString(),
                                timestamp = dynamicobject.timestamp.ToString(),
                                data = dynamicobject.data.ToString()
                            });
                        }
                        else
                        {
                            objDetail.Add(new DumpTripStatusResult()
                            {
                                code = (int)response.StatusCode,
                                status = "Failed",
                                message = response.RequestMessage.ToString(),
                                timestamp = DateTime.Now.ToString(),
                            });
                        }
                        //}
                        result = objDetail;
                        return Ok(result);
                    }
                    else
                    {

                        objDetail.Add(new DumpTripStatusResult()
                        {
                            code = 404,
                            status = "Failed",
                            message = "GIS Connection Are Not Available",
                            timestamp = DateTime.Now.ToString(),
                            data = ""
                        });
                        result = objDetail;
                        return NotFound(result);
                    }
                }
                catch (Exception ex)
                {
                    objDetail.Add(new DumpTripStatusResult()
                    {
                        code = 400,
                        status = "Failed",
                        message = ex.Message.ToString(),
                        timestamp = DateTime.Now.ToString(),
                        data = ""
                    });
                    result = objDetail;
                }

                return BadRequest(result);
            }
            else
            {
                objDetail.Add(new DumpTripStatusResult()
                {
                    code = 401,
                    status = "Failed",
                    message = "Unauthorized",
                    timestamp = DateTime.Now.ToString(),
                });

                result = objDetail;
                return Unauthorized(result);
            }

        }

        [HttpGet]
        [Route("GisHouseDetails/all")]
        [EnableCors("MyCorsPolicy")]
        public async Task<ActionResult<HouseGisDetails>> HouseGisDetailsAll([FromHeader] string authorization, [FromHeader] int AppId)
        {

            //var message = "";

            using DevICTSBMMainEntities dbMain = new();
            HouseGisDetails objDetail = new();

            var stream = authorization.Replace("Bearer ", string.Empty);
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(stream);
            var tokenS = jsonToken as JwtSecurityToken;

            var jti = tokenS.Claims.First(claim => claim.Type == "AppId").Value;


            var Auth_AppId = Convert.ToInt32(jti);

            //Extract the payload of the JWT
            //var claims = tokenS.Claims;

            //var jwtPayload = "{";
            //foreach (Claim c in claims)
            //{
            //    jwtPayload += '"' + c.Type + "\":\"" + c.Value + "\",";
            //}
            //jwtPayload += "}";
            //txtJwtOut += "\r\nPayload:\r\n" + JToken.Parse(jwtPayload).ToString(Formatting.Indented);



            if (Auth_AppId == AppId)
            {
                try
                {
                    var GIS_CON = dbMain.GIS_AppConnections.Where(c => c.AppId == AppId).FirstOrDefault();

                    if (GIS_CON != null)
                    {
                        var gis_url = GIS_CON.DataSource;
                        var gis_DBName = GIS_CON.InitialCatalog;
                        var gis_username = GIS_CON.UserId;
                        var gis_password = GIS_CON.Password;

                        HttpClient client = new();
                        //Trial tn = new Trial();

                        //Start Old Code
                        //client.DefaultRequestHeaders.Add("url", gis_url + "/" + gis_DBName);
                        //client.DefaultRequestHeaders.Add("username", gis_username);
                        //client.DefaultRequestHeaders.Add("password", gis_password);

                        //var url = "http://114.143.244.130:9091/house/all";

                        //var response = await client.GetAsync(url);
                        //End Old Code

                        //Start New Code.
                        client.BaseAddress = new Uri("http://114.143.244.130:9091/");
                        //client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        client.DefaultRequestHeaders.Add("url", gis_url + "/" + gis_DBName);
                        client.DefaultRequestHeaders.Add("username", gis_username);
                        client.DefaultRequestHeaders.Add("password", gis_password);
                        var response = await client.GetAsync("house/all");
                        //End New Code

                        if (response.IsSuccessStatusCode)
                        {
                            var responseString = await response.Content.ReadAsStringAsync();
                            //var jsonParsed = JObject.Parse(responseString);
                            var dynamicobject = JsonConvert.DeserializeObject<dynamic>(responseString, new JsonSerializerSettings { DateParseHandling = DateParseHandling.None });

                            var jsonResult = dynamicobject["data"];
                            //Response.Headers.Add("Access-Control-Allow-Origin", "*");
                            //Response.Headers.Add("Access-Control-Allow-Methods", "GET");
                            //Response.Headers.Add("Access-Control-Allow-Headers", "Content-Type");

                            List<GisResult> myresult = jsonResult.ToObject<List<GisResult>>();

                            List<HouseGisDetails> obj = new();

                            using (DevICTSBMChildEntities db = new(AppId))
                            {

                                foreach (var c in myresult)
                                {
                                    if (Convert.ToInt32(c.id) != 0)
                                    {
                                        var house = await db.HouseMasters.Where(x => x.houseId == Convert.ToInt32(c.id)).Select(x => new { x.ReferanceId, x.houseId, x.userId, x.houseOwner, x.houseOwnerMobile, x.houseAddress }).FirstOrDefaultAsync();


                                        var EmployeeName = await db.QrEmployeeMasters.Where(x => x.qrEmpId == Convert.ToInt32(c.createUser)).Select(x => new { x.qrEmpName }).FirstOrDefaultAsync();
                                        var Update_EmployeeName = await db.QrEmployeeMasters.Where(x => x.qrEmpId == Convert.ToInt32(c.updateUser)).Select(x => new { x.qrEmpName }).FirstOrDefaultAsync();


                                        var result1 = myresult.Select(i =>
                                        {
                                            if (i.id == house.houseId)
                                            {
                                                //i.ReferanceId = house.ReferanceId;
                                                //i.HouseOwnerName = house.houseOwner;
                                                //i.HouseOwnerMobileNo = house.houseOwnerMobile;
                                                //i.HouseAddress = house.houseAddress;
                                                //i.CreateEmployeeName = EmployeeName.qrEmpName.ToString();
                                                //if(Update_EmployeeName != null)
                                                //{
                                                //    i.UpdateEmployeeName = Update_EmployeeName.qrEmpName.ToString();

                                                //}
                                                //else
                                                //{
                                                //    i.UpdateEmployeeName = "";
                                                //}

                                                var value = new List<HouseProperty> {
                                                    new HouseProperty { name = "ReferanceId", value = house.ReferanceId, type = "String", Index = 0 },
                                                    new HouseProperty { name = "Create Employee Name", value = (EmployeeName == null ? "" : EmployeeName.qrEmpName.ToString()), type = "String", Index = 1 },
                                                    new HouseProperty { name = "Update Employee Name", value = (Update_EmployeeName == null ? "" : Update_EmployeeName.qrEmpName.ToString()), type = "String", Index = 2 },
                                                    new HouseProperty { name = "House Owner Name", value = house.houseOwner, type = "String", Index = 3 }
                                                };


                                                i.HouseProperty = value;
                                                return i;
                                            }
                                            return i;

                                        }).Where(i => i.id == house.houseId).ToList();

                                    }
                                }



                            }
                            // return objDetail;

                            //objDetail.Add(new HouseGisDetails()
                            //{
                            //    code = dynamicobject.code.ToString(),
                            //    status = dynamicobject.status.ToString(),
                            //    message = dynamicobject.message.ToString(),
                            //    timestamp = dynamicobject.timestamp.ToString(),
                            //    data = result
                            //});

                            objDetail.code = (int)response.StatusCode;
                            objDetail.status = dynamicobject.status.ToString();
                            objDetail.message = dynamicobject.message.ToString();
                            objDetail.timestamp = DateTime.Now.ToString();
                            objDetail.data = myresult;

                            result = objDetail;
                            return Ok(result);

                        }
                        else
                        {
                            objDetail.code = (int)response.StatusCode;
                            objDetail.status = "Failed";
                            objDetail.message = "";
                            objDetail.timestamp = DateTime.Now.ToString();

                            result = objDetail;
                            return NotFound(result);
                        }
                    }
                    else
                    {

                        //objDetail.Add(new HouseGisDetails()
                        //{
                        //    code = "",
                        //    status = "",
                        //    message = "GIS Connection Are Not Available",
                        //   // errorMessages = "GIS Connection Are Not Available",
                        //    timestamp = "",
                        //    data = ""
                        //});
                        objDetail.code = 401;
                        objDetail.status = "Failed";
                        objDetail.message = "GIS Connection Are Not Available";
                        objDetail.timestamp = DateTime.Now.ToString();

                        result = objDetail;
                        return NotFound(result);
                    }
                }
                catch (Exception ex)
                {
                    //objDetail.Add(new HouseGisDetails()
                    //{
                    //    code = "",
                    //    status = "",
                    //    message = ex.Message.ToString(),
                    //    //errorMessages = ex.Message.ToString(),
                    //    timestamp = "",
                    //    data = ""
                    //});

                    objDetail.code = 400;
                    objDetail.status = "Failed";
                    objDetail.message = ex.Message.ToString();
                    objDetail.timestamp = DateTime.Now.ToString();

                    result = objDetail;
                    return BadRequest(result);
                }
                //return objDetail;
            }
            else
            {

                objDetail.code = 401;
                objDetail.status = "Failed";
                objDetail.message = "Unauthorized";
                objDetail.timestamp = DateTime.Now.ToString();


                result = objDetail;
                return Unauthorized(result);
            }


        }

        [HttpPost]
        [Route("GisHouseDetails/search")]
        [EnableCors("MyCorsPolicy")]
        public async Task<ActionResult<HouseGisDetails>> HouseGisDetailsSearch([FromHeader] string authorization, [FromHeader] int AppId, [FromBody] GisSearch obj)
        {
            using DevICTSBMMainEntities dbMain = new DevICTSBMMainEntities();
            HouseGisDetails objDetail = new HouseGisDetails();

            var stream = authorization.Replace("Bearer ", string.Empty);
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(stream);
            var tokenS = jsonToken as JwtSecurityToken;

            var jti = tokenS.Claims.First(claim => claim.Type == "AppId").Value;


            var Auth_AppId = Convert.ToInt32(jti);

            if (Auth_AppId == AppId)
            {
                try
                {
                    var GIS_CON = dbMain.GIS_AppConnections.Where(c => c.AppId == AppId).FirstOrDefault();

                    if (GIS_CON != null)
                    {
                        var gis_url = GIS_CON.DataSource;
                        var gis_DBName = GIS_CON.InitialCatalog;
                        var gis_username = GIS_CON.UserId;
                        var gis_password = GIS_CON.Password;

                        HttpClient client = new();



                        //Start Old Code
                        //var json = JsonConvert.SerializeObject(obj, Formatting.Indented);
                        //var stringContent = new StringContent(json);
                        //stringContent.Headers.ContentType.MediaType = "application/json";
                        //stringContent.Headers.Add("url", gis_url + "/" + gis_DBName);
                        //stringContent.Headers.Add("username", gis_username);
                        //stringContent.Headers.Add("password", gis_password);

                        //var response = await client.PostAsync("http://114.143.244.130:9091/house/search", stringContent);
                        //End Old Code

                        //Start New Code.
                        client.BaseAddress = new Uri("http://114.143.244.130:9091/");
                        //client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        client.DefaultRequestHeaders.Add("url", gis_url + "/" + gis_DBName);
                        client.DefaultRequestHeaders.Add("username", gis_username);
                        client.DefaultRequestHeaders.Add("password", gis_password);
                        HttpResponseMessage response = await client.PostAsJsonAsync("house/search", obj);
                        //End New Code



                        if (response.IsSuccessStatusCode)
                        {
                            var responseString = await response.Content.ReadAsStringAsync();

                            //result = await response.Content.ReadFromJsonAsync<DumpTripStatusResult>();

                            //var jsonParsed = JObject.Parse(responseString);
                            var dynamicobject = JsonConvert.DeserializeObject<dynamic>(responseString, new JsonSerializerSettings { DateParseHandling = DateParseHandling.None });

                            var jsonResult = dynamicobject["data"];

                            List<GisResult> myresult = jsonResult.ToObject<List<GisResult>>();



                            using (DevICTSBMChildEntities db = new(AppId))
                            {

                                foreach (var c in myresult)
                                {
                                    if (Convert.ToInt32(c.id) != 0)
                                    {
                                        var house = await db.HouseMasters.Where(x => x.houseId == Convert.ToInt32(c.id)).Select(x => new { x.ReferanceId, x.houseId, x.userId, x.houseOwner, x.houseOwnerMobile, x.houseAddress }).FirstOrDefaultAsync();


                                        var EmployeeName = await db.QrEmployeeMasters.Where(x => x.qrEmpId == Convert.ToInt32(c.createUser)).Select(x => new { x.qrEmpName }).FirstOrDefaultAsync();
                                        var Update_EmployeeName = await db.QrEmployeeMasters.Where(x => x.qrEmpId == Convert.ToInt32(c.updateUser)).Select(x => new { x.qrEmpName }).FirstOrDefaultAsync();


                                        var result1 = myresult.Select(i =>
                                        {
                                            if (i.id == house.houseId)
                                            {


                                                var value = new List<HouseProperty> {
                                                    new HouseProperty { name = "ReferanceId", value = house.ReferanceId, type = "String", Index = 0 },
                                                    new HouseProperty { name = "Create Employee Name", value = (EmployeeName == null ? "" : EmployeeName.qrEmpName.ToString()), type = "String", Index = 1 },
                                                    new HouseProperty { name = "Update Employee Name", value = (Update_EmployeeName == null ? "" : Update_EmployeeName.qrEmpName.ToString()), type = "String", Index = 2 },
                                                    new HouseProperty { name = "House Owner Name", value = house.houseOwner, type = "String", Index = 3 }
                                                };


                                                i.HouseProperty = value;
                                                return i;
                                            }
                                            return i;

                                        }).Where(i => i.id == house.houseId).ToList();

                                    }
                                }



                            }
                            objDetail.code = (int)response.StatusCode;
                            objDetail.status = "Success";
                            objDetail.message = "Data Found";
                            objDetail.timestamp = DateTime.Now.ToString();
                            objDetail.data = myresult;

                            result = objDetail;
                            return Ok(result);
                        }
                        else
                        {
                            objDetail.code = (int)response.StatusCode;
                            objDetail.status = "Failed";
                            objDetail.message = "";
                            objDetail.timestamp = DateTime.Now.ToString();

                            result = objDetail;
                            return NotFound(result);
                        }


                    }
                    else
                    {

                        objDetail.code = 404;
                        objDetail.status = "Failed";
                        objDetail.message = "GIS Connection Are Not Available";
                        objDetail.timestamp = DateTime.Now.ToString();

                        result = objDetail;
                        return NotFound(result);
                    }
                }
                catch (Exception ex)
                {

                    objDetail.code = 400;
                    objDetail.status = "Failed";
                    objDetail.message = ex.Message.ToString();
                    objDetail.timestamp = DateTime.Now.ToString();

                    result = objDetail;
                    return BadRequest(result);
                }

            }
            else
            {
                objDetail.code = 401;
                objDetail.status = "Failed";
                objDetail.message = "Unauthorized";
                objDetail.timestamp = DateTime.Now.ToString();


                result = objDetail;
                return Unauthorized(result);
            }
        }

        [HttpGet]
        [Route("GisGarbageTrail/all")]
        [EnableCors("MyCorsPolicy")]
        public async Task<ActionResult<TrailsDetails>> GarbageTrailgisAll([FromHeader] string authorization, [FromHeader] int AppId)
        {
            //var message = "";

            using DevICTSBMMainEntities dbMain = new();
            TrailsDetails objDetail = new();

            var stream = authorization.Replace("Bearer ", string.Empty);
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(stream);
            var tokenS = jsonToken as JwtSecurityToken;

            var jti = tokenS.Claims.First(claim => claim.Type == "AppId").Value;


            var Auth_AppId = Convert.ToInt32(jti);

            if (Auth_AppId == AppId)
            {
                try
                {
                    var GIS_CON = dbMain.GIS_AppConnections.Where(c => c.AppId == AppId).FirstOrDefault();

                    if (GIS_CON != null)
                    {
                        var gis_url = GIS_CON.DataSource;
                        var gis_DBName = GIS_CON.InitialCatalog;
                        var gis_username = GIS_CON.UserId;
                        var gis_password = GIS_CON.Password;

                        HttpClient client = new();
                        //Trial tn = new Trial();

                        //Start Old Code
                        //client.DefaultRequestHeaders.Add("url", gis_url + "/" + gis_DBName);
                        //client.DefaultRequestHeaders.Add("username", gis_username);
                        //client.DefaultRequestHeaders.Add("password", gis_password);

                        //var url = "http://114.143.244.130:9091/garbage-trail/all";

                        //var response = await client.GetAsync(url);
                        //End Old Code

                        //Start New Code.
                        client.BaseAddress = new Uri("http://114.143.244.130:9091/");
                        //client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        client.DefaultRequestHeaders.Add("url", gis_url + "/" + gis_DBName);
                        client.DefaultRequestHeaders.Add("username", gis_username);
                        client.DefaultRequestHeaders.Add("password", gis_password);
                        HttpResponseMessage response = await client.GetAsync("garbage-trail/all");
                        //End New Code

                        if (response.IsSuccessStatusCode)
                        {
                            var responseString = await response.Content.ReadAsStringAsync();
                            //var jsonParsed = JObject.Parse(responseString);
                            var dynamicobject = JsonConvert.DeserializeObject<dynamic>(responseString, new JsonSerializerSettings { DateParseHandling = DateParseHandling.None });

                            var jsonResult = dynamicobject["data"];


                            List<GisTrailResult> myresult = jsonResult.ToObject<List<GisTrailResult>>();

                            objDetail.code = (int)response.StatusCode;
                            objDetail.status = "Success";
                            objDetail.message = "Data Found";
                            objDetail.timestamp = DateTime.Now.ToString();
                            objDetail.data = myresult;

                            result = objDetail;
                            return Ok(result);

                        }
                        else
                        {
                            objDetail.code = (int)response.StatusCode;
                            objDetail.status = "Failed";
                            objDetail.message = "";
                            objDetail.timestamp = DateTime.Now.ToString();

                            result = objDetail;
                            return NotFound(result);
                        }


                    }
                    else
                    {

                        objDetail.code = 404;
                        objDetail.status = "Failed";
                        objDetail.message = "GIS Connection Are Not Available";
                        objDetail.timestamp = DateTime.Now.ToString();

                        result = objDetail;

                        return NotFound(result);
                    }
                }
                catch (Exception ex)
                {
                    //objDetail.Add(new TrailsDetails()
                    //{
                    //    code = "",
                    //    status = "",
                    //    message = ex.Message.ToString(),
                    //    //errorMessages = ex.Message.ToString(),
                    //    timestamp = "",
                    //    data = ""
                    //});

                    objDetail.code = 400;
                    objDetail.status = "Failed";
                    objDetail.message = ex.Message.ToString();
                    objDetail.timestamp = DateTime.Now.ToString();

                    result = objDetail;
                    return BadRequest(result);
                }

            }
            else
            {
                objDetail.code = 401;
                objDetail.status = "Failed";
                objDetail.message = "Unauthorized";
                objDetail.timestamp = DateTime.Now.ToString();


                result = objDetail;
                return Unauthorized(result);
            }

        }

        [HttpPost]
        [Route("GisGarbageTrail/search")]
        [EnableCors("MyCorsPolicy")]
        public async Task<ActionResult<HouseGisDetails>> GarbageTrailgisSearch([FromHeader] string authorization, [FromHeader] int AppId, [FromBody] GisSearch obj)
        {
            using DevICTSBMMainEntities dbMain = new();
            HouseGisDetails objDetail = new();

            var stream = authorization.Replace("Bearer ", string.Empty);
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(stream);
            var tokenS = jsonToken as JwtSecurityToken;

            var jti = tokenS.Claims.First(claim => claim.Type == "AppId").Value;


            var Auth_AppId = Convert.ToInt32(jti);

            if (Auth_AppId == AppId)
            {
                try
                {
                    var GIS_CON = dbMain.GIS_AppConnections.Where(c => c.AppId == AppId).FirstOrDefault();

                    if (GIS_CON != null)
                    {
                        var gis_url = GIS_CON.DataSource;
                        var gis_DBName = GIS_CON.InitialCatalog;
                        var gis_username = GIS_CON.UserId;
                        var gis_password = GIS_CON.Password;

                        HttpClient client = new();


                        //Start Old Code
                        //var json = JsonConvert.SerializeObject(obj, Formatting.Indented);
                        //var stringContent = new StringContent(json);
                        //stringContent.Headers.ContentType.MediaType = "application/json";
                        //stringContent.Headers.Add("url", gis_url + "/" + gis_DBName);
                        //stringContent.Headers.Add("username", gis_username);
                        //stringContent.Headers.Add("password", gis_password);

                        //var response = await client.PostAsync("http://114.143.244.130:9091/garbage-trail/search", stringContent);
                        //End Old Code

                        //Start New Code
                        client.BaseAddress = new Uri("http://114.143.244.130:9091/");
                        //client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        client.DefaultRequestHeaders.Add("url", gis_url + "/" + gis_DBName);
                        client.DefaultRequestHeaders.Add("username", gis_username);
                        client.DefaultRequestHeaders.Add("password", gis_password);
                        HttpResponseMessage response = await client.PostAsJsonAsync("garbage-trail/search", obj);
                        //End New Code

                        if (response.IsSuccessStatusCode)
                        {
                            var responseString = await response.Content.ReadAsStringAsync();
                            //var jsonParsed = JObject.Parse(responseString);
                            //var dynamicobject = JsonConvert.DeserializeObject<dynamic>(responseString);

                            var dynamicobject = JsonConvert.DeserializeObject<dynamic>(responseString, new JsonSerializerSettings { DateParseHandling = DateParseHandling.None });

                            var jsonResult = dynamicobject["data"];

                            List<GisTrailResult> myresult = jsonResult.ToObject<List<GisTrailResult>>();


                            using (DevICTSBMChildEntities db = new(AppId))
                            {

                                foreach (var c in myresult)
                                {

                                    //var GCDetails = await db.GarbageCollectionDetails.Where(x => x.userId == Convert.ToInt32(c.createUser) && x.gcDate >= Convert.ToDateTime(tn.startTs) && x.gcDate <= Convert.ToDateTime(tn.endTs)).Select(x => new { x.houseId, x.userId, x.gcDate, houselat = x.Lat, houselong = x.Long  }).ToListAsync();
                                    string st = c.startTs;
                                    string et = c.endTs;
                                    // performs multiple replacement  
                                    string starttime = st.Replace("T", " ").Replace("+00:00", "");
                                    string endtime = et.Replace("T", " ").Replace("+00:00", "");

                                    var query = (from s in db.GarbageCollectionDetails
                                                 from cs in db.HouseMasters
                                                 from um in db.UserMasters
                                                 where s.houseId == cs.houseId
                                                 where s.userId == um.userId
                                                 where s.userId == Convert.ToInt32(c.createUser) && s.gcDate >= Convert.ToDateTime(starttime) && s.gcDate <= Convert.ToDateTime(endtime)
                                                 select new
                                                 {
                                                     houseId = s.houseId,
                                                     userId = s.userId,
                                                     gcDate = s.gcDate,
                                                     //houselat = s.Lat,
                                                     //houselong = s.Long,
                                                     ReferanceId = cs.ReferanceId,
                                                     houseOwner = cs.houseOwner,
                                                     houseOwnerMobile = cs.houseOwnerMobile,
                                                     houseAddress = cs.houseAddress,
                                                     employeename = um.userName
                                                 }).ToList();



                                    if (query.Count > 0)
                                    {
                                        JavaScriptSerializer serializer = new();
                                        var output = serializer.Serialize(query);
                                        var housedatalist = new JavaScriptSerializer().Deserialize<GisHouseList[]>(output);

                                        var result1 = myresult.Select(i =>
                                        {
                                            i.HouseList = housedatalist;

                                            return i;

                                        }).ToList();

                                        var EmployeeName = await db.UserMasters.Where(x => x.userId == Convert.ToInt32(c.createUser)).Select(x => new { x.userName }).FirstOrDefaultAsync();
                                        var Update_EmployeeName = await db.UserMasters.Where(x => x.userId == Convert.ToInt32(c.updateUser)).Select(x => new { x.userName }).FirstOrDefaultAsync();


                                    }

                                }

                            }

                            objDetail.code = (int)response.StatusCode;
                            objDetail.status = "Success";
                            objDetail.message = "Data Found";
                            objDetail.timestamp = DateTime.Now.ToString();
                            objDetail.data = myresult;

                            result = objDetail;
                            return Ok(result);

                        }
                        else
                        {
                            objDetail.code = (int)response.StatusCode;
                            objDetail.status = "Failed";
                            objDetail.message = "";
                            objDetail.timestamp = DateTime.Now.ToString();

                            result = objDetail;
                            return NotFound(result);
                        }


                    }
                    else
                    {

                        objDetail.code = 404;
                        objDetail.status = "Failed";
                        objDetail.message = "GIS Connection Are Not Available";
                        objDetail.timestamp = DateTime.Now.ToString();

                        result = objDetail;
                        return NotFound(result);
                    }
                }
                catch (Exception ex)
                {

                    objDetail.code = 400;
                    objDetail.status = "Failed";
                    objDetail.message = ex.Message.ToString();
                    objDetail.timestamp = DateTime.Now.ToString();

                    result = objDetail;
                    return BadRequest(result);
                }

            }
            else
            {
                objDetail.code = 401;
                objDetail.status = "Failed";
                objDetail.message = "Unauthorized";
                objDetail.timestamp = DateTime.Now.ToString();


                result = objDetail;
                return Unauthorized(result);
            }
        }

        [HttpGet]
        [Route("GisHouseTrail/all")]
        [EnableCors("MyCorsPolicy")]
        public async Task<ActionResult<TrailsDetails>> HouseTrailgisAll([FromHeader] string authorization, [FromHeader] int AppId)
        {
            var message = "";

            using DevICTSBMMainEntities dbMain = new DevICTSBMMainEntities();
            TrailsDetails objDetail = new TrailsDetails();


            var stream = authorization.Replace("Bearer ", string.Empty);
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(stream);
            var tokenS = jsonToken as JwtSecurityToken;

            var jti = tokenS.Claims.First(claim => claim.Type == "AppId").Value;


            var Auth_AppId = Convert.ToInt32(jti);

            if (Auth_AppId == AppId)
            {
                try
                {
                    var GIS_CON = dbMain.GIS_AppConnections.Where(c => c.AppId == AppId).FirstOrDefault();

                    if (GIS_CON != null)
                    {
                        var gis_url = GIS_CON.DataSource;
                        var gis_DBName = GIS_CON.InitialCatalog;
                        var gis_username = GIS_CON.UserId;
                        var gis_password = GIS_CON.Password;

                        HttpClient client = new HttpClient();
                        //Trial tn = new Trial();

                        //Start Old Code
                        //var json = JsonConvert.SerializeObject(tn, Formatting.Indented);
                        //var stringContent = new StringContent(json);

                        //client.DefaultRequestHeaders.Add("url", gis_url + "/" + gis_DBName);
                        //client.DefaultRequestHeaders.Add("username", gis_username);
                        //client.DefaultRequestHeaders.Add("password", gis_password);

                        //var url = "http://114.143.244.130:9091/house-trail/all";

                        //var response = await client.GetAsync(url);
                        //End Old Code


                        //Start New Code
                        client.BaseAddress = new Uri("http://114.143.244.130:9091/");
                        //client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        client.DefaultRequestHeaders.Add("url", gis_url + "/" + gis_DBName);
                        client.DefaultRequestHeaders.Add("username", gis_username);
                        client.DefaultRequestHeaders.Add("password", gis_password);
                        HttpResponseMessage response = await client.GetAsync("house-trail/all");
                        //End New Code

                        if (response.IsSuccessStatusCode)
                        {
                            var responseString = await response.Content.ReadAsStringAsync();
                            //var jsonParsed = JObject.Parse(responseString);
                            var dynamicobject = JsonConvert.DeserializeObject<dynamic>(responseString, new JsonSerializerSettings { DateParseHandling = DateParseHandling.None });

                            var jsonResult = dynamicobject["data"];


                            List<GisTrailResult> myresult = jsonResult.ToObject<List<GisTrailResult>>();

                            //objDetail.Add(new TrailsDetails()
                            //{
                            //    code = dynamicobject.code.ToString(),
                            //    status = dynamicobject.status.ToString(),
                            //    message = dynamicobject.message.ToString(),
                            //    timestamp = dynamicobject.timestamp.ToString(),
                            //    data = result
                            //});

                            objDetail.code = (int)response.StatusCode;
                            objDetail.status = "Success";
                            objDetail.message = "Data Found";
                            objDetail.timestamp = DateTime.Now.ToString();
                            objDetail.data = myresult;

                            result = objDetail;
                            return Ok(result);

                        }
                        else
                        {
                            objDetail.code = (int)response.StatusCode;
                            objDetail.status = "Failed";
                            objDetail.message = "Not Found";
                            objDetail.timestamp = DateTime.Now.ToString();

                            result = objDetail;
                            return NotFound(result);
                        }


                    }
                    else
                    {

                        //objDetail.Add(new TrailsDetails()
                        //{
                        //    code = "",
                        //    status = "",
                        //    message = "GIS Connection Are Not Available",
                        //    //errorMessages = "GIS Connection Are Not Available",
                        //    timestamp = "",
                        //    data = ""
                        //});

                        objDetail.code = 404;
                        objDetail.status = "Failed";
                        objDetail.message = "GIS Connection Are Not Available";
                        objDetail.timestamp = DateTime.Now.ToString();

                        result = objDetail;

                        return NotFound(result);
                    }
                }
                catch (Exception ex)
                {
                    //objDetail.Add(new TrailsDetails()
                    //{
                    //    code = "",
                    //    status = "",
                    //    message = ex.Message.ToString(),
                    //    //errorMessages = ex.Message.ToString(),
                    //    timestamp = "",
                    //    data = ""
                    //});

                    objDetail.code = 400;
                    objDetail.status = "Failed";
                    objDetail.message = ex.Message.ToString();
                    objDetail.timestamp = DateTime.Now.ToString();

                    result = objDetail;
                    return NotFound(result);
                }
                //return objDetail;
            }
            else
            {
                objDetail.code = 401;
                objDetail.status = "Failed";
                objDetail.message = "Unauthorized";
                objDetail.timestamp = DateTime.Now.ToString();


                result = objDetail;
                return Unauthorized(result);
            }

        }

        [HttpPost]
        [Route("GisHouseTrail/search")]
        [EnableCors("MyCorsPolicy")]
        public async Task<ActionResult<HouseGisDetails>> HouseTrailgisSearch([FromHeader] string authorization, [FromHeader] int AppId, [FromBody] GisSearch obj)
        {
            using DevICTSBMMainEntities dbMain = new();
            HouseGisDetails objDetail = new();

            var stream = authorization.Replace("Bearer ", string.Empty);
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(stream);
            var tokenS = jsonToken as JwtSecurityToken;

            var jti = tokenS.Claims.First(claim => claim.Type == "AppId").Value;


            var Auth_AppId = Convert.ToInt32(jti);

            if (Auth_AppId == AppId)
            {
                try
                {
                    var GIS_CON = dbMain.GIS_AppConnections.Where(c => c.AppId == AppId).FirstOrDefault();

                    if (GIS_CON != null)
                    {
                        var gis_url = GIS_CON.DataSource;
                        var gis_DBName = GIS_CON.InitialCatalog;
                        var gis_username = GIS_CON.UserId;
                        var gis_password = GIS_CON.Password;

                        HttpClient client = new();

                        //Start Old Code
                        //var json = JsonConvert.SerializeObject(obj, Formatting.Indented);
                        //var stringContent = new StringContent(json);
                        //stringContent.Headers.ContentType.MediaType = "application/json";
                        //stringContent.Headers.Add("url", gis_url + "/" + gis_DBName);
                        //stringContent.Headers.Add("username", gis_username);
                        //stringContent.Headers.Add("password", gis_password);

                        //var response = await client.PostAsync("http://114.143.244.130:9091/house-trail/search", stringContent);
                        //End Old Code

                        //Start New Code
                        client.BaseAddress = new Uri("http://114.143.244.130:9091/");
                        //client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        client.DefaultRequestHeaders.Add("url", gis_url + "/" + gis_DBName);
                        client.DefaultRequestHeaders.Add("username", gis_username);
                        client.DefaultRequestHeaders.Add("password", gis_password);
                        HttpResponseMessage response = await client.PostAsJsonAsync("house-trail/search", obj);
                        //End New Code

                        if (response.IsSuccessStatusCode)
                        {
                            var responseString = await response.Content.ReadAsStringAsync();
                            //var jsonParsed = JObject.Parse(responseString);
                            //var dynamicobject = JsonConvert.DeserializeObject<dynamic>(responseString);

                            var dynamicobject = JsonConvert.DeserializeObject<dynamic>(responseString, new JsonSerializerSettings { DateParseHandling = DateParseHandling.None });

                            var jsonResult = dynamicobject["data"];

                            List<GisTrailResult> myresult = jsonResult.ToObject<List<GisTrailResult>>();

                            using (DevICTSBMChildEntities db = new(AppId))
                            {

                                foreach (var c in myresult)
                                {

                                    //var GCDetails = await db.GarbageCollectionDetails.Where(x => x.userId == Convert.ToInt32(c.createUser) && x.gcDate >= Convert.ToDateTime(tn.startTs) && x.gcDate <= Convert.ToDateTime(tn.endTs)).Select(x => new { x.houseId, x.userId, x.gcDate, houselat = x.Lat, houselong = x.Long  }).ToListAsync();

                                    string st = c.startTs;
                                    string et = c.endTs;
                                    // performs multiple replacement  
                                    string starttime = st.Replace("T", " ").Replace("+00:00", "");
                                    string endtime = et.Replace("T", " ").Replace("+00:00", "");

                                    var query = (from hm in db.HouseMasters
                                                 from em in db.QrEmployeeMasters
                                                 where hm.userId == em.qrEmpId
                                                 where hm.userId == Convert.ToInt32(c.createUser) && hm.modified >= Convert.ToDateTime(starttime) && hm.modified <= Convert.ToDateTime(endtime)
                                                 select new
                                                 {
                                                     houseId = hm.houseId,
                                                     userId = em.qrEmpId,
                                                     gcDate = hm.modified,
                                                     ReferanceId = hm.ReferanceId,
                                                     houseOwner = hm.houseOwner,
                                                     houseOwnerMobile = hm.houseOwnerMobile,
                                                     houseAddress = hm.houseAddress,
                                                     employeename = em.qrEmpName
                                                 }).ToList();



                                    if (query.Count > 0)
                                    {
                                        JavaScriptSerializer serializer = new JavaScriptSerializer();
                                        var output = serializer.Serialize(query);
                                        var housedatalist = new JavaScriptSerializer().Deserialize<GisHouseList[]>(output);

                                        var result1 = myresult.Select(i =>
                                        {
                                            i.HouseList = housedatalist;

                                            return i;

                                        }).ToList();

                                        var EmployeeName = await db.QrEmployeeMasters.Where(x => x.qrEmpId == Convert.ToInt32(c.createUser)).Select(x => new { x.qrEmpName }).FirstOrDefaultAsync();
                                        var Update_EmployeeName = await db.QrEmployeeMasters.Where(x => x.qrEmpId == Convert.ToInt32(c.updateUser)).Select(x => new { x.qrEmpName }).FirstOrDefaultAsync();


                                    }

                                }

                            }

                            objDetail.code = (int)response.StatusCode;
                            objDetail.status = "Success";
                            if (myresult.Count > 0)
                            {
                                objDetail.message = "Data Found";
                            }
                            else
                            {
                                objDetail.message = "Data Not Found";
                            }
                            objDetail.timestamp = DateTime.Now.ToString();
                            objDetail.data = myresult;

                            result = objDetail;

                            return Ok(result);
                        }
                        else
                        {
                            objDetail.code = (int)response.StatusCode;
                            objDetail.status = "Failed";
                            objDetail.message = "Not Found";
                            objDetail.timestamp = DateTime.Now.ToString();

                            result = objDetail;

                            return objDetail.code == 400 ? BadRequest(result) : NotFound(result);
                        }


                    }
                    else
                    {

                        objDetail.code = 404;
                        objDetail.status = "Failed";
                        objDetail.message = "GIS Connection Are Not Available";
                        objDetail.timestamp = DateTime.Now.ToString();

                        result = objDetail;
                        return NotFound(result);
                    }
                }
                catch (Exception ex)
                {

                    objDetail.code = 400;
                    objDetail.status = "Failed";
                    objDetail.message = ex.Message.ToString();
                    objDetail.timestamp = DateTime.Now.ToString();

                    result = objDetail;

                    return NotFound(result);
                }

            }
            else
            {
                objDetail.code = 401;
                objDetail.status = "Failed";
                objDetail.message = "Unauthorized";
                objDetail.timestamp = DateTime.Now.ToString();


                result = objDetail;
                return Unauthorized(result);
            }
        }

        [HttpPost]
        [Route("GisHouseOnMap/all")]
        [EnableCors("MyCorsPolicy")]

        public async Task<ActionResult<HouseGisDetails>> HouseOnMapGisAsync([FromHeader] string authorization, [FromHeader] int AppId, HouseOnMapSearch obj)
        {
            using DevICTSBMMainEntities dbMain = new DevICTSBMMainEntities();
            HouseGisDetails objDetail = new HouseGisDetails();
            List<HouseOnMapVM> houseLocation = new List<HouseOnMapVM>();
            var stream = authorization.Replace("Bearer ", string.Empty);
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(stream);
            var tokenS = jsonToken as JwtSecurityToken;

            var jti = tokenS.Claims.First(claim => claim.Type == "AppId").Value;


            var Auth_AppId = Convert.ToInt32(jti);

            if (Auth_AppId == AppId)
            {
                try
                {
                    var GIS_CON = dbMain.GIS_AppConnections.Where(c => c.AppId == AppId).FirstOrDefault();

                    if (GIS_CON != null)
                    {
                        var gis_url = GIS_CON.DataSource;
                        var gis_DBName = GIS_CON.InitialCatalog;
                        var gis_username = GIS_CON.UserId;
                        var gis_password = GIS_CON.Password;

                        HttpClient client = new HttpClient();

                        int user;
                        int area;
                        int ward;
                        int? GarbageType;
                        int FilterType;
                        var zoneId = 0;
                        if (obj.userid == null)
                        {
                            user = 0;
                        }
                        else
                        {
                            user = Convert.ToInt32(obj.userid);
                        }

                        if (obj.areaId == null)
                        {
                            area = 0;
                        }
                        else
                        {
                            area = Convert.ToInt32(obj.areaId);
                        }
                        if (obj.wardNo == null)
                        {
                            ward = 0;
                        }
                        else
                        {
                            ward = Convert.ToInt32(obj.wardNo);
                        }
                        if (obj.garbageType == null || obj.garbageType == -1)
                        {
                            GarbageType = null;
                        }
                        else
                        {
                            GarbageType = Convert.ToInt32(obj.garbageType);
                        }
                        if (obj.filterType == null)
                        {
                            FilterType = 0;
                        }
                        else
                        {
                            FilterType = Convert.ToInt32(obj.filterType);
                        }
                        if (obj.date == null || obj.date == "")
                        {
                            obj.date = DateTime.Now.ToShortDateString();
                        }



                        using (DevICTSBMChildEntities db = new DevICTSBMChildEntities(AppId))
                        using (SqlConnection connection = new SqlConnection(db.Database.GetDbConnection().ConnectionString))
                        {
                            connection.Open();

                            var command = connection.CreateCommand();

                            const string CheckIfTableExistsStatement = "SELECT * FROM sys.objects WHERE name = N'SP_HouseOnMapDetails'";
                            command.CommandText = CheckIfTableExistsStatement;
                            var executeScalar = command.ExecuteScalar();
                            if (executeScalar != null)
                            {
                                CultureInfo culture = new CultureInfo("en-US");

                                DateTime dt2 = Convert.ToDateTime(obj.date, culture);

                                List<SqlParameter> parms = new List<SqlParameter>
                                                {
                                                    // Create parameter(s)    
                                                    new SqlParameter { ParameterName = "@gcDate", Value = dt2 },
                                                    new SqlParameter { ParameterName = "@UserId", Value = user },
                                                    new SqlParameter { ParameterName = "@ZoneId", Value = zoneId },
                                                    new SqlParameter { ParameterName = "@AreaId", Value = area },
                                                    new SqlParameter { ParameterName = "@WardNo", Value = ward },
                                                    new SqlParameter { ParameterName = "@GarbageType", Value = GarbageType == null ? DBNull.Value : GarbageType },
                                                    new SqlParameter { ParameterName = "@FilterType", Value = FilterType },
                                                };
                                var data = await db.SP_HouseOnMapDetails_Results.FromSqlRaw<SP_HouseOnMapDetails_Result>("EXEC SP_HouseOnMapDetails @gcDate,@UserId,@ZoneId,@AreaId,@WardNo,@GarbageType,@FilterType", parms.ToArray()).ToListAsync();


                                foreach (var x in data)
                                {
                                    if (x.houseId > 0)
                                    {
                                        DateTime dt = DateTime.Parse(x.gcDate == null ? DateTime.Now.ToString() : x.gcDate.ToString());
                                        //string gcTime = x.gcDate.ToString();
                                        houseLocation.Add(new HouseOnMapVM()
                                        {
                                            //dyid = Convert.ToInt32(x.dyid),
                                            //ssid = Convert.ToInt32(x.ssid),
                                            //lwid = Convert.ToInt32(x.lwid),
                                            houseId = Convert.ToInt32(x.houseId),
                                            ReferanceId = x.ReferanceId,
                                            houseOwner = (x.houseOwner ?? ""),
                                            houseOwnerMobile = (x.houseOwnerMobile ?? ""),
                                            // houseAddress = checkNull(x.houseAddress).Replace("Unnamed Road, ", ""),
                                            gcDate = dt.ToString("dd-MM-yyyy"),
                                            gcTime = dt.ToString("h:mm tt"), // 7:00 AM // 12 hour clock
                                                                             //string gcTime = x.gcDate.ToString(),
                                                                             //gcTime = x.gcDate.ToString("hh:mm tt"),
                                                                             //myDateTime.ToString("HH:mm:ss")
                                            ///date = Convert.ToDateTime(x.datt).ToString("dd/MM/yyyy"),
                                            //time = Convert.ToDateTime(x.datt).ToString("hh:mm:ss tt"),
                                            //houseLat = x.houseLat,
                                            //houseLong = x.houseLong,
                                            // address = x.houseAddress,
                                            //vehcileNumber = x.v,
                                            //userMobile = x.mobile,
                                            garbageType = x.garbageType,
                                        });
                                    }

                                }
                            }

                            //Create the Command Object
                            objDetail.code = 200;
                            objDetail.status = "Success";
                            objDetail.timestamp = DateTime.Now.ToString();
                            if (houseLocation.Count > 0)
                            {
                                objDetail.message = "Data Found";

                            }
                            else
                            {
                                objDetail.message = "Data Not Available";
                            }
                            objDetail.data = houseLocation;


                        }
                        result = objDetail;
                        return Ok(result);
                    }
                    else
                    {

                        objDetail.code = 404;
                        objDetail.status = "Failed";
                        objDetail.message = "GIS Connection Are Not Available";
                        objDetail.timestamp = DateTime.Now.ToString();

                        result = objDetail;
                        return NotFound(result);
                    }
                }
                catch (Exception ex)
                {

                    objDetail.code = 400;
                    objDetail.status = "Failed";
                    objDetail.message = ex.Message.ToString();
                    objDetail.timestamp = DateTime.Now.ToString();

                    result = objDetail;

                    return NotFound(result);
                }

            }
            else
            {
                objDetail.code = 401;
                objDetail.status = "Failed";
                objDetail.message = "Unauthorized";
                objDetail.timestamp = DateTime.Now.ToString();


                result = objDetail;
                return Unauthorized(result);
            }
        }


        [HttpPost]
        [Route("GisHouse/Update")]
        [EnableCors("MyCorsPolicy")]
        public async Task<ActionResult<HouseGisDetails>> GisHouseUpdate([FromHeader] string authorization, [FromHeader] int AppId, [FromBody] HouseGisUpdate obj)
        {
            using DevICTSBMMainEntities dbMain = new();
            //Trial tn = new();
            DumpTripStatusResult objDetail = new();

            var stream = authorization.Replace("Bearer ", string.Empty);
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(stream);
            var tokenS = jsonToken as JwtSecurityToken;

            var jti = tokenS.Claims.First(claim => claim.Type == "AppId").Value;


            var Auth_AppId = Convert.ToInt32(jti);
            if (Auth_AppId == AppId)
            {
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
                        //tn.id = obj.id;
                        //tn.updateUser = obj.updateUser;
                        //tn.updateTs = obj.updateTs;
                        //tn.geom = obj.geom;
                        GisSearch stn = new GisSearch();

                        stn.id = obj.id.ToString();
                        HttpClient client1 = new();

                        client1.BaseAddress = new Uri("http://114.143.244.130:9091/");
                        //client1.DefaultRequestHeaders.Accept.Clear();
                        client1.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        client1.DefaultRequestHeaders.Add("url", gis_url + "/" + gis_DBName);
                        client1.DefaultRequestHeaders.Add("username", gis_username);
                        client1.DefaultRequestHeaders.Add("password", gis_password);
                        HttpResponseMessage response1 = await client1.PostAsJsonAsync("house/search", stn);

                        if (response1.IsSuccessStatusCode)
                        {
                            HttpClient client = new();

                            //Start Old Code
                            //var json = JsonConvert.SerializeObject(tn, Formatting.Indented);
                            //var stringContent = new StringContent(json);
                            //stringContent.Headers.ContentType.MediaType = "application/json";
                            //stringContent.Headers.Add("url", gis_url + "/" + gis_DBName);
                            //stringContent.Headers.Add("username", gis_username);
                            //stringContent.Headers.Add("password", gis_password);
                            //var response = await client.PostAsync("http://114.143.244.130:9091/house", stringContent);
                            //End Old Code
                            if (obj.updateTs == null)
                            {
                                obj.updateTs = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fff");


                            }
                            if (obj.updateUser == null)
                            {
                                obj.updateUser = 0;
                            }

                            //Start New Code
                            client.BaseAddress = new Uri("http://114.143.244.130:9091/");
                            //client.DefaultRequestHeaders.Accept.Clear();
                            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                            client.DefaultRequestHeaders.Add("url", gis_url + "/" + gis_DBName);
                            client.DefaultRequestHeaders.Add("username", gis_username);
                            client.DefaultRequestHeaders.Add("password", gis_password);
                            HttpResponseMessage response = await client.PostAsJsonAsync("house", obj);
                            //End New Code
                            if (response.IsSuccessStatusCode)
                            {
                                var responseString = await response.Content.ReadFromJsonAsync<DumpTripStatusResult>();
                                //var dynamicobject = JsonConvert.DeserializeObject<dynamic>(responseString);
                                //objDetail.Add(new DumpTripStatusResult()
                                //{
                                //    code = (int)response.StatusCode,
                                //    status = "Success",
                                //    message = dynamicobject.message.ToString(),
                                //    errorMessages = dynamicobject.errorMessages.ToString(),
                                //    timestamp = DateTime.Now.ToString(),
                                //    data = dynamicobject.data
                                //});

                                objDetail = responseString;
                                return Ok(objDetail);
                            }
                            else
                            {
                                //objDetail.Add(new DumpTripStatusResult()
                                //{
                                //    code = (int)response.StatusCode,
                                //    status = "Failed",
                                //    message = "",
                                //    timestamp = DateTime.Now.ToString()
                                //});
                                var responseString = await response.Content.ReadAsStringAsync();
                                var dynamicobject = JsonConvert.DeserializeObject<dynamic>(responseString);

                                objDetail.code = (int)response.StatusCode;
                                objDetail.status = "Failed";
                                objDetail.timestamp = DateTime.Now.ToString();
                                return NotFound(objDetail);
                            }
                        }
                        else
                        {
                            objDetail.code = (int)response1.StatusCode;
                            objDetail.status = "Failed";
                            objDetail.message = "HouseId Not Available";
                            objDetail.timestamp = DateTime.Now.ToString();
                            return BadRequest(objDetail);
                        }

                    }
                    else
                    {

                        objDetail.code = 404;
                        objDetail.status = "Failed";
                        objDetail.message = "Gis Connection Not Available";
                        objDetail.timestamp = DateTime.Now.ToString();
                        return NotFound(objDetail);

                    }
                }
                catch (Exception ex)
                {
                    objDetail.code = 400;
                    objDetail.status = "Failed";
                    objDetail.message = ex.Message.ToString();
                    objDetail.timestamp = DateTime.Now.ToString();
                    return NotFound(objDetail);

                }
            }
            else
            {
                objDetail.code = 401;
                objDetail.status = "Failed";
                objDetail.message = "Unauthorized";
                objDetail.timestamp = DateTime.Now.ToString();


                result = objDetail;
                return Unauthorized(result);
            }

        }

        [HttpGet]
        [Route("GisDumpYardDetails/all")]
        [EnableCors("MyCorsPolicy")]
        public async Task<ActionResult<HouseGisDetails>> DumpYardGisDetailsAll([FromHeader] string authorization, [FromHeader] int AppId)
        {

            //var message = "";

            using DevICTSBMMainEntities dbMain = new();
            HouseGisDetails objDetail = new();

            var stream = authorization.Replace("Bearer ", string.Empty);
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(stream);
            var tokenS = jsonToken as JwtSecurityToken;

            var jti = tokenS.Claims.First(claim => claim.Type == "AppId").Value;


            var Auth_AppId = Convert.ToInt32(jti);

            //Extract the payload of the JWT
            //var claims = tokenS.Claims;

            //var jwtPayload = "{";
            //foreach (Claim c in claims)
            //{
            //    jwtPayload += '"' + c.Type + "\":\"" + c.Value + "\",";
            //}
            //jwtPayload += "}";
            //txtJwtOut += "\r\nPayload:\r\n" + JToken.Parse(jwtPayload).ToString(Formatting.Indented);



            if (Auth_AppId == AppId)
            {
                try
                {
                    var GIS_CON = dbMain.GIS_AppConnections.Where(c => c.AppId == AppId).FirstOrDefault();

                    if (GIS_CON != null)
                    {
                        var gis_url = GIS_CON.DataSource;
                        var gis_DBName = GIS_CON.InitialCatalog;
                        var gis_username = GIS_CON.UserId;
                        var gis_password = GIS_CON.Password;

                        HttpClient client = new();
                        //Trial tn = new Trial();

                        //Start Old Code
                        //client.DefaultRequestHeaders.Add("url", gis_url + "/" + gis_DBName);
                        //client.DefaultRequestHeaders.Add("username", gis_username);
                        //client.DefaultRequestHeaders.Add("password", gis_password);

                        //var url = "http://114.143.244.130:9091/house/all";

                        //var response = await client.GetAsync(url);
                        //End Old Code

                        //Start New Code.
                        client.BaseAddress = new Uri("http://114.143.244.130:9091/");
                        //client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        client.DefaultRequestHeaders.Add("url", gis_url + "/" + gis_DBName);
                        client.DefaultRequestHeaders.Add("username", gis_username);
                        client.DefaultRequestHeaders.Add("password", gis_password);
                        var response = await client.GetAsync("dump-yard/all");
                        //End New Code

                        if (response.IsSuccessStatusCode)
                        {
                            var responseString = await response.Content.ReadAsStringAsync();
                            //var jsonParsed = JObject.Parse(responseString);
                            var dynamicobject = JsonConvert.DeserializeObject<dynamic>(responseString, new JsonSerializerSettings { DateParseHandling = DateParseHandling.None });

                            var jsonResult = dynamicobject["data"];
                            //Response.Headers.Add("Access-Control-Allow-Origin", "*");
                            //Response.Headers.Add("Access-Control-Allow-Methods", "GET");
                            //Response.Headers.Add("Access-Control-Allow-Headers", "Content-Type");

                            List<GisResult> myresult = jsonResult.ToObject<List<GisResult>>();

                            List<HouseGisDetails> obj = new();

                            using (DevICTSBMChildEntities db = new(AppId))
                            {

                                foreach (var c in myresult)
                                {
                                    if (Convert.ToInt32(c.id) != 0)
                                    {
                                        var house = await db.DumpYardDetails.Where(x => x.dyId == Convert.ToInt32(c.id)).Select(x => new { x.ReferanceId, x.dyId, x.userId, x.dyName, x.dyAddress }).FirstOrDefaultAsync();


                                        var EmployeeName = await db.QrEmployeeMasters.Where(x => x.qrEmpId == Convert.ToInt32(c.createUser)).Select(x => new { x.qrEmpName }).FirstOrDefaultAsync();
                                        var Update_EmployeeName = await db.QrEmployeeMasters.Where(x => x.qrEmpId == Convert.ToInt32(c.updateUser)).Select(x => new { x.qrEmpName }).FirstOrDefaultAsync();


                                        var result1 = myresult.Select(i =>
                                        {
                                            if (i.id == house.dyId)
                                            {
                                                //i.ReferanceId = house.ReferanceId;
                                                //i.HouseOwnerName = house.houseOwner;
                                                //i.HouseOwnerMobileNo = house.houseOwnerMobile;
                                                //i.HouseAddress = house.houseAddress;
                                                //i.CreateEmployeeName = EmployeeName.qrEmpName.ToString();
                                                //if(Update_EmployeeName != null)
                                                //{
                                                //    i.UpdateEmployeeName = Update_EmployeeName.qrEmpName.ToString();

                                                //}
                                                //else
                                                //{
                                                //    i.UpdateEmployeeName = "";
                                                //}

                                                var value = new List<HouseProperty> {
                                                    new HouseProperty { name = "ReferanceId", value = house.ReferanceId, type = "String", Index = 0 },
                                                    new HouseProperty { name = "Create Employee Name", value = (EmployeeName == null ? "" : EmployeeName.qrEmpName.ToString()), type = "String", Index = 1 },
                                                    new HouseProperty { name = "Update Employee Name", value = (Update_EmployeeName == null ? "" : Update_EmployeeName.qrEmpName.ToString()), type = "String", Index = 2 },
                                                    new HouseProperty { name = "Dump Yard Address", value = house.dyAddress, type = "String", Index = 3 }
                                                };


                                                i.HouseProperty = value;
                                                return i;
                                            }
                                            return i;

                                        }).Where(i => i.id == house.dyId).ToList();

                                    }
                                }



                            }
                            // return objDetail;

                            //objDetail.Add(new HouseGisDetails()
                            //{
                            //    code = dynamicobject.code.ToString(),
                            //    status = dynamicobject.status.ToString(),
                            //    message = dynamicobject.message.ToString(),
                            //    timestamp = dynamicobject.timestamp.ToString(),
                            //    data = result
                            //});

                            objDetail.code = (int)response.StatusCode;
                            objDetail.status = dynamicobject.status.ToString();
                            objDetail.message = dynamicobject.message.ToString();
                            objDetail.timestamp = DateTime.Now.ToString();
                            objDetail.data = myresult;

                            result = objDetail;
                            return Ok(result);

                        }
                        else
                        {
                            objDetail.code = (int)response.StatusCode;
                            objDetail.status = "Failed";
                            objDetail.message = "";
                            objDetail.timestamp = DateTime.Now.ToString();

                            result = objDetail;
                            return NotFound(result);
                        }
                    }
                    else
                    {

                        //objDetail.Add(new HouseGisDetails()
                        //{
                        //    code = "",
                        //    status = "",
                        //    message = "GIS Connection Are Not Available",
                        //   // errorMessages = "GIS Connection Are Not Available",
                        //    timestamp = "",
                        //    data = ""
                        //});
                        objDetail.code = 401;
                        objDetail.status = "Failed";
                        objDetail.message = "GIS Connection Are Not Available";
                        objDetail.timestamp = DateTime.Now.ToString();

                        result = objDetail;
                        return NotFound(result);
                    }
                }
                catch (Exception ex)
                {
                    //objDetail.Add(new HouseGisDetails()
                    //{
                    //    code = "",
                    //    status = "",
                    //    message = ex.Message.ToString(),
                    //    //errorMessages = ex.Message.ToString(),
                    //    timestamp = "",
                    //    data = ""
                    //});

                    objDetail.code = 400;
                    objDetail.status = "Failed";
                    objDetail.message = ex.Message.ToString();
                    objDetail.timestamp = DateTime.Now.ToString();

                    result = objDetail;
                    return BadRequest(result);
                }
                //return objDetail;
            }
            else
            {

                objDetail.code = 401;
                objDetail.status = "Failed";
                objDetail.message = "Unauthorized";
                objDetail.timestamp = DateTime.Now.ToString();


                result = objDetail;
                return Unauthorized(result);
            }


        }

        [HttpPost]
        [Route("GisDumpYardDetails/search")]
        [EnableCors("MyCorsPolicy")]
        public async Task<ActionResult<HouseGisDetails>> DumpYardGisDetailsSearch([FromHeader] string authorization, [FromHeader] int AppId, [FromBody] GisSearch obj)
        {
            using DevICTSBMMainEntities dbMain = new DevICTSBMMainEntities();
            HouseGisDetails objDetail = new HouseGisDetails();

            var stream = authorization.Replace("Bearer ", string.Empty);
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(stream);
            var tokenS = jsonToken as JwtSecurityToken;

            var jti = tokenS.Claims.First(claim => claim.Type == "AppId").Value;


            var Auth_AppId = Convert.ToInt32(jti);

            if (Auth_AppId == AppId)
            {
                try
                {
                    var GIS_CON = dbMain.GIS_AppConnections.Where(c => c.AppId == AppId).FirstOrDefault();

                    if (GIS_CON != null)
                    {
                        var gis_url = GIS_CON.DataSource;
                        var gis_DBName = GIS_CON.InitialCatalog;
                        var gis_username = GIS_CON.UserId;
                        var gis_password = GIS_CON.Password;

                        HttpClient client = new();


                        //Start New Code.
                        client.BaseAddress = new Uri("http://114.143.244.130:9091/");
                        //client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        client.DefaultRequestHeaders.Add("url", gis_url + "/" + gis_DBName);
                        client.DefaultRequestHeaders.Add("username", gis_username);
                        client.DefaultRequestHeaders.Add("password", gis_password);
                        HttpResponseMessage response = await client.PostAsJsonAsync("dump-yard/search", obj);
                        //End New Code



                        if (response.IsSuccessStatusCode)
                        {
                            var responseString = await response.Content.ReadAsStringAsync();

                            //result = await response.Content.ReadFromJsonAsync<DumpTripStatusResult>();

                            //var jsonParsed = JObject.Parse(responseString);
                            var dynamicobject = JsonConvert.DeserializeObject<dynamic>(responseString, new JsonSerializerSettings { DateParseHandling = DateParseHandling.None });

                            var jsonResult = dynamicobject["data"];

                            List<GisResult> myresult = jsonResult.ToObject<List<GisResult>>();



                            using (DevICTSBMChildEntities db = new(AppId))
                            {

                                foreach (var c in myresult)
                                {
                                    if (Convert.ToInt32(c.id) != 0)
                                    {
                                        var house = await db.DumpYardDetails.Where(x => x.dyId == Convert.ToInt32(c.id)).Select(x => new { x.ReferanceId, x.dyId, x.userId, x.dyName, x.dyAddress }).FirstOrDefaultAsync();


                                        var EmployeeName = await db.QrEmployeeMasters.Where(x => x.qrEmpId == Convert.ToInt32(c.createUser)).Select(x => new { x.qrEmpName }).FirstOrDefaultAsync();
                                        var Update_EmployeeName = await db.QrEmployeeMasters.Where(x => x.qrEmpId == Convert.ToInt32(c.updateUser)).Select(x => new { x.qrEmpName }).FirstOrDefaultAsync();


                                        var result1 = myresult.Select(i =>
                                        {
                                            if (i.id == house.dyId)
                                            {


                                                var value = new List<HouseProperty> {
                                                    new HouseProperty { name = "ReferanceId", value = house.ReferanceId, type = "String", Index = 0 },
                                                    new HouseProperty { name = "Create Employee Name", value = (EmployeeName == null ? "" : EmployeeName.qrEmpName.ToString()), type = "String", Index = 1 },
                                                    new HouseProperty { name = "Update Employee Name", value = (Update_EmployeeName == null ? "" : Update_EmployeeName.qrEmpName.ToString()), type = "String", Index = 2 },
                                                    new HouseProperty { name = "Dump Yard Address", value = house.dyAddress, type = "String", Index = 3 }
                                                };


                                                i.HouseProperty = value;
                                                return i;
                                            }
                                            return i;

                                        }).Where(i => i.id == house.dyId).ToList();

                                    }
                                }



                            }
                            objDetail.code = (int)response.StatusCode;
                            objDetail.status = "Success";
                            if(myresult.Count > 0)
                            {
                                objDetail.message = "Data Found";
                            }
                            else
                            {
                                objDetail.message = "Data Not Found";
                            }
                            
                            objDetail.timestamp = DateTime.Now.ToString();
                            objDetail.data = myresult;

                            result = objDetail;
                            return Ok(result);
                        }
                        else
                        {
                            objDetail.code = (int)response.StatusCode;
                            objDetail.status = "Failed";
                            objDetail.message = "";
                            objDetail.timestamp = DateTime.Now.ToString();

                            result = objDetail;
                            return NotFound(result);
                        }


                    }
                    else
                    {

                        objDetail.code = 404;
                        objDetail.status = "Failed";
                        objDetail.message = "GIS Connection Are Not Available";
                        objDetail.timestamp = DateTime.Now.ToString();

                        result = objDetail;
                        return NotFound(result);
                    }
                }
                catch (Exception ex)
                {

                    objDetail.code = 400;
                    objDetail.status = "Failed";
                    objDetail.message = ex.Message.ToString();
                    objDetail.timestamp = DateTime.Now.ToString();

                    result = objDetail;
                    return BadRequest(result);
                }

            }
            else
            {
                objDetail.code = 401;
                objDetail.status = "Failed";
                objDetail.message = "Unauthorized";
                objDetail.timestamp = DateTime.Now.ToString();


                result = objDetail;
                return Unauthorized(result);
            }
        }

        [HttpPost]
        [Route("GisDumpYard/Update")]
        [EnableCors("MyCorsPolicy")]
        public async Task<ActionResult<HouseGisDetails>> GisDumpYardUpdate([FromHeader] string authorization, [FromHeader] int AppId, [FromBody] HouseGisUpdate obj)
        {
            using DevICTSBMMainEntities dbMain = new();
            //Trial tn = new();
            DumpTripStatusResult objDetail = new();

            var stream = authorization.Replace("Bearer ", string.Empty);
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(stream);
            var tokenS = jsonToken as JwtSecurityToken;

            var jti = tokenS.Claims.First(claim => claim.Type == "AppId").Value;


            var Auth_AppId = Convert.ToInt32(jti);
            if (Auth_AppId == AppId)
            {
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
                        //tn.id = obj.id;
                        //tn.updateUser = obj.updateUser;
                        //tn.updateTs = obj.updateTs;
                        //tn.geom = obj.geom;
                        GisSearch stn = new GisSearch();

                        stn.id = obj.id.ToString();
                        HttpClient client1 = new();

                        client1.BaseAddress = new Uri("http://114.143.244.130:9091/");
                        //client1.DefaultRequestHeaders.Accept.Clear();
                        client1.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        client1.DefaultRequestHeaders.Add("url", gis_url + "/" + gis_DBName);
                        client1.DefaultRequestHeaders.Add("username", gis_username);
                        client1.DefaultRequestHeaders.Add("password", gis_password);
                        HttpResponseMessage response1 = await client1.PostAsJsonAsync("dump-yard/search", stn);

                        if (response1.IsSuccessStatusCode)
                        {
                            HttpClient client = new();

                            //Start Old Code
                            //var json = JsonConvert.SerializeObject(tn, Formatting.Indented);
                            //var stringContent = new StringContent(json);
                            //stringContent.Headers.ContentType.MediaType = "application/json";
                            //stringContent.Headers.Add("url", gis_url + "/" + gis_DBName);
                            //stringContent.Headers.Add("username", gis_username);
                            //stringContent.Headers.Add("password", gis_password);
                            //var response = await client.PostAsync("http://114.143.244.130:9091/house", stringContent);
                            //End Old Code
                            if (obj.updateTs == null)
                            {
                                obj.updateTs = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fff");


                            }
                            if (obj.updateUser == null)
                            {
                                obj.updateUser = 0;
                            }

                            //Start New Code
                            client.BaseAddress = new Uri("http://114.143.244.130:9091/");
                            //client.DefaultRequestHeaders.Accept.Clear();
                            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                            client.DefaultRequestHeaders.Add("url", gis_url + "/" + gis_DBName);
                            client.DefaultRequestHeaders.Add("username", gis_username);
                            client.DefaultRequestHeaders.Add("password", gis_password);
                            HttpResponseMessage response = await client.PostAsJsonAsync("dump-yard", obj);
                            //End New Code
                            if (response.IsSuccessStatusCode)
                            {
                                var responseString = await response.Content.ReadFromJsonAsync<DumpTripStatusResult>();
                                //var dynamicobject = JsonConvert.DeserializeObject<dynamic>(responseString);
                                //objDetail.Add(new DumpTripStatusResult()
                                //{
                                //    code = (int)response.StatusCode,
                                //    status = "Success",
                                //    message = dynamicobject.message.ToString(),
                                //    errorMessages = dynamicobject.errorMessages.ToString(),
                                //    timestamp = DateTime.Now.ToString(),
                                //    data = dynamicobject.data
                                //});

                                objDetail = responseString;
                                return Ok(objDetail);
                            }
                            else
                            {
                                //objDetail.Add(new DumpTripStatusResult()
                                //{
                                //    code = (int)response.StatusCode,
                                //    status = "Failed",
                                //    message = "",
                                //    timestamp = DateTime.Now.ToString()
                                //});
                                var responseString = await response.Content.ReadAsStringAsync();
                                var dynamicobject = JsonConvert.DeserializeObject<dynamic>(responseString);

                                objDetail.code = (int)response.StatusCode;
                                objDetail.status = "Failed";
                                objDetail.timestamp = DateTime.Now.ToString();
                                return NotFound(objDetail);
                            }
                        }
                        else
                        {
                            objDetail.code = (int)response1.StatusCode;
                            objDetail.status = "Failed";
                            objDetail.message = "HouseId Not Available";
                            objDetail.timestamp = DateTime.Now.ToString();
                            return BadRequest(objDetail);
                        }

                    }
                    else
                    {

                        objDetail.code = 404;
                        objDetail.status = "Failed";
                        objDetail.message = "Gis Connection Not Available";
                        objDetail.timestamp = DateTime.Now.ToString();
                        return NotFound(objDetail);

                    }
                }
                catch (Exception ex)
                {
                    objDetail.code = 400;
                    objDetail.status = "Failed";
                    objDetail.message = ex.Message.ToString();
                    objDetail.timestamp = DateTime.Now.ToString();
                    return NotFound(objDetail);

                }
            }
            else
            {
                objDetail.code = 401;
                objDetail.status = "Failed";
                objDetail.message = "Unauthorized";
                objDetail.timestamp = DateTime.Now.ToString();


                result = objDetail;
                return Unauthorized(result);
            }

        }


        [HttpPost]
        [Route("EmployeeList")]
        [EnableCors("MyCorsPolicy")]
        public async Task<ActionResult<HouseGisDetails>> EmployeeList([FromHeader] string authorization, [FromHeader] int AppId)
        {
            using DevICTSBMMainEntities dbMain = new DevICTSBMMainEntities();
            HouseGisDetails objDetail = new HouseGisDetails();
            List<EmployeeList> EmpList = new List<EmployeeList>();

            var stream = authorization.Replace("Bearer ", string.Empty);
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(stream);
            var tokenS = jsonToken as JwtSecurityToken;

            var jti = tokenS.Claims.First(claim => claim.Type == "AppId").Value;


            var Auth_AppId = Convert.ToInt32(jti);

            if (Auth_AppId == AppId)
            {
                try
                {
                    var GIS_CON = dbMain.GIS_AppConnections.Where(c => c.AppId == AppId).FirstOrDefault();

                    if (GIS_CON != null)
                    {
                        var gis_url = GIS_CON.DataSource;
                        var gis_DBName = GIS_CON.InitialCatalog;
                        var gis_username = GIS_CON.UserId;
                        var gis_password = GIS_CON.Password;

                        HttpClient client = new HttpClient();

                        using (DevICTSBMChildEntities db = new DevICTSBMChildEntities(AppId))
                        {
                            var EmployeeList = await db.UserMasters.Select(x => new { x.userId, x.userName }).ToListAsync();

                            //foreach (var x in EmployeeList)
                            //{
                            //    if (x.userId > 0)
                            //    {

                            //        EmpList.Add(new EmployeeList()
                            //        {
                            //            Userid = x.userId,
                            //            Empname = x.userName,
                            //        });
                            //    }

                            //}

                            objDetail.code = 200;
                            objDetail.status = "Success";
                            objDetail.timestamp = DateTime.Now.ToString();
                            if (EmployeeList.Count > 0)
                            {
                                objDetail.message = "Data Found";

                            }
                            else
                            {
                                objDetail.message = "Data Not Available";
                            }
                            objDetail.data = EmployeeList;
                        }

                        result = objDetail;
                        return Ok(result);
                    }
                    else
                    {

                        objDetail.code = 404;
                        objDetail.status = "Failed";
                        objDetail.message = "GIS Connection Are Not Available";
                        objDetail.timestamp = DateTime.Now.ToString();

                        result = objDetail;
                        return NotFound(result);
                    }
                }
                catch (Exception ex)
                {

                    objDetail.code = 400;
                    objDetail.status = "Failed";
                    objDetail.message = ex.Message.ToString();
                    objDetail.timestamp = DateTime.Now.ToString();

                    result = objDetail;

                    return NotFound(result);
                }
            }
            else
            {
                objDetail.code = 401;
                objDetail.status = "Failed";
                objDetail.message = "Unauthorized";
                objDetail.timestamp = DateTime.Now.ToString();


                result = objDetail;
                return Unauthorized(result);
            }
        }

        [HttpPost]
        [Route("WardList")]
        [EnableCors("MyCorsPolicy")]
        public async Task<ActionResult<HouseGisDetails>> WardList([FromHeader] string authorization, [FromHeader] int AppId)
        {
            using DevICTSBMMainEntities dbMain = new DevICTSBMMainEntities();
            HouseGisDetails objDetail = new HouseGisDetails();

            var stream = authorization.Replace("Bearer ", string.Empty);
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(stream);
            var tokenS = jsonToken as JwtSecurityToken;

            var jti = tokenS.Claims.First(claim => claim.Type == "AppId").Value;


            var Auth_AppId = Convert.ToInt32(jti);

            if (Auth_AppId == AppId)
            {
                try
                {
                    var GIS_CON = dbMain.GIS_AppConnections.Where(c => c.AppId == AppId).FirstOrDefault();

                    if (GIS_CON != null)
                    {
                        var gis_url = GIS_CON.DataSource;
                        var gis_DBName = GIS_CON.InitialCatalog;
                        var gis_username = GIS_CON.UserId;
                        var gis_password = GIS_CON.Password;

                        HttpClient client = new HttpClient();

                        using (DevICTSBMChildEntities db = new DevICTSBMChildEntities(AppId))
                        {
                            var WardList = await db.WardNumbers.Select(x => new { x.Id, x.WardNo }).ToListAsync();


                            objDetail.code = 200;
                            objDetail.status = "Success";
                            objDetail.timestamp = DateTime.Now.ToString();
                            if (WardList.Count > 0)
                            {
                                objDetail.message = "Data Found";

                            }
                            else
                            {
                                objDetail.message = "Data Not Available";
                            }
                            objDetail.data = WardList;
                        }

                        result = objDetail;
                        return Ok(result);
                    }
                    else
                    {

                        objDetail.code = 404;
                        objDetail.status = "Failed";
                        objDetail.message = "GIS Connection Are Not Available";
                        objDetail.timestamp = DateTime.Now.ToString();

                        result = objDetail;
                        return NotFound(result);
                    }
                }
                catch (Exception ex)
                {

                    objDetail.code = 400;
                    objDetail.status = "Failed";
                    objDetail.message = ex.Message.ToString();
                    objDetail.timestamp = DateTime.Now.ToString();

                    result = objDetail;

                    return NotFound(result);
                }
            }
            else
            {
                objDetail.code = 401;
                objDetail.status = "Failed";
                objDetail.message = "Unauthorized";
                objDetail.timestamp = DateTime.Now.ToString();


                result = objDetail;
                return Unauthorized(result);
            }
        }


        [HttpPost]
        [Route("AreaList")]
        [EnableCors("MyCorsPolicy")]
        public async Task<ActionResult<HouseGisDetails>> AreaList([FromHeader] string authorization, [FromHeader] int AppId)
        {
            using DevICTSBMMainEntities dbMain = new DevICTSBMMainEntities();
            HouseGisDetails objDetail = new HouseGisDetails();

            var stream = authorization.Replace("Bearer ", string.Empty);
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(stream);
            var tokenS = jsonToken as JwtSecurityToken;

            var jti = tokenS.Claims.First(claim => claim.Type == "AppId").Value;


            var Auth_AppId = Convert.ToInt32(jti);

            if (Auth_AppId == AppId)
            {
                try
                {
                    var GIS_CON = dbMain.GIS_AppConnections.Where(c => c.AppId == AppId).FirstOrDefault();

                    if (GIS_CON != null)
                    {
                        var gis_url = GIS_CON.DataSource;
                        var gis_DBName = GIS_CON.InitialCatalog;
                        var gis_username = GIS_CON.UserId;
                        var gis_password = GIS_CON.Password;

                        HttpClient client = new HttpClient();

                        using (DevICTSBMChildEntities db = new DevICTSBMChildEntities(AppId))
                        {
                            var AreaList = await db.TeritoryMasters.Select(x => new { x.Id, x.Area }).ToListAsync();

                            objDetail.code = 200;
                            objDetail.status = "Success";
                            objDetail.timestamp = DateTime.Now.ToString();
                            if (AreaList.Count > 0)
                            {
                                objDetail.message = "Data Found";

                            }
                            else
                            {
                                objDetail.message = "Data Not Available";
                            }
                            objDetail.data = AreaList;
                        }

                        result = objDetail;
                        return Ok(result);
                    }
                    else
                    {

                        objDetail.code = 404;
                        objDetail.status = "Failed";
                        objDetail.message = "GIS Connection Are Not Available";
                        objDetail.timestamp = DateTime.Now.ToString();

                        result = objDetail;
                        return NotFound(result);
                    }
                }
                catch (Exception ex)
                {

                    objDetail.code = 400;
                    objDetail.status = "Failed";
                    objDetail.message = ex.Message.ToString();
                    objDetail.timestamp = DateTime.Now.ToString();

                    result = objDetail;

                    return NotFound(result);
                }
            }
            else
            {
                objDetail.code = 401;
                objDetail.status = "Failed";
                objDetail.message = "Unauthorized";
                objDetail.timestamp = DateTime.Now.ToString();


                result = objDetail;
                return Unauthorized(result);
            }
        }


        private object checkNull(string str)
        {
            string result = "";
            if (str == null || str == "")
            {
                result = "";
                return result;
            }
            else
            {
                result = str;
                return result;
            }
        }
    }

}

