﻿using ICTSBMCOREAPI.Dal.DataContexts.Models.DB;
using ICTSBMCOREAPI.SwachhBharat.API.Bll.Repository.Repository;
using ICTSBMCOREAPI.SwachhBhart.API.Bll.ViewModels.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
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
        public async Task<ActionResult<List<DumpTripStatusResult>>> GarbageMapTrail([FromHeader] string authorization, [FromBody] List<Trial> obj, [FromHeader] int AppId)
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

            if(Auth_AppId == AppId)
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

                        foreach (var item in obj)
                        {
                            tn.id = item.id;
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
                            timestamp = "",
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
            else
            {
                return Unauthorized();
            }
           
        }

        [HttpPost]
        [Route("Save/HouseMapTrail")]
        public async Task<ActionResult<List<DumpTripStatusResult>>> HouseMapTrail([FromHeader] string authorization, [FromBody] List<Trial> obj, [FromHeader] int AppId)
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

                        foreach (var item in obj)
                        {
                            tn.id = item.id;
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
                    return objDetail;
                }

                return objDetail;
            }
            else
            {
                return Unauthorized();
            }
           
        }

        [EnableCors("MyCorsPolicy")]
        [HttpGet]
        [Route("GisHouseDetails/all")]
      
        public async Task<ActionResult<List<HouseGisDetails>>> HouseGisDetailsAll(Type type, [FromHeader] string authorization, [FromHeader] int AppId)
        {
            
            //var message = "";

            using DevICTSBMMainEntities dbMain = new DevICTSBMMainEntities();
            List<HouseGisDetails> objDetail = new List<HouseGisDetails>();

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

                        var url = "http://114.143.244.130:9091/house/all";

                        var response = await client.GetAsync(url);


                        //var responseString = await response.Content.ReadAsStringAsync();
                        //var dynamicobject = JsonConvert.DeserializeObject<dynamic>(responseString);

                        if (response.IsSuccessStatusCode)
                        {
                            var responseString = await response.Content.ReadAsStringAsync();
                            var jsonParsed = JObject.Parse(responseString);
                            var dynamicobject = JsonConvert.DeserializeObject<dynamic>(responseString);
                            var jsonResult = jsonParsed["data"];
                            Response.Headers.Add("Access-Control-Allow-Origin", Request.Headers);
                            Response.Headers.Add("Access-Control-Allow-Methods", "GET");
                            Response.Headers.Add("Access-Control-Allow-Headers", "Content-Type");

                            List<GisResult> result = jsonResult.ToObject<List<GisResult>>();

                            objDetail.Add(new HouseGisDetails()
                            {
                                code = dynamicobject.code.ToString(),
                                status = dynamicobject.status.ToString(),
                                message = dynamicobject.message.ToString(),
                                errorMessages = dynamicobject.errorMessages.ToString(),
                                timestamp = dynamicobject.timestamp.ToString(),
                                data = result
                            });

                            return objDetail;

                        }
                        else
                        {
                            return null;
                        }
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
            else
            {
                return Unauthorized();
            }
           
            
        }



        [HttpGet]
        [Route("GisGarbageTrail/all")]
        public async Task<ActionResult<List<TrailsDetails>>> GarbageTrailgisAll([FromHeader] string authorization, [FromHeader] int AppId)
        {
            var message = "";

            using DevICTSBMMainEntities dbMain = new DevICTSBMMainEntities();
            List<TrailsDetails> objDetail = new List<TrailsDetails>();

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


                        //var responseString = await response.Content.ReadAsStringAsync();
                        //var dynamicobject = JsonConvert.DeserializeObject<dynamic>(responseString);

                        if (response.IsSuccessStatusCode)
                        {
                            var responseString = await response.Content.ReadAsStringAsync();
                            var jsonParsed = JObject.Parse(responseString);
                            var dynamicobject = JsonConvert.DeserializeObject<dynamic>(responseString);
                            var jsonResult = jsonParsed["data"];


                            List<GisTrailResult> result = jsonResult.ToObject<List<GisTrailResult>>();

                            objDetail.Add(new TrailsDetails()
                            {
                                code = dynamicobject.code.ToString(),
                                status = dynamicobject.status.ToString(),
                                message = dynamicobject.message.ToString(),
                                errorMessages = dynamicobject.errorMessages.ToString(),
                                timestamp = dynamicobject.timestamp.ToString(),
                                data = result
                            });

                            return objDetail;

                        }
                        else
                        {
                            return null;
                        }

                      
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
            else
            {
                return Unauthorized();
            }
          
        }

        [HttpGet]
        [Route("GisHouseTrail/all")]
        public async Task<ActionResult<List<TrailsDetails>>> HouseTrailgisAll([FromHeader] string authorization, [FromHeader] int AppId)
        {
            var message = "";

            using DevICTSBMMainEntities dbMain = new DevICTSBMMainEntities();
            List<TrailsDetails> objDetail = new List<TrailsDetails>();


            var stream = authorization.Replace("Bearer ", string.Empty);
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(stream);
            var tokenS = jsonToken as JwtSecurityToken;

            var jti = tokenS.Claims.First(claim => claim.Type == "AppId").Value;


            var Auth_AppId = Convert.ToInt32(jti);

            if(Auth_AppId == AppId)
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


                        //var responseString = await response.Content.ReadAsStringAsync();
                        //var dynamicobject = JsonConvert.DeserializeObject<dynamic>(responseString);

                        if (response.IsSuccessStatusCode)
                        {
                            var responseString = await response.Content.ReadAsStringAsync();
                            var jsonParsed = JObject.Parse(responseString);
                            var dynamicobject = JsonConvert.DeserializeObject<dynamic>(responseString);
                            var jsonResult = jsonParsed["data"];


                            List<GisTrailResult> result = jsonResult.ToObject<List<GisTrailResult>>();

                            objDetail.Add(new TrailsDetails()
                            {
                                code = dynamicobject.code.ToString(),
                                status = dynamicobject.status.ToString(),
                                message = dynamicobject.message.ToString(),
                                errorMessages = dynamicobject.errorMessages.ToString(),
                                timestamp = dynamicobject.timestamp.ToString(),
                                data = result
                            });

                            return objDetail;

                        }
                        else
                        {
                            return null;
                        }


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
            else
            {
                return Unauthorized();
            }
           
        }
    }

}

