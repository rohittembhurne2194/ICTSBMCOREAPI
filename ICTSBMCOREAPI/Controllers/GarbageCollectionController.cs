using ICTSBMCOREAPI.Dal.DataContexts.Models.DB;
using ICTSBMCOREAPI.Dal.DataContexts.Models.DB.ChildModels;
using ICTSBMCOREAPI.SwachhBharat.API.Bll.Repository.Repository;
using ICTSBMCOREAPI.SwachhBhart.API.Bll.ViewModels.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace ICTSBMCOREAPI.Controllers
{
    [Authorize]
    [Route("api")]
    [ApiController]
    public class GarbageCollectionController : ControllerBase
    {
        private readonly ILogger<GarbageCollectionController> _logger;
        private readonly IRepository objRep;
        private readonly DevICTSBMMainEntities dbMain;

        public GarbageCollectionController(ILogger<GarbageCollectionController> logger, IRepository repository, DevICTSBMMainEntities dbMain)
        {
            _logger = logger;
            objRep = repository;
            this.dbMain = dbMain;
        }
        [HttpPost]
        [Route("Save/GarbageCollectionOfflineUpload")]
        public async Task<ActionResult<List<CollectionSyncResult>>> OfflineUpload([FromHeader] string authorization, [FromHeader] int AppId, [FromHeader] string batteryStatus, List<SBGarbageCollectionView> objRaw)
        {

            SBGarbageCollectionView gcDetail = new SBGarbageCollectionView();
            List<CollectionSyncResult> objres = new List<CollectionSyncResult>();
            int _typeId = 0;

            var stream = authorization.Replace("Bearer ", string.Empty);
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(stream);
            var tokenS = jsonToken as JwtSecurityToken;
            var jti = tokenS.Claims.First(claim => claim.Type == "AppId").Value;
            var Auth_AppId = Convert.ToInt32(jti);
            if (Auth_AppId == AppId)
            {
                int i = 0;
                try
                {
                    string imagePath, FileName;
                    var objmain = await dbMain.AppDetails.Where(x => x.AppId == AppId).FirstOrDefaultAsync();
                    var AppDetailURL = objmain.baseImageUrl + objmain.basePath + objmain.Collection + "/";
                    string hour = DateTime.Now.ToString("hh:mm tt");

                    DateTime scheduledRun = DateTime.Today.AddHours(15);
                    if (hour == "08:00 AM" && AppId == 3035)
                    {
                        await objRep.SaveAttendenceSettingsDetailAsync(AppId, hour);
                    }
                    //List<Task> tasks = new List<Task>();
                    foreach (var item in objRaw)
                    {
                        // DevSwachhBharatNagpurEntities db = new DevSwachhBharatNagpurEntities(AppId);
                        DevICTSBMChildEntities db = new DevICTSBMChildEntities(AppId);
                        bool IsExist = false;
                        DateTime Dateeee = Convert.ToDateTime(item.gcDate);
                        DateTime startDateTime = new DateTime(Dateeee.Year, Dateeee.Month, Dateeee.Day, 00, 00, 00, 000);
                        DateTime endDateTime = new DateTime(Dateeee.Year, Dateeee.Month, Dateeee.Day, 23, 59, 59, 999);
                        var gethouseid = await db.HouseMasters.Where(c => c.ReferanceId == item.ReferenceID).Select(c => c.houseId).FirstOrDefaultAsync();
                        IsExist = (from p in db.GarbageCollectionDetails where p.houseId == gethouseid && p.gcDate >= startDateTime && p.gcDate <= endDateTime select p).Count() > 0;
                        //var gcd = db.GarbageCollectionDetails.Where(c => c.houseId == gethouseid && c.userId == item.userId && EntityFunctions.TruncateTime(c.gcDate) == EntityFunctions.TruncateTime(Dateeee)).FirstOrDefault();
                        var gcd = await db.GarbageCollectionDetails.Where(c => c.houseId == gethouseid && c.userId == item.userId && EF.Functions.DateDiffDay(c.gcDate, Dateeee) == 0).FirstOrDefaultAsync();
                        CollectionSyncResult result = new CollectionSyncResult();
                        if (item.ReferenceID == null || item.Lat == null || item.Long == null || item.gcDate == null || (item.gcType == 3 && item.totalGcWeight == null) || item.EmpType == null || item.userId == 0 || item.vehicleNumber == null)
                        {
                            result.ID = 0;
                            result.status = "error";
                            result.referenceID = item.ReferenceID;
                            if (item.ReferenceID == null)
                            {
                                result.message = "Reference ID Is Empty";
                                result.messageMar = "रेफेरेंस आयडी रिक्त आहे";
                            }
                            if (item.Lat == null || item.Long == null)
                            {
                                result.message = "Lat Long Is Empty";
                                result.messageMar = "ल्याट लॉन्ग रिक्त आहे";
                            }
                            if (item.gcDate == null)
                            {
                                result.message = "Date Is Empty";
                                result.messageMar = "तारीख रिक्त आहे";
                            }
                            if ((item.gcType == 3 && item.totalGcWeight == null))
                            {
                                result.message = "Total Weight Is Empty";
                                result.messageMar = "टोटल वेट रिक्त आहे";
                            }
                            if (item.userId == 0)
                            {
                                result.message = "UserId Is Empty";
                                result.messageMar = "यूजर आयडी रिक्त आहे";
                            }
                            if (item.vehicleNumber == null)
                            {
                                result.message = "Vehicle Number Is Empty";
                                result.messageMar = "वाहन क्रमांक रिक्त आहे";
                            }
                            if (item.EmpType == null)
                            {
                                result.message = "Employee Type Is Empty";
                                result.messageMar = "कर्मचारी प्रकार रिक्त आहे";
                            }

                            objres.Add(new CollectionSyncResult()
                            {
                                ID = result.ID,
                                status = result.status,
                                messageMar = result.messageMar,
                                message = result.message,
                                referenceID = result.referenceID,
                            });



                        }
                        else if ((item.userId != 0 && item.ReferenceID != null && item.gcType == 1) && IsExist == true && gcd == null && (item.Lat != null && item.Long != null))
                        {
                            if (gcd == null)
                            {
                                objres.Add(new CollectionSyncResult()
                                {
                                    ID = item.OfflineID,
                                    message = "This house id already scanned.",
                                    messageMar = "हे घर आयडी आधीच स्कॅन केले आहे.",
                                    status = "error",
                                    referenceID = item.ReferenceID,
                                    IsExist = true
                                });
                            }

                        }

                        else
                        {
                            //tasks.Add(Task.Run(async () => {
                            gcDetail.userId = item.userId;
                            gcDetail.Distance = item.Distance;

                            switch (item.gcType)
                            {
                                case 1:
                                    string houseid1 = item.ReferenceID;
                                    string[] houseList = houseid1.Split(',');
                                    gcDetail.houseId = houseList[0];
                                    if (houseList.Length > 1)
                                    {
                                        gcDetail.wastetype = houseList[1];
                                    }
                                    //   gcDetail.houseId = item.ReferenceID;
                                    gcDetail.gcType = item.gcType;
                                    gcDetail.EmpType = item.EmpType;
                                    break;
                                case 2:
                                    gcDetail.gpId = item.ReferenceID;
                                    gcDetail.gcType = item.gcType;
                                    gcDetail.EmpType = item.EmpType;
                                    break;
                                case 3:
                                    gcDetail.dyId = item.ReferenceID;
                                    gcDetail.gcType = item.gcType;
                                    gcDetail.totalGcWeight = item.totalGcWeight;
                                    gcDetail.totalDryWeight = item.totalDryWeight;
                                    gcDetail.totalWetWeight = item.totalWetWeight;
                                    gcDetail.EmpType = item.EmpType;
                                    break;
                                case 4:
                                    gcDetail.LWId = item.ReferenceID;
                                    gcDetail.gcType = item.gcType;
                                    gcDetail.totalGcWeight = item.totalGcWeight;
                                    gcDetail.totalDryWeight = item.totalDryWeight;
                                    gcDetail.totalWetWeight = item.totalWetWeight;
                                    gcDetail.EmpType = item.EmpType;
                                    break;
                                case 5:
                                    gcDetail.SSId = item.ReferenceID;
                                    gcDetail.gcType = item.gcType;
                                    gcDetail.totalGcWeight = item.totalGcWeight;
                                    gcDetail.totalDryWeight = item.totalDryWeight;
                                    gcDetail.totalWetWeight = item.totalWetWeight;
                                    gcDetail.EmpType = item.EmpType;
                                    break;
                                case 6:
                                    gcDetail.vqrId = item.ReferenceID;
                                    gcDetail.gcType = item.gcType;
                                    gcDetail.totalGcWeight = item.totalGcWeight;
                                    gcDetail.totalDryWeight = item.totalDryWeight;
                                    gcDetail.totalWetWeight = item.totalWetWeight;
                                    gcDetail.EmpType = item.EmpType;
                                    break;
                                default:
                                    gcDetail.houseId = "";
                                    gcDetail.gpId = "";
                                    gcDetail.dyId = "";
                                    break;
                            }

                            gcDetail.OfflineID = item.OfflineID;
                            gcDetail.Lat = item.Lat;
                            gcDetail.Long = item.Long;
                            gcDetail.note = item.note;
                            gcDetail.garbageType = item.garbageType;
                            gcDetail.vehicleNumber = item.vehicleNumber;
                            gcDetail.gcDate = item.gcDate;
                            gcDetail.batteryStatus = item.batteryStatus;
                            gcDetail.Distance = item.Distance;
                            gcDetail.IsLocation = item.IsLocation;
                            gcDetail.IsOffline = item.IsOffline;


                            string imageStart = "", imageEnd = "";
                            imageStart = item.gpBeforImage;
                            imageEnd = item.gpAfterImage;
                            gcDetail.gpBeforImage = imageStart;
                            gcDetail.gpAfterImage = imageEnd;

                            CollectionSyncResult detail = await objRep.SaveGarbageCollectionOfflineAsync(gcDetail, AppId, _typeId);

                            if (detail.message == "")
                            {
                                objres.Add(new CollectionSyncResult()
                                {
                                    ID = detail.ID,
                                    status = "error",
                                    message = "Record not inserted",
                                    messageMar = "रेकॉर्ड सबमिट केले नाही"
                                });
                            }

                            objres.Add(new CollectionSyncResult()
                            {
                                ID = detail.ID,
                                status = detail.status,
                                messageMar = detail.messageMar,
                                message = detail.message,
                                isAttendenceOff = detail.isAttendenceOff,
                                referenceID = item.ReferenceID,
                            });

                        }
                    }

                    //await Task.WhenAll(tasks);
                    return objres;

                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.ToString(), ex);
                    objres.Add(new CollectionSyncResult()
                    {
                        houseId = gcDetail.houseId,
                        ID = gcDetail.OfflineID,
                        status = "error",
                        message = "Something is wrong,Try Again.. ",
                        messageMar = "काहीतरी चुकीचे आहे, पुन्हा प्रयत्न करा..",
                    });
                    return objres;

                }

                objres.Add(new CollectionSyncResult()
                {
                    ID = 0,
                    status = "error",
                    message = "Record not inserted",
                    messageMar = "रेकॉर्ड सबमिट केले नाही",
                });

                return objres;
            }
            else
            {
                return Unauthorized();
            }

        }

        [HttpPost]
        [Route("Save/GarbageCollectionOnlineUpload")]
        public async Task<ActionResult<List<CollectionSyncResult>>> OnlineUpload([FromHeader] string authorization, [FromHeader] int AppId, [FromHeader] string batteryStatus, SBGarbageCollectionView item)
        {

            SBGarbageCollectionView gcDetail = new SBGarbageCollectionView();
            List<CollectionSyncResult> objres = new List<CollectionSyncResult>();
            int _typeId = 0;

            var stream = authorization.Replace("Bearer ", string.Empty);
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(stream);
            var tokenS = jsonToken as JwtSecurityToken;
            var jti = tokenS.Claims.First(claim => claim.Type == "AppId").Value;
            var Auth_AppId = Convert.ToInt32(jti);
            if (Auth_AppId == AppId)
            {
                int i = 0;
                try
                {
                    if (item != null)
                    {

                        string imagePath, FileName;
                        var objmain = await dbMain.AppDetails.Where(x => x.AppId == AppId).FirstOrDefaultAsync();
                        var AppDetailURL = objmain.baseImageUrl + objmain.basePath + objmain.Collection + "/";
                        string hour = DateTime.Now.ToString("hh:mm tt");

                        DateTime scheduledRun = DateTime.Today.AddHours(15);
                        if (hour == "08:00 AM" && AppId == 3035)
                        {
                            await objRep.SaveAttendenceSettingsDetailAsync(AppId, hour);
                        }
                        //List<Task> tasks = new List<Task>();
                        //foreach (var item in objRaw)
                        //{
                        // DevSwachhBharatNagpurEntities db = new DevSwachhBharatNagpurEntities(AppId);
                        DevICTSBMChildEntities db = new DevICTSBMChildEntities(AppId);
                        bool IsExist = false;
                        DateTime Dateeee = Convert.ToDateTime(item.gcDate);
                        DateTime startDateTime = new DateTime(Dateeee.Year, Dateeee.Month, Dateeee.Day, 00, 00, 00, 000);
                        DateTime endDateTime = new DateTime(Dateeee.Year, Dateeee.Month, Dateeee.Day, 23, 59, 59, 999);
                        var gethouseid = await db.HouseMasters.Where(c => c.ReferanceId == item.ReferenceID).Select(c => c.houseId).FirstOrDefaultAsync();
                        IsExist = (from p in db.GarbageCollectionDetails where p.houseId == gethouseid && p.gcDate >= startDateTime && p.gcDate <= endDateTime select p).Count() > 0;
                        //var gcd = db.GarbageCollectionDetails.Where(c => c.houseId == gethouseid && c.userId == item.userId && EntityFunctions.TruncateTime(c.gcDate) == EntityFunctions.TruncateTime(Dateeee)).FirstOrDefault();
                        var gcd = await db.GarbageCollectionDetails.Where(c => c.houseId == gethouseid && c.userId == item.userId && EF.Functions.DateDiffDay(c.gcDate, Dateeee) == 0).FirstOrDefaultAsync();
                        CollectionSyncResult result = new CollectionSyncResult();
                        if (item.ReferenceID == null || item.Lat == null || item.Long == null || item.gcDate == null || (item.gcType == 3 && item.totalGcWeight == null) || item.EmpType == null || item.userId == 0 || item.vehicleNumber == null)
                        {
                            result.ID = 0;
                            result.status = "error";
                            result.referenceID = item.ReferenceID;
                            if (item.ReferenceID == null)
                            {
                                result.message = "Reference ID Is Empty";
                                result.messageMar = "रेफेरेंस आयडी रिक्त आहे";
                            }
                            if (item.Lat == null || item.Long == null)
                            {
                                result.message = "Lat Long Is Empty";
                                result.messageMar = "ल्याट लॉन्ग रिक्त आहे";
                            }
                            if (item.gcDate == null)
                            {
                                result.message = "Date Is Empty";
                                result.messageMar = "तारीख रिक्त आहे";
                            }
                            if ((item.gcType == 3 && item.totalGcWeight == null))
                            {
                                result.message = "Total Weight Is Empty";
                                result.messageMar = "टोटल वेट रिक्त आहे";
                            }
                            if (item.userId == 0)
                            {
                                result.message = "UserId Is Empty";
                                result.messageMar = "यूजर आयडी रिक्त आहे";
                            }
                            if (item.vehicleNumber == null)
                            {
                                result.message = "Vehicle Number Is Empty";
                                result.messageMar = "वाहन क्रमांक रिक्त आहे";
                            }
                            if (item.EmpType == null)
                            {
                                result.message = "Employee Type Is Empty";
                                result.messageMar = "कर्मचारी प्रकार रिक्त आहे";
                            }

                            objres.Add(new CollectionSyncResult()
                            {
                                ID = result.ID,
                                status = result.status,
                                messageMar = result.messageMar,
                                message = result.message,
                                referenceID = result.referenceID,
                            });



                        }
                        else if ((item.userId != 0 && item.ReferenceID != null && item.gcType == 1) && IsExist == true && gcd == null && (item.Lat != null && item.Long != null))
                        {
                            if (gcd == null)
                            {
                                objres.Add(new CollectionSyncResult()
                                {
                                    ID = item.OfflineID,
                                    message = "This house id already scanned.",
                                    messageMar = "हे घर आयडी आधीच स्कॅन केले आहे.",
                                    status = "error",
                                    referenceID = item.ReferenceID,
                                    IsExist = true
                                });
                            }

                        }

                        else
                        {
                            //tasks.Add(Task.Run(async () => {
                            gcDetail.userId = item.userId;
                            gcDetail.Distance = item.Distance;

                            switch (item.gcType)
                            {
                                case 1:
                                    string houseid1 = item.ReferenceID;
                                    string[] houseList = houseid1.Split(',');
                                    gcDetail.houseId = houseList[0];
                                    if (houseList.Length > 1)
                                    {
                                        gcDetail.wastetype = houseList[1];
                                    }
                                    //   gcDetail.houseId = item.ReferenceID;
                                    gcDetail.gcType = item.gcType;
                                    gcDetail.EmpType = item.EmpType;
                                    break;
                                case 2:
                                    gcDetail.gpId = item.ReferenceID;
                                    gcDetail.gcType = item.gcType;
                                    gcDetail.EmpType = item.EmpType;
                                    break;
                                case 3:
                                    gcDetail.dyId = item.ReferenceID;
                                    gcDetail.gcType = item.gcType;
                                    gcDetail.totalGcWeight = item.totalGcWeight;
                                    gcDetail.totalDryWeight = item.totalDryWeight;
                                    gcDetail.totalWetWeight = item.totalWetWeight;
                                    gcDetail.EmpType = item.EmpType;
                                    break;
                                case 4:
                                    gcDetail.LWId = item.ReferenceID;
                                    gcDetail.gcType = item.gcType;
                                    gcDetail.totalGcWeight = item.totalGcWeight;
                                    gcDetail.totalDryWeight = item.totalDryWeight;
                                    gcDetail.totalWetWeight = item.totalWetWeight;
                                    gcDetail.EmpType = item.EmpType;
                                    break;
                                case 5:
                                    gcDetail.SSId = item.ReferenceID;
                                    gcDetail.gcType = item.gcType;
                                    gcDetail.totalGcWeight = item.totalGcWeight;
                                    gcDetail.totalDryWeight = item.totalDryWeight;
                                    gcDetail.totalWetWeight = item.totalWetWeight;
                                    gcDetail.EmpType = item.EmpType;
                                    break;
                                case 6:
                                    gcDetail.vqrId = item.ReferenceID;
                                    gcDetail.gcType = item.gcType;
                                    gcDetail.totalGcWeight = item.totalGcWeight;
                                    gcDetail.totalDryWeight = item.totalDryWeight;
                                    gcDetail.totalWetWeight = item.totalWetWeight;
                                    gcDetail.EmpType = item.EmpType;
                                    break;
                                default:
                                    gcDetail.houseId = "";
                                    gcDetail.gpId = "";
                                    gcDetail.dyId = "";
                                    break;
                            }

                            gcDetail.OfflineID = item.OfflineID;
                            gcDetail.Lat = item.Lat;
                            gcDetail.Long = item.Long;
                            gcDetail.note = item.note;
                            gcDetail.garbageType = item.garbageType;
                            gcDetail.vehicleNumber = item.vehicleNumber;
                            gcDetail.gcDate = item.gcDate;
                            gcDetail.batteryStatus = item.batteryStatus;
                            gcDetail.Distance = item.Distance;
                            gcDetail.IsLocation = item.IsLocation;
                            gcDetail.IsOffline = item.IsOffline;


                            string imageStart = "", imageEnd = "";
                            imageStart = item.gpBeforImage;
                            imageEnd = item.gpAfterImage;
                            gcDetail.gpBeforImage = imageStart;
                            gcDetail.gpAfterImage = imageEnd;

                            CollectionSyncResult detail = await objRep.SaveGarbageCollectionOfflineAsync(gcDetail, AppId, _typeId);

                            if (detail.message == "")
                            {
                                objres.Add(new CollectionSyncResult()
                                {
                                    ID = detail.ID,
                                    status = "error",
                                    message = "Record not inserted",
                                    messageMar = "रेकॉर्ड सबमिट केले नाही"
                                });
                            }

                            objres.Add(new CollectionSyncResult()
                            {
                                ID = detail.ID,
                                status = detail.status,
                                messageMar = detail.messageMar,
                                message = detail.message,
                                isAttendenceOff = detail.isAttendenceOff,
                                referenceID = item.ReferenceID,
                            });

                        }
                        //  }

                        //await Task.WhenAll(tasks);
                        return objres;
                    }
                    else
                    {
                        objres.Add(new CollectionSyncResult()
                        {
                            houseId = gcDetail.houseId,
                            ID = gcDetail.OfflineID,
                            status = "error",
                            message = "Missing or Invalid Request Parameters.. ",
                            messageMar = "काहीतरी चुकीचे आहे, पुन्हा प्रयत्न करा..",
                        });
                        return objres;
                    }

                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.ToString(), ex);
                    objres.Add(new CollectionSyncResult()
                    {
                        houseId = gcDetail.houseId,
                        ID = gcDetail.OfflineID,
                        status = "error",
                        message = "Something is wrong,Try Again.. ",
                        messageMar = "काहीतरी चुकीचे आहे, पुन्हा प्रयत्न करा..",
                    });
                    return objres;

                }

                objres.Add(new CollectionSyncResult()
                {
                    ID = 0,
                    status = "error",
                    message = "Record not inserted",
                    messageMar = "रेकॉर्ड सबमिट केले नाही",
                });

                return objres;
            }
            else
            {
                return Unauthorized();
            }

        }

        [HttpPost]
        [Route("Save/SurveyDetails")]

        public async Task<ActionResult<List<CollectionSyncResult>>> AddSurveyDetails([FromHeader] int AppId,List<SurveyFormDetails> objRaw)
        {
           
            SurveyFormDetails svDetail = new SurveyFormDetails();
            List<CollectionSyncResult> objres = new List<CollectionSyncResult>();
            try
            {
                foreach (var item in objRaw)
                {
                    svDetail.ReferanceId = item.ReferanceId;
                    svDetail.name = item.name;
                    svDetail.houseLat = item.houseLat;
                    svDetail.houseLong = item.houseLong;
                    svDetail.mobileNumber = item.mobileNumber;
                    svDetail.age = item.age;
                    svDetail.dateOfBirth = item.dateOfBirth;
                    svDetail.gender = item.gender;
                    svDetail.bloodGroup = item.bloodGroup;
                    svDetail.qualification = item.qualification;
                    svDetail.occupation = item.occupation;
                    svDetail.maritalStatus = item.maritalStatus;
                    svDetail.marriageDate = item.marriageDate;
                    svDetail.livingStatus = item.livingStatus;

                    svDetail.totalAdults = item.totalAdults;
                    svDetail.totalChildren = item.totalChildren;
                    svDetail.totalSrCitizen = item.totalSrCitizen;
                    svDetail.totalMember = item.totalMember;

                    svDetail.willingStart = item.willingStart;
                    svDetail.resourcesAvailable = item.resourcesAvailable;

                    svDetail.memberJobOtherCity = item.memberJobOtherCity;

                    svDetail.noOfVehicle = item.noOfVehicle;
                    svDetail.vehicleType = item.vehicleType;
                    svDetail.twoWheelerQty = item.twoWheelerQty;
                    svDetail.threeWheelerQty = item.threeWheelerQty;
                    svDetail.fourWheelerQty = item.fourWheelerQty;
                    svDetail.noPeopleVote = item.noPeopleVote;
                    svDetail.socialMedia = item.socialMedia;
                    svDetail.onlineShopping = item.onlineShopping;
                    svDetail.paymentModePrefer = item.paymentModePrefer;
                    svDetail.onlinePayApp = item.onlinePayApp;
                    svDetail.insurance = item.insurance;

                    svDetail.underInsurer = item.underInsurer;
                    svDetail.ayushmanBeneficiary = item.ayushmanBeneficiary;
                    svDetail.boosterShot = item.boosterShot;
                    svDetail.memberDivyang = item.memberDivyang;

                    svDetail.createUserId = item.createUserId;
                    svDetail.createDate = item.createDate;
                    svDetail.updateUserId = item.updateUserId;
                    svDetail.updateDate = item.updateDate;

                    CollectionSyncResult detail = await objRep.SaveSurveyDetails(svDetail, AppId);

                    if (detail.message == "")
                    {
                        objres.Add(new CollectionSyncResult()
                        {
                            ID = detail.ID,
                            status = "error",
                            message = "Record not inserted",
                            messageMar = "रेकॉर्ड सबमिट केले नाही"
                        });
                    }

                    objres.Add(new CollectionSyncResult()
                    {

                        status = detail.status,
                        messageMar = detail.messageMar,
                        message = detail.message,
                        houseId = detail.houseId

                    });



                }
                return objres;

            }
            catch (Exception ex)
            {

                objres.Add(new CollectionSyncResult()
                {
                    ID = 0,
                    status = "error",
                    message = "Something is wrong,Try Again.. ",
                    messageMar = "काहीतरी चुकीचे आहे, पुन्हा प्रयत्न करा..",
                });
                return objres;

            }

            objres.Add(new CollectionSyncResult()
            {
                ID = 0,
                status = "error",
                message = "Record not inserted",
                messageMar = "रेकॉर्ड सबमिट केले नाही",
            });

            return objres;
        }

    }
}
