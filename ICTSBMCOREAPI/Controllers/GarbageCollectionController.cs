using ICTSBMCOREAPI.Dal.DataContexts.Models.DB;
using ICTSBMCOREAPI.SwachhBharat.API.Bll.Repository.Repository;
using ICTSBMCOREAPI.SwachhBhart.API.Bll.ViewModels.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        public async Task<List<CollectionSyncResult>> OfflineUpload([FromHeader] int AppId, [FromHeader] string batteryStatus, List<SBGarbageCollectionView> objRaw)
        {

            SBGarbageCollectionView gcDetail = new SBGarbageCollectionView();
            List<CollectionSyncResult> objres = new List<CollectionSyncResult>();
            int _typeId = 0;


            //  string[] impath = new string[2];
            //  string[] arr = new string[4];
            int i = 0;
            try
            {
                string imagePath, FileName;


                var objmain = await dbMain.AppDetails.Where(x => x.AppId == AppId).FirstOrDefaultAsync();
                var AppDetailURL = objmain.baseImageUrl + objmain.basePath + objmain.Collection + "/";


                //  TimeSpan start = new TimeSpan(15, 0, 0);
                //  TimeSpan end = new TimeSpan(16, 0, 0);
                string hour = DateTime.Now.ToString("hh:mm tt");

                DateTime scheduledRun = DateTime.Today.AddHours(15);
                //if (AppId == 3035)
                //{
                //    _RepositoryApi.SaveAttendenceSettingsDetail(AppId, hour);

                //}
                //String timeStamp = GetTimestamp(new scheduledRun);
                //   DateTime.UtcNow - x.Timestamp == TimeSpan.FromMinutes(15)

                if (hour == "08:00 AM" && AppId == 3035)
                {
                    await objRep.SaveAttendenceSettingsDetailAsync(AppId, hour);
                }
                //List<Task> tasks = new List<Task>();
                foreach (var item in objRaw)
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

                    //string Image = "";
                    //if (impath.Length == 0 || impath[0] == null)
                    //{
                    //    gcDetail.gpBeforImage = "";
                    //    gcDetail.gpAfterImage = "";
                    //}
                    //else
                    //{
                    //    if (imageStart == "" || imageStart == string.Empty || imageStart == null)
                    //    {
                    //        gcDetail.gpBeforImage = "";
                    //        if (imageEnd != "" || imageEnd != string.Empty || imageEnd != null)

                    //        {
                    //            gcDetail.gpAfterImage = impath[0];
                    //        }
                    //    }
                    //    else
                    //    {
                    //        gcDetail.gpBeforImage = impath[0];

                    //        if (impath.Length == 0 || i <= 1)
                    //        {
                    //            gcDetail.gpAfterImage = "";
                    //        }
                    //        else
                    //        {
                    //            if (imageEnd != "" || imageEnd != string.Empty || imageEnd != null)

                    //            {
                    //                gcDetail.gpAfterImage = impath[1];
                    //            }
                    //        }
                    //    }
                    //}


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
                        isAttendenceOff = detail.isAttendenceOff
                    });
                    //}));
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
    }
}
