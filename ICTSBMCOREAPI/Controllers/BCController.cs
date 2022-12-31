using ICTSBMCOREAPI.Dal.DataContexts.Models.DB;
using ICTSBMCOREAPI.Dal.DataContexts.Models.DB.ChildModels;
using ICTSBMCOREAPI.Dal.DataContexts.Models.DB.MainModels;
using ICTSBMCOREAPI.SwachhBharat.API.Bll.Repository.Repository;
using ICTSBMCOREAPI.SwachhBhart.API.Bll.ViewModels.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
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
    public class BCController : ControllerBase
    {
        private readonly ILogger<BCController> _logger;
        private readonly IRepository objRep;
        public BCController(ILogger<BCController> logger, IRepository repository)
        {
            _logger = logger;
            objRep = repository;

        }

        [HttpPost]
        [Route("Save/DumpyardTrip")]
        public async Task<ActionResult<List<CollectionDumpSyncResult>>> SaveDumpyardTrip([FromHeader] string authorization, [FromHeader] int AppId, [FromBody] List<DumpTripVM> objRaw)
        {

            var stream = authorization.Replace("Bearer ", string.Empty);
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(stream);
            var tokenS = jsonToken as JwtSecurityToken;

            var jti = tokenS.Claims.First(claim => claim.Type == "AppId").Value;


            var Auth_AppId = Convert.ToInt32(jti);

            if (Auth_AppId == AppId)
            {
                DumpTripVM gcDetail = new DumpTripVM();
                DumpTripBCVM gcbcDetail = new DumpTripBCVM();
                TransIdVM transIdd = new TransIdVM();
                TransDumpTD objDetailDump = new TransDumpTD();
                List<CollectionDumpSyncResult> objres = new List<CollectionDumpSyncResult>();
                using DevICTSBMMainEntities dbMain = new DevICTSBMMainEntities();
                try
                {
                    HttpClient client = new HttpClient();
                    int ptid = 0;
                    foreach (var item in objRaw)
                    {
                        string[] transList = item.transId.Split('&');
                        int AppIds = Convert.ToInt32(transList[0]);
                        using (DevICTSBMChildEntities db = new DevICTSBMChildEntities(AppIds))
                        {
                            int ptripid = 0;
                            var pdtripid = db.TransDumpTDs.OrderByDescending(c => c.TransBcId).FirstOrDefault();
                            if (pdtripid != null)
                            {
                                ptripid = pdtripid.TransBcId;
                            }
                            ptid = Convert.ToInt32(ptripid) + 1;
                            gcbcDetail.tripId = ptid;
                            AppDetail objmain = dbMain.AppDetails.Where(x => x.AppId == AppIds).FirstOrDefault();
                            transList[2] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                            gcbcDetail.transId = string.Join("&", transList);
                            item.transId = string.Join("&", transList);
                            string encryptedString = EnryptString(item.transId);
                            gcbcDetail.transId = encryptedString;
                            gcbcDetail.dyId = item.dyId;
                            gcbcDetail.startDateTime = item.startDateTime;
                            gcbcDetail.endDateTime = item.endDateTime;
                            gcbcDetail.userId = item.userId;
                            gcbcDetail.houseList = item.houseList;
                            gcbcDetail.tripNo = item.tripNo;
                            gcbcDetail.vehicleNumber = item.vehicleNumber;
                            gcbcDetail.totalDryWeight = item.totalDryWeight;
                            gcbcDetail.totalWetWeight = item.totalWetWeight;
                            gcbcDetail.totalGcWeight = item.totalGcWeight;
                            gcbcDetail.totalNumberOfHouses = item.houseList.Length;
                            TimeSpan ts = Convert.ToDateTime(item.endDateTime) - Convert.ToDateTime(item.startDateTime);
                            gcbcDetail.totalHours = ts;
                            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
                            TimeSpan diffs = Convert.ToDateTime(item.startDateTime).ToUniversalTime() - origin;
                            var sd = Math.Floor(diffs.TotalSeconds);
                            gcbcDetail.startDateTime = Convert.ToString(sd);

                            TimeSpan diffe = Convert.ToDateTime(item.endDateTime).ToUniversalTime() - origin;
                            var ed = Math.Floor(diffe.TotalSeconds);
                            gcbcDetail.endDateTime = Convert.ToString(ed);
                            var json = JsonConvert.SerializeObject(gcbcDetail, Formatting.Indented);
                            var stringContent = new StringContent(json);
                            stringContent.Headers.ContentType.MediaType = "application/json";
                            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                            var username = "adminUser";
                            var password = "password";
                            string encoded = System.Convert.ToBase64String(Encoding.GetEncoding("ISO-8859-1")
                                                           .GetBytes(username + ":" + password));
                            client.DefaultRequestHeaders.Add("Authorization", "Basic " + encoded);

                            gcDetail.transId = gcbcDetail.transId;

                            string[] transList1 = item.transId.Split('&');
                            int AppId1 = Convert.ToInt32(transList1[0]);
                            AppDetail objmain1 = dbMain.AppDetails.Where(x => x.AppId == AppId1).FirstOrDefault();
                            CollectionDumpSyncResult result = new CollectionDumpSyncResult();
                            var response = client.PostAsync("http://35.164.93.75/trips", stringContent);
                            HttpResponseMessage rs = response.Result;
                            string responseString = rs.Content.ReadAsStringAsync().Result;
                            String[] spearator = responseString.Split(',');
                            string sdsd2 = spearator[2].Remove(0, 8);
                            var bcTransId = sdsd2.Substring(0, sdsd2.Length - 2);
                            gcDetail.bcTransId = bcTransId;
                            transIdd.transId = gcbcDetail.transId;

                            var Getresponse1 = client.GetAsync("http://35.164.93.75/trips?transId=" + gcbcDetail.transId);

                            HttpResponseMessage htr = Getresponse1.Result;
                            string getresponseString = htr.Content.ReadAsStringAsync().Result;
                            String[] getspearator = getresponseString.Split(',');
                            string getstatus = getspearator[3].Remove(0, 37);
                            var getstatus2 = getstatus.Substring(0, getstatus.Length - 2);
                            if (getstatus2 == "FAILED")
                            {
                                gcDetail.TStatus = false;
                            }
                            if (getstatus2 == "SUCCESS")
                            {
                                gcDetail.TStatus = true;
                            }
                            gcDetail.startDateTime = item.startDateTime;
                            gcDetail.endDateTime = item.endDateTime;
                            gcDetail.bcStartDateTime = Convert.ToInt32(sd);
                            gcDetail.bcEndDateTime = Convert.ToInt32(ed);
                            gcDetail.totalDryWeight = item.totalDryWeight;
                            gcDetail.totalWetWeight = item.totalWetWeight;
                            gcDetail.totalGcWeight = item.totalGcWeight;
                            Int64 dec1 = 9071858188;
                            Int64 a = 100000000;
                            gcDetail.bcTotalDryWeight = (Decimal.ToInt64(item.totalDryWeight * a) * dec1);
                            gcDetail.bcTotalWetWeight = (Decimal.ToInt64(item.totalWetWeight * a) * dec1);
                            gcDetail.bcTotalGcWeight = (Decimal.ToInt64(item.totalGcWeight * a) * dec1);
                            gcDetail.transId = item.transId;
                            gcDetail.houseList = item.houseList;
                            gcDetail.tripNo = item.tripNo;
                            gcDetail.vehicleNumber = item.vehicleNumber;
                            gcDetail.dyId = item.dyId;
                            gcDetail.userId = item.userId;
                            gcDetail.totalNumberOfHouses = item.houseList.Length;
                            gcDetail.totalHours = item.totalHours;

                            string time = Convert.ToString(gcbcDetail.totalHours);
                            double seconds = TimeSpan.Parse(time).TotalSeconds;
                            gcDetail.bcThr = Convert.ToInt32(seconds);

                            CollectionDumpSyncResult detail = await objRep.SaveDumpyardTripCollection(gcDetail);


                            objres.Add(new CollectionDumpSyncResult()
                            {
                                tripId = ptid,
                                transId = detail.transId,
                                status = detail.status,
                                messageMar = detail.messageMar,
                                message = detail.message,
                                dumpId = detail.dumpId,
                                bcTransId = detail.bcTransId,
                                gvstatus=detail.gvstatus
                            });
                        }
                    }
                }
                catch (Exception ex)
                {
                    objres.Add(new CollectionDumpSyncResult()
                    {
                        status = "error",
                        message = ex.Message,
                        messageMar = "काहीतरी चुकीचे आहे, पुन्हा प्रयत्न करा..",

                    });
                    return objres;
                }
                return objres;
            }
            else
            {
                return Unauthorized();
            }

           
        }

        [HttpGet]
        public string EnryptString(string strEncrypted)
        {
            byte[] b = System.Text.ASCIIEncoding.ASCII.GetBytes(strEncrypted);
            string encrypted = Convert.ToBase64String(b);
            return encrypted;
        }
    }
}
