using ICTSBMCOREAPI.SwachhBharat.API.Bll.Repository.Repository;
using ICTSBMCOREAPI.SwachhBhart.API.Bll.ViewModels.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace ICTSBMCOREAPI.Controllers
{
    [Authorize]
    [Route("api/Supervisor")]
    [ApiController]
    public class SupervisorLoginController : ControllerBase
    {
        private readonly ILogger<SupervisorLoginController> _logger;
        private readonly IRepository objRep;
        public SupervisorLoginController(ILogger<SupervisorLoginController> logger, IRepository repository)
        {
            _logger = logger;
            objRep = repository;
        }



        [Route("Login")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<SBUser>> GetLogin([FromBody] SBUser objlogin)
        {
            //IEnumerable<string> headerValue1 = Request.Headers.GetValues("appId");
            //var id = headerValue1.FirstOrDefault();
            //int AppId = int.Parse(id);

           var EmpType = "A";

            SBUser objresponse = await objRep.CheckSupervisorUserLoginAsync(objlogin.userLoginId, objlogin.userPassword, EmpType);
            return objresponse;
        }


        [HttpGet]
        [Route("AllUlb")]
        public async Task<ActionResult<List<NameULB>>> GetUlb([FromHeader] string authorization, [FromHeader] int userId,[FromHeader] string status, [FromBody] SBUser objlogin)
        {
            var stream = authorization.Replace("Bearer ", string.Empty);
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(stream);
            var tokenS = jsonToken as JwtSecurityToken;

            var auth_userid = tokenS.Claims.First(claim => claim.Type == "userLoginId").Value;
            var auth_password = tokenS.Claims.First(claim => claim.Type == "userPassword").Value;



            if(auth_userid == objlogin.userLoginId && auth_password == objlogin.userPassword)
            {
                List<NameULB> objDetail = new List<NameULB>();
                objDetail = await objRep.GetUlbAsync(userId, status.ToLower());
                return objDetail;
            }
            else
            {
                return Unauthorized();
            }
        }

        [HttpGet]
        [Route("SelectedUlb")]
        public async Task<ActionResult<HSDashboard>> GetSelectedUlbData([FromHeader] string authorization, [FromHeader] int userId, [FromHeader] string EmpType, [FromHeader] int appId)
        {
            HSDashboard objDetail = new HSDashboard();
            objDetail = await objRep.GetSelectedUlbDataAsync(userId, EmpType, appId);
            return objDetail;
        }

        [HttpGet]
        [Route("QREmployeeList")]
        // Active Employee List For Filter
        public async Task<ActionResult<List<HSEmployee>>> GetQREmployeeList([FromHeader] string authorization, [FromHeader] int userId, [FromHeader] string EmpType, [FromHeader] int appId)
        {

            List<HSEmployee> objDetail = new List<HSEmployee>();
            objDetail = await objRep.GetQREmployeeListAsync(userId, EmpType, appId);
            return objDetail;
        }


        [HttpGet]
        [Route("QREmployeeDetailsList")]
        public async Task<ActionResult<List<HouseScanifyEmployeeDetails>>> GetQREmployeeDetailsList([FromHeader] string authorization, [FromHeader] int userId, [FromHeader] string EmpType, [FromHeader] int appId, [FromHeader] int qrEmpId, [FromHeader] bool status)
        {


            List<HouseScanifyEmployeeDetails> objDetail = new List<HouseScanifyEmployeeDetails>();
            objDetail = await objRep.GetQREmployeeDetailsListAsync(userId, EmpType, appId, qrEmpId, status);
            return objDetail;
        }

        [HttpGet]
        [Route("HouseScanifyDetailsGridRow")]
        // Show Live Data On Dashboard
        public async Task<ActionResult<List<HouseScanifyDetailsGridRow>>> GetHouseScanifyDetails([FromHeader] string authorization, [FromHeader] int userId, [FromHeader] int appId, [FromHeader] DateTime FromDate, [FromHeader] DateTime Todate)
        {

            IEnumerable<HouseScanifyDetailsGridRow> objDetail = new List<HouseScanifyDetailsGridRow>();
            objDetail = await objRep.GetHouseScanifyDetailsAsync(userId, FromDate, Todate, appId);
            return objDetail.ToList();
        }


        [HttpGet]
        [Route("AttendanceGridRow")]
        public async Task<ActionResult<List<HSAttendanceGrid>>> GetAttendanceDetails([FromHeader] string authorization, [FromHeader] int qrEmpId, [FromHeader] int appId, [FromHeader] DateTime FromDate, [FromHeader] DateTime Todate)
        {

            IEnumerable<HSAttendanceGrid> objDetail = new List<HSAttendanceGrid>();
            objDetail = await objRep.GetAttendanceDetailsAsync(qrEmpId, FromDate, Todate, appId);
            return objDetail.ToList();
        }

        [HttpGet]
        [Route("HouseDetails")]
        public async Task<ActionResult<List<HSHouseDetailsGrid>>> GetHouseDetails([FromHeader] string authorization, [FromHeader] int userId, [FromHeader] int appId, [FromHeader] DateTime FromDate, [FromHeader] DateTime Todate, [FromHeader] string ReferanceId)
        {


            IEnumerable<HSHouseDetailsGrid> objDetail = new List<HSHouseDetailsGrid>();
            objDetail = await objRep.GetHouseDetailsAsync(userId, FromDate, Todate, appId, ReferanceId);
            return objDetail.ToList();
        }


        [HttpGet]
        [Route("DumpYardDetails")]
        public async Task<ActionResult<List<HSDumpYardDetailsGrid>>> GetDumpYardDetails([FromHeader] string authorization, [FromHeader] int userId, [FromHeader] int appId, [FromHeader] DateTime FromDate, [FromHeader] DateTime Todate)
        {

            IEnumerable<HSDumpYardDetailsGrid> objDetail = new List<HSDumpYardDetailsGrid>();
            objDetail = await objRep.GetDumpYardDetailsAsync(userId, FromDate, Todate, appId);
            return objDetail.ToList();
        }


        [HttpGet]
        [Route("LiquidDetails")]
        public async Task<ActionResult<List<HSLiquidDetailsGrid>>> GetLiquidDetails([FromHeader] string authorization, [FromHeader] int userId, [FromHeader] int appId, [FromHeader] DateTime FromDate, [FromHeader] DateTime Todate)
        {

            IEnumerable<HSLiquidDetailsGrid> objDetail = new List<HSLiquidDetailsGrid>();
            objDetail = await objRep.GetLiquidDetailsAsync(userId, FromDate, Todate, appId);
            return objDetail.ToList();
        }


        [HttpGet]
        [Route("StreetDetails")]
        public async Task<ActionResult<List<HSStreetDetailsGrid>>> GetStreetDetails([FromHeader] string authorization, [FromHeader] int userId, [FromHeader] int appId, [FromHeader] DateTime FromDate, [FromHeader] DateTime Todate)
        {

            IEnumerable<HSStreetDetailsGrid> objDetail = new List<HSStreetDetailsGrid>();
            objDetail = await objRep.GetStreetDetailsAsync(userId, FromDate, Todate, appId);
            return objDetail.ToList();
        }

        [HttpGet]
        [Route("UserRoleList")]
        public async Task<ActionResult<List<UserRoleDetails>>> UserRoleList([FromHeader] string authorization, [FromHeader] int userId, [FromHeader] string EmpType, [FromHeader] bool status, [FromHeader] int EmpId)
        {

            IEnumerable<UserRoleDetails> objDetail = new List<UserRoleDetails>();
            objDetail = await objRep.UserRoleListAsync(userId, EmpType, status, EmpId);
            return objDetail.ToList();
        }

        [Route("AddHouseScanifyEmployee")]
        [HttpPost]
        public async Task<ActionResult<List<CollectionSyncResult>>> AddEmployee([FromHeader] string authorization, [FromHeader]int appId,[FromBody] List<HouseScanifyEmployeeDetails> objRaw)
        {

            
            HouseScanifyEmployeeDetails gcDetail = new HouseScanifyEmployeeDetails();
            List<CollectionSyncResult> objres = new List<CollectionSyncResult>();
            try
            {

                foreach (var item in objRaw)
                {
                    gcDetail.qrEmpId = item.qrEmpId;
                    gcDetail.qrEmpName = item.qrEmpName;
                    gcDetail.qrEmpLoginId = item.qrEmpLoginId;
                    gcDetail.qrEmpPassword = item.qrEmpPassword;
                    gcDetail.qrEmpMobileNumber = item.qrEmpMobileNumber;
                    gcDetail.qrEmpAddress = item.qrEmpAddress;
                    gcDetail.imoNo = item.imoNo;
                    gcDetail.isActive = item.isActive;

                    CollectionSyncResult detail = await objRep.SaveAddEmployeeAsync(gcDetail, appId);
                    if (string.IsNullOrEmpty(detail.message))
                    {
                        objres.Add(new CollectionSyncResult()
                        {
                            ID = detail.ID,
                            status = "error",
                            message = "Record not inserted",
                            messageMar = "रेकॉर्ड सबमिट केले नाही"
                        });
                    }
                    else
                    {
                        objres.Add(new CollectionSyncResult()
                        {

                            status = detail.status,
                            messageMar = detail.messageMar,
                            message = detail.message

                        });
                    }
                    return objres;

                }


            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString(), ex);
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


        [Route("AddHouseScanifyUserRole")]
        [HttpPost]
        public async Task<ActionResult<List<CollectionSyncResult>>> AddUserRole([FromHeader] string authorization, [FromHeader] int appId, [FromBody]List<UserRoleDetails> objRaw)
        {
            
            UserRoleDetails gcDetail = new UserRoleDetails();
            List<CollectionSyncResult> objres = new List<CollectionSyncResult>();
            try
            {

                //IEnumerable<string> headerValue1 = Request.Headers.GetValues("appId");
                //var AppId = Convert.ToInt32(headerValue1.FirstOrDefault());


                foreach (var item in objRaw)
                {
                    gcDetail.EmpId = item.EmpId;
                    gcDetail.EmpName = item.EmpName;
                    gcDetail.LoginId = item.LoginId;
                    gcDetail.Password = item.Password;
                    gcDetail.EmpMobileNumber = item.EmpMobileNumber;
                    gcDetail.EmpAddress = item.EmpAddress;
                    gcDetail.type = item.type;
                    gcDetail.isActive = item.isActive;
                    gcDetail.isActiveULB = item.isActiveULB;
                    //gcDetail.LastModifyDateEntry = item.LastModifyDateEntry;

                    CollectionSyncResult detail = await objRep.SaveAddUserRoleAsync(gcDetail);
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
                    else
                    {
                        objres.Add(new CollectionSyncResult()
                        {

                            status = detail.status,
                            messageMar = detail.messageMar,
                            message = detail.message

                        });
                    }
                    return objres;

                }


            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString(), ex);
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





        [Route("UpdateQRstatus")]
        [HttpPost]
        public async Task<ActionResult<List<CollectionQRStatusResult>>> UpdateQRstatus([FromHeader] string authorization, [FromHeader] int appId, [FromBody] List<HSHouseDetailsGrid> objRaw)
        {

            
            HSHouseDetailsGrid gcDetail = new HSHouseDetailsGrid();
            List<CollectionQRStatusResult> objres = new List<CollectionQRStatusResult>();
            try
            {

               


                foreach (var item in objRaw)
                {

                    gcDetail.QRStatus = item.QRStatus;
                    gcDetail.ReferanceId = item.ReferanceId;
                    CollectionQRStatusResult detail = await objRep.UpdateQRstatusAsync(gcDetail, appId);
                    if (detail.message == "")
                    {
                        objres.Add(new CollectionQRStatusResult()
                        {
                            ReferanceId = detail.ReferanceId,
                            status = "error",
                            message = "Record not inserted",
                            messageMar = "रेकॉर्ड सबमिट केले नाही"
                        });
                    }
                    else
                    {
                        objres.Add(new CollectionQRStatusResult()
                        {
                            ReferanceId = detail.ReferanceId,
                            status = detail.status,
                            messageMar = detail.messageMar,
                            message = detail.message

                        });
                    }
                    return objres;

                }


            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString(), ex);
                objres.Add(new CollectionQRStatusResult()
                {
                    ReferanceId = "",
                    status = "error",
                    message = "Something is wrong,Try Again.. ",
                    messageMar = "काहीतरी चुकीचे आहे, पुन्हा प्रयत्न करा..",
                });
                return objres;

            }

            objres.Add(new CollectionQRStatusResult()
            {
                ReferanceId = "",
                status = "error",
                message = "Record not inserted",
                messageMar = "रेकॉर्ड सबमिट केले नाही",
            });

            return objres;

        }


        [HttpPost]
        [Route("SupervisorAttendenceIn")]
        public async Task<ActionResult<Result>> SaveQrEmployeeAttendence([FromHeader] string authorization, [FromBody]BigVQREmployeeAttendenceVM obj)
        {
           
            Result objDetail = new Result();
            objDetail = await objRep.SaveSupervisorAttendenceAsync(obj, 0);
            return objDetail;
        }


        [HttpPost]
        [Route("SupervisorAttendenceOut")]
        public async Task<ActionResult<Result>> SaveQrEmployeeAttendenceOut([FromHeader] string authorization, [FromBody]BigVQREmployeeAttendenceVM obj)
        {
            
            Result objDetail = new Result();
            objDetail = await objRep.SaveSupervisorAttendenceAsync(obj, 1);
            return objDetail;
        }
        [HttpPost]
        [Route("SupervisorAttendenceCheck")]
        public async Task<ActionResult<Result>> CheckQrEmployeeAttendence([FromHeader] string authorization, [FromBody]BigVQREmployeeAttendenceVM obj)
        {
            
            Result objDetail = new Result();
            objDetail = await objRep.CheckSupervisorAttendenceAsync(obj);
            return objDetail;
        }
        [HttpGet]
        [Route("GetHouseList")]
        public async Task<ActionResult<List<HouseDetailsVM>>> GetHouseWise([FromHeader] string authorization, [FromHeader] int appId, [FromHeader] string ListType)
        {
            List<HouseDetailsVM> objDetail = new List<HouseDetailsVM>();
            
            objDetail = await objRep.GetHouseListAsync(appId, ListType);
            return objDetail;

        }



        [HttpGet]
        [Route("GetAllHDSLDetails")]
        public async Task<ActionResult<List<HSHouseDetailsGrid>>> GetHouseListById([FromHeader] string authorization, [FromHeader] int appId,[FromHeader] string ReferanceId)
        {
            string EmpType = string.Empty;
            if (!string.IsNullOrEmpty(ReferanceId))
            {
                 EmpType = ReferanceId.Length > 0 ? ReferanceId.Substring(0, 1) : "";
            }
            
           
            List<HSHouseDetailsGrid> objDetail = new List<HSHouseDetailsGrid>();
            objDetail = await objRep.GetHDSLListAsync(appId, EmpType, ReferanceId);
            return objDetail;

        }

        [HttpGet]
        [Route("GetUserRoleAttendance")]
        public async Task<ActionResult<List<UREmployeeAttendence>>> UserRoleAttendance([FromHeader] string authorization, [FromHeader] int userId,[FromHeader] DateTime FromDate, [FromHeader] DateTime Todate,[FromHeader] bool IsMobile)
        {

            IEnumerable<UREmployeeAttendence> objDetail = new List<UREmployeeAttendence>();
            objDetail = await objRep.UserRoleAttendanceAsync(userId, FromDate, Todate, IsMobile);
            return objDetail.ToList();
        }

    }
}
