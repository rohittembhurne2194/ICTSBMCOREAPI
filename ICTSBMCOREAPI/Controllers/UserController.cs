using ICTSBMCOREAPI.Dal.DataContexts.Models.DB;
using ICTSBMCOREAPI.SwachhBharat.API.Bll.Repository.Repository;
using ICTSBMCOREAPI.SwachhBhart.API.Bll.ViewModels.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
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
        public async Task<List<SyncResult1>> SaveUserAttendenceOffline([FromHeader] int AppId, [FromHeader] string date, [FromHeader] string EmpType, [FromBody] List<SBUserAttendence> obj)
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
        [Route("Save/GarbageMapTrail")]
        public async Task<List<DumpTripStatusResult>> GarbageMapTrail([FromBody] List<Trial> obj, [FromHeader] int AppId)
        {
            var message = "";
           
              using DevICTSBMMainEntities dbMain = new DevICTSBMMainEntities();
            List<DumpTripStatusResult> objDetail = new List<DumpTripStatusResult>();
            try {
                    var GIS_CON = dbMain.GIS_AppConnections.Where(c => c.AppId == AppId).FirstOrDefault();
                  
                    if (GIS_CON != null)
                    {
                        var gis_url = GIS_CON.DataSource;
                        var gis_DBName = GIS_CON.InitialCatalog;
                        var gis_username = GIS_CON.UserId;
                        var gis_password = GIS_CON.Password;

                        HttpClient client = new HttpClient();
                        Trial tn = new Trial();
                       
                        foreach (var item in obj)
                        {
                            tn.trailId = item.trailId;
                            tn.startTs = item.startTs;
                            tn.endTs = item.endTs;
                            tn.createUser = item.createUser;
                            tn.geom = item.geom;
                            tn.createTs = item.createTs;
                            tn.updateTs = item.updateTs;
                            tn.updateUser = item.updateUser;


                            var json = JsonConvert.SerializeObject(tn, Formatting.Indented);
                            var stringContent = new StringContent(json);
                            stringContent.Headers.ContentType.MediaType = "application/json";
                            stringContent.Headers.Add("url", gis_url + "/" + gis_DBName);
                            stringContent.Headers.Add("username", gis_username);
                            stringContent.Headers.Add("password", gis_password);
                            var response = await client.PostAsync("http://114.143.244.130:9091/garbage-trail", stringContent);
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
                        }
                        return objDetail;
                    }
                    else
                    {

                        objDetail.Add(new DumpTripStatusResult()
                        {
                            code = "",
                            status = "",
                            message = "",
                            errorMessages = "GIS Connection Are Not Available",
                            timestamp ="",
                            data = ""
                        });
                        return objDetail;
                    }
              }
            
            catch (Exception ex)
            {
                //_logger.LogError(ex.ToString(), ex);

                objDetail.Add(new DumpTripStatusResult()
                {
                    code = "",
                    status = "",
                    message = "",
                    errorMessages = ex.Message.ToString(),
                    timestamp = "",
                    data = ""
                });
                return objDetail;

            }
            return objDetail;
        }

        [HttpPost]
        [Route("Save/HouseMapTrail")]
        public async Task<List<DumpTripStatusResult>> HouseMapTrail([FromBody] List<Trial> obj, [FromHeader] int AppId)
        {
            var message = "";

            using DevICTSBMMainEntities dbMain = new DevICTSBMMainEntities();
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


                    HttpClient client = new HttpClient();
                    Trial tn = new Trial();
                   
                    foreach (var item in obj)
                    {
                        tn.trailId = item.trailId;
                        tn.startTs = item.startTs;
                        tn.endTs = item.endTs;
                        tn.createUser = item.createUser;
                        tn.geom = item.geom;
                        tn.createTs = item.createTs;
                        tn.updateTs = item.updateTs;
                        tn.updateUser = item.updateUser;

                        var json = JsonConvert.SerializeObject(tn, Formatting.Indented);
                        var stringContent = new StringContent(json);
                        stringContent.Headers.ContentType.MediaType = "application/json";
                        stringContent.Headers.Add("url", gis_url + "/" + gis_DBName);
                        stringContent.Headers.Add("username", gis_username);
                        stringContent.Headers.Add("password", gis_password);

                        var response = await client.PostAsync("http://114.143.244.130:9091/house-trail", stringContent);
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
                    }
                    return objDetail;
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
                    return objDetail;
                }
            }
            catch(Exception ex)
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
                return objDetail;
            }

            return objDetail;
        }

        [HttpGet]
        [Route("GisHouseDetails/all")]
        public async Task<List<HouseGisDetails>> HouseGisDetailsAll([FromHeader] string authorization, [FromHeader] int AppId)
        {
            var txtJwtOut = "";
            //var jwtInput = txtJwtOut.Text;
            var message = "";

            using DevICTSBMMainEntities dbMain = new DevICTSBMMainEntities();
            List<HouseGisDetails> objDetail = new List<HouseGisDetails>();

            var stream = authorization.Replace("Bearer ", string.Empty);
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(stream);
            var tokenS = jsonToken as JwtSecurityToken;

            var jti = tokenS.Claims.First(claim => claim.Type == "jti").Value;

            //Extract the payload of the JWT
            var claims = tokenS.Claims;
            var jwtPayload = "{";
            foreach (Claim c in claims)
            {
                jwtPayload += '"' + c.Type + "\":\"" + c.Value + "\",";
            }
            jwtPayload += "}";
            txtJwtOut += "\r\nPayload:\r\n" + JToken.Parse(jwtPayload).ToString(Formatting.Indented);
        

            //ValidateToken(authorization);



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

                    var json = JsonConvert.SerializeObject(tn,Formatting.Indented);
                    var stringContent = new StringContent(json);
               
                    //stringContent.Headers.ContentType.MediaType = "application/json";
                    //stringContent.Headers.Add("url", gis_url + "/" + gis_DBName);
                    //stringContent.Headers.Add("username", gis_username);
                    //stringContent.Headers.Add("password", gis_password);

                    client.DefaultRequestHeaders.Add("url", gis_url + "/" + gis_DBName);
                    client.DefaultRequestHeaders.Add("username", gis_username);
                    client.DefaultRequestHeaders.Add("password", gis_password);

                    var url = "http://114.143.244.130:9091/house/all";

                    var response = await client.GetAsync(url);


                    var responseString = await response.Content.ReadAsStringAsync();
                    var dynamicobject = JsonConvert.DeserializeObject<dynamic>(responseString);


                    objDetail.Add(new HouseGisDetails()
                    {
                        code = dynamicobject.code.ToString(),
                        status = dynamicobject.status.ToString(),
                        message = dynamicobject.message.ToString(),
                        errorMessages = dynamicobject.errorMessages.ToString(),
                        timestamp = dynamicobject.timestamp.ToString(),
                        data = dynamicobject.data.ToString()
                    });

                    return objDetail;
                }
                else
                {

                    objDetail.Add(new HouseGisDetails()
                    {
                        code = "",
                        status = "",
                        message = "",
                        errorMessages = "GIS Connection Are Not Available",
                        timestamp = "",
                        data = ""
                    });
                    return objDetail;
                }
            }
            catch (Exception ex)
            {
                objDetail.Add(new HouseGisDetails()
                {
                    code = "",
                    status = "",
                    message = "",
                    errorMessages = ex.Message.ToString(),
                    timestamp = "",
                    data = ""
                });
                return objDetail;
            }
            return objDetail;
        }

        //private static bool ValidateToken(string authorization)
        //{
        //    var tokenHandler = new JwtSecurityTokenHandler();
        //    var validationParameters = GetValidationParameters();

        //    SecurityToken validatedToken;
        //    IPrincipal principal = tokenHandler.ValidateToken(authorization, validationParameters, out validatedToken);
        //    return true;
        //}

        //private static TokenValidationParameters GetValidationParameters()
        //{
        //    return new TokenValidationParameters()
        //    {
        //        ValidateLifetime = false, // Because there is no expiration in the generated token
        //        ValidateAudience = false, // Because there is no audiance in the generated token
        //        ValidateIssuer = false,   // Because there is no issuer in the generated token
        //        ValidIssuer = "Sample",
        //        ValidAudience = "Sample"
        //        //IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)) // The same key as the one that generate the token
        //    };
        //}

        [HttpGet]
        [Route("GisGarbageTrail/all")]
        public async Task<List<TrailsDetails>> GarbageTrailgisAll([FromHeader] int AppId)
        {
            var message = "";

            using DevICTSBMMainEntities dbMain = new DevICTSBMMainEntities();
            List<TrailsDetails> objDetail = new List<TrailsDetails>();

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

                    var json = JsonConvert.SerializeObject(tn, Formatting.Indented);
                    var stringContent = new StringContent(json);

                    //stringContent.Headers.ContentType.MediaType = "application/json";
                    //stringContent.Headers.Add("url", gis_url + "/" + gis_DBName);
                    //stringContent.Headers.Add("username", gis_username);
                    //stringContent.Headers.Add("password", gis_password);

                    client.DefaultRequestHeaders.Add("url", gis_url + "/" + gis_DBName);
                    client.DefaultRequestHeaders.Add("username", gis_username);
                    client.DefaultRequestHeaders.Add("password", gis_password);

                    var url = "http://114.143.244.130:9091/garbage-trail/all";

                    var response = await client.GetAsync(url);


                    var responseString = await response.Content.ReadAsStringAsync();
                    var dynamicobject = JsonConvert.DeserializeObject<dynamic>(responseString);


                    objDetail.Add(new TrailsDetails()
                    {
                        code = dynamicobject.code.ToString(),
                        status = dynamicobject.status.ToString(),
                        message = dynamicobject.message.ToString(),
                        errorMessages = dynamicobject.errorMessages.ToString(),
                        timestamp = dynamicobject.timestamp.ToString(),
                        data = dynamicobject.data.ToString()
                    });

                    return objDetail;
                }
                else
                {

                    objDetail.Add(new TrailsDetails()
                    {
                        code = "",
                        status = "",
                        message = "",
                        errorMessages = "GIS Connection Are Not Available",
                        timestamp = "",
                        data = ""
                    });
                    return objDetail;
                }
            }
            catch (Exception ex)
            {
                objDetail.Add(new TrailsDetails()
                {
                    code = "",
                    status = "",
                    message = "",
                    errorMessages = ex.Message.ToString(),
                    timestamp = "",
                    data = ""
                });
                return objDetail;
            }
            return objDetail;
        }

        [HttpGet]
        [Route("GisHouseTrail/all")]
        public async Task<List<TrailsDetails>> HouseTrailgisAll([FromHeader] int AppId)
        {
            var message = "";

            using DevICTSBMMainEntities dbMain = new DevICTSBMMainEntities();
            List<TrailsDetails> objDetail = new List<TrailsDetails>();

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

                    var json = JsonConvert.SerializeObject(tn, Formatting.Indented);
                    var stringContent = new StringContent(json);

                    //stringContent.Headers.ContentType.MediaType = "application/json";
                    //stringContent.Headers.Add("url", gis_url + "/" + gis_DBName);
                    //stringContent.Headers.Add("username", gis_username);
                    //stringContent.Headers.Add("password", gis_password);

                    client.DefaultRequestHeaders.Add("url", gis_url + "/" + gis_DBName);
                    client.DefaultRequestHeaders.Add("username", gis_username);
                    client.DefaultRequestHeaders.Add("password", gis_password);

                    var url = "http://114.143.244.130:9091/house-trail/all";

                    var response = await client.GetAsync(url);


                    var responseString = await response.Content.ReadAsStringAsync();
                    var dynamicobject = JsonConvert.DeserializeObject<dynamic>(responseString);


                    objDetail.Add(new TrailsDetails()
                    {
                        code = dynamicobject.code.ToString(),
                        status = dynamicobject.status.ToString(),
                        message = dynamicobject.message.ToString(),
                        errorMessages = dynamicobject.errorMessages.ToString(),
                        timestamp = dynamicobject.timestamp.ToString(),
                        data = dynamicobject.data.ToString()
                    });

                    return objDetail;
                }
                else
                {

                    objDetail.Add(new TrailsDetails()
                    {
                        code = "",
                        status = "",
                        message = "",
                        errorMessages = "GIS Connection Are Not Available",
                        timestamp = "",
                        data = ""
                    });
                    return objDetail;
                }
            }
            catch (Exception ex)
            {
                objDetail.Add(new TrailsDetails()
                {
                    code = "",
                    status = "",
                    message = "",
                    errorMessages = ex.Message.ToString(),
                    timestamp = "",
                    data = ""
                });
                return objDetail;
            }
            return objDetail;
        }
    }

}

