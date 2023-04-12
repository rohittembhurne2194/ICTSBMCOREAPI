using ICTSBMCOREAPI.Dal.DataContexts.Models.DB;
using ICTSBMCOREAPI.Dal.DataContexts.Models.DB.ChildModels;
using ICTSBMCOREAPI.Dal.DataContexts.Models.DB.ChildSPModels;
using ICTSBMCOREAPI.Dal.DataContexts.Models.DB.MainModels;
using ICTSBMCOREAPI.Dal.DataContexts.Models.DB.MainSPModels;
using ICTSBMCOREAPI.SwachhBhart.API.Bll.ViewModels.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity.Spatial;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;

using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Transactions;

namespace ICTSBMCOREAPI.SwachhBharat.API.Bll.Repository.Repository
{
    public class Repository : IRepository
    {
        //private readonly DevICTSBMMainEntities dbMain;
        private readonly IConfiguration _configuration;
        private readonly ILogger<Repository> _logger;

        private object result;

        public Repository(IConfiguration configuration, ILogger<Repository> logger)
        {
            //dbMain = devICTSBMMainEntities;
            _configuration = configuration;
            _logger = logger;
        }
        public async Task<string> LoginAsync(int AppId)
        {
            try
            {
                using (DevICTSBMMainEntities dbMain = new DevICTSBMMainEntities())
                {
                    var UserId = await dbMain.UserInApps.Where(a => a.AppId == AppId).Select(a => a.UserId).FirstOrDefaultAsync();
                    string Email = string.Empty;
                    //var result = await _signInManager.PasswordSignInAsync(signInModel.Email, signInModel.Password, false, false);
                    if (!string.IsNullOrEmpty(UserId))
                    {
                        Email = await dbMain.AspNetUsers.Where(a => a.Id == UserId).Select(a => a.Email).FirstOrDefaultAsync();
                    }

                    if (string.IsNullOrEmpty(Email))
                    {
                        return null;
                    }
                    var authClaims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name,Email),
                         new Claim("AppId",AppId.ToString()),
                        new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                    };
                    var authSigninkey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["JWT:Secret"]));
                    var token = new JwtSecurityToken(
                        issuer: _configuration["JWT:ValidIssuer"],
                        audience: _configuration["JWT:ValidAudience"],
                        expires: DateTime.Now.AddHours(12),
                        claims: authClaims,
                        signingCredentials: new SigningCredentials(authSigninkey, SecurityAlgorithms.HmacSha256Signature));
                    return new JwtSecurityTokenHandler().WriteToken(token);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString(), ex);
                return "Error Occured";
            }


        }

        public async Task<GisLoginResult> GisLoginAsync(string userLoginId, string userPassword)
        {
            GisLoginResult GisuserDetails = new GisLoginResult();
            try
            {
                using (DevICTSBMMainEntities dbMain = new DevICTSBMMainEntities())
                {
                    // Check the User exists
                    var GIS_CON = dbMain.AspNetGisUsers.Where(c => c.UserName == userLoginId && c.Password == userPassword).FirstOrDefault();

                    if(GIS_CON != null)
                    {
                        var UserId = await dbMain.UserInApps.Where(a => a.AppId == GIS_CON.AppId).Select(a => a.UserId).FirstOrDefaultAsync();
                        string Email = string.Empty;
                        //var result = await _signInManager.PasswordSignInAsync(signInModel.Email, signInModel.Password, false, false);
                        if (!string.IsNullOrEmpty(UserId))
                        {
                            Email = await dbMain.AspNetUsers.Where(a => a.Id == UserId).Select(a => a.Email).FirstOrDefaultAsync();
                        }

                        if (string.IsNullOrEmpty(Email))
                        {
                            return null;
                        }
                        var authClaims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name,Email),
                             new Claim("AppId",GIS_CON.AppId.ToString()),
                            new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                        };

                        var authSigninkey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["JWT:Secret"]));
                        var token = new JwtSecurityToken(
                            issuer: _configuration["JWT:ValidIssuer"],
                            audience: _configuration["JWT:ValidAudience"],
                            expires: DateTime.Now.AddHours(12),
                            claims: authClaims,
                            signingCredentials: new SigningCredentials(authSigninkey, SecurityAlgorithms.HmacSha256Signature));


                        logindata data = new logindata();
                        data.Appid = GIS_CON.AppId;
                        data.token = new JwtSecurityTokenHandler().WriteToken(token);

                        GisuserDetails.code = 200;
                        GisuserDetails.Status = "Success";
                        GisuserDetails.Message = "Login Successfully Done";
                        GisuserDetails.timestamp = DateTime.Now.ToString();
                        GisuserDetails.data = data;

                    }
                    else
                    {
                       

                        GisuserDetails.code = 200;
                        GisuserDetails.Status = "Failed";
                        GisuserDetails.Message = "Enter Username And Password Does Not Match";
                        GisuserDetails.timestamp = DateTime.Now.ToString();
                        GisuserDetails.data = null;
                    }
                    return GisuserDetails;
                }
                   
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString(), ex);

                GisuserDetails.code = 400;
                GisuserDetails.Status = "Failed";
                GisuserDetails.Message = ex.Message.ToString();
                GisuserDetails.timestamp = DateTime.Now.ToString();
                GisuserDetails.data = null;
                return GisuserDetails;
            }
        }

        public async Task<SBUser> CheckUserLoginAsync(string userName, string password, string imi, int AppId, string EmpType)
        {
            SBUser user = new SBUser();
            try
            {
                if (EmpType == "N")
                {
                    user = await CheckUserLoginForNormalAsync(userName, password, imi, AppId, EmpType);
                }

                if (EmpType == "L")
                {
                    user = await CheckUserLoginForLiquidAsync(userName, password, imi, AppId, EmpType);
                }

                if (EmpType == "S")
                {
                    user = await CheckUserLoginForStreetAsync(userName, password, imi, AppId, EmpType);
                }

                if (EmpType == "D")
                {
                    user = await CheckUserLoginForDumpAsync(userName, password, imi, AppId, EmpType);
                }
                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString(), ex);
                return user;
            }
        }



        public async Task<SBUser> CheckUserLoginForNormalAsync(string userName, string password, string imi, int AppId, string EmpType)
        {
            SBUser user = new SBUser();
            try
            {
                using (DevICTSBMChildEntities db = new DevICTSBMChildEntities(AppId))
                using (DevICTSBMMainEntities dbMain = new DevICTSBMMainEntities())
                {
                    var objmain = await dbMain.AppDetails.Where(x => x.AppId == AppId).FirstOrDefaultAsync();
                    var AppDetailURL = objmain.baseImageUrlCMS + objmain.basePath + objmain.UserProfile + "/";
                    var obj = await db.UserMasters.Where(c => c.userLoginId == userName & c.userPassword == password & c.isActive == true & c.EmployeeType == null).FirstOrDefaultAsync();
                    var objActive = await db.UserMasters.Where(c => c.userLoginId == userName & c.userPassword == password & c.EmployeeType == null).FirstOrDefaultAsync();



                    var objEmpMst = await db.QrEmployeeMasters.Where(c => c.qrEmpLoginId == userName & c.qrEmpPassword == password & c.isActive == true).FirstOrDefaultAsync();
                    if (obj == null)
                    {
                        user.userId = 0;
                        user.userLoginId = "";
                        user.userPassword = "";
                        user.status = "error";
                        user.gtFeatures = false;
                        user.imiNo = "";
                        user.EmpType = "";
                        user.message = "Contact Your Authorized Person.";
                        user.messageMar = "आपल्या अधिकृत व्यक्तीशी संपर्क साधा.";
                    }


                    if (obj != null && obj.userLoginId == userName && obj.userPassword == password)
                    {
                        if (obj.imoNo != null)
                        {
                            UserMaster us = await db.UserMasters.Where(c => c.userId == obj.userId).FirstOrDefaultAsync();
                            us.imoNo2 = imi;
                            await db.SaveChangesAsync();

                            user.type = checkNull(obj.Type);
                            user.userId = obj.userId;
                            user.userLoginId = "";
                            user.userPassword = "";
                            user.EmpType = "N";
                            user.imiNo = us.imoNo;
                            user.gtFeatures = objmain.NewFeatures;
                            user.status = "success"; user.message = "Login Successfully"; user.messageMar = "लॉगिन यशस्वी";
                        }
                        if (obj.imoNo == null || obj.imoNo.Trim() == "")
                        {
                            UserMaster us = await db.UserMasters.Where(c => c.userId == obj.userId).FirstOrDefaultAsync();
                            us.imoNo = imi;
                            await db.SaveChangesAsync();

                            user.type = checkNull(obj.Type);
                            user.userId = obj.userId;
                            user.userLoginId = "";
                            user.userPassword = "";
                            user.imiNo = us.imoNo2;
                            user.EmpType = "N";
                            user.gtFeatures = objmain.NewFeatures;
                            user.status = "success"; user.message = "Login Successfully"; user.messageMar = "लॉगिन यशस्वी";
                        }
                        //if (obj.imoNo != null && obj.imoNo2 !=null)
                        //{
                        //    UserMaster us = db.UserMasters.Where(c => c.userId == obj.userId).FirstOrDefault();
                        //    us.imoNo = imi;                      
                        //    user.type = checkNull(obj.Type);
                        //    user.userId = obj.userId;
                        //    user.userLoginId = "";
                        //    user.userPassword = "";
                        //    user.imiNo = us.imoNo2;             
                        //    user.EmpType = "N";
                        //    user.gtFeatures = objmain.NewFeatures;
                        //    user.status = "success"; user.message = "Login Successfully"; user.messageMar = "लॉगिन यशस्वी";
                        //    us.imoNo2 = null;
                        //    db.SaveChanges();
                        //}
                        else
                        {
                            if (obj.imoNo == imi)
                            {
                                user.type = checkNull(obj.Type);
                                user.typeId = checkIntNull(obj.Type);
                                user.userId = obj.userId;
                                user.userLoginId = "";
                                user.userPassword = "";
                                user.imiNo = obj.imoNo2;
                                user.EmpType = "N";
                                user.gtFeatures = objmain.NewFeatures;
                                user.status = "success"; user.message = "Login Successfully"; user.messageMar = "लॉगिन यशस्वी";
                            }
                            //if (obj.imoNo2 == imi)
                            //{
                            //    user.type = checkNull(obj.Type);
                            //    user.typeId = checkIntNull(obj.Type);
                            //    user.userId = obj.userId;
                            //    user.userLoginId = "";
                            //    user.userPassword = "";
                            //    user.imiNo = obj.imoNo;
                            //    user.EmpType = "N";
                            //    user.gtFeatures = objmain.NewFeatures;
                            //    user.status = "success"; user.message = "Login Successfully"; user.messageMar = "लॉगिन यशस्वी";
                            //}
                            //if (obj.imoNo2 == imi)
                            //{
                            //    user.type = checkNull(obj.Type);
                            //    user.typeId = checkIntNull(obj.Type);
                            //    user.userId = obj.userId;
                            //    user.userLoginId = "";
                            //    user.userPassword = "";
                            //    user.imiNo = "";
                            //    user.EmployeeType = obj.EmployeeType;
                            //    user.gtFeatures = objmain.NewFeatures;
                            //    user.status = "success"; user.message = "Login Successfully"; user.messageMar = "लॉगिन यशस्वी";
                            //}
                            else
                            {
                                user.userId = 0;
                                user.userLoginId = "";
                                user.userPassword = "";
                                user.status = "error";
                                user.gtFeatures = false;
                                user.imiNo = "";
                                user.EmpType = "";
                                user.message = "User is already login with another mobile.";
                                user.messageMar = "वापरकर्ता दुसर्या मोबाइलवर आधीपासूनच लॉगिन आहे.";

                            }
                        }

                    }

                    else if (obj != null && obj.userLoginId == userName && obj.userPassword != password)
                    {
                        user.userId = 0;
                        user.userLoginId = "";
                        user.userPassword = "";
                        user.status = "error";
                        user.gtFeatures = false;
                        user.imiNo = "";
                        user.EmpType = "";
                        user.message = "UserName or Passward not Match.";
                        user.messageMar = "वापरकर्ता नाव किंवा पासवर्ड जुळत नाही.";
                    }
                    //else if (objEmpMst == null)
                    // {
                    //     user.userId = 0;
                    //     user.userLoginId = "";
                    //     user.userPassword = "";
                    //     user.status = "error";
                    //     user.gtFeatures = false;
                    //     user.imiNo = "";
                    //     user.message = "Contact Your Authorized Person.";
                    //     user.messageMar = "आपल्या अधिकृत व्यक्तीशी संपर्क साधा.";
                    // }
                    else if (objEmpMst != null && objEmpMst.qrEmpLoginId == userName && objEmpMst.qrEmpPassword == password)
                    {


                        if (objEmpMst.imoNo == null || objEmpMst.imoNo.Trim() == "")
                        {
                            QrEmployeeMaster us = await db.QrEmployeeMasters.Where(c => c.qrEmpId == objEmpMst.qrEmpId).FirstOrDefaultAsync();
                            us.imoNo = imi;
                            await db.SaveChangesAsync();

                            user.type = checkNull(objEmpMst.type);
                            user.typeId = Convert.ToInt32(objEmpMst.typeId);
                            user.userId = objEmpMst.qrEmpId;
                            user.userLoginId = "";
                            user.userPassword = "";
                            user.imiNo = "";
                            user.EmpType = "N";
                            user.gtFeatures = objmain.NewFeatures;
                            user.status = "success"; user.message = "Login Successfully"; user.messageMar = "लॉगिन यशस्वी";
                        }
                        else
                        {
                            if (objEmpMst.imoNo == imi || imi == null)
                            {
                                user.type = checkNull(objEmpMst.type);
                                user.typeId = Convert.ToInt32(objEmpMst.typeId);
                                user.userId = objEmpMst.qrEmpId;
                                user.userLoginId = "";
                                user.userPassword = "";
                                user.imiNo = "";
                                user.EmpType = "N";
                                user.gtFeatures = objmain.NewFeatures;
                                user.status = "success"; user.message = "Login Successfully"; user.messageMar = "लॉगिन यशस्वी";
                            }
                            else
                            if (objEmpMst.isActive == false)
                            {
                                user.userId = 0;
                                user.userLoginId = "";
                                user.userPassword = "";
                                user.status = "error";
                                user.gtFeatures = false;
                                user.imiNo = "";
                                user.EmpType = "";
                                user.message = "Contact To Administrator.";
                                user.messageMar = "प्रशासकाशी संपर्क साधा.";
                            }
                            else
                            {
                                user.userId = 0;
                                user.userLoginId = "";
                                user.userPassword = "";
                                user.status = "error";
                                user.gtFeatures = false;
                                user.imiNo = "";
                                user.EmpType = "";
                                user.message = "User is already login with another mobile.";
                                user.messageMar = "वापरकर्ता दुसर्या मोबाइलवर आधीपासूनच लॉगिन आहे.";

                            }

                        }
                    }
                    //else if (objEmpMst.isActive == false)
                    //{
                    //    user.userId = 0;
                    //    user.userLoginId = "";
                    //    user.userPassword = "";
                    //    user.status = "error";
                    //    user.gtFeatures = false;
                    //    user.imiNo = "";
                    //    user.EmpType = "";
                    //    user.message = "Contact To Administrator.";
                    //    user.messageMar = "प्रशासकाशी संपर्क साधा.";
                    //}
                    else
                    {
                        var objEmpMst1 = await db.QrEmployeeMasters.Where(c => c.qrEmpLoginId == userName & c.qrEmpPassword == password).FirstOrDefaultAsync();
                        var objEmpMst2 = await db.UserMasters.Where(c => c.userLoginId == userName & c.userPassword == password & c.EmployeeType == null).FirstOrDefaultAsync();
                        if (objEmpMst1 != null)
                        {
                            if (objEmpMst1.isActive == false)
                            {
                                user.userId = 0;
                                user.userLoginId = "";
                                user.userPassword = "";
                                user.status = "error";
                                user.gtFeatures = false;
                                user.imiNo = "";
                                user.EmpType = "";
                                user.message = "Contact To Administrator.";
                                user.messageMar = "प्रशासकाशी संपर्क साधा.";
                            }
                            else if (objEmpMst1 != null && (objEmpMst1.qrEmpLoginId != userName || objEmpMst1.qrEmpPassword != password))
                            {
                                user.userId = 0;
                                user.userLoginId = "";
                                user.userPassword = "";
                                user.status = "error";
                                user.gtFeatures = false;
                                user.imiNo = "";
                                user.EmpType = "";
                                user.message = "UserName or Passward not Match.";
                                user.messageMar = "वापरकर्ता नाव किंवा पासवर्ड जुळत नाही.";
                            }
                        }
                        else if (objEmpMst2 != null)
                        {
                            if (objEmpMst2.isActive == false)
                            {
                                user.userId = 0;
                                user.userLoginId = "";
                                user.userPassword = "";
                                user.status = "error";
                                user.gtFeatures = false;
                                user.imiNo = "";
                                user.EmpType = "";
                                user.message = "Contact To Administrator.";
                                user.messageMar = "प्रशासकाशी संपर्क साधा.";
                            }
                            else if(objEmpMst2!=null && (objEmpMst2.userLoginId!= userName || objEmpMst2.userPassword != password))
                            {
                                user.userId = 0;
                                user.userLoginId = "";
                                user.userPassword = "";
                                user.status = "error";
                                user.gtFeatures = false;
                                user.imiNo = "";
                                user.EmpType = "";
                                user.message = "UserName or Passward not Match.";
                                user.messageMar = "वापरकर्ता नाव किंवा पासवर्ड जुळत नाही.";
                            }
                        }
                        else
                        {
                            user.userId = 0;
                            user.userLoginId = "";
                            user.userPassword = "";
                            user.status = "error";
                            user.gtFeatures = false;
                            user.imiNo = "";
                            user.message = "UserName or Passward not Match.";
                            user.EmpType = "";
                            user.messageMar = "वापरकर्ता नाव किंवा पासवर्ड जुळत नाही.";
                        }

                    }
                }
                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString(), ex);
                return user;
            }
        }
        public async Task<SBUser> CheckUserLoginForLiquidAsync(string userName, string password, string imi, int AppId, string EmpType)
        {
            SBUser user = new SBUser();
            try
            {
                using (DevICTSBMChildEntities db = new DevICTSBMChildEntities(AppId))
                using (DevICTSBMMainEntities dbMain = new DevICTSBMMainEntities())
                {
                    var objmain = await dbMain.AppDetails.Where(x => x.AppId == AppId).FirstOrDefaultAsync();
                    var AppDetailURL = objmain.baseImageUrlCMS + objmain.basePath + objmain.UserProfile + "/";
                    var obj = await db.UserMasters.Where(c => c.userLoginId == userName & c.isActive == true & c.EmployeeType == "L").FirstOrDefaultAsync();
                    var objEmpMst = await db.QrEmployeeMasters.Where(c => c.qrEmpLoginId == userName & c.isActive == true).FirstOrDefaultAsync();
                    if (obj == null)
                    {
                        user.userId = 0;
                        user.userLoginId = "";
                        user.userPassword = "";
                        user.status = "error";
                        user.gtFeatures = false;
                        user.imiNo = "";
                        user.EmpType = "";
                        user.message = "Contact Your Authorized Person.";
                        user.messageMar = "आपल्या अधिकृत व्यक्तीशी संपर्क साधा.";
                    }


                    if (obj != null && obj.userLoginId == userName && obj.userPassword == password)
                    {
                        if (obj.imoNo != null)
                        {
                            UserMaster us = await db.UserMasters.Where(c => c.userId == obj.userId).FirstOrDefaultAsync();
                            us.imoNo2 = imi;
                            await db.SaveChangesAsync();

                            user.type = checkNull(obj.Type);
                            user.userId = obj.userId;
                            user.userLoginId = "";
                            user.userPassword = "";
                            user.EmpType = "L";
                            user.gtFeatures = objmain.NewFeatures;
                            user.status = "success"; user.message = "Login Successfully"; user.messageMar = "लॉगिन यशस्वी";
                        }
                        if (obj.imoNo == null || obj.imoNo.Trim() == "")
                        {
                            UserMaster us = await db.UserMasters.Where(c => c.userId == obj.userId).FirstOrDefaultAsync();
                            us.imoNo = imi;
                            await db.SaveChangesAsync();

                            user.type = checkNull(obj.Type);
                            user.userId = obj.userId;
                            user.userLoginId = "";
                            user.userPassword = "";
                            user.imiNo = "";
                            user.EmpType = "L";
                            user.gtFeatures = objmain.NewFeatures;
                            user.status = "success"; user.message = "Login Successfully"; user.messageMar = "लॉगिन यशस्वी";
                        }

                        else
                        {
                            if (obj.imoNo == imi)
                            {
                                user.type = checkNull(obj.Type);
                                user.typeId = checkIntNull(obj.Type);
                                user.userId = obj.userId;
                                user.userLoginId = "";
                                user.userPassword = "";
                                user.imiNo = "";
                                user.EmpType = "L";
                                user.gtFeatures = objmain.NewFeatures;
                                user.status = "success"; user.message = "Login Successfully"; user.messageMar = "लॉगिन यशस्वी";
                            }
                            //if (obj.imoNo2 == imi)
                            //{
                            //    user.type = checkNull(obj.Type);
                            //    user.typeId = checkIntNull(obj.Type);
                            //    user.userId = obj.userId;
                            //    user.userLoginId = "";
                            //    user.userPassword = "";
                            //    user.imiNo = "";
                            //    user.EmployeeType = obj.EmployeeType;
                            //    user.gtFeatures = objmain.NewFeatures;
                            //    user.status = "success"; user.message = "Login Successfully"; user.messageMar = "लॉगिन यशस्वी";
                            //}
                            else
                            {
                                user.userId = 0;
                                user.userLoginId = "";
                                user.userPassword = "";
                                user.status = "error";
                                user.gtFeatures = false;
                                user.imiNo = "";
                                user.EmpType = "";
                                user.message = "User is already login with another mobile.";
                                user.messageMar = "वापरकर्ता दुसर्या मोबाइलवर आधीपासूनच लॉगिन आहे.";

                            }
                        }

                    }

                    else if (obj != null && obj.userLoginId == userName && obj.userPassword != password)
                    {
                        user.userId = 0;
                        user.userLoginId = "";
                        user.userPassword = "";
                        user.status = "error";
                        user.gtFeatures = false;
                        user.imiNo = "";
                        user.EmpType = "";
                        user.message = "UserName or Passward not Match.";
                        user.messageMar = "वापरकर्ता नाव किंवा पासवर्ड जुळत नाही.";
                    }
                    //else if (objEmpMst == null)
                    // {
                    //     user.userId = 0;
                    //     user.userLoginId = "";
                    //     user.userPassword = "";
                    //     user.status = "error";
                    //     user.gtFeatures = false;
                    //     user.imiNo = "";
                    //     user.message = "Contact Your Authorized Person.";
                    //     user.messageMar = "आपल्या अधिकृत व्यक्तीशी संपर्क साधा.";
                    // }
                    else if (objEmpMst != null && objEmpMst.qrEmpLoginId == userName && objEmpMst.qrEmpPassword == password)
                    {


                        if (objEmpMst.imoNo == null || objEmpMst.imoNo.Trim() == "")
                        {
                            QrEmployeeMaster us = await db.QrEmployeeMasters.Where(c => c.qrEmpId == objEmpMst.qrEmpId).FirstOrDefaultAsync();
                            us.imoNo = imi;
                            await db.SaveChangesAsync();

                            user.type = checkNull(objEmpMst.type);
                            user.typeId = Convert.ToInt32(objEmpMst.typeId);
                            user.userId = objEmpMst.qrEmpId;
                            user.userLoginId = "";
                            user.userPassword = "";
                            user.imiNo = "";
                            user.EmpType = obj.EmployeeType;
                            user.gtFeatures = objmain.NewFeatures;
                            user.status = "success"; user.message = "Login Successfully"; user.messageMar = "लॉगिन यशस्वी";
                        }
                        else
                        {
                            if (objEmpMst.imoNo == imi || imi == null)
                            {
                                user.type = checkNull(objEmpMst.type);
                                user.typeId = Convert.ToInt32(objEmpMst.typeId);
                                user.userId = objEmpMst.qrEmpId;
                                user.userLoginId = "";
                                user.userPassword = "";
                                user.imiNo = "";
                                user.EmpType = "L";
                                user.gtFeatures = objmain.NewFeatures;
                                user.status = "success"; user.message = "Login Successfully"; user.messageMar = "लॉगिन यशस्वी";
                            }
                            else
                            {
                                user.userId = 0;
                                user.userLoginId = "";
                                user.userPassword = "";
                                user.status = "error";
                                user.gtFeatures = false;
                                user.imiNo = "";
                                user.EmpType = "";
                                user.message = "User is already login with another mobile.";
                                user.messageMar = "वापरकर्ता दुसर्या मोबाइलवर आधीपासूनच लॉगिन आहे.";

                            }

                        }
                    }

                    else
                    {

                        user.userId = 0;
                        user.userLoginId = "";
                        user.userPassword = "";
                        user.status = "error";
                        user.gtFeatures = false;
                        user.imiNo = "";
                        user.EmpType = "";
                        user.message = "UserName or Passward not Match.";
                        user.messageMar = "वापरकर्ता नाव किंवा पासवर्ड जुळत नाही.";
                    }
                }
                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString(), ex);
                return user;
            }
        }
        public async Task<SBUser> CheckUserLoginForStreetAsync(string userName, string password, string imi, int AppId, string EmpType)
        {
            SBUser user = new SBUser();
            try
            {
                using (DevICTSBMChildEntities db = new DevICTSBMChildEntities(AppId))
                using (DevICTSBMMainEntities dbMain = new DevICTSBMMainEntities())
                {
                    var objmain = await dbMain.AppDetails.Where(x => x.AppId == AppId).FirstOrDefaultAsync();
                    var AppDetailURL = objmain.baseImageUrlCMS + objmain.basePath + objmain.UserProfile + "/";
                    var obj = await db.UserMasters.Where(c => c.userLoginId == userName & c.isActive == true & c.EmployeeType == "S").FirstOrDefaultAsync();
                    var objEmpMst = await db.QrEmployeeMasters.Where(c => c.qrEmpLoginId == userName & c.isActive == true).FirstOrDefaultAsync();
                    if (obj == null)
                    {
                        user.userId = 0;
                        user.userLoginId = "";
                        user.userPassword = "";
                        user.status = "error";
                        user.gtFeatures = false;
                        user.imiNo = "";
                        user.EmpType = "";
                        user.message = "Contact Your Authorized Person.";
                        user.messageMar = "आपल्या अधिकृत व्यक्तीशी संपर्क साधा.";
                    }


                    if (obj != null && obj.userLoginId == userName && obj.userPassword == password)
                    {
                        if (obj.imoNo != null)
                        {
                            UserMaster us = await db.UserMasters.Where(c => c.userId == obj.userId).FirstOrDefaultAsync();
                            us.imoNo2 = imi;
                            await db.SaveChangesAsync();

                            user.type = checkNull(obj.Type);
                            user.userId = obj.userId;
                            user.userLoginId = "";
                            user.userPassword = "";
                            user.EmpType = obj.EmployeeType;
                            user.gtFeatures = objmain.NewFeatures;
                            user.status = "success"; user.message = "Login Successfully"; user.messageMar = "लॉगिन यशस्वी";
                        }
                        if (obj.imoNo == null || obj.imoNo.Trim() == "")
                        {
                            UserMaster us = await db.UserMasters.Where(c => c.userId == obj.userId).FirstOrDefaultAsync();
                            us.imoNo = imi;
                            await db.SaveChangesAsync();

                            user.type = checkNull(obj.Type);
                            user.userId = obj.userId;
                            user.userLoginId = "";
                            user.userPassword = "";
                            user.imiNo = "";
                            user.EmpType = us.EmployeeType;
                            user.gtFeatures = objmain.NewFeatures;
                            user.status = "success"; user.message = "Login Successfully"; user.messageMar = "लॉगिन यशस्वी";
                        }

                        else
                        {
                            if (obj.imoNo == imi)
                            {
                                user.type = checkNull(obj.Type);
                                user.typeId = checkIntNull(obj.Type);
                                user.userId = obj.userId;
                                user.userLoginId = "";
                                user.userPassword = "";
                                user.imiNo = "";
                                user.EmpType = obj.EmployeeType;
                                user.gtFeatures = objmain.NewFeatures;
                                user.status = "success"; user.message = "Login Successfully"; user.messageMar = "लॉगिन यशस्वी";
                            }
                            //if (obj.imoNo2 == imi)
                            //{
                            //    user.type = checkNull(obj.Type);
                            //    user.typeId = checkIntNull(obj.Type);
                            //    user.userId = obj.userId;
                            //    user.userLoginId = "";
                            //    user.userPassword = "";
                            //    user.imiNo = "";
                            //    user.EmployeeType = obj.EmployeeType;
                            //    user.gtFeatures = objmain.NewFeatures;
                            //    user.status = "success"; user.message = "Login Successfully"; user.messageMar = "लॉगिन यशस्वी";
                            //}
                            else
                            {
                                user.userId = 0;
                                user.userLoginId = "";
                                user.userPassword = "";
                                user.status = "error";
                                user.gtFeatures = false;
                                user.imiNo = "";
                                user.EmpType = "";
                                user.message = "User is already login with another mobile.";
                                user.messageMar = "वापरकर्ता दुसर्या मोबाइलवर आधीपासूनच लॉगिन आहे.";

                            }
                        }

                    }

                    else if (obj != null && obj.userLoginId == userName && obj.userPassword != password)
                    {
                        user.userId = 0;
                        user.userLoginId = "";
                        user.userPassword = "";
                        user.status = "error";
                        user.gtFeatures = false;
                        user.imiNo = "";
                        user.EmpType = "";
                        user.message = "UserName or Passward not Match.";
                        user.messageMar = "वापरकर्ता नाव किंवा पासवर्ड जुळत नाही.";
                    }
                    //else if (objEmpMst == null)
                    // {
                    //     user.userId = 0;
                    //     user.userLoginId = "";
                    //     user.userPassword = "";
                    //     user.status = "error";
                    //     user.gtFeatures = false;
                    //     user.imiNo = "";
                    //     user.message = "Contact Your Authorized Person.";
                    //     user.messageMar = "आपल्या अधिकृत व्यक्तीशी संपर्क साधा.";
                    // }
                    else if (objEmpMst != null && objEmpMst.qrEmpLoginId == userName && objEmpMst.qrEmpPassword == password)
                    {


                        if (objEmpMst.imoNo == null || objEmpMst.imoNo.Trim() == "")
                        {
                            QrEmployeeMaster us = await db.QrEmployeeMasters.Where(c => c.qrEmpId == objEmpMst.qrEmpId).FirstOrDefaultAsync();
                            us.imoNo = imi;
                            await db.SaveChangesAsync();

                            user.type = checkNull(objEmpMst.type);
                            user.typeId = Convert.ToInt32(objEmpMst.typeId);
                            user.userId = objEmpMst.qrEmpId;
                            user.userLoginId = "";
                            user.userPassword = "";
                            user.imiNo = "";
                            user.EmpType = obj.EmployeeType;
                            user.gtFeatures = objmain.NewFeatures;
                            user.status = "success"; user.message = "Login Successfully"; user.messageMar = "लॉगिन यशस्वी";
                        }
                        else
                        {
                            if (objEmpMst.imoNo == imi || imi == null)
                            {
                                user.type = checkNull(objEmpMst.type);
                                user.typeId = Convert.ToInt32(objEmpMst.typeId);
                                user.userId = objEmpMst.qrEmpId;
                                user.userLoginId = "";
                                user.userPassword = "";
                                user.imiNo = "";
                                user.EmpType = "S";
                                user.gtFeatures = objmain.NewFeatures;
                                user.status = "success"; user.message = "Login Successfully"; user.messageMar = "लॉगिन यशस्वी";
                            }
                            else
                            {
                                user.userId = 0;
                                user.userLoginId = "";
                                user.userPassword = "";
                                user.status = "error";
                                user.gtFeatures = false;
                                user.imiNo = "";
                                user.EmpType = "";
                                user.message = "User is already login with another mobile.";
                                user.messageMar = "वापरकर्ता दुसर्या मोबाइलवर आधीपासूनच लॉगिन आहे.";

                            }

                        }
                    }

                    else
                    {
                        user.userId = 0;
                        user.userLoginId = "";
                        user.userPassword = "";
                        user.status = "error";
                        user.gtFeatures = false;
                        user.imiNo = "";
                        user.message = "UserName or Passward not Match.";
                        user.EmpType = "";
                        user.messageMar = "वापरकर्ता नाव किंवा पासवर्ड जुळत नाही.";
                    }
                }
                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString(), ex);
                return user;
            }
        }
        public async Task<SBUser> CheckUserLoginForDumpAsync(string userName, string password, string imi, int AppId, string EmpType)
        {
            SBUser user = new SBUser();
            try
            {
                using (DevICTSBMChildEntities db = new DevICTSBMChildEntities(AppId))
                using (DevICTSBMMainEntities dbMain = new DevICTSBMMainEntities())
                {
                    var objmain = await dbMain.AppDetails.Where(x => x.AppId == AppId).FirstOrDefaultAsync();
                    var AppDetailURL = objmain.baseImageUrlCMS + objmain.basePath + objmain.UserProfile + "/";
                    var obj = await db.UserMasters.Where(c => c.userLoginId == userName & c.isActive == true & c.EmployeeType == "D").FirstOrDefaultAsync();
                    var objEmpMst = await db.QrEmployeeMasters.Where(c => c.qrEmpLoginId == userName & c.isActive == true).FirstOrDefaultAsync();
                    if (obj == null)
                    {
                        user.userId = 0;
                        user.userLoginId = "";
                        user.userPassword = "";
                        user.status = "error";
                        user.gtFeatures = false;
                        user.imiNo = "";
                        user.EmpType = "";
                        user.message = "Contact Your Authorized Person.";
                        user.messageMar = "आपल्या अधिकृत व्यक्तीशी संपर्क साधा.";
                    }


                    if (obj != null && obj.userLoginId == userName && obj.userPassword == password)
                    {
                        if (obj.imoNo != null)
                        {
                            UserMaster us = await db.UserMasters.Where(c => c.userId == obj.userId).FirstOrDefaultAsync();
                            us.imoNo2 = imi;
                            await db.SaveChangesAsync();

                            user.type = checkNull(obj.Type);
                            user.userId = obj.userId;
                            user.userLoginId = "";
                            user.userPassword = "";
                            user.EmpType = obj.EmployeeType;
                            user.gtFeatures = objmain.NewFeatures;
                            user.status = "success"; user.message = "Login Successfully"; user.messageMar = "लॉगिन यशस्वी";
                        }
                        if (obj.imoNo == null || obj.imoNo.Trim() == "")
                        {
                            UserMaster us = await db.UserMasters.Where(c => c.userId == obj.userId).FirstOrDefaultAsync();
                            us.imoNo = imi;
                            await db.SaveChangesAsync();

                            user.type = checkNull(obj.Type);
                            user.userId = obj.userId;
                            user.userLoginId = "";
                            user.userPassword = "";
                            user.imiNo = "";
                            user.EmpType = us.EmployeeType;
                            user.gtFeatures = objmain.NewFeatures;
                            user.status = "success"; user.message = "Login Successfully"; user.messageMar = "लॉगिन यशस्वी";
                        }

                        else
                        {
                            if (obj.imoNo == imi)
                            {
                                user.type = checkNull(obj.Type);
                                user.typeId = checkIntNull(obj.Type);
                                user.userId = obj.userId;
                                user.userLoginId = "";
                                user.userPassword = "";
                                user.imiNo = "";
                                user.EmpType = obj.EmployeeType;
                                user.gtFeatures = objmain.NewFeatures;
                                user.status = "success"; user.message = "Login Successfully"; user.messageMar = "लॉगिन यशस्वी";
                            }

                            else
                            {
                                user.userId = 0;
                                user.userLoginId = "";
                                user.userPassword = "";
                                user.status = "error";
                                user.gtFeatures = false;
                                user.imiNo = "";
                                user.EmpType = "";
                                user.message = "User is already login with another mobile.";
                                user.messageMar = "वापरकर्ता दुसर्या मोबाइलवर आधीपासूनच लॉगिन आहे.";

                            }
                        }

                    }

                    else if (obj != null && obj.userLoginId == userName && obj.userPassword != password)
                    {
                        user.userId = 0;
                        user.userLoginId = "";
                        user.userPassword = "";
                        user.status = "error";
                        user.gtFeatures = false;
                        user.imiNo = "";
                        user.EmpType = "";
                        user.message = "UserName or Passward not Match.";
                        user.messageMar = "वापरकर्ता नाव किंवा पासवर्ड जुळत नाही.";
                    }

                    else if (objEmpMst != null && objEmpMst.qrEmpLoginId == userName && objEmpMst.qrEmpPassword == password)
                    {


                        if (objEmpMst.imoNo == null || objEmpMst.imoNo.Trim() == "")
                        {
                            QrEmployeeMaster us = await db.QrEmployeeMasters.Where(c => c.qrEmpId == objEmpMst.qrEmpId).FirstOrDefaultAsync();
                            us.imoNo = imi;
                            await db.SaveChangesAsync();

                            user.type = checkNull(objEmpMst.type);
                            user.typeId = Convert.ToInt32(objEmpMst.typeId);
                            user.userId = objEmpMst.qrEmpId;
                            user.userLoginId = "";
                            user.userPassword = "";
                            user.imiNo = "";
                            user.EmpType = obj.EmployeeType;
                            user.gtFeatures = objmain.NewFeatures;
                            user.status = "success"; user.message = "Login Successfully"; user.messageMar = "लॉगिन यशस्वी";
                        }
                        else
                        {
                            if (objEmpMst.imoNo == imi || imi == null)
                            {
                                user.type = checkNull(objEmpMst.type);
                                user.typeId = Convert.ToInt32(objEmpMst.typeId);
                                user.userId = objEmpMst.qrEmpId;
                                user.userLoginId = "";
                                user.userPassword = "";
                                user.imiNo = "";
                                user.EmpType = "D";
                                user.gtFeatures = objmain.NewFeatures;
                                user.status = "success"; user.message = "Login Successfully"; user.messageMar = "लॉगिन यशस्वी";
                            }
                            else
                            {
                                user.userId = 0;
                                user.userLoginId = "";
                                user.userPassword = "";
                                user.status = "error";
                                user.gtFeatures = false;
                                user.imiNo = "";
                                user.EmpType = "";
                                user.message = "User is already login with another mobile.";
                                user.messageMar = "वापरकर्ता दुसर्या मोबाइलवर आधीपासूनच लॉगिन आहे.";

                            }

                        }
                    }

                    else
                    {
                        user.userId = 0;
                        user.userLoginId = "";
                        user.userPassword = "";
                        user.status = "error";
                        user.gtFeatures = false;
                        user.imiNo = "";
                        user.message = "UserName or Passward not Match.";
                        user.EmpType = "";
                        user.messageMar = "वापरकर्ता नाव किंवा पासवर्ड जुळत नाही.";
                    }
                }
                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString(), ex);
                return user;
            }
        }



        public async Task<Result> SaveUserAttendenceAsync(SBUserAttendence obj, int AppId, int type, string batteryStatus)
        {
            Result result = new Result();

            if (obj.daDate.ToString("yyyy-MM-dd") != DateTime.Now.ToString("yyyy-MM-dd") && type==0)
            {
                result.status = "error";
                result.message = "Invalid Date";
                result.messageMar = "अवैध तारीख";
                result.emptype = obj.EmpType;
                return result;
            }

            if (obj.daEndDate.ToString("yyyy-MM-dd") != DateTime.Now.ToString("yyyy-MM-dd") && type == 1)
            {
                result.status = "error";
                result.message = "Invalid Date";
                result.messageMar = "अवैध तारीख";
                result.emptype = obj.EmpType;
                return result;
            }

            if ((string.IsNullOrEmpty(obj.EmpType)) == true)
            {
                result.status = "error";
                result.message = "Your Employee Type Is Empty";
                result.messageMar = "तुमचे कर्मचारी प्रकार रिक्त आहे";
                result.emptype = obj.EmpType;
                return result;
            }

            if (((obj.userId)) == 0)
            {
                result.status = "error";
                result.message = "Your UserID Is Empty";
                result.messageMar = "तुमची युजर आयडी रिक्त आहेत";
                result.emptype = obj.EmpType;
                return result;
            }

            if ((string.IsNullOrEmpty(obj.vtId)) == true || (string.IsNullOrEmpty(obj.vehicleNumber)) == true)
            {
                result.status = "error";
                result.message = "Your Vehicle Details Empty";
                result.messageMar = "तुमचे वाहन तपशील रिक्त आहे";
                result.emptype = obj.EmpType;
                return result;
            }

            if ((string.IsNullOrEmpty(obj.startLat)) == true && (string.IsNullOrEmpty(obj.startLong)) == true && type == 0)
            {
                result.status = "error";
                result.message = "Your start Lat / Long are Empty ";
                result.messageMar = "तुमची सुरुवात लॅट / लॉन्ग  रिक्त आहेत";
                result.emptype = obj.EmpType;
                return result;
            }



            if ((string.IsNullOrEmpty(obj.endLat)) == true && (string.IsNullOrEmpty(obj.endLong)) == true && type == 1)
            {
                result.status = "error";
                result.message = "Your End Lat / Long are Empty ";
                result.messageMar = "तुमचा शेवट लॅट / लॉन्ग रिक्त आहेत";
                result.emptype = obj.EmpType;
                return result;
            }


            try
            {
                if (obj.EmpType == "N")
                {
                    result = await SaveUserAttendenceForNormalAsync(obj, AppId, type, batteryStatus);
                }
                if (obj.EmpType == "L")
                {
                    result = await SaveUserAttendenceForLiquidAsync(obj, AppId, type, batteryStatus);
                }
                if (obj.EmpType == "S")
                {
                    result = await SaveUserAttendenceForStreetAsync(obj, AppId, type, batteryStatus);
                }
                if (obj.EmpType == "D")
                {
                    result = await SaveUserAttendenceForDumpAsync(obj, AppId, type, batteryStatus);
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString(), ex);
                return result;
            }
        }


        public async Task<CollectionDumpSyncResult> SaveDumpyardTripCollection(DumpTripVM obj)
        {
            string[] transList = obj.transId.Split('&');
            int AppId = Convert.ToInt32(transList[0]);
            using DevICTSBMMainEntities dbMain = new DevICTSBMMainEntities();
            AppDetail objmain = dbMain.AppDetails.Where(x => x.AppId == AppId).FirstOrDefault();

            obj.transId = string.Join("&", transList);
            string encryptedString = EnryptString(obj.transId);
            obj.transId = encryptedString;
            string decryptedString = DecryptString(encryptedString);
            CollectionDumpSyncResult result = new CollectionDumpSyncResult();
            using (DevICTSBMChildEntities db = new DevICTSBMChildEntities(AppId))
            {


                TransDumpTD objTransDumpTD = new TransDumpTD();
                DateTime Dateeee = Convert.ToDateTime(obj.endDateTime);
                string hlist = string.Empty;
                foreach (var item in obj.houseList)
                {
                    if (obj.houseList.Length > 1)
                    {
                        hlist += item + ",";
                    }

                }
                if (obj.houseList.Length > 1)
                {
                    hlist = hlist.Substring(0, hlist.Length - 1);
                }


                try
                {


                    if (obj.userId == 0 || string.IsNullOrEmpty(obj.transId) || string.IsNullOrEmpty(obj.dyId) || string.IsNullOrEmpty(obj.houseList.ToString()) || string.IsNullOrEmpty(obj.vehicleNumber) || obj.tripNo == 0 || obj.totalDryWeight == 0 || obj.totalWetWeight == 0 || obj.totalGcWeight == 0 || string.IsNullOrEmpty(obj.startDateTime.ToString()) || string.IsNullOrEmpty(obj.endDateTime.ToString()))
                    {

                        if (obj.userId == 0)
                        {
                            result.message = "User Id Is Not Null or Empty.";
                            result.messageMar = "वापरकर्ता आयडी शून्य किंवा रिक्त नाही.";
                        }
                        if (string.IsNullOrEmpty(obj.transId))
                        {
                            result.message = "transId Is Not Null or Empty.";
                            result.messageMar = "ट्रान्स आयडी शून्य किंवा रिक्त नाही.";
                        }
                        if (string.IsNullOrEmpty(obj.dyId))
                        {
                            result.message = "dyId Is Not Null or Empty.";
                            result.messageMar = "डी वाय आयडी शून्य किंवा रिक्त नाही.";
                        }
                        if (string.IsNullOrEmpty(hlist))
                        {
                            result.message = "houseList Is Not Null or Empty.";
                            result.messageMar = "घरांची लिस्ट शून्य किंवा रिक्त नाही.";
                        }
                        if (string.IsNullOrEmpty(obj.vehicleNumber))
                        {
                            result.message = "vehicleNumber Is Not Null or Empty.";
                            result.messageMar = "वाहन क्रमांक शून्य किंवा रिक्त नाही.";
                        }
                        if (obj.tripNo == 0)
                        {
                            result.message = "tripNo Is Not Null or Empty.";
                            result.messageMar = "ट्रिप क्रमांक शून्य किंवा रिक्त नाही.";
                        }
                        if (obj.totalDryWeight == 0)
                        {
                            result.message = "totalDryWeight Is Not Null or Empty.";
                            result.messageMar = "एकूण कोरडे वजन शून्य किंवा रिक्त नाही.";
                        }
                        if (obj.totalWetWeight == 0)
                        {
                            result.message = "totalWetWeight Is Not Null or Empty.";
                            result.messageMar = "एकूण ओले वजन शून्य किंवा रिक्त नाही.";
                        }
                        if (obj.totalGcWeight == 0)
                        {
                            result.message = "totalGcWeight Is Not Null or Empty.";
                            result.messageMar = "एकूण कचरा संकलन वजन शून्य किंवा रिकामे नाही.";
                        }
                        if (string.IsNullOrEmpty(obj.startDateTime.ToString()))
                        {
                            result.message = "startDateTime Is Not Null or Empty.";
                            result.messageMar = "प्रारंभ तारीख वेळ शून्य किंवा रिक्त नाही.";
                        }
                        if (string.IsNullOrEmpty(obj.endDateTime.ToString()))
                        {
                            result.message = "endDateTime Is Not Null or Empty.";
                            result.messageMar = "समाप्ती तारीख वेळ शून्य किंवा रिक्त नाही.";
                        }
                        result.status = "Error";
                        result.dumpId = obj.dyId;
                        result.transId = obj.transId;
                        result.gvstatus = obj.TStatus;
                    }



                    objTransDumpTD.transId = obj.transId;
                    objTransDumpTD.dyId = checkNullDump(obj.dyId);
                    objTransDumpTD.startDateTime = Convert.ToDateTime(obj.startDateTime);
                    objTransDumpTD.endDateTime = Convert.ToDateTime(obj.endDateTime);
                    objTransDumpTD.userId = obj.userId;
                    objTransDumpTD.houseList = hlist;
                    objTransDumpTD.tripNo = obj.tripNo;
                    objTransDumpTD.vehicleNumber = obj.vehicleNumber;
                    objTransDumpTD.totalDryWeight = obj.totalDryWeight;
                    objTransDumpTD.totalWetWeight = obj.totalWetWeight;
                    objTransDumpTD.totalGcWeight = obj.totalGcWeight;
                    objTransDumpTD.bcTransId = obj.bcTransId;
                    objTransDumpTD.TStatus = obj.TStatus;
                    objTransDumpTD.bcTotalGcWeight = obj.bcTotalGcWeight;
                    objTransDumpTD.bcTotalWetWeight = obj.bcTotalWetWeight;
                    objTransDumpTD.bcTotalDryWeight = obj.bcTotalDryWeight;
                    objTransDumpTD.bcStartDateTime = obj.bcStartDateTime;
                    objTransDumpTD.bcEndDateTime = obj.bcEndDateTime;
                    objTransDumpTD.tHr = obj.totalHours;
                    objTransDumpTD.tNh = obj.totalNumberOfHouses;
                    objTransDumpTD.bcThr = obj.bcThr;
                    objTransDumpTD.UsTotalDryWeight = obj.USTotalDryWeight;
                    objTransDumpTD.UsTotalGcWeight = obj.USTotalGcWeight;
                    objTransDumpTD.UsTotalWetWeight = obj.USTotalWetWeight;
                    objTransDumpTD.TotalDryWeightKg = obj.totalDryWeightkg;
                    objTransDumpTD.TotalWetWeightKg = obj.totalWetWeightkg;
                    objTransDumpTD.TotalGcWeightKg = obj.totalGcWeightkg;
                    db.TransDumpTDs.Add(objTransDumpTD);
                    db.SaveChanges();
                    result.message = "Dump Yard Trip Transaction Save Successfully!!";
                    result.messageMar = "डंप यार्ड ट्रिप व्यवहार यशस्वीरित्या जतन करा!!";
                    result.status = "Success";
                    result.dumpId = obj.dyId;
                    result.transId = obj.transId;
                    result.bcTransId = obj.bcTransId;
                    result.gvstatus = obj.TStatus;


                }
                catch (Exception ex)
                {
                    result.message = ex.Message;
                    result.messageMar = ex.Message;
                    result.status = "Error";
                    result.dumpId = obj.dyId;
                    result.transId = obj.transId;
                    result.bcTransId = obj.bcTransId;
                    result.gvstatus = obj.TStatus;
                }
            }
            return result;

        }
        public string checkNullDump(string str)
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
        public string DecryptString(string encrString)
        {
            byte[] b;
            string decrypted;
            try
            {
                b = Convert.FromBase64String(encrString);
                decrypted = System.Text.ASCIIEncoding.ASCII.GetString(b);
            }
            catch (FormatException fe)
            {
                decrypted = "";
            }
            return decrypted;
        }

        public string EnryptString(string strEncrypted)
        {
            byte[] b = System.Text.ASCIIEncoding.ASCII.GetBytes(strEncrypted);
            string encrypted = Convert.ToBase64String(b);
            return encrypted;
        }

        public async Task<Result> SaveUserAttendenceForNormalAsync(SBUserAttendence obj, int AppId, int type, string batteryStatus)
        {

            Result result = new Result();
            try
            {

                using (DevICTSBMChildEntities db = new DevICTSBMChildEntities(AppId))
                {
                    var user = await db.UserMasters.Where(c => c.userId == obj.userId && c.EmployeeType == null).FirstOrDefaultAsync();
                    var Vehicaldetail = await db.Vehical_QR_Masters.Where(c => c.ReferanceId == obj.ReferanceId && c.VehicalNumber != null && c.VehicalType != null).FirstOrDefaultAsync();
                    if (type == 0)
                    {
                        if ((obj.daDate) == null || (string.IsNullOrEmpty(obj.startTime)) == true || (obj.daDate.Year) == 1)
                        {
                            result.status = "error";
                            result.message = "Invalid Date Time";
                            result.messageMar = "तुमची सुरुवात तारीख वेळ रिक्त आहे";
                            result.emptype = obj.EmpType;
                            return result;
                        }

                        try
                        {
                            if (user.isActive == true)
                            {
                                //Daily_Attendance data = db.Daily_Attendance.Where(c => c.daDate == EntityFunctions.TruncateTime(obj.daDate) && c.userId == obj.userId && (c.endTime == null || c.endTime == "")).FirstOrDefault();
                                Daily_Attendance data = await db.Daily_Attendances.Where(c => c.userId == obj.userId && (c.endTime == null || c.endTime == "") && c.EmployeeType == null).FirstOrDefaultAsync();
                                if (data != null)
                                {
                                    data.endTime = obj.startTime;
                                    data.daEndDate = obj.daDate;
                                    data.endLat = obj.startLat;
                                    data.endLong = obj.startLong;
                                    data.batteryStatus = batteryStatus;
                                    data.totalKm = obj.totalKm;
                                    data.EmployeeType = null;
                                    if ((string.IsNullOrEmpty(obj.QrCodeImage)) == false)
                                    {
                                        obj.QrCodeImage = obj.QrCodeImage.Replace("data:image/jpeg;base64,", "");
                                        data.BinaryQrCodeImage = Convert.FromBase64String(obj.QrCodeImage);
                                    }
                                    if (Vehicaldetail != null)
                                    {
                                        data.VQRId = Vehicaldetail.vqrId;
                                        data.vehicleNumber = Vehicaldetail.VehicalNumber;
                                        data.vtId = Vehicaldetail.VehicalType;
                                    }
                                    else
                                    {
                                        data.VQRId = null;
                                        data.vehicleNumber = obj.vehicleNumber;
                                        data.vtId = obj.vtId;
                                    }

                                    await db.SaveChangesAsync();
                                }
                                try
                                {
                                    Daily_Attendance objdata = new Daily_Attendance();

                                    objdata.userId = obj.userId;
                                    objdata.daDate = obj.daDate;
                                    objdata.endLat = "";
                                    objdata.startLat = obj.startLat;
                                    objdata.startLong = obj.startLong;
                                    objdata.startTime = obj.startTime;
                                    objdata.endTime = "";
                                    objdata.vtId = obj.vtId;
                                    obj.vehicleNumber = obj.vehicleNumber;
                                    objdata.daStartNote = obj.daStartNote;
                                    objdata.daEndNote = obj.daEndNote;
                                    objdata.vehicleNumber = obj.vehicleNumber;
                                    //   objdata.startAddress = Address(obj.startLat + "," + obj.startLong); 
                                    objdata.batteryStatus = batteryStatus;
                                    objdata.totalKm = obj.totalKm;
                                    objdata.EmployeeType = null;
                                    if ((string.IsNullOrEmpty(obj.QrCodeImage)) == false)
                                    {
                                        obj.QrCodeImage = obj.QrCodeImage.Replace("data:image/jpeg;base64,", "");
                                        objdata.BinaryQrCodeImage = Convert.FromBase64String(obj.QrCodeImage);
                                    }
                                    if (Vehicaldetail != null)
                                    {
                                        objdata.VQRId = Vehicaldetail.vqrId;
                                        objdata.vehicleNumber = Vehicaldetail.VehicalNumber;
                                        objdata.vtId = Vehicaldetail.VehicalType;
                                    }
                                    else
                                    {
                                        objdata.VQRId = null;
                                        objdata.vehicleNumber = obj.vehicleNumber;
                                        objdata.vtId = obj.vtId;
                                    }
                                    db.Daily_Attendances.Add(objdata);
                                    string Time2 = obj.startTime;
                                    DateTime date2 = DateTime.Parse(Time2, System.Globalization.CultureInfo.CurrentCulture);
                                    string t2 = date2.ToString("hh:mm:ss tt");
                                    string dt2 = Convert.ToDateTime(obj.daDate).ToString("MM/dd/yyyy");
                                    DateTime? sdate = Convert.ToDateTime(dt2 + " " + t2);
                                    Location loc = new Location();
                                    loc.userId = obj.userId;
                                    loc.datetime = sdate;
                                    loc.lat = obj.startLat;
                                    loc._long = obj.startLong;
                                    loc.batteryStatus = batteryStatus;
                                    loc.EmployeeType = null;
                                    loc.address = Address(obj.startLat + "," + obj.startLong);
                                    if (loc.address != "")
                                    { loc.area = area(loc.address); }
                                    else
                                    {
                                        loc.area = "";
                                    }
                                    loc.CreatedDate = DateTime.Now;
                                    db.Locations.Add(loc);
                                    await db.SaveChangesAsync();
                                    result.status = "success";
                                    result.message = "Shift started Successfully";
                                    result.messageMar = "शिफ्ट सुरू";
                                    result.emptype = "N";
                                    return result;
                                }

                                catch (Exception ex)
                                {
                                    _logger.LogError(ex.ToString(), ex);
                                    result.status = "error";
                                    //result.message = "Something is wrong,Try Again.. ";
                                    result.message = ex.Message;
                                    result.messageMar = "काहीतरी चुकीचे आहे, पुन्हा प्रयत्न करा..";
                                    result.emptype = "N";
                                    return result;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            result.status = "error";
                            //result.message = "Something is wrong,Try Again.. ";
                            result.message = ex.Message;
                            result.messageMar = "काहीतरी चुकीचे आहे, पुन्हा प्रयत्न करा..";
                            result.emptype = "N";
                            return result;
                        }

                        result.status = "error";
                        result.message = "Please contact your administrator.. ";
                        result.messageMar = "कृपया आपल्या ऍडमिनिस्ट्रेटरशी संपर्क साधा..";
                        result.emptype = "N";
                        return result;

                    }
                    else
                    {
                        if ((obj.daEndDate) == null || (string.IsNullOrEmpty(obj.endTime)) == true || (obj.daEndDate.Year) == 1)
                        {
                            result.status = "error";
                            result.message = "Invalid Date Time";
                            result.messageMar = "तुमची शेवट तारीख वेळ रिक्त आहे";
                            result.emptype = obj.EmpType;
                            return result;
                        }

                        try
                        {
                            //DateTime.TryParseExact(obj.daDate.ToString("d"), "d", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dtObj);
                            Daily_Attendance objdata = await db.Daily_Attendances.Where(c => EF.Functions.DateDiffDay(c.daDate, obj.daDate) == 0 && c.userId == obj.userId && (c.endTime == "" || c.endTime == null) && c.EmployeeType == null).FirstOrDefaultAsync();
                            if (objdata != null)
                            {

                                objdata.userId = obj.userId;
                                objdata.daEndDate = obj.daDate;
                                objdata.endLat = obj.endLat;
                                objdata.endLong = obj.endLong;
                                objdata.endTime = obj.endTime;
                                objdata.daEndNote = obj.daEndNote;
                                objdata.OutbatteryStatus = batteryStatus;
                                objdata.totalKm = obj.totalKm;
                                objdata.EmployeeType = null;
                                if ((string.IsNullOrEmpty(obj.QrCodeImage)) == false)
                                {
                                    obj.QrCodeImage = obj.QrCodeImage.Replace("data:image/jpeg;base64,", "");
                                    objdata.BinaryQrCodeImage = Convert.FromBase64String(obj.QrCodeImage);
                                }
                                if (Vehicaldetail != null)
                                {
                                    objdata.VQRId = Vehicaldetail.vqrId;
                                    objdata.vehicleNumber = Vehicaldetail.VehicalNumber;
                                    objdata.vtId = Vehicaldetail.VehicalType;
                                }
                                else
                                {
                                    objdata.VQRId = null;
                                    objdata.vehicleNumber = obj.vehicleNumber;
                                    objdata.vtId = obj.vtId;
                                }
                                //       objdata.endAddress = Address(objdata.endLat + "," + objdata.endLong);

                                string Time2 = obj.endTime;
                                DateTime date2 = DateTime.Parse(Time2, System.Globalization.CultureInfo.CurrentCulture);
                                string t2 = date2.ToString("hh:mm:ss tt");
                                string dt2 = Convert.ToDateTime(obj.daDate).ToString("MM/dd/yyyy");
                                DateTime? edate = Convert.ToDateTime(dt2 + " " + t2);
                                Location loc = new Location();
                                loc.userId = obj.userId;
                                loc.datetime = edate;
                                loc.lat = obj.endLat;
                                loc._long = obj.endLong;
                                loc.batteryStatus = batteryStatus;
                                loc.address = Address(obj.endLat + "," + obj.endLong);
                                if (loc.address != "")
                                { loc.area = area(loc.address); }
                                else
                                {
                                    loc.area = "";
                                }
                                loc.CreatedDate = DateTime.Now;
                                loc.EmployeeType = null;
                                db.Locations.Add(loc);
                                await db.SaveChangesAsync();
                                result.status = "success";
                                result.message = "Shift ended successfully";
                                result.messageMar = "शिफ्ट संपले";
                                result.isAttendenceOff = true;
                                result.emptype = "N";
                                return result;
                            }
                            else
                            {
                                Daily_Attendance objdata2 = await db.Daily_Attendances.Where(c => c.userId == obj.userId && (c.endTime == "" || c.endTime == null) && c.EmployeeType == null).OrderByDescending(c => c.daID).FirstOrDefaultAsync();
                                objdata2.userId = obj.userId;
                                objdata2.daEndDate = DateTime.Now;
                                objdata2.endLat = obj.endLat;
                                objdata2.endLong = obj.endLong;
                                objdata2.endTime = obj.endTime;
                                objdata2.daEndNote = obj.daEndNote;
                                objdata2.OutbatteryStatus = batteryStatus;
                                objdata2.EmployeeType = null;
                                if ((string.IsNullOrEmpty(obj.QrCodeImage)) == false)
                                {
                                    obj.QrCodeImage = obj.QrCodeImage.Replace("data:image/jpeg;base64,", "");
                                    objdata2.BinaryQrCodeImage = Convert.FromBase64String(obj.QrCodeImage);
                                }
                                if (Vehicaldetail != null)
                                {
                                    objdata2.VQRId = Vehicaldetail.vqrId;
                                    objdata2.vehicleNumber = Vehicaldetail.VehicalNumber;
                                    objdata2.vtId = Vehicaldetail.VehicalType;
                                }
                                else
                                {
                                    objdata2.VQRId = null;
                                    objdata2.vehicleNumber = obj.vehicleNumber;
                                    objdata2.vtId = obj.vtId;
                                }

                                //       objdata.endAddress = Address(objdata.endLat + "," + objdata.endLong);
                                Location loc = new Location();
                                loc.userId = obj.userId;
                                loc.datetime = DateTime.Now;
                                loc.lat = obj.endLat;
                                loc._long = obj.endLong;
                                loc.address = Address(obj.endLat + "," + obj.endLong);
                                if (loc.address != "")
                                { loc.area = area(loc.address); }
                                else
                                {
                                    loc.area = "";
                                }
                                loc.EmployeeType = null;
                                loc.CreatedDate = DateTime.Now;
                                db.Locations.Add(loc);
                                await db.SaveChangesAsync();
                                result.status = "success";
                                result.message = "Shift ended successfully";
                                result.messageMar = "शिफ्ट संपले";
                                result.isAttendenceOff = true;
                                result.emptype = "N";
                                return result;
                            }
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex.ToString(), ex);
                            result.status = "error";
                            //result.message = "Something is wrong,Try Again.. ";
                            result.message = ex.Message;
                            result.messageMar = "काहीतरी चुकीचे आहे, पुन्हा प्रयत्न करा..";
                            result.emptype = "N";
                            return result;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString(), ex);
                return result;

            }

        }

        public async Task<Result> SaveUserAttendenceForLiquidAsync(SBUserAttendence obj, int AppId, int type, string batteryStatus)
        {
            Result result = new Result();
            try
            {


                using (DevICTSBMChildEntities db = new DevICTSBMChildEntities(AppId))
                {
                    var user = await db.UserMasters.Where(c => c.userId == obj.userId && c.EmployeeType == "L").FirstOrDefaultAsync();

                    if (type == 0)
                    {
                        if (user.isActive == true)
                        {
                            //Daily_Attendance data = db.Daily_Attendance.Where(c => c.daDate == EntityFunctions.TruncateTime(obj.daDate) && c.userId == obj.userId && (c.endTime == null || c.endTime == "")).FirstOrDefault();
                            Daily_Attendance data = await db.Daily_Attendances.Where(c => c.userId == obj.userId && (c.endTime == null || c.endTime == "") && c.EmployeeType == "L").FirstOrDefaultAsync();
                            if (data != null)
                            {
                                data.endTime = obj.startTime;
                                data.daEndDate = obj.daDate;
                                data.endLat = obj.startLat;
                                data.endLong = obj.startLong;
                                data.batteryStatus = batteryStatus;
                                data.totalKm = obj.totalKm;
                                data.EmployeeType = "L";
                                await db.SaveChangesAsync();
                            }
                            try
                            {
                                Daily_Attendance objdata = new Daily_Attendance();

                                objdata.userId = obj.userId;
                                objdata.daDate = obj.daDate;
                                objdata.endLat = "";
                                objdata.startLat = obj.startLat;
                                objdata.startLong = obj.startLong;
                                objdata.startTime = obj.startTime;
                                objdata.endTime = "";
                                objdata.vtId = obj.vtId;
                                obj.vehicleNumber = obj.vehicleNumber;
                                objdata.daStartNote = obj.daStartNote;
                                objdata.daEndNote = obj.daEndNote;
                                objdata.vehicleNumber = obj.vehicleNumber;
                                //   objdata.startAddress = Address(obj.startLat + "," + obj.startLong); 
                                objdata.batteryStatus = batteryStatus;
                                objdata.totalKm = obj.totalKm;
                                objdata.EmployeeType = "L";
                                db.Daily_Attendances.Add(objdata);
                                string Time2 = obj.startTime;
                                DateTime date2 = DateTime.Parse(Time2, System.Globalization.CultureInfo.CurrentCulture);
                                string t2 = date2.ToString("hh:mm:ss tt");
                                string dt2 = Convert.ToDateTime(obj.daDate).ToString("MM/dd/yyyy");
                                DateTime? sdate = Convert.ToDateTime(dt2 + " " + t2);
                                Location loc = new Location();
                                loc.userId = obj.userId;
                                loc.datetime = sdate;
                                loc.lat = obj.startLat;
                                loc._long = obj.startLong;
                                loc.batteryStatus = batteryStatus;
                                loc.EmployeeType = "L";
                                loc.address = Address(obj.startLat + "," + obj.startLong);
                                if (loc.address != "")
                                { loc.area = area(loc.address); }
                                else
                                {
                                    loc.area = "";
                                }
                                loc.CreatedDate = DateTime.Now;
                                db.Locations.Add(loc);
                                await db.SaveChangesAsync();
                                result.status = "success";
                                result.message = "Shift started Successfully";
                                result.messageMar = "शिफ्ट सुरू";
                                result.emptype = "L";
                                return result;
                            }

                            catch (Exception ex)
                            {

                                result.status = "error";
                                result.message = "Something is wrong,Try Again.. ";
                                result.messageMar = "काहीतरी चुकीचे आहे, पुन्हा प्रयत्न करा..";
                                result.emptype = "L";
                                return result;
                            }
                        }

                        result.status = "error";
                        result.message = "Please contact your administrator.. ";
                        result.messageMar = "कृपया आपल्या ऍडमिनिस्ट्रेटरशी संपर्क साधा..";
                        result.emptype = "L";
                        return result;

                    }
                    else
                    {

                        try
                        {
                            //DateTime.TryParseExact(obj.daDate.ToString("d"), "d", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dtObj);
                            Daily_Attendance objdata = await db.Daily_Attendances.Where(c => EF.Functions.DateDiffDay(c.daDate, obj.daDate) == 0 && c.userId == obj.userId && (c.endTime == "" || c.endTime == null) && c.EmployeeType == "L").FirstOrDefaultAsync();
                            if (objdata != null)
                            {

                                objdata.userId = obj.userId;
                                objdata.daEndDate = obj.daDate;
                                objdata.endLat = obj.endLat;
                                objdata.endLong = obj.endLong;
                                objdata.endTime = obj.endTime;
                                objdata.daEndNote = obj.daEndNote;
                                objdata.OutbatteryStatus = batteryStatus;
                                objdata.totalKm = obj.totalKm;
                                objdata.EmployeeType = "L";
                                //       objdata.endAddress = Address(objdata.endLat + "," + objdata.endLong);

                                string Time2 = obj.endTime;
                                DateTime date2 = DateTime.Parse(Time2, System.Globalization.CultureInfo.CurrentCulture);
                                string t2 = date2.ToString("hh:mm:ss tt");
                                string dt2 = Convert.ToDateTime(obj.daDate).ToString("MM/dd/yyyy");
                                DateTime? edate = Convert.ToDateTime(dt2 + " " + t2);
                                Location loc = new Location();
                                loc.userId = obj.userId;
                                loc.datetime = edate;
                                loc.lat = obj.endLat;
                                loc._long = obj.endLong;
                                loc.batteryStatus = batteryStatus;
                                loc.address = Address(obj.endLat + "," + obj.endLong);
                                if (loc.address != "")
                                { loc.area = area(loc.address); }
                                else
                                {
                                    loc.area = "";
                                }
                                loc.CreatedDate = DateTime.Now;
                                loc.EmployeeType = "L";
                                db.Locations.Add(loc);
                                await db.SaveChangesAsync();
                                result.status = "success";
                                result.message = "Shift ended successfully";
                                result.messageMar = "शिफ्ट संपले";
                                result.isAttendenceOff = true;
                                result.emptype = "L";
                                return result;
                            }
                            else
                            {
                                Daily_Attendance objdata2 = await db.Daily_Attendances.Where(c => c.userId == obj.userId && (c.endTime == "" || c.endTime == null) && c.EmployeeType == "L").OrderByDescending(c => c.daID).FirstOrDefaultAsync();
                                objdata2.userId = obj.userId;
                                objdata2.daEndDate = DateTime.Now;
                                objdata2.endLat = obj.endLat;
                                objdata2.endLong = obj.endLong;
                                objdata2.endTime = obj.endTime;
                                objdata2.daEndNote = obj.daEndNote;
                                objdata2.OutbatteryStatus = batteryStatus;
                                objdata2.EmployeeType = "L";
                                //       objdata.endAddress = Address(objdata.endLat + "," + objdata.endLong);
                                Location loc = new Location();
                                loc.userId = obj.userId;
                                loc.datetime = DateTime.Now;
                                loc.lat = obj.endLat;
                                loc._long = obj.endLong;
                                loc.address = Address(obj.endLat + "," + obj.endLong);
                                if (loc.address != "")
                                { loc.area = area(loc.address); }
                                else
                                {
                                    loc.area = "";
                                }
                                loc.EmployeeType = "L";
                                loc.CreatedDate = DateTime.Now;
                                db.Locations.Add(loc);
                                await db.SaveChangesAsync();
                                result.status = "success";
                                result.message = "Shift ended successfully";
                                result.messageMar = "शिफ्ट संपले";
                                result.isAttendenceOff = true;
                                result.emptype = "L";
                                return result;
                            }
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex.ToString(), ex);
                            result.status = "error";
                            result.message = "Something is wrong,Try Again.. ";
                            result.messageMar = "काहीतरी चुकीचे आहे, पुन्हा प्रयत्न करा..";
                            result.emptype = "L";
                            return result;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString(), ex);
                return result;

            }
        }
        public async Task<Result> SaveUserAttendenceForStreetAsync(SBUserAttendence obj, int AppId, int type, string batteryStatus)
        {
            Result result = new Result();
            try
            {
                using (DevICTSBMChildEntities db = new DevICTSBMChildEntities(AppId))
                {
                    var user = await db.UserMasters.Where(c => c.userId == obj.userId && c.EmployeeType == "S").FirstOrDefaultAsync();

                    if (type == 0)
                    {
                        if (user.isActive == true)
                        {
                            //Daily_Attendance data = db.Daily_Attendance.Where(c => c.daDate == EntityFunctions.TruncateTime(obj.daDate) && c.userId == obj.userId && (c.endTime == null || c.endTime == "")).FirstOrDefault();
                            Daily_Attendance data = await db.Daily_Attendances.Where(c => c.userId == obj.userId && (c.endTime == null || c.endTime == "") && c.EmployeeType == "S").FirstOrDefaultAsync();
                            if (data != null)
                            {
                                data.endTime = obj.startTime;
                                data.daEndDate = obj.daDate;
                                data.endLat = obj.startLat;
                                data.endLong = obj.startLong;
                                data.batteryStatus = batteryStatus;
                                data.totalKm = obj.totalKm;
                                data.EmployeeType = "S";
                                await db.SaveChangesAsync();
                            }
                            try
                            {
                                Daily_Attendance objdata = new Daily_Attendance();

                                objdata.userId = obj.userId;
                                objdata.daDate = obj.daDate;
                                objdata.endLat = "";
                                objdata.startLat = obj.startLat;
                                objdata.startLong = obj.startLong;
                                objdata.startTime = obj.startTime;
                                objdata.endTime = "";
                                objdata.vtId = obj.vtId;
                                obj.vehicleNumber = obj.vehicleNumber;
                                objdata.daStartNote = obj.daStartNote;
                                objdata.daEndNote = obj.daEndNote;
                                objdata.vehicleNumber = obj.vehicleNumber;
                                //   objdata.startAddress = Address(obj.startLat + "," + obj.startLong); 
                                objdata.batteryStatus = batteryStatus;
                                objdata.totalKm = obj.totalKm;
                                objdata.EmployeeType = "S";
                                db.Daily_Attendances.Add(objdata);
                                string Time2 = obj.startTime;
                                DateTime date2 = DateTime.Parse(Time2, System.Globalization.CultureInfo.CurrentCulture);
                                string t2 = date2.ToString("hh:mm:ss tt");
                                string dt2 = Convert.ToDateTime(obj.daDate).ToString("MM/dd/yyyy");
                                DateTime? sdate = Convert.ToDateTime(dt2 + " " + t2);
                                Location loc = new Location();
                                loc.userId = obj.userId;
                                loc.datetime = sdate;
                                loc.lat = obj.startLat;
                                loc._long = obj.startLong;
                                loc.batteryStatus = batteryStatus;
                                loc.EmployeeType = "S";
                                loc.address = Address(obj.startLat + "," + obj.startLong);
                                if (loc.address != "")
                                { loc.area = area(loc.address); }
                                else
                                {
                                    loc.area = "";
                                }
                                loc.CreatedDate = DateTime.Now;
                                db.Locations.Add(loc);
                                await db.SaveChangesAsync();
                                result.status = "success";
                                result.message = "Shift started Successfully";
                                result.messageMar = "शिफ्ट सुरू";
                                result.emptype = "S";
                                return result;
                            }

                            catch (Exception ex)
                            {
                                _logger.LogError(ex.ToString(), ex);
                                result.status = "error";
                                result.message = "Something is wrong,Try Again.. ";
                                result.messageMar = "काहीतरी चुकीचे आहे, पुन्हा प्रयत्न करा..";
                                result.emptype = "S";
                                return result;
                            }
                        }

                        result.status = "error";
                        result.message = "Please contact your administrator.. ";
                        result.messageMar = "कृपया आपल्या ऍडमिनिस्ट्रेटरशी संपर्क साधा..";
                        result.emptype = "S";
                        return result;

                    }
                    else
                    {

                        try
                        {
                            //DateTime.TryParseExact(obj.daDate.ToString("d"), "d", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dtObj);
                            Daily_Attendance objdata = await db.Daily_Attendances.Where(c => EF.Functions.DateDiffDay(c.daDate, obj.daDate) == 0 && c.userId == obj.userId && (c.endTime == "" || c.endTime == null) && c.EmployeeType == "S").FirstOrDefaultAsync();
                            if (objdata != null)
                            {

                                objdata.userId = obj.userId;
                                objdata.daEndDate = obj.daDate;
                                objdata.endLat = obj.endLat;
                                objdata.endLong = obj.endLong;
                                objdata.endTime = obj.endTime;
                                objdata.daEndNote = obj.daEndNote;
                                objdata.OutbatteryStatus = batteryStatus;
                                objdata.totalKm = obj.totalKm;
                                objdata.EmployeeType = "S";
                                //       objdata.endAddress = Address(objdata.endLat + "," + objdata.endLong);

                                string Time2 = obj.endTime;
                                DateTime date2 = DateTime.Parse(Time2, System.Globalization.CultureInfo.CurrentCulture);
                                string t2 = date2.ToString("hh:mm:ss tt");
                                string dt2 = Convert.ToDateTime(obj.daDate).ToString("MM/dd/yyyy");
                                DateTime? edate = Convert.ToDateTime(dt2 + " " + t2);
                                Location loc = new Location();
                                loc.userId = obj.userId;
                                loc.datetime = edate;
                                loc.lat = obj.endLat;
                                loc._long = obj.endLong;
                                loc.batteryStatus = batteryStatus;
                                loc.address = Address(obj.endLat + "," + obj.endLong);
                                if (loc.address != "")
                                { loc.area = area(loc.address); }
                                else
                                {
                                    loc.area = "";
                                }
                                loc.CreatedDate = DateTime.Now;
                                loc.EmployeeType = "L";
                                db.Locations.Add(loc);
                                await db.SaveChangesAsync();
                                result.status = "success";
                                result.message = "Shift ended successfully";
                                result.messageMar = "शिफ्ट संपले";
                                result.isAttendenceOff = true;
                                result.emptype = "S";
                                return result;
                            }
                            else
                            {
                                Daily_Attendance objdata2 = await db.Daily_Attendances.Where(c => c.userId == obj.userId && (c.endTime == "" || c.endTime == null) && c.EmployeeType == "S").OrderByDescending(c => c.daID).FirstOrDefaultAsync();
                                objdata2.userId = obj.userId;
                                objdata2.daEndDate = DateTime.Now;
                                objdata2.endLat = obj.endLat;
                                objdata2.endLong = obj.endLong;
                                objdata2.endTime = obj.endTime;
                                objdata2.daEndNote = obj.daEndNote;
                                objdata2.OutbatteryStatus = batteryStatus;
                                objdata2.EmployeeType = "S";
                                //       objdata.endAddress = Address(objdata.endLat + "," + objdata.endLong);
                                Location loc = new Location();
                                loc.userId = obj.userId;
                                loc.datetime = DateTime.Now;
                                loc.lat = obj.endLat;
                                loc._long = obj.endLong;
                                loc.address = Address(obj.endLat + "," + obj.endLong);
                                if (loc.address != "")
                                { loc.area = area(loc.address); }
                                else
                                {
                                    loc.area = "";
                                }
                                loc.EmployeeType = "S";
                                loc.CreatedDate = DateTime.Now;
                                db.Locations.Add(loc);
                                await db.SaveChangesAsync();
                                result.status = "success";
                                result.message = "Shift ended successfully";
                                result.messageMar = "शिफ्ट संपले";
                                result.isAttendenceOff = true;
                                result.emptype = "S";
                                return result;
                            }
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex.ToString(), ex);
                            result.status = "error";
                            result.message = "Something is wrong,Try Again.. ";
                            result.messageMar = "काहीतरी चुकीचे आहे, पुन्हा प्रयत्न करा..";
                            result.emptype = "S";
                            return result;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString(), ex);
                return result;
            }
        }
        public async Task<Result> SaveUserAttendenceForDumpAsync(SBUserAttendence obj, int AppId, int type, string batteryStatus)
        {
            Result result = new Result();
            try
            {
                using (DevICTSBMChildEntities db = new DevICTSBMChildEntities(AppId))
                {
                    var user = await db.UserMasters.Where(c => c.userId == obj.userId && c.EmployeeType == "D").FirstOrDefaultAsync();
                    var dy = await db.DumpYardDetails.Where(x => x.ReferanceId == obj.ReferanceId).FirstOrDefaultAsync();
                    if (dy != null)
                    {
                        if (type == 0)
                        {
                            if (user.isActive == true)
                            {
                                //Daily_Attendance data = db.Daily_Attendance.Where(c => c.daDate == EntityFunctions.TruncateTime(obj.daDate) && c.userId == obj.userId && (c.endTime == null || c.endTime == "")).FirstOrDefault();
                                Daily_Attendance data = await db.Daily_Attendances.Where(c => c.userId == obj.userId && (c.endTime == null || c.endTime == "") && c.EmployeeType == "D").FirstOrDefaultAsync();
                                if (data != null)
                                {
                                    data.endTime = obj.startTime;
                                    data.daEndDate = obj.daDate;
                                    data.endLat = obj.startLat;
                                    data.endLong = obj.startLong;
                                    data.batteryStatus = batteryStatus;
                                    data.totalKm = obj.totalKm;
                                    data.EmployeeType = "D";
                                    data.vtId = "1";
                                    data.vehicleNumber = "1";
                                    if (dy != null)
                                    {
                                        data.dyid = dy.dyId;
                                    }
                                    await db.SaveChangesAsync();
                                }
                                try
                                {
                                    Daily_Attendance objdata = new Daily_Attendance();

                                    objdata.userId = obj.userId;
                                    objdata.daDate = obj.daDate;
                                    objdata.endLat = "";
                                    objdata.startLat = obj.startLat;
                                    objdata.startLong = obj.startLong;
                                    objdata.startTime = obj.startTime;
                                    objdata.endTime = "";
                                    objdata.vtId = obj.vtId;
                                    obj.vehicleNumber = obj.vehicleNumber;
                                    objdata.daStartNote = obj.daStartNote;
                                    objdata.daEndNote = obj.daEndNote;
                                    objdata.vehicleNumber = obj.vehicleNumber;
                                    //   objdata.startAddress = Address(obj.startLat + "," + obj.startLong); 
                                    objdata.batteryStatus = batteryStatus;
                                    objdata.totalKm = obj.totalKm;
                                    objdata.EmployeeType = "D";
                                    if (dy != null)
                                    {
                                        objdata.dyid = dy.dyId;
                                    }
                                    objdata.vtId = "1";
                                    objdata.vehicleNumber = "1";
                                    db.Daily_Attendances.Add(objdata);
                                    string Time2 = obj.startTime;
                                    DateTime date2 = DateTime.Parse(Time2, System.Globalization.CultureInfo.CurrentCulture);
                                    string t2 = date2.ToString("hh:mm:ss tt");
                                    string dt2 = Convert.ToDateTime(obj.daDate).ToString("MM/dd/yyyy");
                                    DateTime? sdate = Convert.ToDateTime(dt2 + " " + t2);
                                    Location loc = new Location();
                                    loc.userId = obj.userId;
                                    loc.datetime = sdate;
                                    loc.lat = obj.startLat;
                                    loc._long = obj.startLong;
                                    loc.batteryStatus = batteryStatus;
                                    loc.EmployeeType = "D";
                                    loc.address = Address(obj.startLat + "," + obj.startLong);
                                    if (loc.address != "")
                                    { loc.area = area(loc.address); }
                                    else
                                    {
                                        loc.area = "";
                                    }
                                    loc.CreatedDate = DateTime.Now;
                                    db.Locations.Add(loc);
                                    await db.SaveChangesAsync();
                                    result.status = "success";
                                    result.message = "Shift started Successfully";
                                    result.messageMar = "शिफ्ट सुरू";
                                    result.emptype = "D";
                                    return result;
                                }

                                catch (Exception ex)
                                {
                                    _logger.LogError(ex.ToString(), ex);
                                    result.status = "error";
                                    result.message = "Something is wrong,Try Again.. ";
                                    result.messageMar = "काहीतरी चुकीचे आहे, पुन्हा प्रयत्न करा..";
                                    result.emptype = "D";
                                    return result;
                                }
                            }

                            result.status = "error";
                            result.message = "Please contact your administrator.. ";
                            result.messageMar = "कृपया आपल्या ऍडमिनिस्ट्रेटरशी संपर्क साधा..";
                            result.emptype = "D";
                            return result;

                        }
                        else
                        {

                            try
                            {
                                //DateTime.TryParseExact(obj.daDate.ToString("d"), "d", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dtObj);
                                Daily_Attendance objdata = await db.Daily_Attendances.Where(c => EF.Functions.DateDiffDay(c.daDate, obj.daDate) == 0 && c.userId == obj.userId && (c.endTime == "" || c.endTime == null) && c.EmployeeType == "D").FirstOrDefaultAsync();
                                if (objdata != null)
                                {

                                    objdata.userId = obj.userId;
                                    objdata.daEndDate = obj.daDate;
                                    objdata.endLat = obj.endLat;
                                    objdata.endLong = obj.endLong;
                                    objdata.endTime = obj.endTime;
                                    objdata.daEndNote = obj.daEndNote;
                                    objdata.batteryStatus = batteryStatus;
                                    objdata.totalKm = obj.totalKm;
                                    objdata.EmployeeType = "D";
                                    objdata.vtId = "1";
                                    objdata.vehicleNumber = "1";
                                    if (dy != null)
                                    {
                                        objdata.dyid = dy.dyId;
                                    }
                                    //       objdata.endAddress = Address(objdata.endLat + "," + objdata.endLong);

                                    string Time2 = obj.endTime;
                                    DateTime date2 = DateTime.Parse(Time2, System.Globalization.CultureInfo.CurrentCulture);
                                    string t2 = date2.ToString("hh:mm:ss tt");
                                    string dt2 = Convert.ToDateTime(obj.daDate).ToString("MM/dd/yyyy");
                                    DateTime? edate = Convert.ToDateTime(dt2 + " " + t2);
                                    Location loc = new Location();
                                    loc.userId = obj.userId;
                                    loc.datetime = edate;
                                    loc.lat = obj.endLat;
                                    loc._long = obj.endLong;
                                    loc.batteryStatus = batteryStatus;
                                    loc.address = Address(obj.endLat + "," + obj.endLong);
                                    if (loc.address != "")
                                    { loc.area = area(loc.address); }
                                    else
                                    {
                                        loc.area = "";
                                    }
                                    loc.CreatedDate = DateTime.Now;
                                    loc.EmployeeType = "D";
                                    db.Locations.Add(loc);
                                    await db.SaveChangesAsync();
                                    result.status = "success";
                                    result.message = "Shift ended successfully";
                                    result.messageMar = "शिफ्ट संपले";
                                    result.isAttendenceOff = true;
                                    result.emptype = "D";
                                    return result;
                                }
                                else
                                {
                                    Daily_Attendance objdata2 = await db.Daily_Attendances.Where(c => c.userId == obj.userId && (c.endTime == "" || c.endTime == null) && c.EmployeeType == "D").OrderByDescending(c => c.daID).FirstOrDefaultAsync();
                                    objdata2.userId = obj.userId;
                                    objdata2.daEndDate = DateTime.Now;
                                    objdata2.endLat = obj.endLat;
                                    objdata2.endLong = obj.endLong;
                                    objdata2.endTime = obj.endTime;
                                    objdata2.daEndNote = obj.daEndNote;
                                    objdata2.batteryStatus = batteryStatus;
                                    objdata2.EmployeeType = "D";
                                    objdata2.vtId = "1";
                                    objdata2.vehicleNumber = "1";
                                    if (dy != null)
                                    {
                                        objdata2.dyid = dy.dyId;
                                    }
                                    //       objdata.endAddress = Address(objdata.endLat + "," + objdata.endLong);
                                    Location loc = new Location();
                                    loc.userId = obj.userId;
                                    loc.datetime = DateTime.Now;
                                    loc.lat = obj.endLat;
                                    loc._long = obj.endLong;
                                    loc.address = Address(obj.endLat + "," + obj.endLong);
                                    if (loc.address != "")
                                    { loc.area = area(loc.address); }
                                    else
                                    {
                                        loc.area = "";
                                    }
                                    loc.EmployeeType = "D";
                                    loc.CreatedDate = DateTime.Now;
                                    db.Locations.Add(loc);
                                    await db.SaveChangesAsync();
                                    result.status = "success";
                                    result.message = "Shift ended successfully";
                                    result.messageMar = "शिफ्ट संपले";
                                    result.isAttendenceOff = true;
                                    result.emptype = "D";
                                    return result;
                                }
                            }
                            catch (Exception ex)
                            {
                                _logger.LogError(ex.ToString(), ex);
                                result.status = "error";
                                result.message = "Something is wrong,Try Again.. ";
                                result.messageMar = "काहीतरी चुकीचे आहे, पुन्हा प्रयत्न करा..";
                                result.emptype = "D";
                                return result;
                            }
                        }
                    }

                    else
                    {
                        result.status = "error";
                        result.message = "Inavlid Dumpyard Id.. ";
                        result.messageMar = "अवैध डम्पयार्ड आयडी..";
                        result.emptype = "D";
                        return result;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString(), ex);
                return result;
            }

        }
        public async Task<List<SyncResult>> SaveUserLocationAsync(List<SBUserLocation> obj, int AppId, string batteryStatus, int typeId, string EmpType)
        {
            List<SyncResult> result = new List<SyncResult>();
            if (EmpType == "N" || EmpType == "S" || EmpType == "L")
            {
                result = await SaveUserLocationNSLAsync(obj, AppId, batteryStatus, typeId, EmpType);
            }

            if (EmpType == "SA")
            {
                result = await SaveUserLocationSAAsync(obj, AppId, batteryStatus, typeId, EmpType);
            }

            return result;
        }
        public async Task<List<SyncResult>> SaveUserLocationNSLAsync(List<SBUserLocation> obj, int AppId, string batteryStatus, int typeId, string EmpType)
        {

            using (DevICTSBMChildEntities db = new DevICTSBMChildEntities(AppId))
            using (DevICTSBMMainEntities dbMain = new DevICTSBMMainEntities())
            {
                try
                {
                    List<SyncResult> result = new List<SyncResult>();
                    var distCount = "";

                    if (typeId == 0)
                    {
                        foreach (var x in obj)
                        {
                            DateTime Dateeee = Convert.ToDateTime(x.datetime);
                            DateTime newTime = Dateeee;
                            DateTime oldTime;
                            TimeSpan span = TimeSpan.Zero;

                            //var gcd = db.Locations.Where(c => c.userId == x.userId && c.type == null && EntityFunctions.TruncateTime(c.datetime) == EntityFunctions.TruncateTime(Dateeee)).OrderByDescending(c => c.locId).FirstOrDefault();

                            var gcd = await db.Locations.Where(c => c.userId == x.userId && c.type == null && EF.Functions.DateDiffDay(c.datetime, Dateeee) == 0).OrderByDescending(c => c.locId).FirstOrDefaultAsync();
                            if (gcd != null)
                            {
                                oldTime = gcd.datetime.Value;
                                span = newTime.Subtract(oldTime);
                            }

                            if (gcd == null || span.TotalMinutes >= 9)
                            {
                                //    var IsSameRecord_Location = db.Locations.Where(c => c.userId == x.userId && c.datetime == x.datetime).FirstOrDefault();

                                //if (IsSameRecord_Location == null)
                                //{
                                var u = await db.UserMasters.Where(c => c.userId == x.userId).FirstOrDefaultAsync();

                                DateTime Offlinedate = Convert.ToDateTime(x.datetime);
                                //var atten = db.Daily_Attendance.Where(c => c.userId == x.userId & c.endTime == "" & c.daDate == EntityFunctions.TruncateTime(x.OfflineId == 0 ? DateTime.Now : Offlinedate)).FirstOrDefault();
                                //DateTime.TryParseExact(Offlinedate.ToString("d"), "d", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dtObj);

                                var atten = await db.Daily_Attendances.Where(c => c.userId == x.userId & EF.Functions.DateDiffDay(c.daDate, Offlinedate) == 0).FirstOrDefaultAsync();

                                if (atten == null)
                                {
                                    result.Add(new SyncResult()
                                    {
                                        ID = Convert.ToInt32(x.OfflineId),
                                        isAttendenceOff = true,
                                        status = "error",
                                        message = "Your duty is currently off, please start again.. ",
                                        messageMar = "आपली ड्यूटी सध्या बंद आहे, कृपया पुन्हा सुरू करा..",
                                    });

                                    //return result;
                                    continue;
                                }

                                if (u != null & x.userId > 0)
                                {
                                    string addr = "", ar = "";
                                    addr = Address(x.lat + "," + x.@long);
                                    if (addr != "")
                                    {
                                        ar = area(addr);
                                    }


                                    //Location objdata = new Location();
                                    //objdata.userId = obj.userId;
                                    //objdata.gcDate = DateTime.Now;
                                    //objdata.Lat = obj.Lat;
                                    //    Location loc = new Location();
                                    List<SqlParameter> parms = new List<SqlParameter>
                                                                    {
                                                                        // Create parameter(s)    
                                                                        new SqlParameter { ParameterName = "@userid", Value = x.userId },
                                                                        new SqlParameter { ParameterName = "@typeId", Value = 0 }
                                                                    };
                                    //var locc = db.SP_UserLatLongDetail(x.userId, 0).FirstOrDefault();
                                    var Listlocc = await db.SP_UserLatLongDetail_Results.FromSqlRaw<SP_UserLatLongDetail_Result>("EXEC SP_UserLatLongDetail @userid, @typeId", parms.ToArray()).ToListAsync();
                                    var locc = Listlocc == null ? null : Listlocc.FirstOrDefault();
                                    //var locc = await db.Database.ex("SP_UserLatLongDetail @userid,@typeId", parms);
                                    if (locc == null || locc.lat == "" || locc.@long == "")
                                    {
                                        //string a = objdata.Lat;
                                        //string b = objdata.Long;

                                        string a = x.lat;
                                        string b = x.@long;

                                        //var dist = db.SP_DistanceCount(Convert.ToDouble(a), Convert.ToDouble(b), Convert.ToDouble(x.lat), Convert.ToDouble(x.@long)).FirstOrDefault();

                                        List<SqlParameter> parms1 = new List<SqlParameter>
                                                                    {
                                                                        // Create parameter(s)    
                                                                        new SqlParameter { ParameterName = "@sLat", Value = Convert.ToDouble(a) },
                                                                        new SqlParameter { ParameterName = "@sLong", Value = Convert.ToDouble(b) },
                                                                        new SqlParameter { ParameterName = "@dLat", Value = Convert.ToDouble(x.lat) },
                                                                        new SqlParameter { ParameterName = "@dLong", Value = Convert.ToDouble(x.@long) }
                                                                    };
                                        var Listdist = await db.SP_DistanceCount_Results.FromSqlRaw<SP_DistanceCount_Result>("EXEC SP_DistanceCount @sLat,@sLong,@dLat,@dLong", parms1.ToArray()).ToListAsync();
                                        var dist = Listdist == null ? null : Listdist.FirstOrDefault();
                                        distCount = dist.Distance_in_KM.ToString();
                                    }
                                    else
                                    {

                                        // var dist = db.SP_DistanceCount(Convert.ToDouble(locc.lat), Convert.ToDouble(locc.@long), Convert.ToDouble(x.lat), Convert.ToDouble(x.@long)).FirstOrDefault();
                                        List<SqlParameter> parms1 = new List<SqlParameter>
                                                                    {
                                                                        // Create parameter(s)    
                                                                        new SqlParameter { ParameterName = "@sLat", Value = Convert.ToDouble(locc.lat) },
                                                                        new SqlParameter { ParameterName = "@sLong", Value = Convert.ToDouble(locc.@long) },
                                                                        new SqlParameter { ParameterName = "@dLat", Value = Convert.ToDouble(x.lat) },
                                                                        new SqlParameter { ParameterName = "@dLong", Value = Convert.ToDouble(x.@long) }
                                                                    };
                                        var Listdist = await db.SP_DistanceCount_Results.FromSqlRaw<SP_DistanceCount_Result>("EXEC SP_DistanceCount @sLat,@sLong,@dLat,@dLong", parms1.ToArray()).ToListAsync();
                                        var dist = Listdist == null ? null : Listdist.FirstOrDefault();
                                        distCount = dist.Distance_in_KM.ToString();
                                    }

                                    if (EmpType == "N")
                                    {
                                        EmpType = null;
                                    }

                                    db.Locations.Add(new Location()
                                    {
                                        userId = x.userId,
                                        lat = x.lat,
                                        _long = x.@long,
                                        datetime = x.datetime,
                                        address = addr,
                                        area = ar,
                                        batteryStatus = batteryStatus,
                                        Distnace = Convert.ToDecimal(distCount),
                                        CreatedDate = DateTime.Now,
                                        EmployeeType = EmpType,
                                    });
                                    await db.SaveChangesAsync();
                                }
                            }

                            result.Add(new SyncResult()
                            {
                                ID = Convert.ToInt32(x.OfflineId),
                                status = "success",
                                message = "Uploaded successfully",
                                messageMar = "सबमिट यशस्वी",
                            });


                        }

                    }
                    else if (typeId == 1)
                    {
                        foreach (var x in obj)
                        {

                            DateTime newTime = x.datetime;
                            DateTime oldTime;
                            TimeSpan span = TimeSpan.Zero;
                            var IsSameRecordQr_Location = await db.Qr_Locations.Where(c => c.empId == x.userId && c.type == null && EF.Functions.DateDiffDay(c.datetime, x.datetime) == 0).OrderByDescending(c => c.locId).FirstOrDefaultAsync();
                            if (IsSameRecordQr_Location != null)
                            {
                                oldTime = IsSameRecordQr_Location.datetime.Value;
                                span = newTime.Subtract(oldTime);
                            }

                            if (IsSameRecordQr_Location == null || span.Minutes >= 9)

                            {
                                var u = await db.QrEmployeeMasters.Where(c => c.qrEmpId == x.userId).FirstOrDefaultAsync();

                                DateTime Offlinedate = Convert.ToDateTime(x.datetime);
                                var atten = await db.Qr_Employee_Daily_Attendances.Where(c => c.qrEmpId == x.userId & c.endTime == "" & EF.Functions.DateDiffDay(c.startDate, x.OfflineId == 0 ? DateTime.Now : Offlinedate) == 0).FirstOrDefaultAsync();
                                if (atten == null)
                                {
                                    result.Add(new SyncResult()
                                    {
                                        ID = Convert.ToInt32(x.OfflineId),
                                        isAttendenceOff = true,
                                        status = "error",
                                        message = "Your duty is currently off, please start again.. ",
                                        messageMar = "आपली ड्यूटी सध्या बंद आहे, कृपया पुन्हा सुरू करा..",
                                    });

                                    //return result;
                                    continue;
                                }

                                if (u != null & x.userId > 0)
                                {
                                    string addr = "", ar = "";
                                    addr = Address(x.lat + "," + x.@long);
                                    if (addr != "")
                                    {
                                        ar = area(addr);
                                    }

                                    //Location objdata = new Location();
                                    //objdata.userId = obj.userId;
                                    //objdata.gcDate = DateTime.Now;
                                    //objdata.Lat = obj.Lat;
                                    //    Location loc = new Location();


                                    //var locc = db.SP_UserLatLongDetail(x.userId, typeId).FirstOrDefault();
                                    List<SqlParameter> parms = new List<SqlParameter>
                                                                    {
                                                                        // Create parameter(s)    
                                                                        new SqlParameter { ParameterName = "@userid", Value = x.userId },
                                                                        new SqlParameter { ParameterName = "@typeId", Value = typeId }
                                                                    };
                                    var Listlocc = await db.SP_UserLatLongDetail_Results.FromSqlRaw<SP_UserLatLongDetail_Result>("EXEC SP_UserLatLongDetail @userid,@typeId", parms.ToArray()).ToListAsync();
                                    var locc = Listlocc == null ? null : Listlocc.FirstOrDefault();
                                    if (locc == null || locc.lat == "" || locc.@long == "")
                                    {
                                        //string a = objdata.Lat;
                                        //string b = objdata.Long;

                                        string a = x.lat;
                                        string b = x.@long;

                                        //var dist = db.SP_DistanceCount(Convert.ToDouble(a), Convert.ToDouble(b), Convert.ToDouble(x.lat), Convert.ToDouble(x.@long)).FirstOrDefault();
                                        List<SqlParameter> parms1 = new List<SqlParameter>
                                                                    {
                                                                        // Create parameter(s)    
                                                                        new SqlParameter { ParameterName = "@sLat", Value = Convert.ToDouble(a) },
                                                                        new SqlParameter { ParameterName = "@sLong", Value = Convert.ToDouble(b) },
                                                                        new SqlParameter { ParameterName = "@dLat", Value = Convert.ToDouble(x.lat) },
                                                                        new SqlParameter { ParameterName = "@dLong", Value = Convert.ToDouble(x.@long) }
                                                                    };
                                        var Listdist = await db.SP_DistanceCount_Results.FromSqlRaw<SP_DistanceCount_Result>("EXEC SP_DistanceCount @sLat,@sLong,@dLat,@dLong", parms1.ToArray()).ToListAsync();
                                        var dist = Listdist == null ? null : Listdist.FirstOrDefault();
                                        distCount = dist.Distance_in_KM.ToString();
                                    }
                                    else
                                    {

                                        //var dist = db.SP_DistanceCount(Convert.ToDouble(locc.lat), Convert.ToDouble(locc.@long), Convert.ToDouble(x.lat), Convert.ToDouble(x.@long)).FirstOrDefault();
                                        List<SqlParameter> parms1 = new List<SqlParameter>
                                                                    {
                                                                        // Create parameter(s)    
                                                                        new SqlParameter { ParameterName = "@sLat", Value = Convert.ToDouble(locc.lat) },
                                                                        new SqlParameter { ParameterName = "@sLong", Value = Convert.ToDouble(locc.@long) },
                                                                        new SqlParameter { ParameterName = "@dLat", Value = Convert.ToDouble(x.lat) },
                                                                        new SqlParameter { ParameterName = "@dLong", Value = Convert.ToDouble(x.@long) }
                                                                    };
                                        var Listdist = await db.SP_DistanceCount_Results.FromSqlRaw<SP_DistanceCount_Result>("EXEC SP_DistanceCount @sLat,@sLong,@dLat,@dLong", parms1.ToArray()).ToListAsync();
                                        var dist = Listdist == null ? null : Listdist.FirstOrDefault();
                                        distCount = dist.Distance_in_KM.ToString();
                                    }


                                    db.Qr_Locations.Add(new Qr_Location()
                                    {
                                        empId = x.userId,
                                        lat = x.lat,
                                        _long = x.@long,
                                        datetime = x.datetime,
                                        address = addr,
                                        area = ar,
                                        batteryStatus = batteryStatus,
                                        Distnace = Convert.ToDecimal(distCount),

                                    });
                                    await db.SaveChangesAsync();

                                    DateTime newTimeh = DateTime.Now;
                                    DateTime oldTimeh;
                                    TimeSpan spanh = TimeSpan.Zero;
                                    var hd = await db.HouseMasters.Where(c => c.houseLat != null && c.houseLong != null && EF.Functions.DateDiffDay(c.modified, newTimeh) == 0).OrderByDescending(c => c.houseId).FirstOrDefaultAsync();
                                    if (hd != null)
                                    {
                                        oldTimeh = hd.modified.Value;
                                        spanh = newTimeh.Subtract(oldTimeh);
                                    }

                                    if (spanh.Minutes >= 9)
                                    {
                                        var app = dbMain.AppDetails.Where(c => c.AppId == AppId).FirstOrDefault();
                                        app.FAQ = "2";
                                    }


                                    DateTime newTimed = DateTime.Now;
                                    DateTime oldTimed;
                                    TimeSpan spand = TimeSpan.Zero;
                                    var dy = await db.DumpYardDetails.Where(c => c.dyLat != null && c.dyLong != null && EF.Functions.DateDiffDay(c.lastModifiedDate, newTimed) == 0).OrderByDescending(c => c.dyId).FirstOrDefaultAsync();
                                    if (dy != null)
                                    {
                                        oldTimed = dy.lastModifiedDate.Value;
                                        spand = newTimed.Subtract(oldTimed);
                                    }

                                    if (spand.Minutes >= 9)
                                    {
                                        var app = await dbMain.AppDetails.Where(c => c.AppId == AppId).FirstOrDefaultAsync();
                                        app.FAQ = "2";
                                    }


                                    DateTime newTimes = DateTime.Now;
                                    DateTime oldTimes;
                                    TimeSpan spans = TimeSpan.Zero;
                                    var st = await db.StreetSweepingDetails.Where(c => c.SSLat != null && c.SSLong != null && EF.Functions.DateDiffDay(c.lastModifiedDate, newTimes) == 0).OrderByDescending(c => c.SSId).FirstOrDefaultAsync();
                                    if (st != null)
                                    {
                                        oldTimes = st.lastModifiedDate.Value;
                                        spans = newTimes.Subtract(oldTimes);
                                    }

                                    if (spans.Minutes >= 9)
                                    {
                                        var app = await dbMain.AppDetails.Where(c => c.AppId == AppId).FirstOrDefaultAsync();
                                        app.FAQ = "2";
                                    }


                                    DateTime newTimel = DateTime.Now;
                                    DateTime oldTimel;
                                    TimeSpan spanl = TimeSpan.Zero;
                                    var lw = await db.LiquidWasteDetails.Where(c => c.LWLat != null && c.LWLong != null && EF.Functions.DateDiffDay(c.lastModifiedDate, newTimel) == 0).OrderByDescending(c => c.LWId).FirstOrDefaultAsync();
                                    if (lw != null)
                                    {
                                        oldTimel = lw.lastModifiedDate.Value;
                                        spanl = newTimel.Subtract(oldTimel);
                                    }

                                    if (spanl.Minutes >= 9)
                                    {
                                        var app = await dbMain.AppDetails.Where(c => c.AppId == AppId).FirstOrDefaultAsync();
                                        app.FAQ = "2";
                                    }

                                    if (hd == null && dy == null && st == null && lw == null)
                                    {
                                        var app = await dbMain.AppDetails.Where(c => c.AppId == AppId).FirstOrDefaultAsync();
                                        app.FAQ = "0";
                                    }
                                    if ((spanl.Minutes <= 9 && lw != null) || (spans.Minutes <= 9 && st != null) || (spand.Minutes <= 9 && dy != null) || (spanh.Minutes <= 9 && hd != null))
                                    {
                                        var app = await dbMain.AppDetails.Where(c => c.AppId == AppId).FirstOrDefaultAsync();
                                        app.FAQ = "1";
                                    }
                                    await dbMain.SaveChangesAsync();
                                    //List<AppDetail> AppDetailss = dbMain.AppDetails.FromSqlRaw<AppDetail>("exec [Update_Trigger]").ToList();
                                }
                            }

                            result.Add(new SyncResult()
                            {
                                ID = Convert.ToInt32(x.OfflineId),
                                status = "success",
                                message = "Uploaded successfully",
                                messageMar = "सबमिट यशस्वी",
                            });
                        }
                    }
                    return result;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.ToString(), ex);
                    List<SyncResult> objres = new List<SyncResult>();
                    objres.Add(new SyncResult()
                    {
                        ID = 0,
                        status = "error",
                        messageMar = "काहीतरी चुकीचे आहे, पुन्हा प्रयत्न करा..",
                        message = "Something is wrong,Try Again.. ",
                    });


                    return objres;
                }

            }

        }

        public async Task<List<SyncResult>> SaveUserLocationSAAsync(List<SBUserLocation> obj, int AppId, string batteryStatus, int typeId, string EmpType)
        {



            try
            {
                List<SyncResult> result = new List<SyncResult>();
                var distCount = "";

                using (DevICTSBMMainEntities dbMain = new DevICTSBMMainEntities())
                {
                    foreach (var x in obj)
                    {
                        DateTime Dateeee = Convert.ToDateTime(x.datetime);
                        DateTime newTime = Dateeee;
                        DateTime oldTime;
                        TimeSpan span = TimeSpan.Zero;
                        var gcd = await dbMain.UR_Locations.Where(c => c.empId == x.userId && c.type == null && EF.Functions.DateDiffDay(c.datetime, Dateeee) == 0).OrderByDescending(c => c.locId).FirstOrDefaultAsync();
                        if (gcd != null)
                        {
                            oldTime = gcd.datetime.Value;
                            span = newTime.Subtract(oldTime);
                        }

                        if (gcd == null || span.Minutes >= 9)
                        {

                            var u = await dbMain.EmployeeMasters.Where(c => c.EmpId == x.userId && c.isActive == true).FirstOrDefaultAsync();
                            DateTime Offlinedate = Convert.ToDateTime(x.datetime);


                            var atten = await dbMain.HSUR_Daily_Attendances.Where(c => c.userId == x.userId & EF.Functions.DateDiffDay(c.daDate, Offlinedate) == 0).FirstOrDefaultAsync();

                            if (atten == null)
                            {
                                result.Add(new SyncResult()
                                {
                                    OfflineId = Convert.ToInt32(x.OfflineId),
                                    isAttendenceOff = true,
                                    status = "error",
                                    message = "Your duty is currently off, please start again.. ",
                                    messageMar = "आपली ड्यूटी सध्या बंद आहे, कृपया पुन्हा सुरू करा..",
                                });

                                //return result;
                                continue;
                            }

                            if (u != null & x.userId > 0)
                            {
                                string addr = "", ar = "";
                                addr = Address(x.lat + "," + x.@long);
                                if (addr != "")
                                {
                                    ar = area(addr);
                                }


                                //var locc = await db.SP_UserLatLongDetailMains(x.userId, typeId).FirstOrDefault();
                                List<SqlParameter> parms = new List<SqlParameter>
                                                                        {
                                                                            // Create parameter(s)    
                                                                            new SqlParameter { ParameterName = "@userid", Value = x.userId },
                                                                            new SqlParameter { ParameterName = "@typeId", Value = typeId }
                                                                        };
                                var Listlocc = await dbMain.SP_UserLatLongDetailMain_Results.FromSqlRaw<SP_UserLatLongDetailMain_Result>("EXEC SP_UserLatLongDetailMain @userid,@typeId", parms.ToArray()).ToListAsync();
                                var locc = Listlocc == null ? null : Listlocc.FirstOrDefault();

                                if (locc == null || locc.lat == "" || locc.@long == "")
                                {


                                    string a = x.lat;
                                    string b = x.@long;

                                    //var dist = dbMain.SP_DistanceCount(Convert.ToDouble(a), Convert.ToDouble(b), Convert.ToDouble(x.lat), Convert.ToDouble(x.@long)).FirstOrDefault();
                                    List<SqlParameter> parms1 = new List<SqlParameter>
                                                                        {
                                                                            // Create parameter(s)    
                                                                            new SqlParameter { ParameterName = "@sLat", Value = Convert.ToDouble(a) },
                                                                            new SqlParameter { ParameterName = "@sLong", Value = Convert.ToDouble(b) },
                                                                            new SqlParameter { ParameterName = "@dLat", Value = Convert.ToDouble(x.lat) },
                                                                            new SqlParameter { ParameterName = "@dLong", Value = Convert.ToDouble(x.@long) }
                                                                        };
                                    var Listdist = await dbMain.SP_DistanceCount_Main_Results.FromSqlRaw<SP_DistanceCount_Main_Result>("EXEC SP_DistanceCount @sLat,@sLong,@dLat,@dLong", parms1.ToArray()).ToListAsync();
                                    var dist = Listdist == null ? null : Listdist.FirstOrDefault();
                                    distCount = dist.Distance_in_KM.ToString();
                                }
                                else
                                {
                                    //var dist = dbMain.SP_DistanceCount(Convert.ToDouble(locc.lat), Convert.ToDouble(locc.@long), Convert.ToDouble(x.lat), Convert.ToDouble(x.@long)).FirstOrDefault();
                                    List<SqlParameter> parms1 = new List<SqlParameter>
                                                                        {
                                                                            // Create parameter(s)    
                                                                            new SqlParameter { ParameterName = "@sLat", Value = Convert.ToDouble(locc.lat) },
                                                                            new SqlParameter { ParameterName = "@sLong", Value = Convert.ToDouble(locc.@long) },
                                                                            new SqlParameter { ParameterName = "@dLat", Value = Convert.ToDouble(x.lat) },
                                                                            new SqlParameter { ParameterName = "@dLong", Value = Convert.ToDouble(x.@long) }
                                                                        };
                                    var Listdist = await dbMain.SP_DistanceCount_Main_Results.FromSqlRaw<SP_DistanceCount_Main_Result>("EXEC SP_DistanceCount @sLat,@sLong,@dLat,@dLong", parms1.ToArray()).ToListAsync();
                                    var dist = Listdist == null ? null : Listdist.FirstOrDefault();
                                    distCount = dist.Distance_in_KM.ToString();
                                }



                                dbMain.UR_Locations.Add(new UR_Location()
                                {
                                    empId = x.userId,
                                    lat = x.lat,
                                    _long = x.@long,
                                    datetime = x.datetime,
                                    address = addr,
                                    area = ar,
                                    batteryStatus = batteryStatus,
                                    Distnace = Convert.ToDecimal(distCount),
                                    CreatedDate = DateTime.Now,
                                    type = null,
                                });
                                await dbMain.SaveChangesAsync();
                            }
                        }

                        result.Add(new SyncResult()
                        {
                            OfflineId = Convert.ToInt32(x.OfflineId),
                            status = "success",
                            message = "Uploaded successfully",
                            messageMar = "सबमिट यशस्वी",
                        });


                    }



                    return result;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString(), ex);
                List<SyncResult> objres = new List<SyncResult>();
                objres.Add(new SyncResult()
                {
                    OfflineId = 0,
                    status = "error",
                    messageMar = "काहीतरी चुकीचे आहे, पुन्हा प्रयत्न करा..",
                    message = "Something is wrong,Try Again.. ",
                });


                return objres;
            }



        }

        public string Address(string location)
        {
            return "";
        }
        public string area(string address)
        {

            if (!string.IsNullOrEmpty(address))
            {
                string[] ad = address.Split(',');
                int l = ad.Length - 4;
                if (l >= 0 && ad.Length > 0)
                    return ad[l];
            }

            return string.Empty;
        }

        public async Task<List<SBAAttendenceSettingsGridRow>> SaveAttendenceSettingsDetailAsync(int AppId, string hour)
        {


            using (var db = new DevICTSBMChildEntities(AppId))
            {
                List<SBAAttendenceSettingsGridRow> datalist = new List<SBAAttendenceSettingsGridRow>();
                //var data1 = db.sp_MsgNotification(hour).ToList();
                var data1 = await db.sp_MsgNotification_Results.FromSqlRaw<sp_MsgNotification_Result>($"EXEC sp_MsgNotification {hour}").ToListAsync();
                foreach (var data in data1)
                {
                    string mes = "कचरा संकलन कर्मचारी ''" + data.userNameMar + "'' आज ड्युटी वर गैरहजर आहे";
                    string housemob = "9422783030,8830635095,7385888068,9420870617,8605551059,9423684600";
                    sendSMSmar(mes, housemob);
                }
                return datalist;
            }



        }

        public async Task<CollectionSyncResult> SaveGarbageCollectionOfflineAsync(SBGarbageCollectionView obj, int AppId, int type)
        {
            CollectionSyncResult result = new CollectionSyncResult();
            // var appdetails = dbMain.AppDetails.Where(c => c.AppId == AppId).FirstOrDefault();


            using (DevICTSBMChildEntities db = new DevICTSBMChildEntities(AppId))
            using (DevICTSBMMainEntities dbMain = new DevICTSBMMainEntities())
            {
                var appdetails = dbMain.AppDetails.Where(c => c.AppId == AppId).FirstOrDefault();
                string House_Lat = obj.Lat;
                string House_Long = obj.Long;
                string HouseLat = House_Lat.Substring(0, 5);
                string HouseLong = House_Long.Substring(0, 5);
                var house = db.HouseMasters.Where(c => c.ReferanceId == obj.houseId && c.houseLat.Contains(HouseLat) && c.houseLong.Contains(HouseLong)).FirstOrDefault();
                coordinates p = new coordinates()
                {
                    lat = Convert.ToDouble(obj.Lat),
                    lng = Convert.ToDouble(obj.Long)
                };
                if (appdetails.AppAreaLatLong != null)
                {
                    List<List<coordinates>> lstPoly = new List<List<coordinates>>();
                    List<coordinates> poly = new List<coordinates>();
                    AppAreaMapVM ebm = GetEmpBeatMapByAppId(AppId);
                    lstPoly = ebm.AppAreaLatLong;
                    int polyId = 0;
                    if (lstPoly != null && lstPoly.Count > polyId)
                    {
                        poly = lstPoly[polyId];
                    }


                    obj.IsIn = IsPointInPolygon(poly, p);
                }



                //  if ((obj.IsIn == true && appdetails.IsAreaActive == true) || (obj.IsIn == false && appdetails.IsAreaActive == false) ||  (obj.IsIn == true && appdetails.IsAreaActive == false))
                if (appdetails.IsAreaActive == true || appdetails.IsAreaActive == false || appdetails.IsAreaActive == null)
                {
                    if (obj.IsLocation == false && house != null && appdetails.IsScanNear == true)
                    {
                        switch (obj.gcType)
                        {
                            case 1:
                                result = await SaveHouseCollectionSyncAsync(obj, AppId, type);
                                break;
                            case 2:
                                result = await SavePointCollectionSyncAsync(obj, AppId, type);
                                break;
                            case 3:
                                if (obj.EmpType == "N")
                                {
                                    result = await SaveDumpCollectionSyncForNormalAsync(obj, AppId, type);
                                }
                                if (obj.EmpType == "L")
                                {
                                    result = await SaveDumpCollectionSyncForLiquidAsync(obj, AppId, type);
                                }

                                if (obj.EmpType == "S")
                                {
                                    result = await SaveDumpCollectionSyncForStreetAsync(obj, AppId, type);
                                }

                                break;
                            case 4:
                                result = await SaveLiquidCollectionSyncAsync(obj, AppId, type);
                                break;
                            case 5:
                                result = await SaveStreetCollectionSyncAsync(obj, AppId, type);
                                break;
                            case 6:
                                if (obj.EmpType == "D")
                                {
                                    result = await SaveDumpCollectionSyncForDumpAsync(obj, AppId, type);
                                }
                                break;
                        }
                    }

                    else if (obj.IsLocation == false && appdetails.IsScanNear == null)
                    {
                        switch (obj.gcType)
                        {
                            case 1:
                                result = await SaveHouseCollectionSyncAsync(obj, AppId, type);
                                break;
                            case 2:
                                result = await SavePointCollectionSyncAsync(obj, AppId, type);
                                break;
                            case 3:
                                if (obj.EmpType == "N")
                                {
                                    result = await SaveDumpCollectionSyncForNormalAsync(obj, AppId, type);
                                }
                                if (obj.EmpType == "L")
                                {
                                    result = await SaveDumpCollectionSyncForLiquidAsync(obj, AppId, type);
                                }

                                if (obj.EmpType == "S")
                                {
                                    result = await SaveDumpCollectionSyncForStreetAsync(obj, AppId, type);
                                }

                                break;

                            case 4:
                                result = await SaveLiquidCollectionSyncAsync(obj, AppId, type);
                                break;
                            case 5:
                                result = await SaveStreetCollectionSyncAsync(obj, AppId, type);
                                break;
                            case 6:
                                if (obj.EmpType == "D")
                                {
                                    result = await SaveDumpCollectionSyncForDumpAsync(obj, AppId, type);
                                }
                                break;
                        }
                    }
                    else if (obj.IsLocation == true)
                    {
                        result = await SaveUserLocationOfflineSyncAsync(obj, AppId, type);
                    }
                    if (obj.IsLocation == false && house == null && appdetails.IsScanNear == true)
                    {
                        result.message = "You Are Not In Nearby.";
                        result.messageMar = "आपण जवळपास नाही.";

                    }

                    if (obj.IsLocation == false && obj.EmpType == "N" && result.status == "success")
                    {
                        appdetails.Today_Waste_Status = true;
                        //db.BunchListAutoupdate(obj.userId, obj.houseId, Convert.ToDateTime(obj.gcDate), result.IsExist);
                        //await db.Database.ExecuteSqlRawAsync($"EXEC BunchListAutoupdate {obj.userId.ToString()},{obj.houseId.ToString()},{Convert.ToDateTime(obj.gcDate).ToString()},{result.IsExist.ToString()}");

                    }
                    if (obj.IsLocation == false && obj.EmpType == "L" && result.status == "success")
                    {
                        appdetails.Today_Liquid_Status = true;

                    }
                    if (obj.IsLocation == false && obj.EmpType == "S" && result.status == "success")
                    {
                        appdetails.Today_Street_Status = true;
                    }
                    await dbMain.SaveChangesAsync();
                }

                else
                {
                    result.ID = obj.OfflineID;
                    result.message = "Your outside the area,please go to inside the area.. ";
                    result.messageMar = "तुम्ही क्षेत्राबाहेर आहात.कृपया परिसरात जा..";
                    result.status = "error";

                    return result;
                }
            }
            return result;
        }

        public async Task<CollectionSyncResult> SaveHouseCollectionSyncAsync(SBGarbageCollectionView obj, int AppId, int type)
        {
            int locType = 0;
            string mes = string.Empty;
            CollectionSyncResult result = new CollectionSyncResult();
            HouseMaster dbHouse = new HouseMaster();




            //using (new TransactionScope(
            //         TransactionScopeOption.Required,
            //         new TransactionOptions
            //         {
            //             IsolationLevel = IsolationLevel.ReadUncommitted
            //         }))
            //{
            using (DevICTSBMChildEntities db = new DevICTSBMChildEntities(AppId))
            using (DevICTSBMMainEntities dbMain = new DevICTSBMMainEntities())
            {
                //await using var tx = await db.Database.BeginTransactionAsync();
                var appdetails = await dbMain.AppDetails.Where(c => c.AppId == AppId).FirstOrDefaultAsync();
                string name = "", housemob = "", nameMar = "", addre = "";
                var house = await db.HouseMasters.Where(c => c.ReferanceId == obj.houseId).FirstOrDefaultAsync();
                bool IsExist = false;
                if (house == null)
                {
                    result.ID = obj.OfflineID;
                    result.status = "error";
                    result.message = "Invalid House Id";
                    result.messageMar = "अवैध घर आयडी";
                    return result;
                }
                else
                {

                    //bool IsExist = false;
                    DateTime Dateeee = Convert.ToDateTime(obj.gcDate);
                    DateTime startDateTime = new DateTime(Dateeee.Year, Dateeee.Month, Dateeee.Day, 00, 00, 00, 000);
                    DateTime endDateTime = new DateTime(Dateeee.Year, Dateeee.Month, Dateeee.Day, 23, 59, 59, 999);
                    var IsSameHouseRecord = await db.GarbageCollectionDetails.Where(c => c.userId == obj.userId && c.houseId == house.houseId && c.gcDate == Dateeee).FirstOrDefaultAsync();
                    if (IsSameHouseRecord == null)
                    {

                        try
                        {
                            GarbageCollectionDetail objdata = new GarbageCollectionDetail();
                            objdata.userId = obj.userId;
                            objdata.gcDate = Dateeee;
                            objdata.Lat = obj.Lat;
                            objdata.Long = obj.Long;
                            //    objdata.garbageType = obj.garbageType;
                            var atten = await db.Daily_Attendances.Where(c => c.userId == obj.userId && EF.Functions.DateDiffDay(c.daDate, Dateeee) == 0).FirstOrDefaultAsync();

                            Location loc = new Location();

                            if (atten == null)
                            {
                                result.isAttendenceOff = true;
                                result.ID = obj.OfflineID;
                                result.message = "Your duty is currently off, please start again.. ";
                                result.messageMar = "आपली ड्यूटी सध्या बंद आहे, कृपया पुन्हा सुरू करा..";
                                result.status = "success";
                                return result;
                            }
                            else { result.isAttendenceOff = false; }
                            if (obj.houseId != null && obj.houseId != "")
                            {
                                try
                                {
                                    locType = 1;
                                    objdata.houseId = house.houseId;
                                    name = house.houseOwner;
                                    nameMar = checkNull(house.houseOwnerMar);
                                    addre = checkNull(house.houseAddress);
                                    housemob = house.houseOwnerMobile;


                                    //IsExist = (from p in db.GarbageCollectionDetails where p.houseId == objdata.houseId && p.gcDate >= startDateTime && p.gcDate <= endDateTime select p).Count() > 0;
                                    IsExist = await db.GarbageCollectionDetails.Where(p => p.houseId == objdata.houseId && p.gcDate >= startDateTime && p.gcDate <= endDateTime).AnyAsync();

                                    //if (obj.wastetype == "DW")
                                    //{
                                    //   
                                    //    IsExist = (from p in db.GarbageCollectionDetails where p.houseId == objdata.houseId && p.WasteType == "DW" && p.gcDate >= startDateTime && p.gcDate <= endDateTime select p).Count() > 0;
                                    //}
                                    //if (obj.wastetype=="WW")
                                    //{
                                    //    
                                    //    IsExist = (from p in db.GarbageCollectionDetails where p.houseId == objdata.houseId && p.WasteType=="WW" && p.gcDate >= startDateTime && p.gcDate <= endDateTime select p).Count() > 0;
                                    //}                            

                                }
                                catch (Exception ex)
                                {
                                    result.ID = obj.OfflineID;
                                    result.message = "Invalid houseId"; result.messageMar = "अवैध घर आयडी";
                                    result.status = "error";
                                    return result;
                                }

                            }


                            if (IsExist == true)
                            {

                                var gcd = await db.GarbageCollectionDetails.Where(c => c.houseId == house.houseId && c.userId == obj.userId && EF.Functions.DateDiffDay(c.gcDate, Dateeee) == 0).FirstOrDefaultAsync();
                                if (gcd == null)
                                {
                                    result.ID = obj.OfflineID;
                                    result.message = "This house id already scanned."; result.messageMar = "हे घर आयडी आधीच स्कॅन केले आहे.";
                                    result.status = "error";
                                    return result;
                                }
                                if (gcd != null)
                                {
                                    if (Dateeee > gcd.gcDate)
                                    {
                                        gcd.gcType = obj.gcType;
                                        gcd.gpBeforImage = obj.gpBeforImage;
                                        gcd.gpAfterImage = obj.gpAfterImage;
                                        gcd.note = checkNull(obj.note);
                                        gcd.garbageType = checkIntNull(obj.garbageType.ToString());
                                        objdata.garbageType = checkIntNull(obj.garbageType.ToString());
                                        gcd.vehicleNumber = checkNull(obj.vehicleNumber);

                                        gcd.batteryStatus = obj.batteryStatus;
                                        gcd.userId = obj.userId;
                                        gcd.gcDate = Dateeee;
                                        gcd.Lat = obj.Lat;
                                        gcd.Long = obj.Long;

                                        //gcd.Lat = house.houseLat;
                                        //gcd.Long = house.houseLong;
                                    }


                                    //if (AppId == 1003 || AppId == 1010)
                                    //{
                                    //    objdata.locAddresss = Address(objdata.Lat + "," + objdata.Long);
                                    //}
                                    //else
                                    //{
                                    //    objdata.locAddresss = addre;
                                    //}

                                    gcd.locAddresss = addre;
                                    gcd.CreatedDate = DateTime.Now;

                                    //var LocationContext = db.Locations.Where(c => c.datetime == Dateeee && c.userId == obj.userId).FirstOrDefault();

                                    //LocationContext.datetime = Dateeee;
                                    //LocationContext.lat = objdata.Lat;
                                    //LocationContext.@long = objdata.Long;
                                    //LocationContext.address = objdata.locAddresss;
                                    //LocationContext.batteryStatus = obj.batteryStatus; 
                                    //if (objdata.locAddresss != "")
                                    //{ LocationContext.area = area(objdata.locAddresss); }
                                    //else
                                    //{
                                    //    LocationContext.area = "";
                                    //}
                                    //LocationContext.userId = objdata.userId;
                                    //LocationContext.type = 1;
                                    //LocationContext.Distnace = obj.Distance; //Convert.ToDecimal(distCount);
                                    //LocationContext.IsOffline = true;
                                    //LocationContext.ReferanceID = obj.houseId;
                                    //LocationContext.CreatedDate = DateTime.Now;

                                    loc.datetime = Dateeee;
                                    loc.lat = objdata.Lat;
                                    loc._long = objdata.Long;
                                    loc.address = objdata.locAddresss; //Address(objdata.Lat + "," + objdata.Long);
                                    loc.batteryStatus = obj.batteryStatus;
                                    if (objdata.locAddresss != "")
                                    { loc.area = area(loc.address); }
                                    else
                                    {
                                        loc.area = "";
                                    }
                                    loc.userId = objdata.userId;
                                    loc.type = 1;
                                    loc.Distnace = obj.Distance;
                                    loc.IsOffline = obj.IsOffline;

                                    if (!string.IsNullOrEmpty(obj.houseId))
                                    {
                                        loc.ReferanceID = obj.houseId;
                                    }
                                    loc.CreatedDate = DateTime.Now;

                                    db.Locations.Add(loc);
                                    await db.SaveChangesAsync();
                                    result.IsExist = true;

                                }
                            }
                            else
                            {
                                if (house != null)
                                {
                                    if (house.houseLat == null && house.houseLong == null)
                                    {
                                        house.houseLat = obj.Lat;
                                        house.houseLong = obj.Long;
                                    }
                                }

                                objdata.gcType = obj.gcType;
                                objdata.gpBeforImage = obj.gpBeforImage;
                                objdata.gpAfterImage = obj.gpAfterImage;
                                objdata.note = checkNull(obj.note);
                                objdata.garbageType = checkIntNull(obj.garbageType.ToString());
                                objdata.vehicleNumber = checkNull(obj.vehicleNumber);
                                loc.Distnace = obj.Distance; // Convert.ToDecimal(distCount);
                                objdata.batteryStatus = obj.batteryStatus;
                                objdata.userId = obj.userId;

                                //if (AppId == 1010)
                                //{
                                //    objdata.locAddresss = Address(objdata.Lat + "," + objdata.Long);
                                //}
                                //else
                                //{
                                //    objdata.locAddresss = addre;
                                //}

                                objdata.locAddresss = addre;
                                objdata.CreatedDate = DateTime.Now;
                                objdata.WasteType = obj.wastetype;
                                db.GarbageCollectionDetails.Add(objdata);

                                loc.datetime = Dateeee;
                                loc.lat = objdata.Lat;
                                loc._long = objdata.Long;
                                loc.address = objdata.locAddresss;
                                loc.batteryStatus = obj.batteryStatus;
                                if (objdata.locAddresss != "")
                                { loc.area = area(loc.address); }
                                else
                                {
                                    loc.area = "";
                                }
                                loc.userId = objdata.userId;
                                loc.type = 1;
                                loc.Distnace = obj.Distance;
                                loc.IsOffline = obj.IsOffline;
                                if (!string.IsNullOrEmpty(obj.houseId))
                                {
                                    loc.ReferanceID = obj.houseId;
                                }
                                loc.CreatedDate = DateTime.Now;

                                db.Locations.Add(loc);
                                await db.SaveChangesAsync();


                            }

                            result.ID = obj.OfflineID;
                            result.status = "success";
                            result.message = "Uploaded successfully";
                            result.messageMar = "सबमिट यशस्वी";
                            if (appdetails.AppId == 1003 || appdetails.AppId == 1006)
                            {
                                result.messageMar = "सबमिट यशस्वी";
                            }

                            else
                            {
                                if (objdata.garbageType == 3 && objdata.houseId != null)
                                {
                                    //string mes = "Dear Citizen Waste Pattern collected today from your house-Waste Not Specified Suggested Waste Pattern - Dry and Wet Segregated.Thank You! " + appdetails.AppName + ".";

                                    //string mesMar = "नमस्कार! आपल्या घरून कचरा संकलित करण्यात आलेला आहे. कृपया ओला व सुका असा वर्गीकृत केलेला कचरा सफाई कर्मचाऱ्यास सुपूर्द करून सहकार्य करावे धन्यवाद. " + appdetails.yoccContact + " आपल्या सेवेशी " + appdetails.AppName_mar + "";

                                    switch (appdetails.LanguageId)
                                    {
                                        //case 1:
                                        //    mes = "Dear Citizen Waste Pattern collected today from your house-Waste Not Specified Suggested Waste Pattern - Dry and Wet Segregated.Thank You! " + appdetails.AppName + ".";
                                        //    break;

                                        //case 2:
                                        //    mes = "नमस्कार! आपल्या घरून कचरा संकलित करण्यात आलेला आहे. कृपया ओला व सुका असा वर्गीकृत केलेला कचरा सफाई कर्मचाऱ्यास सुपूर्द करून सहकार्य करावे धन्यवाद. " + appdetails.yoccContact + " आपल्या सेवेशी " + appdetails.AppName_mar + "";
                                        //    break;

                                        case 3:
                                            //  mes = "नमस्कार! आपके घर से कचरा एकत्र किया जाता है। गीले और सूखे के रूप में वर्गीकृत कचरा सफाई कर्मचारियों को सौंपने में सहायता के लिए धन्यवाद। आपकी सेवा में " + appdetails.AppName_mar + " " + appdetails.yoccContact;
                                            mes = "" + appdetails.MsgForNotSpecified + " " + appdetails.AppName_mar + " " + appdetails.yoccContact;
                                            break;
                                        case 4:
                                            //  mes = "नमस्कार! आपल्या घरून कचरा संकलित करण्यात आलेला आहे. कृपया ओला व सुका असा वर्गीकृत केलेला कचरा सफाई कर्मचाऱ्यास सुपूर्द करून सहकार्य करावे धन्यवाद. " + appdetails.yoccContact + " आपल्या सेवेशी " + appdetails.AppName_mar + "";
                                            mes = "" + appdetails.MsgForNotSpecified + " " + appdetails.yoccContact + " आपल्या सेवेशी " + appdetails.AppName_mar + "";
                                            break;

                                        default:
                                            //  mes = "Dear Citizen Waste Pattern collected today from your house-Waste Not Specified Suggested Waste Pattern - Dry and Wet Segregated.Thank You! " + appdetails.AppName + ".";
                                            mes = "" + appdetails.MsgForNotSpecified + " " + appdetails.AppName + ".";
                                            break;
                                    }


                                    if (house != null)
                                    {
                                        List<String> ArrayList = await DeviceDetailsFCM(obj.houseId, AppId);

                                        if (ArrayList.Count > 0)
                                        {
                                            PushNotificationMessageBroadCast(mes, ArrayList, appdetails.AppName, appdetails.Android_GCM_pushNotification_Key);
                                        }
                                        else if (housemob != "")
                                        {
                                            if (appdetails.LanguageId == 4)
                                            {
                                                sendSMSmar(mes, housemob);
                                            }
                                            if (appdetails.LanguageId == 1)
                                            {

                                            }

                                            else
                                            {
                                                if (appdetails.LanguageId != 4)
                                                {
                                                    sendSMS(mes, housemob);
                                                }

                                                else
                                                {
                                                    sendSMS(mes, housemob);
                                                }
                                            }
                                        }
                                    }

                                }

                                if (objdata.garbageType == 0)
                                {
                                    //string mes = "Dear Citizen Waste Pattern collected today from your house- mixed Suggested Waste Pattern - Dry and Wet Segregated. Thank You! " + appdetails.AppName + ".";

                                    //string mesMar = "नमस्कार! आपल्या घरातून आज ओला व सुका कचरा मिश्र स्वरूपात संकलित करण्यात आलेला आहे. आपणास विनंती आहे कि दररोज ओला व सुका कचरा विघटित करून सफाई कर्मचाऱ्यास सुपूर्द करून सहयोग करावा धन्यवाद. " + appdetails.yoccContact + " आपल्या सेवेशी " + appdetails.AppName_mar + "";

                                    switch (appdetails.LanguageId)
                                    {
                                        //case 1:
                                        //    mes = "Dear Citizen Waste Pattern collected today from your house- mixed Suggested Waste Pattern - Dry and Wet Segregated. Thank You! " + appdetails.AppName + ".";
                                        //    break;

                                        //case 2:
                                        //    mes = "नमस्कार! आपल्या घरातून आज ओला व सुका कचरा मिश्र स्वरूपात संकलित करण्यात आलेला आहे. आपणास विनंती आहे कि दररोज ओला व सुका कचरा विघटित करून सफाई कर्मचाऱ्यास सुपूर्द करून सहयोग करावा धन्यवाद. " + appdetails.yoccContact + " आपल्या सेवेशी " + appdetails.AppName_mar + "";
                                        //    break;

                                        case 3:
                                            //  mes = "नमस्कार! आज, हमारे घरों से मिश्रित रूप में नम और सूखा कचरा एकत्र किया गया है। आपसे अनुरोध है कि प्रतिदिन नम कचरे की सफाई और निपटान में सहायता करें और सफाई कर्मचारियों को सौंप दें। " + appdetails.yoccContact + " आपकी सेवा में " + appdetails.AppName_mar + "";
                                            mes = "" + appdetails.MsgForMixed + " " + appdetails.yoccContact + " आपकी सेवा में " + appdetails.AppName_mar + "";
                                            break;

                                        case 4:
                                            //  mes = "प्रिय नागरिक आपणाद्वारे आज मिश्र स्वरूपाचा कचरा देण्यात आला आहे. कृपया दररोज ओला व सुका असा वर्गीकृत कचरा देण्यात यावा.आपली सौ स्वातीताई संतोषभाऊ कोल्हे " + appdetails.yoccContact + " आपल्या सेवेशी " + appdetails.AppName_mar + "";
                                            mes = "" + appdetails.MsgForMixed + " " + appdetails.yoccContact + "  आपल्या सेवेशी " + appdetails.AppName_mar + "";
                                            break;
                                        default:
                                            //   mes = "Dear Citizen Waste Pattern collected today from your house- mixed Suggested Waste Pattern - Dry and Wet Segregated. Thank You! " + appdetails.AppName + ".";
                                            mes = "" + appdetails.MsgForMixed + " " + appdetails.AppName + ".";
                                            break;
                                    }

                                    if (house != null)
                                    {
                                        List<String> ArrayList = await DeviceDetailsFCM(obj.houseId, AppId);

                                        if (ArrayList.Count > 0)
                                        {
                                            PushNotificationMessageBroadCast(mes, ArrayList, appdetails.AppName, appdetails.Android_GCM_pushNotification_Key);
                                        }
                                        else if (housemob != "")
                                        {
                                            if (appdetails.LanguageId == 4)
                                            {
                                                sendSMSmar(mes, housemob);
                                            }
                                            if (appdetails.LanguageId == 1)
                                            {

                                            }

                                            else
                                            {
                                                if (appdetails.LanguageId != 4)
                                                {
                                                    sendSMS(mes, housemob);
                                                }

                                                else
                                                {
                                                    sendSMS(mes, housemob);
                                                }
                                            }
                                        }
                                    }

                                }

                                if (objdata.garbageType == 1)
                                {
                                    //string mes = "Dear Citizen Waste Pattern collected today from your house-Segregated Suggested Waste Pattern - Dry and Wet Segregated.Thank You! " + appdetails.AppName + ".";

                                    //string mesMar = "नमस्कार! आपल्या घरातून आज ओला व सुका असा विघटित केलेला कचरा संकलित करण्यात आलेला आहे. आपण केलेल्या सहयोगाबद्दल धन्यवाद. " + appdetails.yoccContact + " आपल्या सेवेशी " + appdetails.AppName_mar + "";

                                    switch (appdetails.LanguageId)
                                    {
                                        //case 1:
                                        //    mes = "Dear Citizen Waste Pattern collected today from your house-Segregated Suggested Waste Pattern - Dry and Wet Segregated.Thank You! " + appdetails.AppName + ".";
                                        //    break;

                                        //case 2:
                                        //    mes = "नमस्कार! आपल्या घरातून आज ओला व सुका असा विघटित केलेला कचरा संकलित करण्यात आलेला आहे. आपण केलेल्या सहयोगाबद्दल धन्यवाद. " + appdetails.yoccContact + " आपल्या सेवेशी " + appdetails.AppName_mar + "";
                                        //    break;

                                        case 3:
                                            //mes = "नमस्कार! आज, हमारे घर से कचरा एकत्र किया जाता है, जो गीला और सूखा होता है। आपके सहयोग के लिए धन्यवाद।" + appdetails.yoccContact + " आपकी सेवा में " + appdetails.AppName_mar + "";
                                            mes = "" + appdetails.MsgForSegregated + " " + appdetails.yoccContact + " आपकी सेवा में " + appdetails.AppName_mar + "";
                                            break;
                                        case 4:
                                            //  mes = "प्रिय नागरिक, आपण घंटागाडीमध्ये ओला व सुका असा वर्गीकृत कचरा दिल्याबद्दल धन्यवाद. आपली सौ स्वातीताई संतोषभाऊ कोल्हे" + appdetails.yoccContact + " आपल्या सेवेशी " + appdetails.AppName_mar + "";
                                            mes = "" + appdetails.MsgForSegregated + " " + appdetails.yoccContact + " आपल्या सेवेशी " + appdetails.AppName_mar + "";
                                            break;

                                        default:
                                            //  mes = "Dear Citizen Waste Pattern collected today from your house-Segregated Suggested Waste Pattern - Dry and Wet Segregated.Thank You! " + appdetails.AppName + ".";

                                            mes = "" + appdetails.MsgForSegregated + " " + appdetails.AppName + ".";
                                            break;
                                    }

                                    if (house != null)
                                    {
                                        List<String> ArrayList = await DeviceDetailsFCM(obj.houseId, AppId);

                                        if (ArrayList.Count > 0)
                                        {
                                            PushNotificationMessageBroadCast(mes, ArrayList, appdetails.AppName, appdetails.Android_GCM_pushNotification_Key);
                                        }
                                        else if (housemob != "")
                                        {
                                            if (appdetails.LanguageId == 4)
                                            {
                                                sendSMSmar(mes, housemob);
                                            }
                                            if (appdetails.LanguageId == 1)
                                            {

                                            }

                                            else
                                            {
                                                if (appdetails.LanguageId != 4)
                                                {
                                                    sendSMS(mes, housemob);
                                                }

                                                else
                                                {
                                                    sendSMS(mes, housemob);
                                                }
                                            }
                                        }
                                    }

                                }

                                if (objdata.garbageType == 2)
                                {
                                    //string mes = "Dear Citizen Waste Pattern collected today from your house-Waste Not Received Suggested Waste Pattern - Dry and Wet Segregated.Thank You! " + appdetails.AppName + ".";

                                    //string mesMar = "नमस्कार! आपल्या घरातून आज कोणत्याही प्रकारचा कचरा सफाई कर्मचाऱ्यास देण्यात आलेला नाही. आपणास विनंती आहे कि दररोज ओला व सुका कचरा विघटित करून सफाई कर्मचाऱ्यास सुपूर्द करून सहयोग करावा धन्यवाद. " + appdetails.yoccContact + " आपल्या सेवेशी " + appdetails.AppName_mar + "";

                                    switch (appdetails.LanguageId)
                                    {
                                        //case 1:
                                        //    mes = "Dear Citizen Waste Pattern collected today from your house-Waste Not Received Suggested Waste Pattern - Dry and Wet Segregated.Thank You! " + appdetails.AppName + ".";
                                        //    break;

                                        //case 2:
                                        //    mes = "नमस्कार! आपल्या घरातून आज कोणत्याही प्रकारचा कचरा सफाई कर्मचाऱ्यास देण्यात आलेला नाही. आपणास विनंती आहे कि दररोज ओला व सुका कचरा विघटित करून सफाई कर्मचाऱ्यास सुपूर्द करून सहयोग करावा धन्यवाद. " + appdetails.yoccContact + " आपल्या सेवेशी " + appdetails.AppName_mar + "";
                                        //    break;

                                        case 3:
                                            //  mes = "नमस्कार! आज आपके घर में कोई भी कचरा उपलब्ध नहीं कराया गया है। आपसे अनुरोध है कि प्रतिदिन गीला और सूखा कचरे की सफाई और निपटान में सहायता करें और सफाई कर्मचारियों को सौंप दें।" + appdetails.yoccContact + " आपकी सेवा में " + appdetails.AppName_mar + "";
                                            mes = "" + appdetails.MsgForNotReceived + " " + appdetails.yoccContact + " आपकी सेवा में " + appdetails.AppName_mar + "";
                                            break;

                                        case 4:
                                            //  mes = "प्रिय नागरिक आपणाद्वारे आज कचरा देण्यात आला नाही. कृपया दररोज ओला व सुका असा वर्गीकृत कचरा देण्यात यावा. आपली सौ स्वातीताई संतोषभाऊ कोल्हे" + appdetails.yoccContact + " आपल्या सेवेशी " + appdetails.AppName_mar + "";
                                            mes = "" + appdetails.MsgForNotReceived + " " + appdetails.yoccContact + " आपल्या सेवेशी " + appdetails.AppName_mar + "";
                                            break;

                                        default:
                                            //  mes = "Dear Citizen Waste Pattern collected today from your house-Segregated Suggested Waste Pattern - Dry and Wet Segregated.Thank You! " + appdetails.AppName + ".";

                                            mes = "" + appdetails.MsgForNotReceived + " " + appdetails.AppName + ".";
                                            break;
                                    }

                                    if (house != null)
                                    {
                                        List<String> ArrayList = await DeviceDetailsFCM(obj.houseId, AppId);

                                        if (ArrayList.Count > 0)
                                        {
                                            PushNotificationMessageBroadCast(mes, ArrayList, appdetails.AppName, appdetails.Android_GCM_pushNotification_Key);
                                        }
                                        else if (housemob != "")
                                        {

                                            if (appdetails.LanguageId == 4)
                                            {
                                                sendSMSmar(mes, housemob);
                                            }
                                            if (appdetails.LanguageId == 1)
                                            {

                                            }

                                            else
                                            {
                                                if (appdetails.LanguageId != 4)
                                                {
                                                    sendSMS(mes, housemob);
                                                }

                                                else
                                                {
                                                    sendSMS(mes, housemob);
                                                }
                                            }
                                        }
                                    }

                                }
                            }

                            //Update House Count ,Liquid Count, Street Count
                            if (result.status == "success")
                            {
                                try
                                {
                                    // Update code
                                    var updateappdetails = await dbMain.SP_DailyScanCount_Results.FromSqlRaw<SP_DailyScanCount_Result>($"EXEC DailyScanCount {AppId.ToString()}").ToListAsync();

                                }
                                catch (Exception ex)
                                {
                                    _logger.LogError(ex.ToString(), ex);
                                }
                            }

                            return result;
                        }

                        catch (WebException ex)
                        {
                            _logger.LogError(ex.ToString(), ex);
                            //await tx.RollbackAsync();
                            result.referenceID = obj.ReferenceID;
                            result.ID = obj.OfflineID;
                            result.message = ex.Message;
                           // result.message = "Something is wrong,Try Again.. ";
                            result.messageMar = "काहीतरी चुकीचे आहे, पुन्हा प्रयत्न करा..";
                            //result.status = Convert.ToInt32(HttpStatusCode.RequestTimeout).ToString();
                            result.status = ((HttpWebResponse)ex.Response).StatusCode.ToString();
                            return result;
                        }

                    }

                    else
                    {
                        result.ID = obj.OfflineID;
                        result.status = "success";
                        result.message = "Uploaded successfully";
                        result.messageMar = "सबमिट यशस्वी";
                        //await tx.CommitAsync();
                        return result;
                    }

                }

            }
            //}
        }

        public async Task<CollectionSyncResult> SavePointCollectionSyncAsync(SBGarbageCollectionView obj, int AppId, int type)
        {
            int locType = 0;
            CollectionSyncResult result = new CollectionSyncResult();
            HouseMaster dbHouse = new HouseMaster();


            //using (new TransactionScope(
            //         TransactionScopeOption.Required,
            //         new TransactionOptions
            //         {
            //             IsolationLevel = IsolationLevel.ReadUncommitted
            //         }))
            //{
            using (DevICTSBMChildEntities db = new DevICTSBMChildEntities(AppId))
            using (DevICTSBMMainEntities dbMain = new DevICTSBMMainEntities())
            {
                var appdetails = dbMain.AppDetails.Where(c => c.AppId == AppId).FirstOrDefault();
                string name = "", housemob = "", nameMar = "", addre = "";

                DateTime Dateeee = Convert.ToDateTime(obj.gcDate);
                DateTime startDateTime = new DateTime(Dateeee.Year, Dateeee.Month, Dateeee.Day, 00, 00, 00, 000);  //Today at 00:00:00
                DateTime endDateTime = new DateTime(Dateeee.Year, Dateeee.Month, Dateeee.Day, 23, 59, 59, 999); // Dateeee.AddDays(1).AddTicks
                try
                {
                    GarbageCollectionDetail objdata = new GarbageCollectionDetail();
                    objdata.userId = obj.userId;
                    objdata.gcDate = Convert.ToDateTime(obj.gcDate);
                    objdata.Lat = obj.Lat;
                    objdata.Long = obj.Long;

                    //var atten = db.Daily_Attendance.Where(c => c.userId == obj.userId & c.endTime == "" & c.daDate == EntityFunctions.TruncateTime(Dateeee)).FirstOrDefault();

                    var atten = await db.Daily_Attendances.Where(c => c.userId == obj.userId && EF.Functions.DateDiffDay(c.daDate, Dateeee) == 0).FirstOrDefaultAsync();

                    Location loc = new Location();

                    if (atten == null)
                    {
                        result.ID = obj.OfflineID;
                        result.isAttendenceOff = true;
                        result.message = "Your duty is currently off, please start again.. ";
                        result.messageMar = "आपली ड्यूटी सध्या बंद आहे, कृपया पुन्हा सुरू करा..";
                        result.status = "success";
                        return result;
                    }
                    else { result.isAttendenceOff = false; }


                    var gpdetails = await db.GarbagePointDetails.Where(c => c.ReferanceId == obj.gpId).FirstOrDefaultAsync();

                    //var start = Dateeee;
                    //var gpCollectionDetails = db.GarbageCollectionDetails.Where(c => c.userId == obj.userId && c.gpId == gpdetails.gpId && EntityFunctions.TruncateTime(c.gcDate) == EntityFunctions.TruncateTime(Dateeee)).OrderByDescending(c => c.gcDate).FirstOrDefault();
                    //DateTime oldDte = Convert.ToDateTime(gpCollectionDetails.gcDate);

                    //if ((start - oldDte).TotalMinutes >= 10)

                    DateTime oldTime;
                    TimeSpan span = TimeSpan.Zero;

                    var gpCollectionDetails = await db.GarbageCollectionDetails.Where(c => c.userId == obj.userId && c.gpId == gpdetails.gpId && EF.Functions.DateDiffDay(c.gcDate, Dateeee) == 0).OrderByDescending(c => c.gcDate).FirstOrDefaultAsync();
                    if (gpCollectionDetails != null)
                    {
                        oldTime = gpCollectionDetails.gcDate.Value;
                        span = Dateeee.Subtract(oldTime);
                    }

                    if (gpCollectionDetails == null || span.Minutes >= 10)
                    {
                        if (obj.gpId != null && obj.gpId != "")
                        {
                            try
                            {
                                locType = 2;
                                //var gpdetails = db.GarbagePointDetails.Where(c => c.ReferanceId == obj.gpId).FirstOrDefault();
                                objdata.gpId = gpdetails.gpId;
                                name = gpdetails.gpName;
                                nameMar = checkNull(gpdetails.gpNameMar);
                                housemob = "";
                                addre = checkNull(gpdetails.gpAddress);

                                var IsSamePointRecord = await db.GarbageCollectionDetails.Where(a => a.gpId == gpdetails.gpId && a.userId == obj.userId && a.gcDate == Dateeee).FirstOrDefaultAsync();

                                if (IsSamePointRecord != null)
                                {
                                    result.ID = obj.OfflineID;
                                    result.status = "success";
                                    result.message = "Uploaded successfully";
                                    result.messageMar = "सबमिट यशस्वी";
                                    return result;
                                }
                            }
                            catch
                            {
                                result.ID = obj.OfflineID;
                                result.message = "Invalid gpId"; result.messageMar = "अवैध जीपी आयडी";
                                result.status = "error";
                                return result;
                            }

                        }

                        objdata.gcType = obj.gcType;
                        objdata.gpBeforImage = obj.gpBeforImage;
                        objdata.gpAfterImage = obj.gpAfterImage;
                        objdata.note = checkNull(obj.note);
                        objdata.garbageType = checkIntNull(obj.garbageType.ToString());
                        objdata.vehicleNumber = checkNull(obj.vehicleNumber);
                        objdata.batteryStatus = obj.batteryStatus;
                        objdata.userId = obj.userId;

                        //if (AppId == 1010)
                        //{
                        //    objdata.locAddresss = Address(objdata.Lat + "," + objdata.Long);
                        //}
                        //else
                        //{
                        //    objdata.locAddresss = addre;
                        //}

                        objdata.locAddresss = addre;
                        objdata.CreatedDate = DateTime.Now;
                        db.GarbageCollectionDetails.Add(objdata);

                        loc.datetime = Dateeee;
                        loc.lat = objdata.Lat;
                        loc._long = objdata.Long;
                        loc.address = objdata.locAddresss;
                        loc.batteryStatus = obj.batteryStatus;
                        if (objdata.locAddresss != "")
                        { loc.area = area(loc.address); }
                        else
                        {
                            loc.area = "";
                        }
                        loc.userId = objdata.userId;
                        loc.type = 1;
                        loc.Distnace = obj.Distance; //Convert.ToDecimal(distCount);
                                                     //loc.IsOffline = obj.IsOffline;
                        loc.ReferanceID = obj.gpId;
                        loc.CreatedDate = DateTime.Now;
                        db.Locations.Add(loc);
                        await db.SaveChangesAsync();

                    }
                    else
                    {
                        if (obj.gpId != null && obj.gpId != "")
                        {
                            try
                            {
                                locType = 2;
                                //var gpdetails = db.GarbagePointDetails.Where(c => c.ReferanceId == obj.gpId).FirstOrDefault();
                                gpCollectionDetails.gpId = gpdetails.gpId;
                                name = gpdetails.gpName;
                                nameMar = checkNull(gpdetails.gpNameMar);
                                housemob = "";
                                addre = checkNull(gpdetails.gpAddress);

                                var IsSamePointRecord = await db.GarbageCollectionDetails.Where(a => a.gpId == gpdetails.gpId && a.userId == obj.userId && a.gcDate == Dateeee).FirstOrDefaultAsync();

                                if (IsSamePointRecord != null)
                                {
                                    result.ID = obj.OfflineID;
                                    result.status = "success";
                                    result.message = "Uploaded successfully";
                                    result.messageMar = "सबमिट यशस्वी";
                                    return result;
                                }
                            }
                            catch
                            {
                                result.ID = obj.OfflineID;
                                result.message = "Invalid gpId"; result.messageMar = "अवैध जीपी आयडी";
                                result.status = "error";
                                return result;
                            }

                        }

                        gpCollectionDetails.gcType = obj.gcType;
                        gpCollectionDetails.gpBeforImage = obj.gpBeforImage;
                        gpCollectionDetails.gpAfterImage = obj.gpAfterImage;
                        gpCollectionDetails.note = checkNull(obj.note);
                        gpCollectionDetails.garbageType = checkIntNull(obj.garbageType.ToString());
                        gpCollectionDetails.vehicleNumber = checkNull(obj.vehicleNumber);
                        gpCollectionDetails.batteryStatus = obj.batteryStatus;
                        gpCollectionDetails.userId = obj.userId;

                        if (AppId == 1010)
                        {
                            gpCollectionDetails.locAddresss = Address(gpCollectionDetails.Lat + "," + gpCollectionDetails.Long);
                        }
                        else
                        {
                            gpCollectionDetails.locAddresss = addre;
                        }

                        //gpCollectionDetails.locAddresss = addre;
                        gpCollectionDetails.CreatedDate = DateTime.Now;
                        //db.GarbageCollectionDetails.Add(objdata);


                        loc.datetime = Dateeee;
                        loc.lat = objdata.Lat;
                        loc._long = objdata.Long;
                        loc.address = objdata.locAddresss;
                        loc.batteryStatus = obj.batteryStatus;
                        if (objdata.locAddresss != "")
                        { loc.area = area(loc.address); }
                        else
                        {
                            loc.area = "";
                        }
                        loc.userId = objdata.userId;
                        loc.type = 1;
                        loc.Distnace = obj.Distance;
                        //loc.IsOffline = obj.IsOffline;

                        if (!string.IsNullOrEmpty(obj.gpId))
                        {
                            loc.ReferanceID = obj.gpId;
                        }

                        loc.CreatedDate = DateTime.Now;

                        db.Locations.Add(loc);

                        await db.SaveChangesAsync();

                    }

                    result.ID = obj.OfflineID;
                    result.status = "success";
                    result.message = "Uploaded successfully";
                    result.messageMar = "सबमिट यशस्वी";

                    return result;
                }

                catch (Exception ex)
                {
                    _logger.LogError(ex.ToString(), ex);
                    result.ID = obj.OfflineID;
                    result.message = "Something is wrong,Try Again.. ";
                    result.messageMar = "काहीतरी चुकीचे आहे, पुन्हा प्रयत्न करा..";
                    result.status = "error";
                    return result;
                }
            }
            //}
        }

        public async Task<CollectionSyncResult> SaveDumpCollectionSyncForNormalAsync(SBGarbageCollectionView obj, int AppId, int type)
        {

            CollectionSyncResult result = new CollectionSyncResult();

            //using (new TransactionScope(
            //         TransactionScopeOption.Required,
            //         new TransactionOptions
            //         {
            //             IsolationLevel = IsolationLevel.ReadUncommitted
            //         }))
            //{
            using (DevICTSBMChildEntities db = new DevICTSBMChildEntities(AppId))
            using (DevICTSBMMainEntities dbMain = new DevICTSBMMainEntities())
            {
                var appdetails = await dbMain.AppDetails.Where(c => c.AppId == AppId).FirstOrDefaultAsync();
                // GarbageCollectionDetail gcd = new GarbageCollectionDetail();
                string name = "", housemob = "", nameMar = "", addre = "";
                // var distCount = "";
                DateTime Dateeee = Convert.ToDateTime(obj.gcDate);
                DateTime newTime = Dateeee;
                DateTime oldTime;
                TimeSpan span = TimeSpan.Zero;
                var dydetails = await db.DumpYardDetails.Where(c => c.ReferanceId == obj.dyId).FirstOrDefaultAsync();
                //var dyId = dydetails.dyId; || tdate.AddMinutes(15) >= gcd.gcDate

                try
                {
                    var gcd = await db.GarbageCollectionDetails.Where(c => c.userId == obj.userId && c.dyId == dydetails.dyId && EF.Functions.DateDiffDay(c.gcDate, Dateeee) == 0).OrderByDescending(c => c.gcDate).FirstOrDefaultAsync();
                    if (gcd != null)
                    {
                        oldTime = gcd.gcDate.Value;
                        span = newTime.Subtract(oldTime);
                    }

                    if (gcd == null || span.TotalMinutes >= 10)
                    {
                        GarbageCollectionDetail objdata = new GarbageCollectionDetail();
                        objdata.userId = obj.userId;
                        objdata.gcDate = Dateeee;
                        objdata.Lat = obj.Lat;
                        objdata.Long = obj.Long;

                        //var atten = db.Daily_Attendance.Where(c => c.userId == obj.userId & c.endTime == "" & c.daDate == EntityFunctions.TruncateTime(Dateeee)).FirstOrDefault();

                        var atten = await db.Daily_Attendances.Where(c => c.userId == obj.userId && EF.Functions.DateDiffDay(c.daDate, Dateeee) == 0).FirstOrDefaultAsync();

                        if (atten == null)
                        {
                            result.isAttendenceOff = true;
                            result.message = "Your duty is currently off, please start again.. ";
                            result.messageMar = "आपली ड्यूटी सध्या बंद आहे, कृपया पुन्हा सुरू करा..";
                            result.status = "success";
                            result.ID = obj.OfflineID;
                            return result;
                        }
                        else { result.isAttendenceOff = false; }

                        if (obj.dyId != null && obj.dyId != "")
                        {
                            try
                            {
                                var gpdetails = await db.DumpYardDetails.Where(c => c.ReferanceId == obj.dyId).FirstOrDefaultAsync();
                                objdata.dyId = gpdetails.dyId;
                                name = gpdetails.dyName;
                                nameMar = checkNull(gpdetails.dyNameMar);
                                housemob = "";
                                addre = checkNull(gpdetails.dyAddress);

                                var IsSameDumpRecord = await db.GarbageCollectionDetails.Where(a => a.gpId == gpdetails.dyId && a.userId == obj.userId && a.gcDate == Dateeee).FirstOrDefaultAsync();

                                if (IsSameDumpRecord != null)
                                {
                                    result.ID = obj.OfflineID;
                                    result.status = "success";
                                    result.message = "Uploaded successfully";
                                    result.messageMar = "सबमिट यशस्वी";
                                    return result;
                                }
                            }
                            catch (Exception ex)
                            {
                                //_logger.LogError(ex.ToString(), ex);
                                result.ID = obj.OfflineID;
                                result.message = "Invalid dyId"; result.messageMar = "अवैध जीपी आयडी";
                                result.status = "error";
                                return result;
                            }

                        }
                        objdata.gcType = obj.gcType;
                        if (obj.gpBeforImage == null)
                        {
                            obj.gpBeforImage = "";
                        }
                        if (obj.gpAfterImage == null)
                        {
                            obj.gpAfterImage = "";
                        }
                        objdata.gpBeforImage = obj.gpBeforImage;
                        objdata.gpAfterImage = obj.gpAfterImage;
                        objdata.note = checkNull(obj.note);
                        //objdata.garbageType = checkIntNull(obj.garbageType.ToString());
                        objdata.vehicleNumber = checkNull(obj.vehicleNumber);
                        objdata.totalGcWeight = obj.totalGcWeight;
                        objdata.totalDryWeight = obj.totalDryWeight;
                        objdata.totalWetWeight = obj.totalWetWeight;
                        objdata.batteryStatus = obj.batteryStatus;
                        objdata.Distance = Convert.ToDouble(obj.Distance);  //Convert.ToDouble(distCount);

                        //if (AppId == 1010)
                        //{
                        //    objdata.locAddresss = Address(objdata.Lat + "," + objdata.Long);
                        //}
                        //else
                        //{
                        //    objdata.locAddresss = addre;
                        //}

                        objdata.locAddresss = addre;
                        objdata.CreatedDate = DateTime.Now;
                        db.GarbageCollectionDetails.Add(objdata);

                        Location loc = new Location();
                        loc.datetime = Dateeee;
                        loc.lat = objdata.Lat;
                        loc._long = objdata.Long;
                        loc.address = objdata.locAddresss;//Address(objdata.Lat + "," + objdata.Long);
                        loc.batteryStatus = objdata.batteryStatus;
                        if (objdata.locAddresss != "")
                        { loc.area = area(loc.address); }
                        else
                        {
                            loc.area = "";
                        }
                        loc.userId = objdata.userId;
                        loc.type = 1;
                        loc.Distnace = obj.Distance;
                        //loc.IsOffline = obj.IsOffline;

                        if (!string.IsNullOrEmpty(obj.dyId))
                        {
                            loc.ReferanceID = obj.dyId;
                        }

                        loc.CreatedDate = DateTime.Now;
                        db.Locations.Add(loc);
                        await db.SaveChangesAsync();

                        result.ID = obj.OfflineID;
                        result.status = "success";
                        result.message = "Uploaded successfully";
                        result.messageMar = "सबमिट यशस्वी";
                        //string mes = "नमस्कार! आपल्या घरून कचरा संकलित करण्यात आलेला आहे. कृपया ओला व सुका असा वर्गीकृत केलेला कचरा सफाई कर्मचाऱ्यास सुपूर्द करून सहकार्य करावे धन्यवाद. " + appdetails.yoccContact + " आपल्या सेवेशी " + appdetails.AppName_mar + "";
                        //if (housemob != "")
                        //{
                        //    sendSMS(mes, housemob);
                        //}
                    }
                    else
                    {
                        // GarbageCollectionDetail objdata = new GarbageCollectionDetail();
                        gcd.userId = obj.userId;
                        gcd.gcDate = Dateeee;
                        gcd.Lat = obj.Lat;
                        gcd.Long = obj.Long;
                        var atten = await db.Daily_Attendances.Where(c => c.userId == obj.userId && EF.Functions.DateDiffDay(c.daDate, Dateeee) == 0).FirstOrDefaultAsync();

                        if (atten == null)
                        {
                            result.ID = obj.OfflineID;
                            result.isAttendenceOff = true;
                            result.message = "Your duty is currently off, please start again.. ";
                            result.messageMar = "आपली ड्यूटी सध्या बंद आहे, कृपया पुन्हा सुरू करा..";
                            result.status = "success";
                            return result;
                        }
                        else { result.isAttendenceOff = false; }

                        if (obj.dyId != null && obj.dyId != "")
                        {
                            try
                            {
                                var gpdetails = await db.DumpYardDetails.Where(c => c.ReferanceId == obj.dyId).FirstOrDefaultAsync();
                                gcd.dyId = gpdetails.dyId;
                                name = gpdetails.dyName;
                                nameMar = checkNull(gpdetails.dyNameMar);
                                housemob = "";
                                addre = checkNull(gpdetails.dyAddress);
                            }
                            catch (Exception ex)
                            {
                                //_logger.LogError(ex.ToString(), ex);
                                result.ID = obj.OfflineID;
                                result.message = "Invalid dyId"; result.messageMar = "अवैध डीवाय आयडी";
                                result.status = "error";
                                return result;
                            }

                        }
                        gcd.gcType = obj.gcType;
                        if (obj.gpBeforImage == null)
                        {
                            obj.gpBeforImage = "";
                        }
                        if (obj.gpAfterImage == null)
                        {
                            obj.gpAfterImage = "";
                        }
                        gcd.gpBeforImage = obj.gpBeforImage;
                        gcd.gpAfterImage = obj.gpAfterImage;
                        gcd.note = checkNull(gcd.note);
                        //objdata.garbageType = checkIntNull(obj.garbageType.ToString());
                        gcd.vehicleNumber = checkNull(gcd.vehicleNumber);
                        gcd.totalGcWeight = obj.totalGcWeight;
                        gcd.totalDryWeight = obj.totalDryWeight;
                        gcd.totalWetWeight = obj.totalWetWeight;
                        gcd.batteryStatus = obj.batteryStatus;
                        gcd.Distance = Convert.ToDouble(obj.Distance); //Convert.ToDouble(distCount);


                        //if (AppId == 1010)
                        //{
                        //    gcd.locAddresss = Address(obj.Lat + "," + obj.Long);
                        //}
                        //else
                        //{
                        //    gcd.locAddresss = addre;
                        //}

                        gcd.locAddresss = addre;
                        gcd.CreatedDate = DateTime.Now;

                        /////////////////////////////////////////////////////////////
                        //GarbageCollectionDetail objdata = new GarbageCollectionDetail();
                        Location loc = new Location();
                        loc.datetime = Dateeee;
                        loc.lat = obj.Lat;
                        loc._long = obj.Long;
                        loc.address = addre; //Address(objdata.Lat + "," + objdata.Long);
                        loc.batteryStatus = obj.batteryStatus;

                        if (addre != "")
                        {
                            loc.area = area(loc.address);
                        }
                        else
                        {
                            loc.area = "";
                        }

                        loc.userId = obj.userId;
                        loc.type = 1;
                        //loc.IsOffline = obj.IsOffline;
                        loc.Distnace = obj.Distance;

                        if (!string.IsNullOrEmpty(obj.dyId))
                        {
                            loc.ReferanceID = obj.dyId;
                        }

                        loc.CreatedDate = DateTime.Now;
                        db.Locations.Add(loc);

                        /////////////////////////////////////////////////////////////

                        await db.SaveChangesAsync();

                        result.ID = obj.OfflineID;
                        result.status = "success";
                        result.message = "Uploaded successfully";
                        result.messageMar = "सबमिट यशस्वी";

                        //string mes = "नमस्कार! आपल्या घरून कचरा संकलित करण्यात आलेला आहे. कृपया ओला व सुका असा वर्गीकृत केलेला कचरा सफाई कर्मचाऱ्यास सुपूर्द करून सहकार्य करावे धन्यवाद. " + appdetails.yoccContact + " आपल्या सेवेशी " + appdetails.AppName_mar + "";
                        //if (housemob != "")
                        //{
                        //    sendSMS(mes, housemob);
                        //}
                    }
                    return result;

                }

                catch (Exception ex)
                {
                    _logger.LogError(ex.ToString(), ex);
                    //result.message = "Something is wrong,Try Again.. ";
                    result.message = ex.Message;
                    result.messageMar = "काहीतरी चुकीचे आहे, पुन्हा प्रयत्न करा..";
                    result.status = "error";
                    return result;
                }

            }
            //}
        }

        public async Task<CollectionSyncResult> SaveDumpCollectionSyncForLiquidAsync(SBGarbageCollectionView obj, int AppId, int type)
        {

            CollectionSyncResult result = new CollectionSyncResult();

            //using (new TransactionScope(
            //         TransactionScopeOption.Required,
            //         new TransactionOptions
            //         {
            //             IsolationLevel = IsolationLevel.ReadUncommitted
            //         }))
            //{
            using (DevICTSBMChildEntities db = new DevICTSBMChildEntities(AppId))
            using (DevICTSBMMainEntities dbMain = new DevICTSBMMainEntities())
            {
                var appdetails = await dbMain.AppDetails.Where(c => c.AppId == AppId).FirstOrDefaultAsync();
                // GarbageCollectionDetail gcd = new GarbageCollectionDetail();
                string name = "", housemob = "", nameMar = "", addre = "";
                // var distCount = "";
                DateTime Dateeee = Convert.ToDateTime(obj.gcDate);
                DateTime newTime = Dateeee;
                DateTime oldTime;
                TimeSpan span = TimeSpan.Zero;
                var dydetails = await db.DumpYardDetails.Where(c => c.ReferanceId == obj.dyId).FirstOrDefaultAsync();
                //var dyId = dydetails.dyId; || tdate.AddMinutes(15) >= gcd.gcDate

                try
                {
                    var gcd = await db.GarbageCollectionDetails.Where(c => c.userId == obj.userId && c.dyId == dydetails.dyId && EF.Functions.DateDiffDay(c.gcDate, Dateeee) == 0).OrderByDescending(c => c.gcDate).FirstOrDefaultAsync();
                    if (gcd != null)
                    {
                        oldTime = gcd.gcDate.Value;
                        span = newTime.Subtract(oldTime);
                    }

                    if (gcd == null || span.Minutes >= 10)
                    {
                        GarbageCollectionDetail objdata = new GarbageCollectionDetail();
                        objdata.userId = obj.userId;
                        objdata.gcDate = Dateeee;
                        objdata.Lat = obj.Lat;
                        objdata.Long = obj.Long;

                        //var atten = db.Daily_Attendance.Where(c => c.userId == obj.userId & c.endTime == "" & c.daDate == EntityFunctions.TruncateTime(Dateeee)).FirstOrDefault();

                        var atten = await db.Daily_Attendances.Where(c => c.userId == obj.userId && EF.Functions.DateDiffDay(c.daDate, Dateeee) == 0).FirstOrDefaultAsync();

                        if (atten == null)
                        {
                            result.isAttendenceOff = true;
                            result.message = "Your duty is currently off, please start again.. ";
                            result.messageMar = "आपली ड्यूटी सध्या बंद आहे, कृपया पुन्हा सुरू करा..";
                            result.status = "success";
                            result.ID = obj.OfflineID;
                            return result;
                        }
                        else { result.isAttendenceOff = false; }

                        if (obj.dyId != null && obj.dyId != "")
                        {
                            try
                            {
                                var gpdetails = await db.DumpYardDetails.Where(c => c.ReferanceId == obj.dyId).FirstOrDefaultAsync();
                                objdata.dyId = gpdetails.dyId;
                                name = gpdetails.dyName;
                                nameMar = checkNull(gpdetails.dyNameMar);
                                housemob = "";
                                addre = checkNull(gpdetails.dyAddress);

                                var IsSameDumpRecord = await db.GarbageCollectionDetails.Where(a => a.gpId == gpdetails.dyId && a.userId == obj.userId && a.gcDate == Dateeee).FirstOrDefaultAsync();

                                if (IsSameDumpRecord != null)
                                {
                                    result.ID = obj.OfflineID;
                                    result.status = "success";
                                    result.message = "Uploaded successfully";
                                    result.messageMar = "सबमिट यशस्वी";
                                    return result;
                                }
                            }
                            catch (Exception ex)
                            {
                                //_logger.LogError(ex.ToString(), ex);
                                result.ID = obj.OfflineID;
                                result.message = "Invalid dyId"; result.messageMar = "अवैध जीपी आयडी";
                                result.status = "error";
                                return result;
                            }

                        }
                        objdata.gcType = obj.gcType;
                        if (obj.gpBeforImage == null)
                        {
                            obj.gpBeforImage = "";
                        }
                        if (obj.gpAfterImage == null)
                        {
                            obj.gpAfterImage = "";
                        }
                        objdata.gpBeforImage = obj.gpBeforImage;
                        objdata.gpAfterImage = obj.gpAfterImage;
                        objdata.note = checkNull(obj.note);
                        //objdata.garbageType = checkIntNull(obj.garbageType.ToString());
                        objdata.vehicleNumber = checkNull(obj.vehicleNumber);
                        objdata.totalGcWeight = obj.totalGcWeight;
                        objdata.totalDryWeight = obj.totalDryWeight;
                        objdata.totalWetWeight = obj.totalWetWeight;
                        objdata.batteryStatus = obj.batteryStatus;
                        objdata.Distance = Convert.ToDouble(obj.Distance);  //Convert.ToDouble(distCount);

                        //if (AppId == 1010)
                        //{
                        //    objdata.locAddresss = Address(objdata.Lat + "," + objdata.Long);
                        //}
                        //else
                        //{
                        //    objdata.locAddresss = addre;
                        //}

                        objdata.locAddresss = addre;
                        objdata.CreatedDate = DateTime.Now;
                        objdata.EmployeeType = "L";
                        db.GarbageCollectionDetails.Add(objdata);

                        Location loc = new Location();
                        loc.datetime = Dateeee;
                        loc.lat = objdata.Lat;
                        loc._long = objdata.Long;
                        loc.address = objdata.locAddresss;//Address(objdata.Lat + "," + objdata.Long);
                        loc.batteryStatus = objdata.batteryStatus;
                        if (objdata.locAddresss != "")
                        { loc.area = area(loc.address); }
                        else
                        {
                            loc.area = "";
                        }
                        loc.userId = objdata.userId;
                        loc.type = 1;
                        loc.Distnace = obj.Distance;
                        //loc.IsOffline = obj.IsOffline;

                        if (!string.IsNullOrEmpty(obj.dyId))
                        {
                            loc.ReferanceID = obj.dyId;
                        }

                        loc.CreatedDate = DateTime.Now;
                        loc.EmployeeType = "L";
                        db.Locations.Add(loc);
                        await db.SaveChangesAsync();

                        result.ID = obj.OfflineID;
                        result.status = "success";
                        result.message = "Uploaded successfully";
                        result.messageMar = "सबमिट यशस्वी";
                        //string mes = "नमस्कार! आपल्या घरून कचरा संकलित करण्यात आलेला आहे. कृपया ओला व सुका असा वर्गीकृत केलेला कचरा सफाई कर्मचाऱ्यास सुपूर्द करून सहकार्य करावे धन्यवाद. " + appdetails.yoccContact + " आपल्या सेवेशी " + appdetails.AppName_mar + "";
                        //if (housemob != "")
                        //{
                        //    sendSMS(mes, housemob);
                        //}
                    }
                    else
                    {
                        // GarbageCollectionDetail objdata = new GarbageCollectionDetail();
                        gcd.userId = obj.userId;
                        gcd.gcDate = Dateeee;
                        gcd.Lat = obj.Lat;
                        gcd.Long = obj.Long;
                        var atten = await db.Daily_Attendances.Where(c => c.userId == obj.userId && EF.Functions.DateDiffDay(c.daDate, Dateeee) == 0).FirstOrDefaultAsync();

                        if (atten == null)
                        {
                            result.ID = obj.OfflineID;
                            result.isAttendenceOff = true;
                            result.message = "Your duty is currently off, please start again.. ";
                            result.messageMar = "आपली ड्यूटी सध्या बंद आहे, कृपया पुन्हा सुरू करा..";
                            result.status = "success";
                            return result;
                        }
                        else { result.isAttendenceOff = false; }

                        if (obj.dyId != null && obj.dyId != "")
                        {
                            try
                            {
                                var gpdetails = await db.DumpYardDetails.Where(c => c.ReferanceId == obj.dyId).FirstOrDefaultAsync();
                                gcd.dyId = gpdetails.dyId;
                                name = gpdetails.dyName;
                                nameMar = checkNull(gpdetails.dyNameMar);
                                housemob = "";
                                addre = checkNull(gpdetails.dyAddress);
                            }
                            catch (Exception ex)
                            {
                                //_logger.LogError(ex.ToString(), ex);
                                result.ID = obj.OfflineID;
                                result.message = "Invalid dyId"; result.messageMar = "अवैध डीवाय आयडी";
                                result.status = "error";
                                return result;
                            }

                        }
                        gcd.gcType = obj.gcType;
                        if (obj.gpBeforImage == null)
                        {
                            obj.gpBeforImage = "";
                        }
                        if (obj.gpAfterImage == null)
                        {
                            obj.gpAfterImage = "";
                        }
                        gcd.gpBeforImage = obj.gpBeforImage;
                        gcd.gpAfterImage = obj.gpAfterImage;
                        gcd.note = checkNull(gcd.note);
                        //objdata.garbageType = checkIntNull(obj.garbageType.ToString());
                        gcd.vehicleNumber = checkNull(gcd.vehicleNumber);
                        gcd.totalGcWeight = obj.totalGcWeight;
                        gcd.totalDryWeight = obj.totalDryWeight;
                        gcd.totalWetWeight = obj.totalWetWeight;
                        gcd.batteryStatus = obj.batteryStatus;
                        gcd.Distance = Convert.ToDouble(obj.Distance); //Convert.ToDouble(distCount);
                        gcd.EmployeeType = "L";

                        //if (AppId == 1010)
                        //{
                        //    gcd.locAddresss = Address(obj.Lat + "," + obj.Long);
                        //}
                        //else
                        //{
                        //    gcd.locAddresss = addre;
                        //}

                        gcd.locAddresss = addre;
                        gcd.CreatedDate = DateTime.Now;

                        /////////////////////////////////////////////////////////////
                        //GarbageCollectionDetail objdata = new GarbageCollectionDetail();
                        Location loc = new Location();
                        loc.datetime = Dateeee;
                        loc.lat = obj.Lat;
                        loc._long = obj.Long;
                        loc.address = addre; //Address(objdata.Lat + "," + objdata.Long);
                        loc.batteryStatus = obj.batteryStatus;

                        if (addre != "")
                        {
                            loc.area = area(loc.address);
                        }
                        else
                        {
                            loc.area = "";
                        }

                        loc.userId = obj.userId;
                        loc.type = 1;
                        //loc.IsOffline = obj.IsOffline;
                        loc.Distnace = obj.Distance;

                        if (!string.IsNullOrEmpty(obj.dyId))
                        {
                            loc.ReferanceID = obj.dyId;
                        }

                        loc.CreatedDate = DateTime.Now;
                        loc.EmployeeType = "L";
                        db.Locations.Add(loc);

                        /////////////////////////////////////////////////////////////

                        await db.SaveChangesAsync();

                        result.ID = obj.OfflineID;
                        result.status = "success";
                        result.message = "Uploaded successfully";
                        result.messageMar = "सबमिट यशस्वी";

                        //string mes = "नमस्कार! आपल्या घरून कचरा संकलित करण्यात आलेला आहे. कृपया ओला व सुका असा वर्गीकृत केलेला कचरा सफाई कर्मचाऱ्यास सुपूर्द करून सहकार्य करावे धन्यवाद. " + appdetails.yoccContact + " आपल्या सेवेशी " + appdetails.AppName_mar + "";
                        //if (housemob != "")
                        //{
                        //    sendSMS(mes, housemob);
                        //}
                    }
                    return result;

                }

                catch (Exception ex)
                {
                    _logger.LogError(ex.ToString(), ex);
                    // result.message = "Something is wrong,Try Again.. ";
                    result.message = ex.Message;
                    result.messageMar = "काहीतरी चुकीचे आहे, पुन्हा प्रयत्न करा..";
                    result.status = "error";
                    return result;
                }

            }
            //}
        }

        public async Task<CollectionSyncResult> SaveDumpCollectionSyncForStreetAsync(SBGarbageCollectionView obj, int AppId, int type)
        {

            CollectionSyncResult result = new CollectionSyncResult();

            //using (new TransactionScope(
            //         TransactionScopeOption.Required,
            //         new TransactionOptions
            //         {
            //             IsolationLevel = IsolationLevel.ReadUncommitted
            //         }))
            //{
            using (DevICTSBMChildEntities db = new DevICTSBMChildEntities(AppId))
            using (DevICTSBMMainEntities dbMain = new DevICTSBMMainEntities())
            {
                var appdetails = await dbMain.AppDetails.Where(c => c.AppId == AppId).FirstOrDefaultAsync();
                // GarbageCollectionDetail gcd = new GarbageCollectionDetail();
                string name = "", housemob = "", nameMar = "", addre = "";
                // var distCount = "";
                DateTime Dateeee = Convert.ToDateTime(obj.gcDate);
                DateTime newTime = Dateeee;
                DateTime oldTime;
                TimeSpan span = TimeSpan.Zero;
                var dydetails = await db.DumpYardDetails.Where(c => c.ReferanceId == obj.dyId).FirstOrDefaultAsync();
                //var dyId = dydetails.dyId; || tdate.AddMinutes(15) >= gcd.gcDate

                try
                {
                    var gcd = await db.GarbageCollectionDetails.Where(c => c.userId == obj.userId && c.dyId == dydetails.dyId && EF.Functions.DateDiffDay(c.gcDate, Dateeee) == 0).OrderByDescending(c => c.gcDate).FirstOrDefaultAsync();
                    if (gcd != null)
                    {
                        oldTime = gcd.gcDate.Value;
                        span = newTime.Subtract(oldTime);
                    }

                    if (gcd == null || span.Minutes >= 10)
                    {
                        GarbageCollectionDetail objdata = new GarbageCollectionDetail();
                        objdata.userId = obj.userId;
                        objdata.gcDate = Dateeee;
                        objdata.Lat = obj.Lat;
                        objdata.Long = obj.Long;

                        //var atten = db.Daily_Attendance.Where(c => c.userId == obj.userId & c.endTime == "" & c.daDate == EntityFunctions.TruncateTime(Dateeee)).FirstOrDefault();

                        var atten = await db.Daily_Attendances.Where(c => c.userId == obj.userId && EF.Functions.DateDiffDay(c.daDate, Dateeee) == 0).FirstOrDefaultAsync();

                        if (atten == null)
                        {
                            result.isAttendenceOff = true;
                            result.message = "Your duty is currently off, please start again.. ";
                            result.messageMar = "आपली ड्यूटी सध्या बंद आहे, कृपया पुन्हा सुरू करा..";
                            result.status = "success";
                            result.ID = obj.OfflineID;
                            return result;
                        }
                        else { result.isAttendenceOff = false; }

                        if (obj.dyId != null && obj.dyId != "")
                        {
                            try
                            {
                                var gpdetails = await db.DumpYardDetails.Where(c => c.ReferanceId == obj.dyId).FirstOrDefaultAsync();
                                objdata.dyId = gpdetails.dyId;
                                name = gpdetails.dyName;
                                nameMar = checkNull(gpdetails.dyNameMar);
                                housemob = "";
                                addre = checkNull(gpdetails.dyAddress);

                                var IsSameDumpRecord = await db.GarbageCollectionDetails.Where(a => a.gpId == gpdetails.dyId && a.userId == obj.userId && a.gcDate == Dateeee).FirstOrDefaultAsync();

                                if (IsSameDumpRecord != null)
                                {
                                    result.ID = obj.OfflineID;
                                    result.status = "success";
                                    result.message = "Uploaded successfully";
                                    result.messageMar = "सबमिट यशस्वी";
                                    return result;
                                }
                            }
                            catch (Exception ex)
                            {
                                //_logger.LogError(ex.ToString(), ex);
                                result.ID = obj.OfflineID;
                                result.message = "Invalid dyId"; result.messageMar = "अवैध जीपी आयडी";
                                result.status = "error";
                                return result;
                            }

                        }
                        objdata.gcType = obj.gcType;
                        if (obj.gpBeforImage == null)
                        {
                            obj.gpBeforImage = "";
                        }
                        if (obj.gpAfterImage == null)
                        {
                            obj.gpAfterImage = "";
                        }
                        objdata.gpBeforImage = obj.gpBeforImage;
                        objdata.gpAfterImage = obj.gpAfterImage;
                        objdata.note = checkNull(obj.note);
                        //objdata.garbageType = checkIntNull(obj.garbageType.ToString());
                        objdata.vehicleNumber = checkNull(obj.vehicleNumber);
                        objdata.totalGcWeight = obj.totalGcWeight;
                        objdata.totalDryWeight = obj.totalDryWeight;
                        objdata.totalWetWeight = obj.totalWetWeight;
                        objdata.batteryStatus = obj.batteryStatus;
                        objdata.Distance = Convert.ToDouble(obj.Distance);  //Convert.ToDouble(distCount);

                        //if (AppId == 1010)
                        //{
                        //    objdata.locAddresss = Address(objdata.Lat + "," + objdata.Long);
                        //}
                        //else
                        //{
                        //    objdata.locAddresss = addre;
                        //}

                        objdata.locAddresss = addre;
                        objdata.CreatedDate = DateTime.Now;
                        objdata.EmployeeType = "S";
                        db.GarbageCollectionDetails.Add(objdata);

                        Location loc = new Location();
                        loc.datetime = Dateeee;
                        loc.lat = objdata.Lat;
                        loc._long = objdata.Long;
                        loc.address = objdata.locAddresss;//Address(objdata.Lat + "," + objdata.Long);
                        loc.batteryStatus = objdata.batteryStatus;
                        if (objdata.locAddresss != "")
                        { loc.area = area(loc.address); }
                        else
                        {
                            loc.area = "";
                        }
                        loc.userId = objdata.userId;
                        loc.type = 1;
                        loc.Distnace = obj.Distance;
                        //loc.IsOffline = obj.IsOffline;

                        if (!string.IsNullOrEmpty(obj.dyId))
                        {
                            loc.ReferanceID = obj.dyId;
                        }

                        loc.CreatedDate = DateTime.Now;
                        loc.EmployeeType = "S";
                        db.Locations.Add(loc);
                        await db.SaveChangesAsync();

                        result.ID = obj.OfflineID;
                        result.status = "success";
                        result.message = "Uploaded successfully";
                        result.messageMar = "सबमिट यशस्वी";
                        //string mes = "नमस्कार! आपल्या घरून कचरा संकलित करण्यात आलेला आहे. कृपया ओला व सुका असा वर्गीकृत केलेला कचरा सफाई कर्मचाऱ्यास सुपूर्द करून सहकार्य करावे धन्यवाद. " + appdetails.yoccContact + " आपल्या सेवेशी " + appdetails.AppName_mar + "";
                        //if (housemob != "")
                        //{
                        //    sendSMS(mes, housemob);
                        //}
                    }
                    else
                    {
                        // GarbageCollectionDetail objdata = new GarbageCollectionDetail();
                        gcd.userId = obj.userId;
                        gcd.gcDate = Dateeee;
                        gcd.Lat = obj.Lat;
                        gcd.Long = obj.Long;
                        var atten = await db.Daily_Attendances.Where(c => c.userId == obj.userId && EF.Functions.DateDiffDay(c.daDate, Dateeee) == 0).FirstOrDefaultAsync();

                        if (atten == null)
                        {
                            result.ID = obj.OfflineID;
                            result.isAttendenceOff = true;
                            result.message = "Your duty is currently off, please start again.. ";
                            result.messageMar = "आपली ड्यूटी सध्या बंद आहे, कृपया पुन्हा सुरू करा..";
                            result.status = "success";
                            return result;
                        }
                        else { result.isAttendenceOff = false; }

                        if (obj.dyId != null && obj.dyId != "")
                        {
                            try
                            {
                                var gpdetails = await db.DumpYardDetails.Where(c => c.ReferanceId == obj.dyId).FirstOrDefaultAsync();
                                gcd.dyId = gpdetails.dyId;
                                name = gpdetails.dyName;
                                nameMar = checkNull(gpdetails.dyNameMar);
                                housemob = "";
                                addre = checkNull(gpdetails.dyAddress);
                            }
                            catch (Exception ex)
                            {
                                //_logger.LogError(ex.ToString(), ex);
                                result.ID = obj.OfflineID;
                                result.message = "Invalid dyId"; result.messageMar = "अवैध डीवाय आयडी";
                                result.status = "error";
                                return result;
                            }

                        }
                        gcd.gcType = obj.gcType;
                        if (obj.gpBeforImage == null)
                        {
                            obj.gpBeforImage = "";
                        }
                        if (obj.gpAfterImage == null)
                        {
                            obj.gpAfterImage = "";
                        }
                        gcd.gpBeforImage = obj.gpBeforImage;
                        gcd.gpAfterImage = obj.gpAfterImage;
                        gcd.note = checkNull(gcd.note);
                        //objdata.garbageType = checkIntNull(obj.garbageType.ToString());
                        gcd.vehicleNumber = checkNull(gcd.vehicleNumber);
                        gcd.totalGcWeight = obj.totalGcWeight;
                        gcd.totalDryWeight = obj.totalDryWeight;
                        gcd.totalWetWeight = obj.totalWetWeight;
                        gcd.batteryStatus = obj.batteryStatus;
                        gcd.Distance = Convert.ToDouble(obj.Distance); //Convert.ToDouble(distCount);
                        gcd.EmployeeType = "S";

                        //if (AppId == 1010)
                        //{
                        //    gcd.locAddresss = Address(obj.Lat + "," + obj.Long);
                        //}
                        //else
                        //{
                        //    gcd.locAddresss = addre;
                        //}

                        gcd.locAddresss = addre;
                        gcd.CreatedDate = DateTime.Now;

                        /////////////////////////////////////////////////////////////
                        //GarbageCollectionDetail objdata = new GarbageCollectionDetail();
                        Location loc = new Location();
                        loc.datetime = Dateeee;
                        loc.lat = obj.Lat;
                        loc._long = obj.Long;
                        loc.address = addre; //Address(objdata.Lat + "," + objdata.Long);
                        loc.batteryStatus = obj.batteryStatus;

                        if (addre != "")
                        {
                            loc.area = area(loc.address);
                        }
                        else
                        {
                            loc.area = "";
                        }

                        loc.userId = obj.userId;
                        loc.type = 1;
                        //loc.IsOffline = obj.IsOffline;
                        loc.Distnace = obj.Distance;

                        if (!string.IsNullOrEmpty(obj.dyId))
                        {
                            loc.ReferanceID = obj.dyId;
                        }

                        loc.CreatedDate = DateTime.Now;
                        loc.EmployeeType = "S";
                        db.Locations.Add(loc);

                        /////////////////////////////////////////////////////////////

                        await db.SaveChangesAsync();

                        result.ID = obj.OfflineID;
                        result.status = "success";
                        result.message = "Uploaded successfully";
                        result.messageMar = "सबमिट यशस्वी";

                        //string mes = "नमस्कार! आपल्या घरून कचरा संकलित करण्यात आलेला आहे. कृपया ओला व सुका असा वर्गीकृत केलेला कचरा सफाई कर्मचाऱ्यास सुपूर्द करून सहकार्य करावे धन्यवाद. " + appdetails.yoccContact + " आपल्या सेवेशी " + appdetails.AppName_mar + "";
                        //if (housemob != "")
                        //{
                        //    sendSMS(mes, housemob);
                        //}
                    }
                    return result;

                }

                catch (Exception ex)
                {
                    _logger.LogError(ex.ToString(), ex);
                    // result.message = "Something is wrong,Try Again.. ";
                    result.message = ex.Message;
                    result.messageMar = "काहीतरी चुकीचे आहे, पुन्हा प्रयत्न करा..";
                    result.status = "error";
                    return result;
                }

            }
            //}
        }
        public async Task<CollectionSyncResult> SaveLiquidCollectionSyncAsync(SBGarbageCollectionView obj, int AppId, int type)
        {

            CollectionSyncResult result = new CollectionSyncResult();

            //using (new TransactionScope(
            //         TransactionScopeOption.Required,
            //         new TransactionOptions
            //         {
            //             IsolationLevel = IsolationLevel.ReadUncommitted
            //         }))
            //{
            using (DevICTSBMChildEntities db = new DevICTSBMChildEntities(AppId))
            using (DevICTSBMMainEntities dbMain = new DevICTSBMMainEntities())
            {
                var appdetails = await dbMain.AppDetails.Where(c => c.AppId == AppId).FirstOrDefaultAsync();
                // GarbageCollectionDetail gcd = new GarbageCollectionDetail();
                string name = "", housemob = "", nameMar = "", addre = "";
                // var distCount = "";
                DateTime Dateeee = Convert.ToDateTime(obj.gcDate);
                DateTime newTime = Dateeee;
                DateTime oldTime;
                TimeSpan span = TimeSpan.Zero;
                var dydetails = await db.LiquidWasteDetails.Where(c => c.ReferanceId == obj.LWId).FirstOrDefaultAsync();
                //var dyId = dydetails.dyId; || tdate.AddMinutes(15) >= gcd.gcDate

                try
                {
                    var gcd = await db.GarbageCollectionDetails.Where(c => c.userId == obj.userId && c.LWId == dydetails.LWId && EF.Functions.DateDiffDay(c.gcDate, Dateeee) == 0).OrderByDescending(c => c.gcDate).FirstOrDefaultAsync();
                    if (gcd != null)
                    {
                        oldTime = gcd.gcDate.Value;
                        span = newTime.Subtract(oldTime);
                    }

                    if (gcd == null)
                    {
                        GarbageCollectionDetail objdata = new GarbageCollectionDetail();
                        objdata.userId = obj.userId;
                        objdata.gcDate = Dateeee;
                        objdata.Lat = obj.Lat;
                        objdata.Long = obj.Long;

                        //var atten = db.Daily_Attendance.Where(c => c.userId == obj.userId & c.endTime == "" & c.daDate == EntityFunctions.TruncateTime(Dateeee)).FirstOrDefault();

                        var atten = await db.Daily_Attendances.Where(c => c.userId == obj.userId && EF.Functions.DateDiffDay(c.daDate, Dateeee) == 0 && c.EmployeeType == "L").FirstOrDefaultAsync();

                        if (atten == null)
                        {
                            result.isAttendenceOff = true;
                            result.message = "Your duty is currently off, please start again.. ";
                            result.messageMar = "आपली ड्यूटी सध्या बंद आहे, कृपया पुन्हा सुरू करा..";
                            result.status = "success";
                            result.ID = obj.OfflineID;
                            return result;
                        }
                        else { result.isAttendenceOff = false; }

                        if (obj.LWId != null && obj.LWId != "")
                        {
                            try
                            {
                                var gpdetails = await db.LiquidWasteDetails.Where(c => c.ReferanceId == obj.LWId).FirstOrDefaultAsync();
                                objdata.LWId = gpdetails.LWId;
                                name = gpdetails.LWName;
                                nameMar = checkNull(gpdetails.LWNameMar);
                                housemob = "";
                                addre = checkNull(gpdetails.LWAddreLW);

                                var IsSameLiquidRecord = await db.GarbageCollectionDetails.Where(a => a.gpId == gpdetails.LWId && a.userId == obj.userId && a.gcDate == Dateeee).FirstOrDefaultAsync();

                                if (IsSameLiquidRecord != null)
                                {
                                    result.ID = obj.OfflineID;
                                    result.status = "success";
                                    result.message = "Uploaded successfully";
                                    result.messageMar = "सबमिट यशस्वी";
                                    return result;
                                }
                            }
                            catch
                            {
                                result.ID = obj.OfflineID;
                                result.message = "Invalid LWId"; result.messageMar = "अवैध जीपी आयडी";
                                result.status = "error";
                                return result;
                            }

                        }
                        objdata.gcType = obj.gcType;
                        objdata.gpBeforImage = obj.gpBeforImage;
                        objdata.gpAfterImage = obj.gpAfterImage;
                        objdata.note = checkNull(obj.note);
                        //objdata.garbageType = checkIntNull(obj.garbageType.ToString());
                        objdata.vehicleNumber = checkNull(obj.vehicleNumber);
                        objdata.totalGcWeight = obj.totalGcWeight;
                        objdata.totalDryWeight = obj.totalDryWeight;
                        objdata.totalWetWeight = obj.totalWetWeight;
                        objdata.batteryStatus = obj.batteryStatus;
                        objdata.Distance = Convert.ToDouble(obj.Distance);  //Convert.ToDouble(distCount);

                        //if (AppId == 1010)
                        //{
                        //    objdata.locAddresss = Address(objdata.Lat + "," + objdata.Long);
                        //}
                        //else
                        //{
                        //    objdata.locAddresss = addre;
                        //}

                        objdata.locAddresss = addre;
                        objdata.CreatedDate = DateTime.Now;
                        objdata.EmployeeType = "L";
                        db.GarbageCollectionDetails.Add(objdata);

                        Location loc = new Location();
                        loc.datetime = Dateeee;
                        loc.lat = objdata.Lat;
                        loc._long = objdata.Long;
                        loc.address = objdata.locAddresss;//Address(objdata.Lat + "," + objdata.Long);
                        loc.batteryStatus = objdata.batteryStatus;
                        if (objdata.locAddresss != "")
                        { loc.area = area(loc.address); }
                        else
                        {
                            loc.area = "";
                        }
                        loc.userId = objdata.userId;
                        loc.type = 1;
                        loc.Distnace = obj.Distance;
                        //loc.IsOffline = obj.IsOffline;

                        if (!string.IsNullOrEmpty(obj.dyId))
                        {
                            loc.ReferanceID = obj.dyId;
                        }

                        loc.CreatedDate = DateTime.Now;
                        loc.EmployeeType = "L";
                        db.Locations.Add(loc);
                        await db.SaveChangesAsync();

                        result.ID = obj.OfflineID;
                        result.status = "success";
                        result.message = "Uploaded successfully";
                        result.messageMar = "सबमिट यशस्वी";
                        //string mes = "नमस्कार! आपल्या घरून कचरा संकलित करण्यात आलेला आहे. कृपया ओला व सुका असा वर्गीकृत केलेला कचरा सफाई कर्मचाऱ्यास सुपूर्द करून सहकार्य करावे धन्यवाद. " + appdetails.yoccContact + " आपल्या सेवेशी " + appdetails.AppName_mar + "";
                        //if (housemob != "")
                        //{
                        //    sendSMS(mes, housemob);
                        //}
                    }
                    else
                    {
                        // GarbageCollectionDetail objdata = new GarbageCollectionDetail();
                        gcd.userId = obj.userId;
                        gcd.gcDate = Dateeee;
                        gcd.Lat = obj.Lat;
                        gcd.Long = obj.Long;
                        var atten = await db.Daily_Attendances.Where(c => c.userId == obj.userId && EF.Functions.DateDiffDay(c.daDate, Dateeee) == 0 && c.EmployeeType == "L").FirstOrDefaultAsync();

                        if (atten == null)
                        {
                            result.ID = obj.OfflineID;
                            result.isAttendenceOff = true;
                            result.message = "Your duty is currently off, please start again.. ";
                            result.messageMar = "आपली ड्यूटी सध्या बंद आहे, कृपया पुन्हा सुरू करा..";
                            result.status = "success";
                            return result;
                        }
                        else { result.isAttendenceOff = false; }

                        if (obj.LWId != null && obj.LWId != "")
                        {
                            try
                            {
                                var gpdetails = await db.LiquidWasteDetails.Where(c => c.ReferanceId == obj.LWId).FirstOrDefaultAsync();
                                gcd.LWId = gpdetails.LWId;
                                name = gpdetails.LWName;
                                nameMar = checkNull(gpdetails.LWNameMar);
                                housemob = "";
                                addre = checkNull(gpdetails.LWAddreLW);
                            }
                            catch
                            {
                                result.ID = obj.OfflineID;
                                result.message = "Invalid LWId"; result.messageMar = "अवैध डीवाय आयडी";
                                result.status = "error";
                                return result;
                            }

                        }
                        gcd.gcType = obj.gcType;
                        gcd.gpBeforImage = obj.gpBeforImage;
                        gcd.gpAfterImage = obj.gpAfterImage;
                        gcd.note = checkNull(gcd.note);
                        //objdata.garbageType = checkIntNull(obj.garbageType.ToString());
                        gcd.vehicleNumber = checkNull(gcd.vehicleNumber);
                        gcd.totalGcWeight = obj.totalGcWeight;
                        gcd.totalDryWeight = obj.totalDryWeight;
                        gcd.totalWetWeight = obj.totalWetWeight;
                        gcd.batteryStatus = obj.batteryStatus;
                        gcd.Distance = Convert.ToDouble(obj.Distance); //Convert.ToDouble(distCount);


                        //if (AppId == 1010)
                        //{
                        //    gcd.locAddresss = Address(obj.Lat + "," + obj.Long);
                        //}
                        //else
                        //{
                        //    gcd.locAddresss = addre;
                        //}

                        gcd.locAddresss = addre;
                        gcd.CreatedDate = DateTime.Now;

                        /////////////////////////////////////////////////////////////
                        //GarbageCollectionDetail objdata = new GarbageCollectionDetail();
                        Location loc = new Location();
                        loc.datetime = Dateeee;
                        loc.lat = obj.Lat;
                        loc._long = obj.Long;
                        loc.address = addre; //Address(objdata.Lat + "," + objdata.Long);
                        loc.batteryStatus = obj.batteryStatus;

                        if (addre != "")
                        {
                            loc.area = area(loc.address);
                        }
                        else
                        {
                            loc.area = "";
                        }

                        loc.userId = obj.userId;
                        loc.type = 1;
                        //loc.IsOffline = obj.IsOffline;
                        loc.Distnace = obj.Distance;

                        if (!string.IsNullOrEmpty(obj.LWId))
                        {
                            loc.ReferanceID = obj.LWId;
                        }

                        loc.CreatedDate = DateTime.Now;
                        loc.EmployeeType = "L";
                        db.Locations.Add(loc);

                        /////////////////////////////////////////////////////////////

                        await db.SaveChangesAsync();

                        result.ID = obj.OfflineID;
                        result.status = "success";
                        result.message = "Uploaded successfully";
                        result.messageMar = "सबमिट यशस्वी";

                        //string mes = "नमस्कार! आपल्या घरून कचरा संकलित करण्यात आलेला आहे. कृपया ओला व सुका असा वर्गीकृत केलेला कचरा सफाई कर्मचाऱ्यास सुपूर्द करून सहकार्य करावे धन्यवाद. " + appdetails.yoccContact + " आपल्या सेवेशी " + appdetails.AppName_mar + "";
                        //if (housemob != "")
                        //{
                        //    sendSMS(mes, housemob);
                        //}
                    }

                    if (result.status == "success")
                    {
                        try
                        {
                            // Update code

                            var updateappdetails = await dbMain.SP_DailyScanCount_Results.FromSqlRaw<SP_DailyScanCount_Result>($"EXEC DailyScanCount {AppId.ToString()}").ToListAsync();


                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex.ToString(), ex);

                        }
                    }

                    return result;

                }

                catch (Exception ex)
                {
                    _logger.LogError(ex.ToString(), ex);
                    //result.message = "Something is wrong,Try Again.. ";
                    result.message = ex.Message;
                    result.messageMar = "काहीतरी चुकीचे आहे, पुन्हा प्रयत्न करा..";
                    result.status = "error";
                    return result;
                }

            }
            //}
        }
        public async Task<CollectionSyncResult> SaveStreetCollectionSyncAsync(SBGarbageCollectionView obj, int AppId, int type)
        {
            int i = 0;
            CollectionSyncResult result = new CollectionSyncResult();

            //using (new TransactionScope(
            //         TransactionScopeOption.Required,
            //         new TransactionOptions
            //         {
            //             IsolationLevel = IsolationLevel.ReadUncommitted
            //         }))
            //{
            using (DevICTSBMChildEntities db = new DevICTSBMChildEntities(AppId))
            using (DevICTSBMMainEntities dbMain = new DevICTSBMMainEntities())
            {
                var appdetails = await dbMain.AppDetails.Where(c => c.AppId == AppId).FirstOrDefaultAsync();
                // GarbageCollectionDetail gcd = new GarbageCollectionDetail();
                string name = "", housemob = "", nameMar = "", addre = "";
                // var distCount = "";
                DateTime Dateeee = Convert.ToDateTime(obj.gcDate);
                DateTime newTime = Dateeee;
                DateTime oldTime;
                TimeSpan span = TimeSpan.Zero;
                var dydetails = await db.StreetSweepingDetails.Where(c => c.ReferanceId == obj.SSId).FirstOrDefaultAsync();
                //var dyId = dydetails.dyId; || tdate.AddMinutes(15) >= gcd.gcDate

                try
                {
                    var gcd = await db.GarbageCollectionDetails.Where(c => c.userId == obj.userId && c.SSId == dydetails.SSId && EF.Functions.DateDiffDay(c.gcDate, Dateeee) == 0).OrderByDescending(c => c.gcDate).FirstOrDefaultAsync();
                    if (gcd != null)
                    {
                        oldTime = gcd.gcDate.Value;
                        span = newTime.Subtract(oldTime);
                    }

                    if (gcd == null)
                    {
                        GarbageCollectionDetail objdata = new GarbageCollectionDetail();
                        objdata.userId = obj.userId;
                        objdata.gcDate = Dateeee;
                        objdata.Lat = obj.Lat;
                        objdata.Long = obj.Long;

                        //var atten = db.Daily_Attendance.Where(c => c.userId == obj.userId & c.endTime == "" & c.daDate == EntityFunctions.TruncateTime(Dateeee)).FirstOrDefault();

                        var atten = await db.Daily_Attendances.Where(c => c.userId == obj.userId && EF.Functions.DateDiffDay(c.daDate, Dateeee) == 0 && c.EmployeeType == "S").FirstOrDefaultAsync();

                        if (atten == null)
                        {
                            result.isAttendenceOff = true;
                            result.message = "Your duty is currently off, please start again.. ";
                            result.messageMar = "आपली ड्यूटी सध्या बंद आहे, कृपया पुन्हा सुरू करा..";
                            result.status = "success";
                            result.ID = obj.OfflineID;
                            return result;
                        }
                        else { result.isAttendenceOff = false; }

                        if (obj.SSId != null && obj.SSId != "")
                        {
                            try
                            {
                                var gpdetails = await db.StreetSweepingDetails.Where(c => c.ReferanceId == obj.SSId).FirstOrDefaultAsync();
                                objdata.SSId = gpdetails.SSId;
                                name = gpdetails.SSName;
                                nameMar = checkNull(gpdetails.SSNameMar);
                                housemob = "";
                                addre = checkNull(gpdetails.SSAddress);

                                var IsSameStreetRecord = await db.GarbageCollectionDetails.Where(a => a.gpId == gpdetails.SSId && a.userId == obj.userId && a.gcDate == Dateeee).FirstOrDefaultAsync();

                                if (IsSameStreetRecord != null)
                                {
                                    result.ID = obj.OfflineID;
                                    result.status = "success";
                                    result.message = "Uploaded successfully";
                                    result.messageMar = "सबमिट यशस्वी";
                                    return result;
                                }
                            }
                            catch
                            {
                                result.ID = obj.OfflineID;
                                result.message = "Invalid SSId"; result.messageMar = "अवैध जीपी आयडी";
                                result.status = "error";
                                return result;
                            }

                        }
                        objdata.gcType = obj.gcType;
                        objdata.gpBeforImage = obj.gpBeforImage;
                        objdata.gpAfterImage = obj.gpAfterImage;
                        objdata.note = checkNull(obj.note);
                        //objdata.garbageType = checkIntNull(obj.garbageType.ToString());
                        objdata.vehicleNumber = checkNull(obj.vehicleNumber);
                        objdata.totalGcWeight = obj.totalGcWeight;
                        objdata.totalDryWeight = obj.totalDryWeight;
                        objdata.totalWetWeight = obj.totalWetWeight;
                        objdata.batteryStatus = obj.batteryStatus;
                        objdata.Distance = Convert.ToDouble(obj.Distance);  //Convert.ToDouble(distCount);

                        //if (AppId == 1010)
                        //{
                        //    objdata.locAddresss = Address(objdata.Lat + "," + objdata.Long);
                        //}
                        //else
                        //{
                        //    objdata.locAddresss = addre;
                        //}

                        objdata.locAddresss = addre;
                        objdata.CreatedDate = DateTime.Now;
                        objdata.EmployeeType = "S";
                        db.GarbageCollectionDetails.Add(objdata);

                        Location loc = new Location();
                        loc.datetime = Dateeee;
                        loc.lat = objdata.Lat;
                        loc._long = objdata.Long;
                        loc.address = objdata.locAddresss;//Address(objdata.Lat + "," + objdata.Long);
                        loc.batteryStatus = objdata.batteryStatus;
                        if (objdata.locAddresss != "")
                        { loc.area = area(loc.address); }
                        else
                        {
                            loc.area = "";
                        }
                        loc.userId = objdata.userId;
                        loc.type = 1;
                        loc.Distnace = obj.Distance;
                        //loc.IsOffline = obj.IsOffline;

                        if (!string.IsNullOrEmpty(obj.dyId))
                        {
                            loc.ReferanceID = obj.dyId;
                        }

                        loc.CreatedDate = DateTime.Now;
                        loc.EmployeeType = "S";
                        db.Locations.Add(loc);
                        await db.SaveChangesAsync();

                        result.ID = obj.OfflineID;
                        result.status = "success";
                        result.message = "Uploaded successfully";
                        result.messageMar = "सबमिट यशस्वी";
                        //string mes = "नमस्कार! आपल्या घरून कचरा संकलित करण्यात आलेला आहे. कृपया ओला व सुका असा वर्गीकृत केलेला कचरा सफाई कर्मचाऱ्यास सुपूर्द करून सहकार्य करावे धन्यवाद. " + appdetails.yoccContact + " आपल्या सेवेशी " + appdetails.AppName_mar + "";
                        //if (housemob != "")
                        //{
                        //    sendSMS(mes, housemob);
                        //}
                    }
                    else
                    {
                        // GarbageCollectionDetail objdata = new GarbageCollectionDetail();
                        gcd.userId = obj.userId;
                        gcd.gcDate = Dateeee;
                        gcd.Lat = obj.Lat;
                        gcd.Long = obj.Long;
                        var atten = await db.Daily_Attendances.Where(c => c.userId == obj.userId && EF.Functions.DateDiffDay(c.daDate, Dateeee) == 0 && c.EmployeeType == "S").FirstOrDefaultAsync();

                        if (atten == null)
                        {
                            result.ID = obj.OfflineID;
                            result.isAttendenceOff = true;
                            result.message = "Your duty is currently off, please start again.. ";
                            result.messageMar = "आपली ड्यूटी सध्या बंद आहे, कृपया पुन्हा सुरू करा..";
                            result.status = "success";
                            return result;
                        }
                        else { result.isAttendenceOff = false; }

                        if (obj.SSId != null && obj.SSId != "")
                        {
                            try
                            {
                                var gpdetails = await db.StreetSweepingDetails.Where(c => c.ReferanceId == obj.SSId).FirstOrDefaultAsync();
                                gcd.SSId = gpdetails.SSId;
                                name = gpdetails.SSName;
                                nameMar = checkNull(gpdetails.SSNameMar);
                                housemob = "";
                                addre = checkNull(gpdetails.SSAddress);
                            }
                            catch
                            {
                                result.ID = obj.OfflineID;
                                result.message = "Invalid SSId"; result.messageMar = "अवैध डीवाय आयडी";
                                result.status = "error";
                                return result;
                            }

                        }
                        gcd.gcType = obj.gcType;
                        gcd.gpBeforImage = obj.gpBeforImage;
                        gcd.gpAfterImage = obj.gpAfterImage;
                        gcd.note = checkNull(obj.note);
                        //objdata.garbageType = checkIntNull(obj.garbageType.ToString());
                        gcd.vehicleNumber = checkNull(gcd.vehicleNumber);
                        gcd.totalGcWeight = obj.totalGcWeight;
                        gcd.totalDryWeight = obj.totalDryWeight;
                        gcd.totalWetWeight = obj.totalWetWeight;
                        gcd.batteryStatus = obj.batteryStatus;
                        gcd.Distance = Convert.ToDouble(obj.Distance); //Convert.ToDouble(distCount);


                        //if (AppId == 1010)
                        //{
                        //    gcd.locAddresss = Address(obj.Lat + "," + obj.Long);
                        //}
                        //else
                        //{
                        //    gcd.locAddresss = addre;
                        //}

                        gcd.locAddresss = addre;
                        gcd.CreatedDate = DateTime.Now;

                        /////////////////////////////////////////////////////////////
                        //GarbageCollectionDetail objdata = new GarbageCollectionDetail();
                        Location loc = new Location();
                        loc.datetime = Dateeee;
                        loc.lat = obj.Lat;
                        loc._long = obj.Long;
                        loc.address = addre; //Address(objdata.Lat + "," + objdata.Long);
                        loc.batteryStatus = obj.batteryStatus;

                        if (addre != "")
                        {
                            loc.area = area(loc.address);
                        }
                        else
                        {
                            loc.area = "";
                        }

                        loc.userId = obj.userId;
                        loc.type = 1;
                        //loc.IsOffline = obj.IsOffline;
                        loc.Distnace = obj.Distance;

                        if (!string.IsNullOrEmpty(obj.LWId))
                        {
                            loc.ReferanceID = obj.LWId;
                        }

                        loc.CreatedDate = DateTime.Now;
                        loc.EmployeeType = "S";
                        db.Locations.Add(loc);

                        /////////////////////////////////////////////////////////////

                        await db.SaveChangesAsync();

                        result.ID = obj.OfflineID;
                        result.status = "success";
                        result.message = "Uploaded successfully";
                        result.messageMar = "सबमिट यशस्वी";


                    }

                    var gc = await db.GarbageCollectionDetails.Where(c => c.userId == obj.userId && c.SSId == dydetails.SSId && EF.Functions.DateDiffDay(c.gcDate, Dateeee) == 0).OrderByDescending(c => c.gcDate).FirstOrDefaultAsync();
                    var sd = await db.StreetSweepingDetails.Where(x => x.SSId == gc.SSId).FirstOrDefaultAsync();
                    var sbeatcount = await db.StreetSweepingBeats.Where(x => x.ReferanceId1 == sd.ReferanceId || x.ReferanceId2 == sd.ReferanceId || x.ReferanceId3 == sd.ReferanceId || x.ReferanceId4 == sd.ReferanceId || x.ReferanceId5 == sd.ReferanceId).FirstOrDefaultAsync();
                    if (sbeatcount != null)
                    {
                        var beatcount = await db.Vw_BitCounts.Where(x => x.BeatId == sbeatcount.BeatId).FirstOrDefaultAsync();
                        var sd1 = await db.StreetSweepingDetails.Where(z => z.ReferanceId == sbeatcount.ReferanceId1 || z.ReferanceId == sbeatcount.ReferanceId2 || z.ReferanceId == sbeatcount.ReferanceId3 || z.ReferanceId == sbeatcount.ReferanceId4 || z.ReferanceId == sbeatcount.ReferanceId5).ToListAsync();
                        foreach (var x in sd1)
                        {
                            var sgcd = await db.GarbageCollectionDetails.Where(c => c.userId == obj.userId && c.SSId == x.SSId && EF.Functions.DateDiffDay(c.gcDate, Dateeee) == 0).OrderByDescending(c => c.gcDate).FirstOrDefaultAsync();
                            if (sgcd != null)
                            {
                                i++;
                            }

                        }

                        if (beatcount.BitCount == i)
                        {
                            result.ID = obj.OfflineID;
                            result.status = "success";
                            result.message = "Street Sweeping Completed Successfully";
                            result.messageMar = "मार्गाची सफाई यशस्वीरित्या पूर्ण झाली";
                        }
                        else
                        {
                            result.ID = obj.OfflineID;
                            result.status = "success";
                            result.message = "Street Sweeping Partially Completed";
                            result.messageMar = "मार्गाची सफाई अर्धवट पूर्ण झाली";
                        }

                    }
                    if (result.status == "success")
                    {
                        try
                        {
                            // Update code
                            //var updateappdetails = dbMain.DailyScanCount(AppId.ToString());
                            var updateappdetails = await dbMain.SP_DailyScanCount_Results.FromSqlRaw<SP_DailyScanCount_Result>($"EXEC DailyScanCount {AppId.ToString()}").ToListAsync();


                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex.ToString(), ex);
                        }
                    }
                    return result;

                }

                catch (Exception ex)
                {
                    _logger.LogError(ex.ToString(), ex);
                    // result.message = "Something is wrong,Try Again.. ";
                    result.message = ex.Message;
                    result.messageMar = "काहीतरी चुकीचे आहे, पुन्हा प्रयत्न करा..";
                    result.status = "error";
                    return result;
                }

            }
            //}
        }
        public async Task<CollectionSyncResult> SaveDumpCollectionSyncForDumpAsync(SBGarbageCollectionView obj, int AppId, int type)
        {

            CollectionSyncResult result = new CollectionSyncResult();

            //using (new TransactionScope(
            //         TransactionScopeOption.Required,
            //         new TransactionOptions
            //         {
            //             IsolationLevel = IsolationLevel.ReadUncommitted
            //         }))
            //{
            using (DevICTSBMChildEntities db = new DevICTSBMChildEntities(AppId))
            using (DevICTSBMMainEntities dbMain = new DevICTSBMMainEntities())
            {
                var appdetails = await dbMain.AppDetails.Where(c => c.AppId == AppId).FirstOrDefaultAsync();
                // GarbageCollectionDetail gcd = new GarbageCollectionDetail();
                string name = "", housemob = "", nameMar = "", addre = "";
                // var distCount = "";
                DateTime Dateeee = Convert.ToDateTime(obj.gcDate);
                DateTime newTime = Dateeee;
                DateTime oldTime;
                TimeSpan span = TimeSpan.Zero;
                var vrdetails = await db.Vehical_QR_Masters.Where(c => c.ReferanceId == obj.vqrId).FirstOrDefaultAsync();
                //var dyId = dydetails.dyId; || tdate.AddMinutes(15) >= gcd.gcDate

                try
                {
                    var gcd = await db.GarbageCollectionDetails.Where(c => c.userId == obj.userId && c.vqrid == vrdetails.vqrId && EF.Functions.DateDiffDay(c.gcDate, c.gcDate) == 0).OrderByDescending(c => c.gcDate).FirstOrDefaultAsync();
                    if (gcd != null)
                    {
                        oldTime = gcd.gcDate.Value;
                        span = newTime.Subtract(oldTime);
                    }

                    if (gcd == null || span.Minutes >= 10)
                    {
                        GarbageCollectionDetail objdata = new GarbageCollectionDetail();
                        objdata.userId = obj.userId;
                        objdata.gcDate = Dateeee;
                        objdata.Lat = obj.Lat;
                        objdata.Long = obj.Long;

                        //var atten = db.Daily_Attendance.Where(c => c.userId == obj.userId & c.endTime == "" & c.daDate == EntityFunctions.TruncateTime(Dateeee)).FirstOrDefault();

                        var atten = await db.Daily_Attendances.Where(c => c.userId == obj.userId && EF.Functions.DateDiffDay(c.daDate, Dateeee) == 0 && c.endTime == "").FirstOrDefaultAsync();

                        if (atten == null)
                        {
                            result.isAttendenceOff = true;
                            result.message = "Your duty is currently off, please start again.. ";
                            result.messageMar = "आपली ड्यूटी सध्या बंद आहे, कृपया पुन्हा सुरू करा..";
                            result.status = "success";
                            result.ID = obj.OfflineID;
                            return result;
                        }
                        else { result.isAttendenceOff = false; }

                        if (obj.vqrId != null && obj.vqrId != "")
                        {
                            try
                            {
                                var gpdetails = await db.Vehical_QR_Masters.Where(c => c.ReferanceId == obj.vqrId).FirstOrDefaultAsync();
                                objdata.vqrid = gpdetails.vqrId;
                                name = gpdetails.VehicalNumber;
                                nameMar = checkNull(gpdetails.VehicalType);
                                housemob = "";
                                //   addre = checkNull(gpdetails.VehicalNumber);

                                var IsSameDumpRecord = await db.GarbageCollectionDetails.Where(a => a.vqrid == gpdetails.vqrId && a.userId == obj.userId && a.gcDate == Dateeee).FirstOrDefaultAsync();

                                if (IsSameDumpRecord != null)
                                {
                                    result.ID = obj.OfflineID;
                                    result.status = "success";
                                    result.message = "Uploaded successfully";
                                    result.messageMar = "सबमिट यशस्वी";
                                    return result;
                                }
                            }
                            catch
                            {
                                result.ID = obj.OfflineID;
                                result.message = "Invalid Vehicle Id"; result.messageMar = "अवैध वाहन आयडी";
                                result.status = "error";
                                return result;
                            }

                        }
                        objdata.gcType = obj.gcType;
                        if (obj.gpBeforImage == null)
                        {
                            obj.gpBeforImage = "";
                        }
                        if (obj.gpAfterImage == null)
                        {
                            obj.gpAfterImage = "";
                        }
                        objdata.gpBeforImage = obj.gpBeforImage;
                        objdata.gpAfterImage = obj.gpAfterImage;
                        objdata.note = checkNull(obj.note);
                        //objdata.garbageType = checkIntNull(obj.garbageType.ToString());
                        objdata.vehicleNumber = checkNull(obj.vehicleNumber);
                        objdata.totalGcWeight = obj.totalGcWeight;
                        objdata.totalDryWeight = obj.totalDryWeight;
                        objdata.totalWetWeight = obj.totalWetWeight;
                        objdata.batteryStatus = obj.batteryStatus;
                        objdata.Distance = Convert.ToDouble(obj.Distance);  //Convert.ToDouble(distCount);

                        //if (AppId == 1010)
                        //{
                        //    objdata.locAddresss = Address(objdata.Lat + "," + objdata.Long);
                        //}
                        //else
                        //{
                        //    objdata.locAddresss = addre;
                        //}

                        objdata.locAddresss = addre;
                        objdata.CreatedDate = DateTime.Now;
                        objdata.EmployeeType = "D";
                        objdata.dyId = atten.dyid;
                        objdata.vqrid = vrdetails.vqrId;
                        db.GarbageCollectionDetails.Add(objdata);

                        Location loc = new Location();
                        loc.datetime = Dateeee;
                        loc.lat = objdata.Lat;
                        loc._long = objdata.Long;
                        loc.address = objdata.locAddresss;//Address(objdata.Lat + "," + objdata.Long);
                        loc.batteryStatus = objdata.batteryStatus;
                        if (objdata.locAddresss != "")
                        { loc.area = area(loc.address); }
                        else
                        {
                            loc.area = "";
                        }
                        loc.userId = objdata.userId;
                        loc.type = 1;
                        loc.Distnace = obj.Distance;
                        //loc.IsOffline = obj.IsOffline;

                        if (!string.IsNullOrEmpty(obj.vqrId))
                        {
                            loc.ReferanceID = obj.vqrId;
                        }

                        loc.CreatedDate = DateTime.Now;
                        loc.EmployeeType = "D";
                        db.Locations.Add(loc);
                        await db.SaveChangesAsync();

                        result.ID = obj.OfflineID;
                        result.status = "success";
                        result.message = "Uploaded successfully";
                        result.messageMar = "सबमिट यशस्वी";
                        //string mes = "नमस्कार! आपल्या घरून कचरा संकलित करण्यात आलेला आहे. कृपया ओला व सुका असा वर्गीकृत केलेला कचरा सफाई कर्मचाऱ्यास सुपूर्द करून सहकार्य करावे धन्यवाद. " + appdetails.yoccContact + " आपल्या सेवेशी " + appdetails.AppName_mar + "";
                        //if (housemob != "")
                        //{
                        //    sendSMS(mes, housemob);
                        //}
                    }
                    else
                    {
                        // GarbageCollectionDetail objdata = new GarbageCollectionDetail();
                        gcd.userId = obj.userId;
                        gcd.gcDate = Dateeee;
                        gcd.Lat = obj.Lat;
                        gcd.Long = obj.Long;
                        var atten = await db.Daily_Attendances.Where(c => c.userId == obj.userId && EF.Functions.DateDiffDay(c.daDate, Dateeee) == 0 && c.endTime == "").FirstOrDefaultAsync();

                        if (atten == null)
                        {
                            result.ID = obj.OfflineID;
                            result.isAttendenceOff = true;
                            result.message = "Your duty is currently off, please start again.. ";
                            result.messageMar = "आपली ड्यूटी सध्या बंद आहे, कृपया पुन्हा सुरू करा..";
                            result.status = "success";
                            return result;
                        }
                        else { result.isAttendenceOff = false; }

                        if (obj.vqrId != null && obj.vqrId != "")
                        {
                            try
                            {
                                var gpdetails = await db.Vehical_QR_Masters.Where(c => c.ReferanceId == obj.vqrId).FirstOrDefaultAsync();
                                gcd.vqrid = gpdetails.vqrId;
                                name = gpdetails.VehicalNumber;
                                nameMar = checkNull(gpdetails.VehicalType);
                                housemob = "";
                                addre = checkNull(gpdetails.VehicalNumber);
                            }
                            catch
                            {
                                result.ID = obj.OfflineID;
                                result.message = "Invalid Vehicle Id"; result.messageMar = "अवैध वाहन आयडी";
                                result.status = "error";
                                return result;
                            }

                        }
                        gcd.gcType = obj.gcType;
                        if (obj.gpBeforImage == null)
                        {
                            obj.gpBeforImage = "";
                        }
                        if (obj.gpAfterImage == null)
                        {
                            obj.gpAfterImage = "";
                        }
                        gcd.gpBeforImage = obj.gpBeforImage;
                        gcd.gpAfterImage = obj.gpAfterImage;
                        gcd.note = checkNull(gcd.note);
                        //objdata.garbageType = checkIntNull(obj.garbageType.ToString());
                        gcd.vehicleNumber = checkNull(gcd.vehicleNumber);
                        gcd.totalGcWeight = obj.totalGcWeight;
                        gcd.totalDryWeight = obj.totalDryWeight;
                        gcd.totalWetWeight = obj.totalWetWeight;
                        gcd.batteryStatus = obj.batteryStatus;
                        gcd.Distance = Convert.ToDouble(obj.Distance); //Convert.ToDouble(distCount);
                        gcd.dyId = atten.dyid;
                        gcd.vqrid = vrdetails.vqrId;
                        gcd.EmployeeType = "D";

                        //if (AppId == 1010)
                        //{
                        //    gcd.locAddresss = Address(obj.Lat + "," + obj.Long);
                        //}
                        //else
                        //{
                        //    gcd.locAddresss = addre;
                        //}

                        gcd.locAddresss = addre;
                        gcd.CreatedDate = DateTime.Now;

                        /////////////////////////////////////////////////////////////
                        //GarbageCollectionDetail objdata = new GarbageCollectionDetail();
                        Location loc = new Location();
                        loc.datetime = Dateeee;
                        loc.lat = obj.Lat;
                        loc._long = obj.Long;
                        loc.address = addre; //Address(objdata.Lat + "," + objdata.Long);
                        loc.batteryStatus = obj.batteryStatus;

                        if (addre != "")
                        {
                            loc.area = area(loc.address);
                        }
                        else
                        {
                            loc.area = "";
                        }

                        loc.userId = obj.userId;
                        loc.type = 1;
                        //loc.IsOffline = obj.IsOffline;
                        loc.Distnace = obj.Distance;

                        if (!string.IsNullOrEmpty(obj.vqrId))
                        {
                            loc.ReferanceID = obj.vqrId;
                        }

                        loc.CreatedDate = DateTime.Now;
                        loc.EmployeeType = "D";
                        db.Locations.Add(loc);

                        /////////////////////////////////////////////////////////////

                        await db.SaveChangesAsync();

                        result.ID = obj.OfflineID;
                        result.status = "success";
                        result.message = "Uploaded successfully";
                        result.messageMar = "सबमिट यशस्वी";

                        //string mes = "नमस्कार! आपल्या घरून कचरा संकलित करण्यात आलेला आहे. कृपया ओला व सुका असा वर्गीकृत केलेला कचरा सफाई कर्मचाऱ्यास सुपूर्द करून सहकार्य करावे धन्यवाद. " + appdetails.yoccContact + " आपल्या सेवेशी " + appdetails.AppName_mar + "";
                        //if (housemob != "")
                        //{
                        //    sendSMS(mes, housemob);
                        //}
                    }
                    return result;

                }

                catch (Exception ex)
                {
                    _logger.LogError(ex.ToString(), ex);
                    // result.message = "Something is wrong,Try Again.. ";
                    result.message = ex.Message;
                    result.messageMar = "काहीतरी चुकीचे आहे, पुन्हा प्रयत्न करा..";
                    result.status = "error";
                    return result;
                }

            }
            //}
        }

        public async Task<CollectionSyncResult> SaveUserLocationOfflineSyncAsync(SBGarbageCollectionView obj, int AppId, int typeId)
        {

            using (DevICTSBMChildEntities db = new DevICTSBMChildEntities(AppId))
            {
                try
                {
                    CollectionSyncResult result = new CollectionSyncResult();
                    DateTime Dateeee = Convert.ToDateTime(obj.gcDate);

                    if (typeId == 0 || typeId == 2)
                    {

                        DateTime newTime = Dateeee;
                        DateTime oldTime;
                        TimeSpan span = TimeSpan.Zero;
                        var gcd = await db.Locations.Where(c => c.userId == obj.userId && c.type == null && EF.Functions.DateDiffDay(c.datetime, Dateeee) == 0).OrderByDescending(c => c.locId).FirstOrDefaultAsync();
                        if (gcd != null)
                        {
                            oldTime = gcd.datetime.Value;
                            span = newTime.Subtract(oldTime);
                        }

                        if (gcd == null || span.Minutes >= 9)
                        //  var IsSameRecordLocation = db.Locations.Where(c => c.userId == obj.userId && c.datetime == Dateeee).FirstOrDefault();

                        //if (IsSameRecordLocation == null)
                        {
                            var u = db.UserMasters.Where(c => c.userId == obj.userId);

                            var atten = await db.Daily_Attendances.Where(c => c.userId == obj.userId && EF.Functions.DateDiffDay(c.daDate, Dateeee) == 0).FirstOrDefaultAsync();

                            if (atten == null)
                            {
                                result.isAttendenceOff = true;
                                result.ID = obj.OfflineID;
                                result.message = "Your duty is currently off, please start again.. ";
                                result.messageMar = "आपली ड्यूटी सध्या बंद आहे, कृपया पुन्हा सुरू करा..";
                                result.status = "error";
                                return result;
                            }
                            else { result.isAttendenceOff = false; }


                            if (u != null & obj.userId > 0)
                            {
                                string addr = "", ar = "";
                                addr = Address(obj.Lat + "," + obj.Long);
                                if (addr != "")
                                {
                                    ar = area(addr);
                                }

                                db.Locations.Add(new Location()
                                {
                                    userId = obj.userId,
                                    lat = obj.Lat,
                                    _long = obj.Long,
                                    datetime = Convert.ToDateTime(obj.gcDate),
                                    address = addr,
                                    area = ar,
                                    batteryStatus = obj.batteryStatus,
                                    Distnace = obj.Distance,
                                    //IsOffline = true,
                                    ReferanceID = obj.ReferenceID,
                                    CreatedDate = DateTime.Now,
                                });
                                db.SaveChanges();
                            }
                        }
                        result.ID = Convert.ToInt32(obj.OfflineID);
                        result.status = "success";
                        result.message = "Uploaded successfully";
                        result.messageMar = "सबमिट यशस्वी";
                        return result;


                    }


                    else if (typeId == 1)
                    {
                        DateTime newTime = Dateeee;
                        DateTime oldTime;
                        TimeSpan span = TimeSpan.Zero;
                        var IsSameRecordQr_Location = await db.Qr_Locations.Where(c => c.empId == obj.userId && c.type == null && EF.Functions.DateDiffDay(c.datetime, Dateeee) == 0).OrderByDescending(c => c.locId).FirstOrDefaultAsync();
                        if (IsSameRecordQr_Location != null)
                        {
                            oldTime = IsSameRecordQr_Location.datetime.Value;
                            span = newTime.Subtract(oldTime);
                        }

                        if (IsSameRecordQr_Location == null || span.Minutes >= 9)

                        //    var IsSameRecordQr_Location = db.Locations.Where(c => c.userId == obj.userId && c.datetime == Dateeee).FirstOrDefault();

                        //   if (IsSameRecordQr_Location == null)
                        {
                            var u = db.QrEmployeeMasters.Where(c => c.qrEmpId == obj.userId);

                            if (obj.OfflineID == 0)
                            {
                                var atten = await db.Qr_Employee_Daily_Attendances.Where(c => c.qrEmpId == obj.userId && EF.Functions.DateDiffDay(c.startDate, Dateeee) == 0).FirstOrDefaultAsync();
                                if (atten == null)
                                {
                                    result.ID = Convert.ToInt32(obj.OfflineID);
                                    result.isAttendenceOff = false;
                                    result.status = "error";
                                    result.message = "Your duty is currently off, please start again.. ";
                                    result.messageMar = "आपली ड्यूटी सध्या बंद आहे, कृपया पुन्हा सुरू करा..";
                                    return result;
                                }
                            }


                            if (u != null & obj.userId > 0)
                            {
                                string addr = "", ar = "";
                                addr = Address(obj.Lat + "," + obj.Long);
                                if (addr != "")
                                {
                                    ar = area(addr);
                                }

                                db.Qr_Locations.Add(new Qr_Location()
                                {
                                    empId = obj.userId,
                                    lat = obj.Lat,
                                    _long = obj.Long,
                                    datetime = Convert.ToDateTime(obj.gcDate),
                                    address = addr,
                                    area = ar,
                                    batteryStatus = obj.batteryStatus,
                                    Distnace = obj.Distance, //Convert.ToDecimal(distCount),
                                });
                                await db.SaveChangesAsync();

                            }
                        }
                        result.ID = Convert.ToInt32(obj.OfflineID);
                        result.status = "success";
                        result.message = "Uploaded successfully";
                        result.messageMar = "सबमिट यशस्वी";

                    }
                    return result;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.ToString(), ex);
                    CollectionSyncResult result = new CollectionSyncResult();
                    result.ID = 0;
                    result.status = "error";
                    result.message = "काहीतरी चुकीचे आहे, पुन्हा प्रयत्न करा..";
                    //result.messageMar = "Something is wrong,Try Again.. ";
                    result.message = ex.Message;

                    return result;
                }

            }

        }


        public async Task<Result> SaveQrEmployeeAttendenceAsync(BigVQREmployeeAttendenceVM obj, int AppId, int type)
        {
            Result result = new Result();
            using (DevICTSBMChildEntities db = new DevICTSBMChildEntities(AppId))
            using (DevICTSBMMainEntities dbMain = new DevICTSBMMainEntities())
            {
                if ((string.IsNullOrEmpty(obj.startLat)) == true && (string.IsNullOrEmpty(obj.startLong)) == true && type == 0)
                {
                    result.status = "error";
                    result.message = "Your start Lat / Long are Empty ";
                    result.messageMar = "तुमची सुरुवात लॅट / लॉन्ग  रिक्त आहेत";
                    return result;
                }



                if ((string.IsNullOrEmpty(obj.endLat)) == true && (string.IsNullOrEmpty(obj.endLong)) == true && type == 1)
                {
                    result.status = "error";
                    result.message = "Your End Lat / Long are Empty ";
                    result.messageMar = "तुमचा शेवट लॅट / लॉन्ग रिक्त आहेत";
                    return result;
                }

                if (type == 0)
                {
                    //Daily_Attendance data = db.Daily_Attendance.Where(c => c.daDate == EntityFunctions.TruncateTime(obj.daDate) && c.userId == obj.userId && (c.endTime == null || c.endTime == "")).FirstOrDefault();
                    Qr_Employee_Daily_Attendance data = await db.Qr_Employee_Daily_Attendances.Where(c => c.qrEmpId == obj.qrEmpId && (c.endTime == null || c.endTime == "")).FirstOrDefaultAsync();
                    if (data != null)
                    {
                        data.endTime = obj.startTime;
                        data.endDate = obj.startDate;
                        data.endLat = obj.startLat;
                        data.endLong = obj.startLong;
                        db.SaveChanges();
                    }
                    try
                    {
                        Qr_Employee_Daily_Attendance objdata = new Qr_Employee_Daily_Attendance();

                        var isActive = await db.QrEmployeeMasters.Where(c => c.qrEmpId == obj.qrEmpId && c.isActive == true).FirstOrDefaultAsync();
                        if (isActive != null)
                        {

                            objdata.qrEmpId = obj.qrEmpId;
                            objdata.startDate = obj.startDate;
                            objdata.endLat = "";
                            objdata.startLat = obj.startLat;
                            objdata.startLong = obj.startLong;
                            objdata.startTime = obj.startTime;
                            objdata.endTime = "";
                            objdata.startNote = obj.startNote;
                            objdata.endNote = obj.endNote;
                            //   objdata.startAddress = Address(obj.startLat + "," + obj.startLong); 
                            db.Qr_Employee_Daily_Attendances.Add(objdata);
                            await db.SaveChangesAsync();
                            result.status = "success";
                            result.message = "Shift started Successfully";
                            result.messageMar = "शिफ्ट सुरू";
                            return result;
                        }
                        else
                        {
                            result.status = "Error";
                            result.message = "Contact To Administrator";
                            result.messageMar = "प्रशासकाशी संपर्क साधा";
                            return result;
                        }
                    }

                    catch (Exception ex)
                    {
                        _logger.LogError(ex.ToString(), ex);
                        result.status = "error";
                        //result.message = "Something is wrong,Try Again.. ";
                        result.message = ex.Message;
                        result.messageMar = "काहीतरी चुकीचे आहे, पुन्हा प्रयत्न करा..";
                        return result;
                    }


                }
                else
                {

                    try
                    {
                        Qr_Employee_Daily_Attendance objdata = await db.Qr_Employee_Daily_Attendances.Where(c => EF.Functions.DateDiffDay(c.startDate, obj.startDate) == 0 && c.qrEmpId == obj.qrEmpId && (c.endTime == "" || c.endTime == null)).FirstOrDefaultAsync();
                        if (objdata != null)
                        {

                            objdata.qrEmpId = obj.qrEmpId;
                            objdata.endDate = obj.startDate;
                            objdata.endLat = obj.endLat;
                            objdata.endLong = obj.endLong;
                            objdata.endTime = obj.endTime;
                            objdata.endNote = obj.endNote;
                            //objdata.endAddress = Address(objdata.endLat + "," + objdata.endLong);

                            ///////////////////////////////////////////////////////////////////

                            Qr_Location loc = new Qr_Location();
                            loc.empId = obj.qrEmpId;
                            loc.datetime = DateTime.Now;
                            loc.lat = obj.endLat;
                            loc._long = obj.endLong;
                            loc.address = Address(obj.endLat + "," + obj.endLong);
                            if (loc.address != "")
                            { loc.area = area(loc.address); }
                            else
                            {
                                loc.area = "";
                            }
                            db.Qr_Locations.Add(loc);

                            ///////////////////////////////////////////////////////////////////

                            await db.SaveChangesAsync();
                            result.status = "success";
                            result.message = "Shift ended successfully";
                            result.messageMar = "शिफ्ट संपले";
                            return result;
                        }


                        else
                        {
                            Qr_Employee_Daily_Attendance objdata2 = await db.Qr_Employee_Daily_Attendances.Where(c => c.qrEmpId == obj.qrEmpId && (c.endTime == "" || c.endTime == null)).OrderByDescending(c => c.qrEmpDaId).FirstOrDefaultAsync();
                            objdata2.qrEmpId = obj.qrEmpId;
                            objdata2.endDate = DateTime.Now;
                            objdata2.endLat = obj.endLat;
                            objdata2.endLong = obj.endLong;
                            objdata2.endTime = obj.endTime;
                            objdata2.endNote = obj.endNote;
                            //       objdata.endAddress = Address(objdata.endLat + "," + objdata.endLong);

                            ///////////////////////////////////////////////////////////////////

                            Qr_Location loc = new Qr_Location();
                            loc.empId = obj.qrEmpId;
                            loc.datetime = DateTime.Now;
                            loc.lat = obj.endLat;
                            loc._long = obj.endLong;
                            loc.address = Address(obj.endLat + "," + obj.endLong);
                            if (loc.address != "")
                            { loc.area = area(loc.address); }
                            else
                            {
                                loc.area = "";
                            }
                            db.Qr_Locations.Add(loc);

                            ///////////////////////////////////////////////////////////////////

                            await db.SaveChangesAsync();
                            result.status = "success";
                            result.message = "Shift ended successfully";
                            result.messageMar = "शिफ्ट संपले";
                            return result;
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex.ToString(), ex);
                        result.status = "error";
                        result.message = ex.Message;
                        //result.message = "Something is wrong,Try Again.. ";
                        result.messageMar = "काहीतरी चुकीचे आहे, पुन्हा प्रयत्न करा..";
                        return result;
                    }


                    var darecord = await db.Qr_Employee_Daily_Attendances.Where(c => EF.Functions.DateDiffDay(c.startDate, DateTime.Now) == 0 && (c.endTime == "" || c.endTime == null)).FirstOrDefaultAsync();
                    var appdetails = await dbMain.AppDetails.Where(x => x.AppId == AppId).FirstOrDefaultAsync();
                    if (darecord == null && appdetails.FAQ != "0")
                    {

                        if (appdetails != null)
                        {
                            appdetails.FAQ = "2";
                            await dbMain.SaveChangesAsync();
                        }
                    }
                }
            }


        }

        public async Task<Result> SaveQrHPDCollectionsAsync(BigVQRHPDVM obj, int AppId, int gcType)
        {
            Result result = new Result();

            // var appdetails = dbMain.AppDetails.Where(c => c.AppId == AppId).FirstOrDefault();
            //using (new TransactionScope(
            //       TransactionScopeOption.Required,
            //       new TransactionOptions
            //       {
            //           IsolationLevel = IsolationLevel.ReadUncommitted
            //       }))
            //{
            using (DevICTSBMChildEntities db = new DevICTSBMChildEntities(AppId))
            using (DevICTSBMMainEntities dbMain = new DevICTSBMMainEntities())
            {
                var distCount = "";
                // double New_Lat = 0;
                //double New_Long = 0;
                try
                {

                    var appdetails = await dbMain.AppDetails.Where(c => c.AppId == AppId).FirstOrDefaultAsync();

                    DateTime Dateeee = DateTime.Now;
                    var atten = await db.Qr_Employee_Daily_Attendances.Where(c => c.qrEmpId == obj.userId && EF.Functions.DateDiffDay(c.startDate, Dateeee) == 0).FirstOrDefaultAsync();
                    var Activeuser = await db.QrEmployeeMasters.Where(c => c.qrEmpId == obj.userId).FirstOrDefaultAsync();
                    if (Activeuser.isActive == false)
                    {
                        result.message = "Contact To Administrator";
                        result.messageMar = "प्रशासकाशी संपर्क साधा.";
                        result.status = "error";
                        return result;
                    }
                    else if (atten != null)
                    {

                        coordinates p = new coordinates()
                        {
                            lat = Convert.ToDouble(obj.Lat),
                            lng = Convert.ToDouble(obj.Long)
                        };


                        obj.IsIn = false;
                        if (appdetails.AppAreaLatLong != null)
                        {
                            List<List<coordinates>> lstPoly = new List<List<coordinates>>();
                            List<coordinates> poly = new List<coordinates>();
                            AppAreaMapVM ebm = GetEmpBeatMapByAppId(AppId);
                            lstPoly = ebm.AppAreaLatLong;
                            int polyId = 0;
                            if (lstPoly != null && lstPoly.Count > polyId)
                            {
                                poly = lstPoly[polyId];
                            }
                            obj.IsIn = IsPointInPolygon(poly, p);
                        }

                        if ((obj.IsIn == true && appdetails.IsAreaActive == true) || (appdetails.IsAreaActive == false))
                        {

                            if (obj.gcType == 5)
                            {
                                var dump = await db.StreetSweepingDetails.Where(x => x.ReferanceId == obj.ReferanceId).FirstOrDefaultAsync();
                                if (dump != null)
                                {
                                    if ((string.IsNullOrEmpty(obj.name)) == false)
                                    {
                                        dump.SSName = obj.name;
                                    }
                                    if ((string.IsNullOrEmpty(obj.namemar)) == false)
                                    {
                                        dump.SSNameMar = obj.namemar;
                                    }
                                    if ((string.IsNullOrEmpty(obj.Address)) == false)
                                    {
                                        dump.SSAddress = obj.Address;
                                    }
                                    if ((string.IsNullOrEmpty(obj.Lat)) == false)
                                    {
                                        dump.SSLat = obj.Lat;
                                    }
                                    if ((string.IsNullOrEmpty(obj.Long)) == false)
                                    {
                                        dump.SSLong = obj.Long;
                                    }

                                    dump.lastModifiedDate = DateTime.Now;


                                    if (obj.areaId > 0 && (string.IsNullOrEmpty(obj.areaId.ToString())) == false)
                                    {
                                        dump.areaId = obj.areaId;
                                    }
                                    if (obj.zoneId > 0 && (string.IsNullOrEmpty(obj.zoneId.ToString())) == false)
                                    {
                                        dump.zoneId = obj.zoneId;
                                    }
                                    if (obj.wardId > 0 && (string.IsNullOrEmpty(obj.wardId.ToString())) == false)
                                    {
                                        dump.wardId = obj.wardId;
                                    }
                                    if (obj.userId > 0 && (string.IsNullOrEmpty(obj.userId.ToString())) == false)
                                    {
                                        dump.userId = obj.userId;
                                    }
                                    //if ((string.IsNullOrEmpty(obj.QRCodeImage)) == false)
                                    //{
                                    //    dump.QRCodeImage = obj.QRCodeImage;
                                    //}
                                    if ((string.IsNullOrEmpty(obj.QRCodeImage)) == false)
                                    {
                                        obj.QRCodeImage = obj.QRCodeImage.Replace("data:image/jpeg;base64,", "");
                                        dump.BinaryQrCodeImage = Convert.FromBase64String(obj.QRCodeImage);
                                    }
                                    //////////////////////////////////////////////////////////////////
                                    obj.date = DateTime.Now;
                                    obj.ReferanceId = obj.ReferanceId;


                                    db.Qr_Locations.Add(await FillLocationDetailsAsync(obj, AppId, false));
                                    //////////////////////////////////////////////////////////////////

                                    await db.SaveChangesAsync();
                                    result.status = "success";
                                    result.message = "Uploaded successfully";
                                    result.messageMar = "सबमिट यशस्वी";
                                }
                                else
                                {
                                    result.status = "error";
                                    result.message = "Invalid Dump Yard ID";
                                    result.messageMar = "अवैध डंप यार्ड आयडी ";
                                }

                            }
                            if (obj.gcType == 4)
                            {
                                var dump = await db.LiquidWasteDetails.Where(x => x.ReferanceId == obj.ReferanceId).FirstOrDefaultAsync();
                                if (dump != null)
                                {
                                    if ((string.IsNullOrEmpty(obj.name)) == false)
                                    {
                                        dump.LWName = obj.name;
                                    }
                                    if ((string.IsNullOrEmpty(obj.namemar)) == false)
                                    {
                                        dump.LWNameMar = obj.namemar;
                                    }
                                    if ((string.IsNullOrEmpty(obj.Address)) == false)
                                    {
                                        dump.LWAddreLW = obj.Address;
                                    }
                                    if ((string.IsNullOrEmpty(obj.Lat)) == false)
                                    {
                                        dump.LWLat = obj.Lat;
                                    }
                                    if ((string.IsNullOrEmpty(obj.Long)) == false)
                                    {
                                        dump.LWLong = obj.Long;
                                    }

                                    dump.lastModifiedDate = DateTime.Now;


                                    if (obj.areaId > 0 && (string.IsNullOrEmpty(obj.areaId.ToString())) == false)
                                    {
                                        dump.areaId = obj.areaId;
                                    }
                                    if (obj.zoneId > 0 && (string.IsNullOrEmpty(obj.zoneId.ToString())) == false)
                                    {
                                        dump.zoneId = obj.zoneId;
                                    }
                                    if (obj.wardId > 0 && (string.IsNullOrEmpty(obj.wardId.ToString())) == false)
                                    {
                                        dump.wardId = obj.wardId;
                                    }
                                    if (obj.userId > 0 && (string.IsNullOrEmpty(obj.userId.ToString())) == false)
                                    {
                                        dump.userId = obj.userId;
                                    }
                                    //if ((string.IsNullOrEmpty(obj.QRCodeImage)) == false)
                                    //{
                                    //    dump.QRCodeImage = obj.QRCodeImage;
                                    //}
                                    if ((string.IsNullOrEmpty(obj.QRCodeImage)) == false)
                                    {
                                        obj.QRCodeImage = obj.QRCodeImage.Replace("data:image/jpeg;base64,", "");
                                        dump.BinaryQrCodeImage = Convert.FromBase64String(obj.QRCodeImage);
                                    }
                                    //////////////////////////////////////////////////////////////////
                                    obj.date = DateTime.Now;
                                    obj.ReferanceId = obj.ReferanceId;

                                    db.Qr_Locations.Add(await FillLocationDetailsAsync(obj, AppId, false));
                                    //////////////////////////////////////////////////////////////////

                                    await db.SaveChangesAsync();
                                    result.status = "success";
                                    result.message = "Uploaded successfully";
                                    result.messageMar = "सबमिट यशस्वी";
                                }
                                else
                                {
                                    result.status = "error";
                                    result.message = "Invalid Dump Yard ID";
                                    result.messageMar = "अवैध डंप यार्ड आयडी ";
                                }

                            }
                            if (obj.gcType == 3)
                            {
                                var dump = await db.DumpYardDetails.Where(x => x.ReferanceId == obj.ReferanceId).FirstOrDefaultAsync();
                                result.dyId = dump.dyId;
                                if (dump != null)
                                {
                                    if ((string.IsNullOrEmpty(obj.name)) == false)
                                    {
                                        dump.dyName = obj.name;
                                    }
                                    if ((string.IsNullOrEmpty(obj.namemar)) == false)
                                    {
                                        dump.dyNameMar = obj.namemar;
                                    }
                                    if ((string.IsNullOrEmpty(obj.Address)) == false)
                                    {
                                        dump.dyAddress = obj.Address;
                                    }
                                    if ((string.IsNullOrEmpty(obj.Lat)) == false)
                                    {
                                        dump.dyLat = obj.Lat;
                                    }
                                    if ((string.IsNullOrEmpty(obj.Long)) == false)
                                    {
                                        dump.dyLong = obj.Long;
                                    }

                                    dump.lastModifiedDate = DateTime.Now;


                                    if (obj.areaId > 0 && (string.IsNullOrEmpty(obj.areaId.ToString())) == false)
                                    {
                                        dump.areaId = obj.areaId;
                                    }
                                    if (obj.zoneId > 0 && (string.IsNullOrEmpty(obj.zoneId.ToString())) == false)
                                    {
                                        dump.zoneId = obj.zoneId;
                                    }
                                    if (obj.wardId > 0 && (string.IsNullOrEmpty(obj.wardId.ToString())) == false)
                                    {
                                        dump.wardId = obj.wardId;
                                    }
                                    if (obj.userId > 0 && (string.IsNullOrEmpty(obj.userId.ToString())) == false)
                                    {
                                        dump.userId = obj.userId;
                                    }
                                    //if ((string.IsNullOrEmpty(obj.QRCodeImage)) == false)
                                    //{
                                    //    dump.QRCodeImage = obj.QRCodeImage;
                                    //}
                                    if ((string.IsNullOrEmpty(obj.QRCodeImage)) == false)
                                    {
                                        obj.QRCodeImage = obj.QRCodeImage.Replace("data:image/jpeg;base64,", "");
                                        dump.BinaryQrCodeImage = Convert.FromBase64String(obj.QRCodeImage);
                                    }
                                    //////////////////////////////////////////////////////////////////
                                    obj.date = DateTime.Now;
                                    obj.ReferanceId = obj.ReferanceId;
                                    db.Qr_Locations.Add(await FillLocationDetailsAsync(obj, AppId, false));
                                    //////////////////////////////////////////////////////////////////

                                    await db.SaveChangesAsync();
                                    result.status = "success";
                                    result.message = "Uploaded successfully";
                                    result.messageMar = "सबमिट यशस्वी";
                                }
                                else
                                {
                                    result.status = "error";
                                    result.message = "Invalid Dump Yard ID";
                                    result.messageMar = "अवैध डंप यार्ड आयडी ";
                                }

                            }
                            else if (obj.gcType == 2)
                            {
                                var gp = await db.GarbagePointDetails.Where(x => x.ReferanceId == obj.ReferanceId).FirstOrDefaultAsync();

                                if (gp != null)
                                {
                                    if ((string.IsNullOrEmpty(obj.name.ToString())) == false)
                                    {
                                        gp.gpName = obj.name;
                                    }
                                    if ((string.IsNullOrEmpty(obj.namemar.ToString())) == false)
                                    {
                                        gp.gpNameMar = obj.namemar;
                                    }
                                    if ((string.IsNullOrEmpty(obj.Address.ToString())) == false)
                                    {
                                        gp.gpAddress = obj.Address;
                                    }
                                    if ((string.IsNullOrEmpty(obj.Lat.ToString())) == false)
                                    {
                                        gp.gpLat = obj.Lat;
                                    }
                                    if ((string.IsNullOrEmpty(obj.Long.ToString())) == false)
                                    {
                                        gp.gpLong = obj.Long;
                                    }

                                    gp.modified = DateTime.Now;

                                    if (obj.areaId > 0 && (string.IsNullOrEmpty(obj.areaId.ToString())) == false)
                                    {
                                        gp.areaId = obj.areaId;
                                    }
                                    if (obj.zoneId > 0 && (string.IsNullOrEmpty(obj.zoneId.ToString())) == false)
                                    {
                                        gp.zoneId = obj.zoneId;
                                    }
                                    if (obj.wardId > 0 && (string.IsNullOrEmpty(obj.wardId.ToString())) == false)
                                    {
                                        gp.wardId = obj.wardId;
                                    }
                                    if (obj.userId > 0 && (string.IsNullOrEmpty(obj.userId.ToString())) == false)
                                    {
                                        gp.userId = obj.userId;
                                    }


                                    //////////////////////////////////////////////////////////////////
                                    obj.date = DateTime.Now;
                                    db.Qr_Locations.Add(await FillLocationDetailsAsync(obj, AppId, false));
                                    //////////////////////////////////////////////////////////////////


                                    await db.SaveChangesAsync();
                                    result.status = "success";
                                    result.message = "Uploaded successfully";
                                    result.messageMar = "सबमिट यशस्वी";
                                }
                                else
                                {
                                    result.status = "error";
                                    result.message = "Invalid Garbage Point ID";
                                    result.messageMar = "अवैध कचरा पॉइंट आयडी";
                                }
                            }
                            else if (obj.gcType == 1)
                            {
                                var house = await db.HouseMasters.Where(x => x.ReferanceId == obj.ReferanceId).FirstOrDefaultAsync();
                                result.houseid = house.houseId;
                                if (house.houseLat != null)
                                {
                                    result.IsExixts = true;
                                }
                                else
                                {
                                    result.IsExixts = false;
                                }
                                if (house != null)
                                {
                                    if ((string.IsNullOrEmpty(obj.houseNumber.ToString())) == false)
                                    {
                                        house.houseNumber = obj.houseNumber;
                                    }
                                    if ((string.IsNullOrEmpty(obj.name.ToString())) == false)
                                    {
                                        house.houseOwner = obj.name;
                                    }
                                    if ((string.IsNullOrEmpty(obj.namemar.ToString())) == false)
                                    {
                                        house.houseOwnerMar = obj.namemar;
                                    }
                                    if ((string.IsNullOrEmpty(obj.Address.ToString())) == false)
                                    {
                                        house.houseAddress = obj.Address;
                                    }
                                    if ((string.IsNullOrEmpty(obj.Lat.ToString())) == false)
                                    {
                                        house.houseLat = obj.Lat;
                                    }

                                    if ((string.IsNullOrEmpty(obj.Long.ToString())) == false)
                                    {
                                        house.houseLong = obj.Long;
                                    }

                                    house.modified = DateTime.Now;

                                    if (obj.areaId > 0 && (string.IsNullOrEmpty(obj.areaId.ToString())) == false)
                                    {
                                        house.AreaId = obj.areaId;
                                    }
                                    if (obj.zoneId > 0 && (string.IsNullOrEmpty(obj.zoneId.ToString())) == false)
                                    {
                                        house.ZoneId = obj.zoneId;
                                    }
                                    if (obj.wardId > 0 && (string.IsNullOrEmpty(obj.wardId.ToString())) == false)
                                    {
                                        house.WardNo = obj.wardId;
                                    }
                                    if (obj.userId > 0 && (string.IsNullOrEmpty(obj.userId.ToString())) == false)
                                    {
                                        house.userId = obj.userId;
                                    }
                                    if ((string.IsNullOrEmpty(obj.mobileno)) == false)
                                    {
                                        house.houseOwnerMobile = obj.mobileno;
                                    }

                                    if ((string.IsNullOrEmpty(obj.wastetype)) == false)
                                    {
                                        house.WasteType = obj.wastetype;
                                    }

                                    //if ((string.IsNullOrEmpty(obj.QRCodeImage)) == false)
                                    //{
                                    //    house.QRCodeImage = obj.QRCodeImage;
                                    //}

                                    if ((string.IsNullOrEmpty(obj.QRCodeImage)) == false)
                                    {
                                        // house.BinaryQrCodeImage = Convert.FromBase64String(obj.QRCodeImage.Substring(obj.QRCodeImage.LastIndexOf(',') + 1));
                                        Regex regex = new Regex(@"^[\w/\:.-]+;base64,");
                                        obj.QRCodeImage = regex.Replace(obj.QRCodeImage, string.Empty);
                                        obj.QRCodeImage = obj.QRCodeImage.Replace("data:image/jpeg;base64,", String.Empty);
                                        house.BinaryQrCodeImage = Convert.FromBase64String(obj.QRCodeImage);
                                    }
                                    house.New_Construction = obj.new_const;
                                    //////////////////////////////////////////////////////////////////
                                    obj.date = DateTime.Now;
                                    db.Qr_Locations.Add(await FillLocationDetailsAsync(obj, AppId, false));
                                    //////////////////////////////////////////////////////////////////


                                    await db.SaveChangesAsync();
                                    result.status = "success";
                                    result.message = "Uploaded successfully";
                                    result.messageMar = "सबमिट यशस्वी";

                                }
                                else
                                {
                                    result.status = "error";
                                    result.message = "Invalid House ID";
                                    result.messageMar = "अवैध घर आयडी";
                                }

                            }
                        }
                        else
                        {
                            result.message = "Your outside the area,please go to inside the area.. ";
                            result.messageMar = "तुम्ही क्षेत्राबाहेर आहात.कृपया परिसरात जा..";
                            result.status = "error";
                            return result;
                        }
                    }
                    else
                    {
                        result.message = "Your duty is currently off, please start again.. ";
                        result.messageMar = "आपली ड्यूटी सध्या बंद आहे, कृपया पुन्हा सुरू करा..";
                        result.status = "error";
                        return result;
                    }

                    if (result.status == "success")
                    {
                        if (appdetails != null)
                        {
                            appdetails.FAQ = "1";
                            await dbMain.SaveChangesAsync();
                        }
                        //List<AppDetail> AppDetailss = dbMain.Database.SqlQuery<AppDetail>("exec [Update_Trigger]").ToList();
                        List<AppDetail> AppDetailss = dbMain.AppDetails.FromSqlRaw<AppDetail>("exec [Update_Trigger]").ToList();


                        var updateappdetails = await dbMain.SP_DailyScanCount_Results.FromSqlRaw<SP_DailyScanCount_Result>($"EXEC DailyScanCount {AppId.ToString()}").ToListAsync();
                    }
                    return result;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.ToString(), ex);
                    result.message = ex.Message;
                    //result.message = "Something is wrong,Try Again.. ";
                    result.messageMar = "काहीतरी चुकीचे आहे, पुन्हा प्रयत्न करा..";
                    //result.name = "";
                    result.status = "error";
                    return result;
                }
            }
            //}
        }

        public async Task<Result> SaveHouseTrail(Trial obj, int AppId)
        {
            Result result = new Result();


            using (DevICTSBMChildEntities db = new DevICTSBMChildEntities(AppId))
            using (DevICTSBMMainEntities dbMain = new DevICTSBMMainEntities())
            {
                try
                {
                    var appdetails = await dbMain.AppDetails.Where(c => c.AppId == AppId).FirstOrDefaultAsync();
                    DateTime Dateeee = DateTime.Now;
                    var atten = await db.Qr_Employee_Daily_Attendances.Where(c => c.qrEmpId == obj.createUser && EF.Functions.DateDiffDay(c.startDate, Dateeee) == 0).FirstOrDefaultAsync();
                    var Activeuser = await db.QrEmployeeMasters.Where(c => c.qrEmpId == obj.createUser).FirstOrDefaultAsync();
                    if (Activeuser.isActive == false)
                    {
                        result.message = "Contact To Administrator";
                        result.messageMar = "प्रशासकाशी संपर्क साधा.";
                        result.status = "error";
                        return result;
                    }
                    else if (atten != null)
                    {
                        using (SqlConnection connection = new SqlConnection(db.Database.GetDbConnection().ConnectionString))
                        {
                            connection.Open();

                            var command = connection.CreateCommand();

                            const string CheckIfTableExistsStatement = "SELECT * FROM sys.objects WHERE name = N'HouseTrail_Insert'";
                            command.CommandText = CheckIfTableExistsStatement;
                            var executeScalar = command.ExecuteScalar();
                            if (executeScalar != null)
                            {
                                CultureInfo culture = new CultureInfo("en-US");

                                DateTime sts = Convert.ToDateTime(obj.startTs, culture);
                                DateTime ets = Convert.ToDateTime(obj.endTs, culture);
                                DateTime cts = Convert.ToDateTime(obj.createTs, culture);
                                DateTime uts = Convert.ToDateTime(obj.updateTs, culture);



                                using (SqlConnection con = new SqlConnection(db.Database.GetDbConnection().ConnectionString))
                                {
                                    using (SqlCommand cmd = new SqlCommand("HouseTrail_Insert", con))
                                    {
                                        cmd.CommandType = CommandType.StoredProcedure;

                                        cmd.Parameters.Add("@Id", SqlDbType.NVarChar).Value = obj.id;
                                        cmd.Parameters.Add("@Start_ts", SqlDbType.DateTime).Value = sts;
                                        cmd.Parameters.Add("@End_ts", SqlDbType.DateTime).Value = ets;
                                        cmd.Parameters.Add("@Create_user", SqlDbType.Int).Value = obj.createUser;
                                        cmd.Parameters.Add("@Create_ts", SqlDbType.DateTime).Value = cts;
                                        cmd.Parameters.Add("@Update_user", SqlDbType.Int).Value = obj.updateUser;
                                        cmd.Parameters.Add("@Update_ts", SqlDbType.DateTime).Value = uts;
                                        cmd.Parameters.Add("@geom", SqlDbType.NVarChar).Value = obj.geom;

                                        con.Open();
                                        int rowsAffected = cmd.ExecuteNonQuery();

                                        con.Close();
                                    }
                                }
                                //var data = await db.HouseTrail_Insert_Results.FromSqlRaw<HouseTrail_Insert_Result>("EXEC HouseTrail_Insert @Id,@Start_ts,@End_ts,@Create_user,@Create_ts,@Update_user,@Update_ts,@geom", parms.ToArray()).ToListAsync();

                            }
                            connection.Close();
                        }

                        result.status = "Success";
                        return result;
                    }
                    else
                    {
                        result.message = "Your duty is currently off, please start again.. ";
                        result.messageMar = "आपली ड्यूटी सध्या बंद आहे, कृपया पुन्हा सुरू करा..";
                        result.status = "error";
                        return result;
                    }
                    return result;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.ToString(), ex);
                    result.message = "Something is wrong,Try Again.. ";
                    result.messageMar = "काहीतरी चुकीचे आहे, पुन्हा प्रयत्न करा..";
                    //result.name = "";
                    result.status = "error";
                    return result;
                }
            }
        }
        public async Task<Result> SaveGarbageTrail(Trial obj, int AppId)
        {
            Result result = new Result();


            using (DevICTSBMChildEntities db = new DevICTSBMChildEntities(AppId))
            using (DevICTSBMMainEntities dbMain = new DevICTSBMMainEntities())
            {
                try
                {
                    var appdetails = await dbMain.AppDetails.Where(c => c.AppId == AppId).FirstOrDefaultAsync();
                    DateTime Dateeee = DateTime.Now;
                    var atten = await db.Daily_Attendances.Where(c => c.userId == obj.createUser && EF.Functions.DateDiffDay(c.daDate, Dateeee) == 0).FirstOrDefaultAsync();
                    var Activeuser = await db.UserMasters.Where(c => c.userId == obj.createUser).FirstOrDefaultAsync();
                    if (Activeuser.isActive == false)
                    {
                        result.message = "Contact To Administrator";
                        result.messageMar = "प्रशासकाशी संपर्क साधा.";
                        result.status = "error";
                        return result;
                    }
                    else if (atten != null)
                    {
                        using (SqlConnection connection = new SqlConnection(db.Database.GetDbConnection().ConnectionString))
                        {
                            connection.Open();

                            var command = connection.CreateCommand();

                            const string CheckIfTableExistsStatement = "SELECT * FROM sys.objects WHERE name = N'GarbageTrail_Insert'";
                            command.CommandText = CheckIfTableExistsStatement;
                            var executeScalar = command.ExecuteScalar();
                            if (executeScalar != null)
                            {
                                CultureInfo culture = new CultureInfo("en-US");

                                DateTime sts = Convert.ToDateTime(obj.startTs, culture);
                                DateTime ets = Convert.ToDateTime(obj.endTs, culture);
                                DateTime cts = Convert.ToDateTime(obj.createTs, culture);
                                DateTime uts = Convert.ToDateTime(obj.updateTs, culture);



                                using (SqlConnection con = new SqlConnection(db.Database.GetDbConnection().ConnectionString))
                                {
                                    using (SqlCommand cmd = new SqlCommand("GarbageTrail_Insert", con))
                                    {
                                        cmd.CommandType = CommandType.StoredProcedure;

                                        cmd.Parameters.Add("@Id", SqlDbType.NVarChar).Value = obj.id;
                                        cmd.Parameters.Add("@Start_ts", SqlDbType.DateTime).Value = sts;
                                        cmd.Parameters.Add("@End_ts", SqlDbType.DateTime).Value = ets;
                                        cmd.Parameters.Add("@Create_user", SqlDbType.Int).Value = obj.createUser;
                                        cmd.Parameters.Add("@Create_ts", SqlDbType.DateTime).Value = cts;
                                        cmd.Parameters.Add("@Update_user", SqlDbType.Int).Value = obj.updateUser;
                                        cmd.Parameters.Add("@Update_ts", SqlDbType.DateTime).Value = uts;
                                        cmd.Parameters.Add("@geom", SqlDbType.NVarChar).Value = obj.geom;

                                        con.Open();
                                        int rowsAffected = cmd.ExecuteNonQuery();

                                        con.Close();

                                        result.code = 201;
                                        result.message = "Created";
                                        result.status = "Success";
                                        return result;
                                    }
                                }
                                //var data = await db.HouseTrail_Insert_Results.FromSqlRaw<HouseTrail_Insert_Result>("EXEC HouseTrail_Insert @Id,@Start_ts,@End_ts,@Create_user,@Create_ts,@Update_user,@Update_ts,@geom", parms.ToArray()).ToListAsync();

                            }
                            connection.Close();
                        }

                        result.status = "Success";
                        return result;
                    }
                    else
                    {
                        result.message = "Your duty is currently off, please start again.. ";
                        result.messageMar = "आपली ड्यूटी सध्या बंद आहे, कृपया पुन्हा सुरू करा..";
                        result.status = "error";
                        return result;
                    }
                    return result;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.ToString(), ex);
                    result.message = "Something is wrong,Try Again.. ";
                    result.messageMar = "काहीतरी चुकीचे आहे, पुन्हा प्रयत्न करा..";
                    //result.name = "";
                    result.status = "error";
                    return result;
                }
            }
        }

        //private TrailHouse FillHouseTrailDataModel(Trial obj)
        //{
        //    TrailHouse model = new TrailHouse();
        //    model.Id = obj.id;
        //    model.StartTs = Convert.ToDateTime(obj.startTs);
        //    model.EndTs = Convert.ToDateTime(obj.endTs);
        //    model.CreateUser = obj.createUser;
        //    model.CreateTs = Convert.ToDateTime(obj.createTs);
        //    model.UpdateUser = obj.updateUser;
        //    model.UpdateTs = Convert.ToDateTime(obj.updateTs);
        //    model.geom = DbGeography.FromText(obj.geom);
        //    return model;
        //}

        private static byte[] ConvertFromBase64String(string input)
        {
            if (String.IsNullOrWhiteSpace(input)) return null;
            try
            {
                string working = input.Replace('-', '+').Replace('_', '/');
                while (working.Length % 4 != 0)
                {
                    working += '=';
                }
                return Convert.FromBase64String(working);
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public async Task<List<CollectionSyncResult>> SaveQrHPDCollectionsOfflineAsync(List<BigVQRHPDVM> obj, int AppId)
        {
            string refId = "";
            List<CollectionSyncResult> myresult = new List<CollectionSyncResult>();

            using (DevICTSBMChildEntities db = new DevICTSBMChildEntities(AppId))
            using (DevICTSBMMainEntities dbMain = new DevICTSBMMainEntities())
            {
                try
                {
                    var appdetails = await dbMain.AppDetails.Where(c => c.AppId == AppId).FirstOrDefaultAsync();
                    foreach (var item in obj)
                    {
                        refId = "";
                        coordinates p = new coordinates()
                        {
                            lat = Convert.ToDouble(item.Lat),
                            lng = Convert.ToDouble(item.Long)
                        };
                        item.IsIn = false;
                        if (appdetails.AppAreaLatLong != null)
                        {
                            List<List<coordinates>> lstPoly = new List<List<coordinates>>();
                            List<coordinates> poly = new List<coordinates>();
                            AppAreaMapVM ebm = GetEmpBeatMapByAppId(AppId);
                            lstPoly = ebm.AppAreaLatLong;
                            int polyId = 0;
                            if (lstPoly != null && lstPoly.Count > polyId)
                            {
                                poly = lstPoly[polyId];
                            }

                            item.IsIn = IsPointInPolygon(poly, p);
                        }



                        if ((item.IsIn == true && appdetails.IsAreaActive == true) || appdetails.IsAreaActive == false)
                        {
                            if (item.gcType == 5)
                            {
                                var dump = await db.StreetSweepingDetails.Where(x => x.ReferanceId == item.ReferanceId).FirstOrDefaultAsync();
                                if (dump != null)
                                {

                                    if (!string.IsNullOrEmpty(item.name))
                                    {
                                        dump.SSName = item.name;
                                    }
                                    if (!string.IsNullOrEmpty(item.namemar))
                                    {
                                        dump.SSNameMar = item.namemar;
                                    }
                                    if (!string.IsNullOrEmpty(item.Address))
                                    {
                                        dump.SSAddress = item.Address;
                                    }
                                    if (!string.IsNullOrEmpty(item.Lat))
                                    {
                                        dump.SSLat = item.Lat;
                                    }
                                    if (!string.IsNullOrEmpty(item.Long))
                                    {
                                        dump.SSLong = item.Long;
                                    }

                                    dump.lastModifiedDate = item.date; //DateTime.Now;

                                    if (item.areaId > 0 && (string.IsNullOrEmpty(item.areaId.ToString())) == false)
                                    {
                                        dump.areaId = item.areaId;
                                    }
                                    if (item.zoneId > 0 && (string.IsNullOrEmpty(item.zoneId.ToString())) == false)
                                    {
                                        dump.zoneId = item.zoneId;
                                    }
                                    if (item.wardId > 0 && (string.IsNullOrEmpty(item.wardId.ToString())) == false)
                                    {
                                        dump.wardId = item.wardId;
                                    }
                                    if (item.userId > 0 && (string.IsNullOrEmpty(item.userId.ToString())) == false)
                                    {
                                        dump.userId = item.userId;
                                    }
                                    if (!string.IsNullOrEmpty(item.QRCodeImage))
                                    {
                                        dump.QRCodeImage = item.QRCodeImage;
                                    }
                                    db.Qr_Locations.Add(await FillLocationDetailsAsync(item, AppId, true));

                                    await db.SaveChangesAsync();

                                    myresult.Add(new CollectionSyncResult()
                                    {
                                        ID = Convert.ToInt32(item.OfflineId),
                                        status = "success",
                                        message = "Uploaded successfully",
                                        messageMar = "सबमिट यशस्वी",
                                        referenceID = item.ReferanceId,
                                    });

                                  
                                    
                                }
                                else
                                {
                                    myresult.Add(new CollectionSyncResult()
                                    {
                                        ID = Convert.ToInt32(item.OfflineId),
                                        status = "error",
                                        message = "Invalid Dump Yard ID",
                                        messageMar = "अवैध डंप यार्ड आयडी ",
                                        referenceID = item.ReferanceId,
                                    });
                                }

                            }


                            if (item.gcType == 4)
                            {
                                var dump = await db.LiquidWasteDetails.Where(x => x.ReferanceId == item.ReferanceId).FirstOrDefaultAsync();
                                if (dump != null)
                                {

                                    if (!string.IsNullOrEmpty(item.name))
                                    {
                                        dump.LWName = item.name;
                                    }
                                    if (!string.IsNullOrEmpty(item.namemar))
                                    {
                                        dump.LWNameMar = item.namemar;
                                    }
                                    if (!string.IsNullOrEmpty(item.Address))
                                    {
                                        dump.LWAddreLW = item.Address;
                                    }
                                    if (!string.IsNullOrEmpty(item.Lat))
                                    {
                                        dump.LWLat = item.Lat;
                                    }
                                    if (!string.IsNullOrEmpty(item.Long))
                                    {
                                        dump.LWLong = item.Long;
                                    }

                                    dump.lastModifiedDate = item.date; //DateTime.Now;

                                    if (item.areaId > 0 && (string.IsNullOrEmpty(item.areaId.ToString())) == false)
                                    {
                                        dump.areaId = item.areaId;
                                    }
                                    if (item.zoneId > 0 && (string.IsNullOrEmpty(item.zoneId.ToString())) == false)
                                    {
                                        dump.zoneId = item.zoneId;
                                    }
                                    if (item.wardId > 0 && (string.IsNullOrEmpty(item.wardId.ToString())) == false)
                                    {
                                        dump.wardId = item.wardId;
                                    }
                                    if (item.userId > 0 && (string.IsNullOrEmpty(item.userId.ToString())) == false)
                                    {
                                        dump.userId = item.userId;
                                    }
                                    if (!string.IsNullOrEmpty(item.QRCodeImage))
                                    {
                                        dump.QRCodeImage = item.QRCodeImage;
                                    }
                                    db.Qr_Locations.Add(await FillLocationDetailsAsync(item, AppId, true));

                                    await db.SaveChangesAsync();

                                    myresult.Add(new CollectionSyncResult()
                                    {
                                        ID = Convert.ToInt32(item.OfflineId),
                                        status = "success",
                                        message = "Uploaded successfully",
                                        messageMar = "सबमिट यशस्वी",
                                        referenceID = item.ReferanceId,
                                    });
                                  

                                }
                                else
                                {
                                    myresult.Add(new CollectionSyncResult()
                                    {
                                        ID = Convert.ToInt32(item.OfflineId),
                                        status = "error",
                                        message = "Invalid Dump Yard ID",
                                        messageMar = "अवैध डंप यार्ड आयडी ",
                                        referenceID = item.ReferanceId,
                                    });
                                }

                            }

                            if (item.gcType == 3)
                            {
                                var dump = await db.DumpYardDetails.Where(x => x.ReferanceId == item.ReferanceId).FirstOrDefaultAsync();
                                if (dump != null)
                                {

                                    if (!string.IsNullOrEmpty(item.name))
                                    {
                                        dump.dyName = item.name;
                                    }
                                    if (!string.IsNullOrEmpty(item.namemar))
                                    {
                                        dump.dyNameMar = item.namemar;
                                    }
                                    if (!string.IsNullOrEmpty(item.Address))
                                    {
                                        dump.dyAddress = item.Address;
                                    }
                                    if (!string.IsNullOrEmpty(item.Lat))
                                    {
                                        dump.dyLat = item.Lat;
                                    }
                                    if (!string.IsNullOrEmpty(item.Long))
                                    {
                                        dump.dyLong = item.Long;
                                    }

                                    dump.lastModifiedDate = item.date; //DateTime.Now;

                                    if (item.areaId > 0 && (string.IsNullOrEmpty(item.areaId.ToString())) == false)
                                    {
                                        dump.areaId = item.areaId;
                                    }
                                    if (item.zoneId > 0 && (string.IsNullOrEmpty(item.zoneId.ToString())) == false)
                                    {
                                        dump.zoneId = item.zoneId;
                                    }
                                    if (item.wardId > 0 && (string.IsNullOrEmpty(item.wardId.ToString())) == false)
                                    {
                                        dump.wardId = item.wardId;
                                    }
                                    if (item.userId > 0 && (string.IsNullOrEmpty(item.userId.ToString())) == false)
                                    {
                                        dump.userId = item.userId;
                                    }
                                    if (!string.IsNullOrEmpty(item.QRCodeImage))
                                    {
                                        dump.QRCodeImage = item.QRCodeImage;
                                    }
                                    db.Qr_Locations.Add(await FillLocationDetailsAsync(item, AppId, true));

                                    await db.SaveChangesAsync();

                                    myresult.Add(new CollectionSyncResult()
                                    {
                                        ID = Convert.ToInt32(item.OfflineId),
                                        status = "success",
                                        message = "Uploaded successfully",
                                        messageMar = "सबमिट यशस्वी",
                                        referenceID = item.ReferanceId,
                                    });
                                 
                                    
                                    //GIS Code Start (28-12-2022)

                                    Result objDetail1 = new Result();

                                    var message = "";


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
                                            tn.startTs = item.startTs;
                                            tn.endTs = item.endTs;
                                            tn.geom = item.geom;



                                            GisSearch stn = new GisSearch();

                                            stn.id = dump.dyId.ToString();
                                            tn.id = dump.dyId.ToString();


                                            HttpClient client1 = new HttpClient();

                                            //Start New Code
                                            client1.BaseAddress = new Uri(GIS_CON.url);
                                            //client1.DefaultRequestHeaders.Accept.Clear();
                                            client1.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                                            client1.DefaultRequestHeaders.Add("url", gis_url + "/" + gis_DBName);
                                            client1.DefaultRequestHeaders.Add("username", gis_username);
                                            client1.DefaultRequestHeaders.Add("password", gis_password);
                                            HttpResponseMessage response1 = await client1.PostAsJsonAsync("dump-yard/search", stn);
                                            //End New Code

                                            if (response1.IsSuccessStatusCode)
                                            {
                                                var responseString1 = await response1.Content.ReadAsStringAsync();
                                                //var jsonParsed1 = JObject.Parse(responseString1);
                                                //var dynamicobject1 = JsonConvert.DeserializeObject<dynamic>(responseString1);
                                                //var jsonResult1 = jsonParsed1["data"];

                                                var dynamicobject1 = JsonConvert.DeserializeObject<dynamic>(responseString1, new JsonSerializerSettings { DateParseHandling = DateParseHandling.None });

                                                var jsonResult1 = dynamicobject1["data"];

                                                List<GisResult> getresult = jsonResult1.ToObject<List<GisResult>>();

                                                if (getresult.Count == 0)
                                                {
                                                    tn.createUser = item.userId;
                                                    tn.createTs = item.createTs;
                                                }
                                                else
                                                {
                                                    tn.updateTs = item.createTs;
                                                    tn.updateUser = item.userId;
                                                }



                                                HttpClient client = new();

                                                //Start New Code
                                                client.BaseAddress = new Uri(GIS_CON.url);
                                                //client.DefaultRequestHeaders.Accept.Clear();
                                                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                                                client.DefaultRequestHeaders.Add("url", gis_url + "/" + gis_DBName);
                                                client.DefaultRequestHeaders.Add("username", gis_username);
                                                client.DefaultRequestHeaders.Add("password", gis_password);
                                                HttpResponseMessage response = await client1.PostAsJsonAsync("dump-yard", tn);
                                                //End New Code

                                                if (response.IsSuccessStatusCode)
                                                {
                                                    var responseString = await response.Content.ReadAsStringAsync();
                                                    var dynamicobject2 = JsonConvert.DeserializeObject<dynamic>(responseString);
                                                    objDetail.Add(new DumpTripStatusResult()
                                                    {
                                                        code = (int)response.StatusCode,
                                                        status = "Success",
                                                        message = "Created",
                                                        errorMessages = dynamicobject2.errorMessages.ToString(),
                                                        timestamp = DateTime.Now.ToString(),
                                                        data = dynamicobject2.data
                                                    });
                                                    //objDetail1.gismessage = dynamicobject2.message.ToString();
                                                    //objDetail1.giserrorMessages = dynamicobject2.errorMessages.ToString();

                                                    //result = objDetail;
                                                }

                                                else
                                                {
                                                    objDetail.Add(new DumpTripStatusResult()
                                                    {
                                                        code = (int)response.StatusCode,
                                                        status = "Failed",
                                                        message = "Failed",
                                                        timestamp = DateTime.Now.ToString()
                                                    });
                                                }
                                            }
                                            else
                                            {
                                                objDetail.Add(new DumpTripStatusResult()
                                                {
                                                    code = (int)response1.StatusCode,
                                                    status = "Failed",
                                                    message = "Failed",
                                                    timestamp = DateTime.Now.ToString()
                                                });
                                            }
                                        }
                                        else
                                        {

                                            objDetail.Add(new DumpTripStatusResult()
                                            {
                                                code = 404,
                                                status = "Failed",
                                                message = "GIS Connection Are Not Available",
                                                timestamp = DateTime.Now.ToString()
                                            });
                                            //objDetail1.gismessage = "GIS Connection Are Not Available";
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        objDetail.Add(new DumpTripStatusResult()
                                        {
                                            code = 400,
                                            status = "Failed",
                                            message = ex.Message.ToString(),
                                            timestamp = DateTime.Now.ToString()
                                        });
                                        //objDetail1.giserrorMessages = ex.Message.ToString();
                                    }
                                    //GIS Code End


                                }
                                else
                                {
                                    myresult.Add(new CollectionSyncResult()
                                    {
                                        ID = Convert.ToInt32(item.OfflineId),
                                        status = "error",
                                        message = "Invalid Dump Yard ID",
                                        messageMar = "अवैध डंप यार्ड आयडी ",
                                        referenceID = item.ReferanceId,
                                    });
                                }

                            }

                            if (item.gcType == 2)
                            {
                                var gp = await db.GarbagePointDetails.Where(x => x.ReferanceId == item.ReferanceId).FirstOrDefaultAsync();

                                if (gp != null)
                                {
                                    if (!string.IsNullOrEmpty(item.name.ToString()))
                                    {
                                        gp.gpName = item.name;
                                    }
                                    if (!string.IsNullOrEmpty(item.namemar.ToString()))
                                    {
                                        gp.gpNameMar = item.namemar;
                                    }
                                    if (!string.IsNullOrEmpty(item.Address.ToString()))
                                    {
                                        gp.gpAddress = item.Address;
                                    }
                                    if (!string.IsNullOrEmpty(item.Lat.ToString()))
                                    {
                                        gp.gpLat = item.Lat;
                                    }
                                    if (!string.IsNullOrEmpty(item.Long.ToString()))
                                    {
                                        gp.gpLong = item.Long;
                                    }

                                    gp.modified = item.date; //DateTime.Now;

                                    if (item.areaId > 0 && (string.IsNullOrEmpty(item.areaId.ToString())) == false)
                                    {
                                        gp.areaId = item.areaId;
                                    }
                                    if (item.zoneId > 0 && (string.IsNullOrEmpty(item.zoneId.ToString())) == false)
                                    {
                                        gp.zoneId = item.zoneId;
                                    }
                                    if (item.wardId > 0 && (string.IsNullOrEmpty(item.wardId.ToString())) == false)
                                    {
                                        gp.wardId = item.wardId;
                                    }
                                    if (item.userId > 0 && (string.IsNullOrEmpty(item.userId.ToString())) == false)
                                    {
                                        gp.userId = item.userId;
                                    }

                                    db.Qr_Locations.Add(await FillLocationDetailsAsync(item, AppId, true));

                                    await db.SaveChangesAsync();

                                    myresult.Add(new CollectionSyncResult()
                                    {
                                        ID = Convert.ToInt32(item.OfflineId),
                                        status = "success",
                                        message = "Uploaded successfully",
                                        messageMar = "सबमिट यशस्वी",
                                        referenceID = item.ReferanceId,
                                    });
                                 
                                    
                                }
                                else
                                {
                                    myresult.Add(new CollectionSyncResult()
                                    {
                                        ID = Convert.ToInt32(item.OfflineId),
                                        status = "error",
                                        message = "Invalid Garbage Point ID",
                                        messageMar = "अवैध कचरा पॉइंट आयडी",
                                        referenceID = item.ReferanceId,
                                    });
                                }
                            }

                            if (item.gcType == 1)
                            {
                                string houseid1 = item.ReferanceId;
                                refId = item.ReferanceId;
                                string[] houseList = houseid1.Split(',');

                                if (houseList.Length > 1)
                                {
                                    item.ReferanceId = houseList[0];
                                    item.wastetype = houseList[1];

                                }
                                var house = await db.HouseMasters.Where(x => x.ReferanceId == item.ReferanceId).FirstOrDefaultAsync();
                                if (house != null)
                                {
                                    if (!string.IsNullOrEmpty(item.houseNumber.ToString()))
                                    {
                                        house.houseNumber = item.houseNumber;
                                    }
                                    if (!string.IsNullOrEmpty(item.name.ToString()))
                                    {
                                        house.houseOwner = item.name;
                                    }
                                    if (!string.IsNullOrEmpty(item.namemar.ToString()))
                                    {
                                        house.houseOwnerMar = item.namemar;
                                    }
                                    if (!string.IsNullOrEmpty(item.Address.ToString()))
                                    {
                                        house.houseAddress = item.Address;
                                    }
                                    if (!string.IsNullOrEmpty(item.Lat.ToString()))
                                    {
                                        house.houseLat = item.Lat;
                                    }
                                    if (!string.IsNullOrEmpty(item.Long.ToString()))
                                    {
                                        house.houseLong = item.Long;
                                    }

                                    house.modified = item.date; //DateTime.Now;

                                    if (item.areaId > 0 && (string.IsNullOrEmpty(item.areaId.ToString())) == false)
                                    {
                                        house.AreaId = item.areaId;
                                    }
                                    if (item.zoneId > 0 && (string.IsNullOrEmpty(item.zoneId.ToString())) == false)
                                    {
                                        house.ZoneId = item.zoneId;
                                    }
                                    if (item.wardId > 0 && (string.IsNullOrEmpty(item.wardId.ToString())) == false)
                                    {
                                        house.WardNo = item.wardId;
                                    }
                                    if (item.userId > 0 && (string.IsNullOrEmpty(item.userId.ToString())) == false)
                                    {
                                        house.userId = item.userId;
                                    }

                                    if (!string.IsNullOrEmpty(item.mobileno))
                                    {
                                        house.houseOwnerMobile = item.mobileno;
                                    }

                                    if (!string.IsNullOrEmpty(item.wastetype))
                                    {
                                        house.WasteType = item.wastetype;
                                    }
                                    if (!string.IsNullOrEmpty(item.QRCodeImage))
                                    {
                                        house.QRCodeImage = item.QRCodeImage;
                                    }
                                    db.Qr_Locations.Add(await FillLocationDetailsAsync(item, AppId, true));

                                    await db.SaveChangesAsync();

                                    myresult.Add(new CollectionSyncResult()
                                    {
                                        ID = Convert.ToInt32(item.OfflineId),
                                        status = "success",
                                        message = "Uploaded successfully",
                                        messageMar = "सबमिट यशस्वी",
                                        referenceID = item.ReferanceId,
                                    });

                                
                                    //GIS Code Start (28-12-2022)

                                    double New_Lat = 0;
                                    double New_Long = 0;

                                    if (item.new_const == 0)
                                    {



                                        using (SqlConnection connection = new SqlConnection(db.Database.GetDbConnection().ConnectionString))
                                        {
                                            connection.Open();

                                            var command = connection.CreateCommand();

                                            const string CheckIfTableExistsStatement = "SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Buildings_CSV]') AND type in (N'U')";
                                            command.CommandText = CheckIfTableExistsStatement;
                                            var executeScalar = command.ExecuteScalar();
                                            if (executeScalar != null)
                                            {
                                                SqlCommand cmd = new SqlCommand()
                                                {
                                                    CommandText = "calculateDistance",
                                                    Connection = connection,
                                                    CommandType = CommandType.StoredProcedure
                                                };
                                                //Set Input Parameter
                                                SqlParameter param1 = new SqlParameter
                                                {
                                                    ParameterName = "@LAT", //Parameter name defined in stored procedure
                                                    SqlDbType = SqlDbType.NVarChar, //Data Type of Parameter
                                                    Value = item.Lat, //Set the value
                                                                      // Direction = ParameterDirection.Input //Specify the parameter as input
                                                };
                                                //Add the parameter to the SqlCommand object
                                                cmd.Parameters.Add(param1);
                                                //Another approach to add Input Parameter
                                                cmd.Parameters.AddWithValue("@LONG", item.Long);

                                                //connection.Open();
                                                SqlDataReader sdr = cmd.ExecuteReader();

                                                while (sdr.Read())
                                                {

                                                    New_Lat = Convert.ToDouble(sdr[1]);
                                                    New_Long = Convert.ToDouble(sdr[2]);
                                                }
                                            }

                                            //Create the Command Object



                                        }
                                    }
                                    Result objDetail1 = new Result();
                                    if (New_Lat != 0 && New_Long != 0)
                                    {
                                        item.geom = "POINT (" + Convert.ToString(New_Long) + " " + Convert.ToString(New_Lat) + ")";
                                    }

                                    var message = "";


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
                                            tn.startTs = item.startTs;
                                            tn.endTs = item.endTs;
                                            tn.geom = item.geom;



                                            GisSearch stn = new GisSearch();

                                            stn.id = house.houseId.ToString();
                                            tn.id = house.houseId.ToString();


                                            HttpClient client1 = new HttpClient();

                                            //Start Old Code
                                            //var json1 = JsonConvert.SerializeObject(stn, Formatting.Indented);
                                            //var stringContent1 = new StringContent(json1);
                                            //stringContent1.Headers.ContentType.MediaType = "application/json";
                                            //stringContent1.Headers.Add("url", gis_url + "/" + gis_DBName);
                                            //stringContent1.Headers.Add("username", gis_username);
                                            //stringContent1.Headers.Add("password", gis_password);

                                            //var response1 = await client1.PostAsync("http://114.143.244.130:9091/house/search", stringContent1);
                                            //End Old Code


                                            //Start New Code
                                            client1.BaseAddress = new Uri(GIS_CON.url);
                                            //client1.DefaultRequestHeaders.Accept.Clear();
                                            client1.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                                            client1.DefaultRequestHeaders.Add("url", gis_url + "/" + gis_DBName);
                                            client1.DefaultRequestHeaders.Add("username", gis_username);
                                            client1.DefaultRequestHeaders.Add("password", gis_password);
                                            HttpResponseMessage response1 = await client1.PostAsJsonAsync("house/search", stn);
                                            //End New Code

                                            if (response1.IsSuccessStatusCode)
                                            {
                                                var responseString1 = await response1.Content.ReadAsStringAsync();
                                                //var jsonParsed1 = JObject.Parse(responseString1);
                                                //var dynamicobject1 = JsonConvert.DeserializeObject<dynamic>(responseString1);
                                                //var jsonResult1 = jsonParsed1["data"];

                                                var dynamicobject1 = JsonConvert.DeserializeObject<dynamic>(responseString1, new JsonSerializerSettings { DateParseHandling = DateParseHandling.None });

                                                var jsonResult1 = dynamicobject1["data"];

                                                List<GisResult> getresult = jsonResult1.ToObject<List<GisResult>>();

                                                if (getresult.Count == 0)
                                                {
                                                    tn.createUser = item.userId;
                                                    tn.createTs = item.createTs;
                                                }
                                                else
                                                {
                                                    tn.updateTs = item.createTs;
                                                    tn.updateUser = item.userId;
                                                }



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


                                                //Start New Code
                                                client.BaseAddress = new Uri(GIS_CON.url);
                                                //client.DefaultRequestHeaders.Accept.Clear();
                                                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                                                client.DefaultRequestHeaders.Add("url", gis_url + "/" + gis_DBName);
                                                client.DefaultRequestHeaders.Add("username", gis_username);
                                                client.DefaultRequestHeaders.Add("password", gis_password);
                                                HttpResponseMessage response = await client1.PostAsJsonAsync("house", tn);
                                                //End New Code

                                                if (response.IsSuccessStatusCode)
                                                {
                                                    var responseString = await response.Content.ReadAsStringAsync();
                                                    var dynamicobject2 = JsonConvert.DeserializeObject<dynamic>(responseString);
                                                    objDetail.Add(new DumpTripStatusResult()
                                                    {
                                                        code = (int)response.StatusCode,
                                                        status = dynamicobject2.status.ToString(),
                                                        message = dynamicobject2.message.ToString(),
                                                        errorMessages = dynamicobject2.errorMessages.ToString(),
                                                        timestamp = DateTime.Now.ToString(),
                                                        data = dynamicobject2.data
                                                    });
                                                    //objDetail1.gismessage = dynamicobject2.message.ToString();
                                                    //objDetail1.giserrorMessages = dynamicobject2.errorMessages.ToString();

                                                    //result = objDetail;
                                                }

                                                else
                                                {
                                                    var responseString = await response.Content.ReadAsStringAsync();
                                                    var dynamicobject2 = JsonConvert.DeserializeObject<dynamic>(responseString);
                                                    myresult.Add(new CollectionSyncResult()
                                                    {
                                                        code = (int)response.StatusCode,
                                                        status = dynamicobject2.status.ToString(),
                                                        message = dynamicobject2.message.ToString(),
                                                        timestamp = DateTime.Now.ToString()
                                                    });
                                                }
                                            }
                                            else
                                            {
                                                var responseString1 = await response1.Content.ReadAsStringAsync();
                                                var dynamicobject = JsonConvert.DeserializeObject<dynamic>(responseString1);
                                                myresult.Add(new CollectionSyncResult()
                                                {
                                                    code = (int)response1.StatusCode,
                                                    status = dynamicobject.status.ToString(),
                                                    message = dynamicobject.message.ToString(),
                                                    timestamp = DateTime.Now.ToString()
                                                });
                                            }
                                        }
                                        else
                                        {

                                            myresult.Add(new CollectionSyncResult()
                                            {
                                                code = 404,
                                                status = "Failed",
                                                message = "GIS Connection Are Not Available",
                                                timestamp = DateTime.Now.ToString()
                                            });
                                            //objDetail1.gismessage = "GIS Connection Are Not Available";
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        myresult.Add(new CollectionSyncResult()
                                        {
                                            code = 400,
                                            status = "Failed",
                                            message = ex.Message.ToString(),
                                            timestamp = DateTime.Now.ToString()
                                        });
                                        //objDetail1.giserrorMessages = ex.Message.ToString();
                                    }
                                    //GIS Code End



                                }
                                else
                                {
                                    myresult.Add(new CollectionSyncResult()
                                    {
                                        code = 400,
                                        ID = Convert.ToInt32(item.OfflineId),
                                        status = "error",
                                        message = "Invalid House ID",
                                        messageMar = "अवैध घर आयडी",
                                        referenceID = item.ReferanceId,
                                    });
                                }

                            }

                     
                        }
                        else
                        {
                            myresult.Add(new CollectionSyncResult()
                            {
                                code = 400,
                                ID = Convert.ToInt32(item.OfflineId),
                                status = "Error",
                                message = "Your outside the area,please go to inside the area.. ",
                                messageMar = "तुम्ही क्षेत्राबाहेर आहात.कृपया परिसरात जा..",
                                referenceID = item.ReferanceId,
                            });


                        }

                    }


                    if (appdetails != null)
                    {
                        appdetails.FAQ = "1";
                        await dbMain.SaveChangesAsync();
                    }
                    //List<AppDetail> AppDetailss = dbMain.Database.SqlQuery<AppDetail>("exec [Update_Trigger]").ToList();
                    List<AppDetail> AppDetailss = dbMain.AppDetails.FromSqlRaw<AppDetail>("exec [Update_Trigger]").ToList();

                    var updateappdetails = await dbMain.SP_DailyScanCount_Results.FromSqlRaw<SP_DailyScanCount_Result>($"EXEC DailyScanCount {AppId.ToString()}").ToListAsync();


                    return myresult;


                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.ToString(), ex);
                    myresult.Add(new CollectionSyncResult()
                    {
                        code = 400,
                        ID = 0,
                        //  message = "Something is wrong,Try Again.. ",
                        message = ex.Message,
                        messageMar = "काहीतरी चुकीचे आहे, पुन्हा प्रयत्न करा..",
                        status = "error",
                        referenceID = refId,
                    });
                    return myresult;
                }


            }

        }

        public async Task<List<SBWorkDetails>> GetQrWorkHistoryAsync(int userId, int year, int month, int appId)
        {
            List<SBWorkDetails> obj = new List<SBWorkDetails>();
            using (DevICTSBMChildEntities db = new DevICTSBMChildEntities(appId))
            {
                try
                {
                    //var data = db.GetQrWorkHistory(userId, year, month).ToList();
                    List<SqlParameter> parms = new List<SqlParameter>
                                                {
                                                    // Create parameter(s)    
                                                    new SqlParameter { ParameterName = "@userid", Value = userId },
                                                    new SqlParameter { ParameterName = "@year", Value = year },
                                                    new SqlParameter { ParameterName = "@month", Value = month },
                                                };
                    var data = await db.GetQrWorkHistory_Results.FromSqlRaw<GetQrWorkHistory_Result>("EXEC GetQrWorkHistory @userid,@year,@month", parms.ToArray()).ToListAsync();

                    foreach (var x in data)
                    {

                        obj.Add(new SBWorkDetails()
                        {
                            date = (x.Day).ToString(),
                            houseCollection = checkIntNull(x.HouseCollection.ToString()),
                            pointCollection = checkIntNull(x.PointCollection.ToString()),
                            DumpYardCollection = checkIntNull(x.DumpYardCollection.ToString()),
                            LiquidCollection = checkIntNull(x.LiquidCollection.ToString()),
                            StreetCollection = checkIntNull(x.StreetCollection.ToString()),
                            SurveyCollection = checkIntNull(x.SurveyCollection.ToString()),
                        });
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.ToString(), ex);
                    return obj;
                }
            }
            return obj;
        }
        public async Task<List<BigVQrworkhistorydetails>> GetQrWorkHistoryDetailsAsync(DateTime date, int AppId, int userId)
        {
            List<BigVQrworkhistorydetails> obj = new List<BigVQrworkhistorydetails>();

            using (DevICTSBMChildEntities db = new DevICTSBMChildEntities(AppId))
            using (DevICTSBMMainEntities dbMain = new DevICTSBMMainEntities())
            {
                try
                {
                    var appdetails = await dbMain.AppDetails.Where(c => c.AppId == AppId).FirstOrDefaultAsync();
                    var data = await db.DumpYardDetails.Where(c => EF.Functions.DateDiffDay(c.lastModifiedDate, date) == 0 && c.userId == userId).ToListAsync();

                    foreach (var x in data)
                    {

                        obj.Add(new BigVQrworkhistorydetails()
                        {
                            Date = Convert.ToDateTime(x.lastModifiedDate).ToString("MM/dd/yyyy"),
                            time = Convert.ToDateTime(x.lastModifiedDate).ToString("HH:mm"),
                            DumpYardNo = x.ReferanceId,
                            type = 3

                        });

                    }

                    var data1 = await db.GarbagePointDetails.Where(c => EF.Functions.DateDiffDay(c.modified, date) == 0 && c.userId == userId).ToListAsync();
                    foreach (var y in data1)
                    {
                        obj.Add(new BigVQrworkhistorydetails()
                        {
                            Date = Convert.ToDateTime(y.modified).ToString("MM/dd/yyyy"),
                            time = Convert.ToDateTime(y.modified).ToString("HH:mm"),
                            PointNo = y.ReferanceId,

                            type = 2
                        });
                    }

                    var data2 = await db.HouseMasters.Where(c => EF.Functions.DateDiffDay(c.modified, date) == 0 && c.userId == userId).ToListAsync();
                    foreach (var z in data2)
                    {
                        obj.Add(new BigVQrworkhistorydetails()
                        {
                            Date = Convert.ToDateTime(z.modified).ToString("MM/dd/yyyy"),
                            time = Convert.ToDateTime(z.modified).ToString("HH:mm"),
                            HouseNo = z.ReferanceId,
                            type = 1

                        });
                    }
                    var data3 = await db.LiquidWasteDetails.Where(c => EF.Functions.DateDiffDay(c.lastModifiedDate, date) == 0 && c.userId == userId).ToListAsync();
                    foreach (var z in data3)
                    {
                        obj.Add(new BigVQrworkhistorydetails()
                        {
                            Date = Convert.ToDateTime(z.lastModifiedDate).ToString("MM/dd/yyyy"),
                            time = Convert.ToDateTime(z.lastModifiedDate).ToString("HH:mm"),
                            LiquidNo = z.ReferanceId,
                            type = 4

                        });
                    }
                    var data4 = await db.StreetSweepingDetails.Where(c => EF.Functions.DateDiffDay(c.lastModifiedDate, date) == 0 && c.userId == userId).ToListAsync();
                    foreach (var z in data4)
                    {
                        obj.Add(new BigVQrworkhistorydetails()
                        {
                            Date = Convert.ToDateTime(z.lastModifiedDate).ToString("MM/dd/yyyy"),
                            time = Convert.ToDateTime(z.lastModifiedDate).ToString("HH:mm"),
                            StreetNo = z.ReferanceId,
                            type = 5

                        });
                    }
                    var data5 = await db.SurveyFormDetails.Where(c => EF.Functions.DateDiffDay(c.CreateDate, date) == 0 && c.CreateUserId == userId).ToListAsync();
                    foreach (var z in data5)
                    {
                        obj.Add(new BigVQrworkhistorydetails()
                        {
                            Date = Convert.ToDateTime(z.CreateDate).ToString("MM/dd/yyyy"),
                            time = Convert.ToDateTime(z.CreateDate).ToString("HH:mm"),
                            SurveyNo = z.ReferanceId,
                            type = 11

                        });
                    }
                    return obj.OrderBy(c => c.Date).OrderBy(c => c.time).ToList();

                    //List<SqlParameter> parms = new List<SqlParameter>
                    //                            {
                    //                                // Create parameter(s)    
                    //                                new SqlParameter { ParameterName = "@userId", Value = userId },
                    //                                new SqlParameter { ParameterName = "@date", Value =  Convert.ToDateTime(date).ToString("MM/dd/yyyy") }
                    //                            };
                    //var Newdata = await db.GetQrWorkHistoryDetails_Results.FromSqlRaw<GetQrWorkHistoryDetails_Result>("EXEC GetQrWorkHistoryDetails @userId,@date", parms.ToArray()).ToListAsync();

                    //foreach(var x in Newdata)
                    //{
                    //    obj.Add(new BigVQrworkhistorydetails()
                    //    {
                    //        Date = Convert.ToDateTime(x.Date).ToString("MM/dd/yyyy"),
                    //        time = Convert.ToDateTime(x.time).ToString("HH:mm"),
                    //        DumpYardNo = x.DumpYardNo,
                    //        HouseNo = x.HouseNo,
                    //        LiquidNo = x.LiquidNo,
                    //        StreetNo = x.StreetNo,
                    //        type = x.type

                    //    });
                    //}
                    //return obj.OrderBy(c => c.Date).OrderBy(c => c.time).ToList();  // Bad Response Time 


                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.ToString(), ex);
                    return obj;
                }


            }
        }

        public async Task<BigVQRHPDVM2> GetScanifyHouseDetailsDataAsync(int appId, string ReferenceId, int gcType)
        {
            BigVQRHPDVM2 Details = new BigVQRHPDVM2();

            try
            {


                using (var db = new DevICTSBMChildEntities(appId))
                using (DevICTSBMMainEntities dbMain = new DevICTSBMMainEntities())
                {
                    var appDetails = dbMain.AppDetails.Where(x => x.AppId == appId).FirstOrDefault();
                    string ThumbnaiUrlCMS = appDetails.baseImageUrlCMS + appDetails.basePath + appDetails.HouseQRCode + "/";
                    //var model = db.SP_HousePointDumpDetails_Scanify(ReferenceId, gcType).FirstOrDefault();
                    List<SqlParameter> parms = new List<SqlParameter>
                                                {
                                                    // Create parameter(s)    
                                                    new SqlParameter { ParameterName = "@referenceId", Value = ReferenceId },
                                                    new SqlParameter { ParameterName = "@gcType", Value = gcType }
                                                };
                    var Listmodel = await db.SP_HousePointDumpDetails_Scanify_Results.FromSqlRaw<SP_HousePointDumpDetails_Scanify_Result>("EXEC SP_HousePointDumpDetails_Scanify @referenceId,@gcType", parms.ToArray()).ToListAsync();
                    var model = Listmodel == null ? null : Listmodel.FirstOrDefault();

                    if (gcType == 1)
                    {
                        if (model != null)
                        {
                            Details.houseId = model.houseId.ToString();
                            Details.wardId = model.WardNoId;
                            Details.areaId = model.AreaId;
                            Details.zoneId = model.zoneId;
                            Details.Address = model.Address;
                            Details.houseNumber = model.HouseNumber;
                            Details.mobileno = model.MobileNumber;
                            Details.name = model.Name;
                            Details.namemar = model.NameMar;
                            Details.QRCode = ThumbnaiUrlCMS + model.Images.Trim();
                            Details.ReferanceId = model.ReferanceId;
                            Details.Lat = model.Lat;
                            Details.Long = model.Long;
                            Details.gcType = gcType;
                            Details.Status = "Success";
                            Details.Message = "Successfully";
                        }
                        else
                        {
                            Details.Status = "error";
                            Details.Message = "HouseID not exists";
                        }
                    }

                    else if (gcType == 2)
                    {
                        if (model != null)
                        {
                            Details.gpId = model.houseId;
                            Details.wardId = model.WardNoId;
                            Details.areaId = model.AreaId;
                            Details.zoneId = model.zoneId;
                            Details.Address = model.Address;
                            Details.houseNumber = model.HouseNumber;
                            Details.mobileno = model.MobileNumber;
                            Details.name = model.Name;
                            Details.namemar = model.NameMar;
                            Details.QRCode = ThumbnaiUrlCMS + model.Images.Trim();
                            Details.ReferanceId = model.ReferanceId;
                            Details.Lat = model.Lat;
                            Details.Long = model.Long;
                            Details.gcType = gcType;
                            Details.Status = "Success";
                            Details.Message = "Successfully";
                        }
                        else
                        {
                            Details.Status = "error";
                            Details.Message = "PointID not exists";
                        }
                    }
                    else if (gcType == 3)
                    {
                        if (model != null)
                        {
                            Details.dyId = model.houseId.ToString();
                            Details.wardId = model.WardNoId;
                            Details.areaId = model.AreaId;
                            Details.zoneId = model.zoneId;
                            Details.Address = model.Address;
                            Details.houseNumber = model.HouseNumber;
                            Details.mobileno = model.MobileNumber;
                            Details.name = model.Name;
                            Details.namemar = model.NameMar;
                            Details.QRCode = ThumbnaiUrlCMS + model.Images.Trim();
                            Details.ReferanceId = model.ReferanceId;
                            Details.Lat = model.Lat;
                            Details.Long = model.Long;
                            Details.gcType = gcType;
                            Details.Status = "Success";
                            Details.Message = "Successfully";
                        }
                        else
                        {
                            Details.Status = "error";
                            Details.Message = "DumpYardID not exists";
                        }
                    }


                    return Details;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString(), ex);
                return Details;
            }
        }
        public async Task<List<VehicleList>> GetVehicleListAsync(int appId, int VehicleTypeId)
        {
            List<VehicleList> obj = new List<VehicleList>();
            try
            {
                using (DevICTSBMChildEntities db = new DevICTSBMChildEntities(appId))
                {
                    {
                        //var data = db.Vehical_QR_Master.Where(c => c.VehicalType == VehicleTypeId.ToString()).ToList();
                        //var data = db.VehicleList_TypeWise(VehicleTypeId).ToList();
                        List<SqlParameter> parms = new List<SqlParameter>
                                                {
                                                    // Create parameter(s)    
                                                    new SqlParameter { ParameterName = "@vehicleType", Value = VehicleTypeId }
                                                };
                        var data = await db.VehicleList_TypeWise_Results.FromSqlRaw<VehicleList_TypeWise_Result>("EXEC VehicleList_TypeWise @vehicleType", parms.ToArray()).ToListAsync();


                        foreach (var x in data)
                        {
                            obj.Add(new VehicleList()
                            {
                                VehicleNo = (x.VehicalNumber.ToString()),
                                VehicleId = (x.vqrId),
                                TypeId = Convert.ToInt32(x.VehicalType),

                            });
                        }
                    }

                    return obj.OrderBy(c => c.VehicleId).ToList();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString(), ex);
                return obj;
            }

        }

        public async Task<SBUserView> GetUserAsync(int AppId, int userId, int typeId)
        {
            SBUserView user = new SBUserView();
            try
            {

                using (DevICTSBMChildEntities db = new DevICTSBMChildEntities(AppId))
                using (DevICTSBMMainEntities dbMain = new DevICTSBMMainEntities())
                {
                    AppDetail objmain = await dbMain.AppDetails.Where(x => x.AppId == AppId).FirstOrDefaultAsync();

                    if (typeId == 0 || typeId == 2)
                    {
                        var obj = await db.UserMasters.Where(c => c.userId == userId).FirstOrDefaultAsync();
                        if (obj != null)
                        {
                            user.type = obj.EmployeeType;
                            user.typeId = obj.Type == null ? 0 : int.Parse(obj.Type);
                            user.userId = obj.userEmployeeNo;
                            user.name = checkNull(obj.userName);
                            user.nameMar = checkNull(obj.userNameMar);
                            user.mobileNumber = obj.userMobileNumber;
                            user.address = obj.userAddress;
                            user.bloodGroup = checkNull(obj.bloodGroup);
                            user.profileImage = ImagePathCMS(objmain.UserProfile, obj.userProfileImage, objmain);
                        }
                    }
                    else if (typeId == 1)
                    {
                        var obj = await db.QrEmployeeMasters.Where(c => c.qrEmpId == userId).FirstOrDefaultAsync();
                        if (obj != null)
                        {
                            user.type = obj.type;
                            user.typeId = Convert.ToInt32(obj.typeId);
                            user.userId = obj.userEmployeeNo;
                            user.name = checkNull(obj.qrEmpName);
                            user.nameMar = checkNull(obj.qrEmpNameMar);
                            user.mobileNumber = obj.qrEmpMobileNumber;
                            user.address = obj.qrEmpAddress;
                            user.bloodGroup = checkNull(obj.bloodGroup);
                            user.profileImage = ""; //ImagePathCMS(objmain.UserProfile, obj., objmain);
                        }
                    }

                }
                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString(), ex);
                return user;
            }
        }


        public async Task<Result> GetVersionUpdateAsync(string version, int AppId)
        {
            Result result = new Result();
            try
            {
                using (DevICTSBMMainEntities dbMain = new DevICTSBMMainEntities())
                {
                    var appdetails = await dbMain.AppDetails.Where(c => c.AppId == AppId).FirstOrDefaultAsync();

                    if (appdetails.AppVersion != version && appdetails.ForceUpdate == true)
                    {
                        if (Convert.ToInt32(appdetails.AppVersion) <= Convert.ToInt32(version))
                        {
                            result.status = "false";
                            result.message = "";
                            result.messageMar = "";
                            result.applink = "";
                            return result;
                        }
                        else
                        {
                            result.status = "true";
                            result.message = "";
                            result.messageMar = "";
                            result.applink = appdetails.AppLink;
                            return result;
                        }
                    }

                    else
                    {

                        result.status = "false";
                        result.message = "";
                        result.messageMar = "";
                        result.applink = "";
                        return result;
                    }

                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString(), ex);
                return result;
            }
        }
        public async Task<Result> CheckAttendenceAsync(int userId, int AppId, int typeId)
        {

            Daily_Attendance objatten = new Daily_Attendance();
            Qr_Employee_Daily_Attendance objqratten = new Qr_Employee_Daily_Attendance();
            Result result = new Result();

            try
            {
                using (DevICTSBMChildEntities db = new DevICTSBMChildEntities(AppId))
                using (DevICTSBMMainEntities dbMain = new DevICTSBMMainEntities())
                {
                    var appdetails = await dbMain.AppDetails.Where(c => c.AppId == AppId).FirstOrDefaultAsync();
                    if (typeId == 0)
                    {
                        objatten = await db.Daily_Attendances.Where(c => c.userId == userId & c.endTime == "" && EF.Functions.DateDiffDay(c.daDate, DateTime.Now) == 0).FirstOrDefaultAsync();
                    }

                    else
                    {
                        objqratten = await db.Qr_Employee_Daily_Attendances.Where(c => c.qrEmpId == userId & c.endTime == "" && EF.Functions.DateDiffDay(c.startDate, DateTime.Now) == 0).FirstOrDefaultAsync();
                    }
                    if (objatten == null || objqratten == null)
                    {
                        result.isAttendenceOff = true;
                        result.message = "Your duty is currently off, please start again.. ";
                        result.messageMar = "आपली ड्यूटी सध्या बंद आहे, कृपया पुन्हा सुरू करा..";
                        result.status = "success";
                        return result;
                    }
                    else
                    {
                        result.isAttendenceOff = false;
                        result.message = "";
                        result.messageMar = "";
                        result.status = "success";
                        return result;

                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString(), ex);
                return result;
            }

            // return result;


        }

        public async Task<List<SyncResult1>> SaveUserAttendenceOfflineAsync(List<SBUserAttendence> obj, int AppId, string cdate, string EmpType)
        {

            List<SyncResult1> result = new List<SyncResult1>();
            //double aaaa = distance(21.142180, 79.067540, 21.138420, 79.069210, 'k');

            if (EmpType == "N")
            {
                result = await SaveUserAttendenceOfflineForNormalAsync(obj, AppId, cdate, EmpType);
            }
            if (EmpType == "L")
            {
                result = await SaveUserAttendenceOfflineForLiquidAsync(obj, AppId, cdate, EmpType);
            }
            if (EmpType == "S")
            {
                result = await SaveUserAttendenceOfflineForStreetAsync(obj, AppId, cdate, EmpType);
            }

            if (EmpType == "D")
            {
                result = await SaveUserAttendenceOfflineForDumpAsync(obj, AppId, cdate, EmpType);
            }
            return result;


        }
        public async Task<List<SyncResult1>> SaveUserAttendenceOfflineForNormalAsync(List<SBUserAttendence> obj, int AppId, string cdate, string EmpType)
        {
            List<SyncResult1> result = new List<SyncResult1>();
            try
            {
                using (DevICTSBMChildEntities db = new DevICTSBMChildEntities(AppId))
                {
                    Daily_Attendance attendance = new Daily_Attendance();

                    foreach (var x in obj)
                    {
                        var Vehicaldetail = await db.Vehical_QR_Masters.Where(c => c.ReferanceId == x.ReferanceId && c.VehicalType != null && c.VehicalNumber != null).FirstOrDefaultAsync();
                        DateTime Datee = Convert.ToDateTime(cdate);
                        var IsSameRecordLocation = await db.Locations.Where(c => c.userId == x.userId && c.datetime == Datee && c.type == null && c.EmployeeType == null).FirstOrDefaultAsync();
                        try
                        {
                            bool _IsInSync = false, _IsOutSync = false;
                            var user = await db.UserMasters.Where(c => c.userId == x.userId && c.EmployeeType == null).FirstOrDefaultAsync();

                            if (user.isActive == true)
                            {

                                var IsSameRecord = await db.Daily_Attendances.Where(
                                       c => c.userId == x.userId &&
                                       //c.startLat == x.startLat &&
                                       //c.startLong == x.startLong &&
                                       //c.endLat == x.endLat &&
                                       //c.endLong == x.endLong &&
                                       c.startTime == x.startTime &&
                                       c.endTime == x.endTime &&
                                       EF.Functions.DateDiffDay(c.daDate, x.daDate) == 0 &&
                                       EF.Functions.DateDiffDay(c.daEndDate, x.daEndDate) == 0 &&
                                       c.EmployeeType == null
                                     ).FirstOrDefaultAsync();

                                if (IsSameRecord == null)
                                {

                                    var objdata = await db.Daily_Attendances.Where(c => EF.Functions.DateDiffDay(c.daDate, x.daDate) == 0 && c.userId == x.userId && (c.endTime == "" || c.endTime == null) && c.EmployeeType == null).FirstOrDefaultAsync();
                                    if (objdata != null && string.IsNullOrEmpty(x.endTime))
                                    {
                                        objdata.endTime = x.startTime;
                                        objdata.daEndDate = x.daDate;
                                        objdata.endLat = x.startLat;
                                        objdata.endLong = x.startLong;
                                        objdata.OutbatteryStatus = x.batteryStatus;
                                        objdata.totalKm = x.totalKm;
                                        objdata.EmployeeType = null;
                                        if ((string.IsNullOrEmpty(x.QrCodeImage)) == false)
                                        {
                                            x.QrCodeImage = x.QrCodeImage.Replace("data:image/jpeg;base64,", "");
                                            objdata.BinaryQrCodeImage = Convert.FromBase64String(x.QrCodeImage);
                                        }
                                        if (Vehicaldetail != null)
                                        {
                                            objdata.VQRId = Vehicaldetail.vqrId;
                                        }
                                        else
                                        {
                                            objdata.VQRId = null;
                                        }
                                        await db.SaveChangesAsync();
                                    }
                                    if (objdata != null)
                                    {

                                        objdata.userId = x.userId;
                                        objdata.startLat = x.startLat;
                                        objdata.startLong = x.startLong;
                                        objdata.startTime = x.startTime;
                                        objdata.daDate = x.daDate;

                                        if (Vehicaldetail != null)
                                        {
                                            objdata.vehicleNumber = Vehicaldetail.VehicalNumber;
                                            objdata.vtId = Vehicaldetail.VehicalType;
                                        }
                                        else
                                        {
                                            objdata.vehicleNumber = x.vehicleNumber;
                                            objdata.vtId = x.vtId;
                                        }
                                        objdata.EmployeeType = null;

                                        //objdata.daEndDate = x.daEndDate;

                                        if (x.daEndDate.Equals(DateTime.MinValue))
                                        {
                                            objdata.daEndDate = null;
                                            objdata.endTime = (string.IsNullOrEmpty(x.endTime) ? "" : x.endTime);
                                        }
                                        else
                                        {
                                            //objdata.daEndDate = x.daEndDate;
                                            if (x.daEndDate == x.daDate)
                                            {
                                                objdata.daEndDate = x.daEndDate;
                                                objdata.endTime = (string.IsNullOrEmpty(x.endTime) ? "" : x.endTime);
                                            }
                                            else
                                            {
                                                objdata.daEndDate = x.daDate;
                                                objdata.endTime = "11:50 PM";
                                            }
                                        }

                                        objdata.endLat = x.endLat;
                                        objdata.endLong = x.endLong;
                                        //objdata.endTime = (string.IsNullOrEmpty(x.endTime) ? "" : x.endTime); //x.endTime;
                                        objdata.daStartNote = x.daStartNote;
                                        objdata.daEndNote = x.daEndNote;
                                        objdata.OutbatteryStatus = x.batteryStatus;
                                        //  objdata.batteryStatus = x.batteryStatus;
                                        if (objdata != null && string.IsNullOrEmpty(x.endTime))
                                        {
                                            if ((string.IsNullOrEmpty(x.QrCodeImage)) == false)
                                            {
                                                x.QrCodeImage = x.QrCodeImage.Replace("data:image/jpeg;base64,", "");
                                                objdata.BinaryQrCodeImage = Convert.FromBase64String(x.QrCodeImage);
                                            }
                                            if (Vehicaldetail != null)
                                            {
                                                objdata.VQRId = Vehicaldetail.vqrId;
                                            }
                                            else
                                            {
                                                objdata.VQRId = null;
                                            }
                                            db.Daily_Attendances.Add(objdata);
                                        }
                                        _IsInSync = true;

                                        if ((!string.IsNullOrEmpty(x.endLat)) && (!string.IsNullOrEmpty(x.endLong)) && IsSameRecordLocation == null)
                                        {
                                            string Time2 = x.endTime;
                                            DateTime date2 = DateTime.Parse(Time2, System.Globalization.CultureInfo.CurrentCulture);
                                            string t2 = date2.ToString("hh:mm:ss tt");
                                            string dt2 = Convert.ToDateTime(x.daEndDate).ToString("MM/dd/yyyy");
                                            DateTime? edate = Convert.ToDateTime(dt2 + " " + t2);

                                            Location loc = new Location();
                                            loc.userId = x.userId;
                                            loc.datetime = edate;
                                            loc.lat = x.endLat;
                                            loc._long = x.endLong;
                                            loc.batteryStatus = x.batteryStatus;
                                            loc.address = Address(x.endLat + "," + x.endLong);
                                            if (loc.address != "")
                                            {
                                                loc.area = area(loc.address);
                                            }
                                            else
                                            {
                                                loc.area = "";
                                            }

                                            loc.IsOffline = true;
                                            loc.CreatedDate = DateTime.Now;
                                            loc.EmployeeType = null;
                                            db.Locations.Add(loc);
                                            _IsOutSync = true;
                                            _IsInSync = false;
                                        }

                                        await db.SaveChangesAsync();
                                    }
                                    else
                                    {
                                        var OutTime = await db.Daily_Attendances.Where(a => a.userId == x.userId && a.startTime == x.startTime && a.daDate == x.daDate && a.EmployeeType == null).OrderByDescending(m => m.daID).FirstOrDefaultAsync();

                                        if (OutTime != null && OutTime.endTime == "11:50 PM")
                                        {
                                            OutTime.userId = x.userId;
                                            OutTime.startLat = x.startLat;
                                            OutTime.startLong = x.startLong;
                                            OutTime.startTime = x.startTime;
                                            OutTime.daDate = x.daDate;
                                            OutTime.vehicleNumber = x.vehicleNumber;
                                            OutTime.vtId = x.vtId;
                                            OutTime.EmployeeType = null;

                                            if (x.daEndDate.Equals(DateTime.MinValue))
                                            {
                                                OutTime.daEndDate = null;
                                                OutTime.endTime = (string.IsNullOrEmpty(x.endTime) ? "" : x.endTime);
                                            }
                                            else
                                            {
                                                if (x.daEndDate == x.daDate)
                                                {
                                                    OutTime.daEndDate = x.daEndDate;
                                                    OutTime.endTime = (string.IsNullOrEmpty(x.endTime) ? "" : x.endTime);
                                                }
                                                else
                                                {
                                                    OutTime.daEndDate = x.daDate;
                                                    OutTime.endTime = "11:50 PM";
                                                }

                                            }

                                            OutTime.endLat = x.endLat;
                                            OutTime.endLong = x.endLong;
                                            //OutTime.endTime = (string.IsNullOrEmpty(x.endTime) ? "" : x.endTime); //x.endTime;
                                            OutTime.daStartNote = x.daStartNote;
                                            OutTime.daEndNote = x.daEndNote;
                                            OutTime.batteryStatus = x.batteryStatus;

                                            //db.Daily_Attendance.Add(attendance);
                                            _IsInSync = true;

                                        }
                                        else
                                        {
                                            attendance.userId = x.userId;
                                            attendance.startLat = x.startLat;
                                            attendance.startLong = x.startLong;
                                            attendance.startTime = x.startTime;
                                            attendance.daDate = x.daDate;

                                            if (Vehicaldetail != null)
                                            {
                                                attendance.vtId = Vehicaldetail.VehicalType;
                                                attendance.vehicleNumber = Vehicaldetail.VehicalNumber;
                                            }
                                            else
                                            {
                                                attendance.vtId = x.vtId;
                                                attendance.vehicleNumber = x.vehicleNumber;
                                            }
                                            attendance.EmployeeType = null;

                                            if ((string.IsNullOrEmpty(x.QrCodeImage)) == false)
                                            {
                                                x.QrCodeImage = x.QrCodeImage.Replace("data:image/jpeg;base64,", "");
                                                attendance.BinaryQrCodeImage = Convert.FromBase64String(x.QrCodeImage);
                                            }
                                            if (Vehicaldetail != null)
                                            {
                                                attendance.VQRId = Vehicaldetail.vqrId;
                                            }
                                            else
                                            {
                                                attendance.VQRId = null;
                                            }
                                            if (x.daEndDate.Equals(DateTime.MinValue))
                                            {
                                                attendance.daEndDate = null;
                                                attendance.endTime = (string.IsNullOrEmpty(x.endTime) ? "" : x.endTime);
                                            }
                                            else
                                            {
                                                if (x.daEndDate == x.daDate)
                                                {
                                                    attendance.daEndDate = x.daEndDate;
                                                    attendance.endTime = (string.IsNullOrEmpty(x.endTime) ? "" : x.endTime);

                                                }
                                                else
                                                {
                                                    attendance.daEndDate = x.daDate;
                                                    attendance.endTime = "11:50 PM";
                                                }
                                            }

                                            attendance.endLat = x.endLat;
                                            attendance.endLong = x.endLong;
                                            //attendance.endTime = (string.IsNullOrEmpty(x.endTime) ? "" : x.endTime); //x.endTime;
                                            attendance.daStartNote = x.daStartNote;
                                            attendance.daEndNote = x.daEndNote;
                                            attendance.batteryStatus = x.batteryStatus;
                                            if (OutTime != null)
                                            {
                                                if (OutTime.endTime == "" || OutTime.endTime == null)
                                                {
                                                    db.Daily_Attendances.Add(attendance);
                                                }
                                                OutTime.endTime = (string.IsNullOrEmpty(x.endTime) ? "" : x.endTime);
                                            }
                                            if (OutTime == null)
                                            {
                                                db.Daily_Attendances.Add(attendance);
                                            }
                                            _IsInSync = true;
                                            //  db.SaveChanges();
                                            //if ((!string.IsNullOrEmpty(x.endLat))  && (!string.IsNullOrEmpty(x.endLong)))
                                            //{
                                            //    string Time2 = x.endTime;
                                            //    DateTime date2 = DateTime.Parse(Time2, System.Globalization.CultureInfo.CurrentCulture);
                                            //    string t2 = date2.ToString("hh:mm:ss tt");
                                            //    string dt2 = Convert.ToDateTime(x.daEndDate).ToString("MM/dd/yyyy");
                                            //    DateTime? edate = Convert.ToDateTime(dt2 + " " + t2);

                                            //    Location loc = new Location();
                                            //    loc.userId = x.userId;
                                            //    loc.datetime = edate;
                                            //    loc.lat = x.endLat;
                                            //    loc.@long = x.endLong;
                                            //    loc.batteryStatus = x.batteryStatus;
                                            //    loc.address = Address(x.endLat + "," + x.endLong);
                                            //    if (loc.address != "")
                                            //    {
                                            //        loc.area = area(loc.address);
                                            //    }
                                            //    else
                                            //    {
                                            //        loc.area = "";
                                            //    }

                                            //    loc.IsOffline = true;
                                            //    loc.CreatedDate = DateTime.Now;

                                            //    db.Locations.Add(loc);
                                            //    _IsOutSync = true;
                                            //}

                                            //db.SaveChanges();

                                        }
                                        if ((!string.IsNullOrEmpty(x.endLat)) && (!string.IsNullOrEmpty(x.endLong)) && IsSameRecordLocation == null)
                                        {
                                            string Time2 = x.endTime;
                                            DateTime date2 = DateTime.Parse(Time2, System.Globalization.CultureInfo.CurrentCulture);
                                            string t2 = date2.ToString("hh:mm:ss tt");
                                            string dt2 = Convert.ToDateTime(x.daEndDate).ToString("MM/dd/yyyy");
                                            DateTime? edate = Convert.ToDateTime(dt2 + " " + t2);

                                            Location loc = new Location();
                                            loc.userId = x.userId;
                                            loc.datetime = edate;
                                            loc.lat = x.endLat;
                                            loc._long = x.endLong;
                                            loc.batteryStatus = x.batteryStatus;
                                            loc.address = Address(x.endLat + "," + x.endLong);
                                            if (loc.address != "")
                                            {
                                                loc.area = area(loc.address);
                                            }
                                            else
                                            {
                                                loc.area = "";
                                            }

                                            loc.IsOffline = true;
                                            loc.CreatedDate = DateTime.Now;
                                            loc.EmployeeType = null;
                                            db.Locations.Add(loc);
                                            _IsOutSync = true;

                                        }

                                        if ((!string.IsNullOrEmpty(x.startLat)) && (!string.IsNullOrEmpty(x.startLong)) && IsSameRecordLocation == null)
                                        {
                                            string Time2 = x.startTime;
                                            DateTime date2 = DateTime.Parse(Time2, System.Globalization.CultureInfo.CurrentCulture);
                                            string t2 = date2.ToString("hh:mm:ss tt");
                                            string dt2 = Convert.ToDateTime(x.daDate).ToString("MM/dd/yyyy");
                                            DateTime? sdate = Convert.ToDateTime(dt2 + " " + t2);

                                            Location loc = new Location();
                                            loc.userId = x.userId;
                                            loc.datetime = sdate;
                                            loc.lat = x.startLat;
                                            loc._long = x.startLong;
                                            loc.batteryStatus = x.batteryStatus;
                                            loc.address = Address(x.startLat + "," + x.startLong);
                                            if (loc.address != "")
                                            {
                                                loc.area = area(loc.address);
                                            }
                                            else
                                            {
                                                loc.area = "";
                                            }

                                            loc.IsOffline = true;
                                            loc.CreatedDate = DateTime.Now;
                                            loc.EmployeeType = null;
                                            db.Locations.Add(loc);
                                            _IsOutSync = false;
                                        }
                                        await db.SaveChangesAsync();
                                    }

                                    result.Add(new SyncResult1()
                                    {
                                        ID = x.OfflineID,
                                        status = "success",
                                        message = "Shift Ended Successfully",
                                        messageMar = "शिफ्ट संपले",
                                        IsInSync = _IsInSync,
                                        IsOutSync = _IsOutSync,
                                        EmpType = "N",

                                    });
                                }
                                else
                                {
                                    result.Add(new SyncResult1()
                                    {
                                        ID = x.OfflineID,
                                        status = "success",
                                        message = "Shift started Successfully",
                                        messageMar = "शिफ्ट सुरू",
                                        IsInSync = true,
                                        IsOutSync = true,
                                        EmpType = "N",
                                    });
                                }
                            }

                        }
                        catch (Exception ex)
                        {
                            result.Add(new SyncResult1()
                            {
                                ID = x.OfflineID,
                                status = "error",
                                message = "Something is wrong,Try Again.. ",
                                messageMar = "काहीतरी चुकीचे आहे, पुन्हा प्रयत्न करा..",
                                IsInSync = false,
                                IsOutSync = false,
                                EmpType = "N",
                            });
                            return result;
                        }
                    }

                }
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString(), ex);
                return result;
            }
        }
        public async Task<List<SyncResult1>> SaveUserAttendenceOfflineForLiquidAsync(List<SBUserAttendence> obj, int AppId, string cdate, string EmpType)
        {
            List<SyncResult1> result = new List<SyncResult1>();
            try
            {
                using (DevICTSBMChildEntities db = new DevICTSBMChildEntities(AppId))
                {
                    Daily_Attendance attendance = new Daily_Attendance();

                    foreach (var x in obj)
                    {
                        try
                        {
                            DateTime Datee = Convert.ToDateTime(cdate);
                            var IsSameRecordLocation = await db.Locations.Where(c => c.userId == x.userId && c.datetime == Datee && c.type == null && c.EmployeeType == "L").FirstOrDefaultAsync();

                            bool _IsInSync = false, _IsOutSync = false;
                            var user = await db.UserMasters.Where(c => c.userId == x.userId && c.EmployeeType == "L").FirstOrDefaultAsync();

                            if (user.isActive == true)
                            {

                                var IsSameRecord = db.Daily_Attendances.Where(
                                       c => c.userId == x.userId &&
                                       //c.startLat == x.startLat &&
                                       //c.startLong == x.startLong &&
                                       //c.endLat == x.endLat &&
                                       //c.endLong == x.endLong &&
                                       c.startTime == x.startTime &&
                                       c.endTime == x.endTime &&
                                       EF.Functions.DateDiffDay(c.daDate, x.daDate) == 0 &&
                                       EF.Functions.DateDiffDay(c.daEndDate, x.daEndDate) == 0 &&
                                       c.EmployeeType == "L"
                                     ).FirstOrDefault();

                                if (IsSameRecord == null)
                                {

                                    var objdata = await db.Daily_Attendances.Where(c => EF.Functions.DateDiffDay(c.daDate, x.daDate) == 0 && c.userId == x.userId && (c.endTime == "" || c.endTime == null) && c.EmployeeType == "L").FirstOrDefaultAsync();
                                    if (objdata != null && x.endTime == null)
                                    {
                                        objdata.endTime = x.startTime;
                                        objdata.daEndDate = x.daDate;
                                        objdata.endLat = x.startLat;
                                        objdata.endLong = x.startLong;
                                        objdata.OutbatteryStatus = x.batteryStatus;
                                        objdata.totalKm = x.totalKm;
                                        objdata.EmployeeType = "L";
                                        db.SaveChanges();
                                    }
                                    if (objdata != null)
                                    {

                                        objdata.userId = x.userId;
                                        objdata.startLat = x.startLat;
                                        objdata.startLong = x.startLong;
                                        objdata.startTime = x.startTime;
                                        objdata.daDate = x.daDate;
                                        objdata.vehicleNumber = x.vehicleNumber;
                                        objdata.vtId = x.vtId;
                                        objdata.EmployeeType = "L";
                                        //objdata.daEndDate = x.daEndDate;

                                        if (x.daEndDate.Equals(DateTime.MinValue))
                                        {
                                            objdata.daEndDate = null;
                                            objdata.endTime = (string.IsNullOrEmpty(x.endTime) ? "" : x.endTime);
                                        }
                                        else
                                        {
                                            //objdata.daEndDate = x.daEndDate;
                                            if (x.daEndDate == x.daDate)
                                            {
                                                objdata.daEndDate = x.daEndDate;
                                                objdata.endTime = (string.IsNullOrEmpty(x.endTime) ? "" : x.endTime);
                                            }
                                            else
                                            {
                                                objdata.daEndDate = x.daDate;
                                                objdata.endTime = "11:50 PM";
                                            }
                                        }

                                        objdata.endLat = x.endLat;
                                        objdata.endLong = x.endLong;
                                        //objdata.endTime = (string.IsNullOrEmpty(x.endTime) ? "" : x.endTime); //x.endTime;
                                        objdata.daStartNote = x.daStartNote;
                                        objdata.daEndNote = x.daEndNote;
                                        objdata.OutbatteryStatus = x.batteryStatus;
                                        //  objdata.batteryStatus = x.batteryStatus;
                                        if (objdata != null && x.endTime == null)
                                        {
                                            db.Daily_Attendances.Add(objdata);
                                        }
                                        _IsInSync = true;

                                        if (x.endLat != null && x.endLong != null && IsSameRecordLocation == null)
                                        {
                                            string Time2 = x.endTime;
                                            DateTime date2 = DateTime.Parse(Time2, System.Globalization.CultureInfo.CurrentCulture);
                                            string t2 = date2.ToString("hh:mm:ss tt");
                                            string dt2 = Convert.ToDateTime(x.daEndDate).ToString("MM/dd/yyyy");
                                            DateTime? edate = Convert.ToDateTime(dt2 + " " + t2);

                                            Location loc = new Location();
                                            loc.userId = x.userId;
                                            loc.datetime = edate;
                                            loc.lat = x.endLat;
                                            loc._long = x.endLong;
                                            loc.batteryStatus = x.batteryStatus;
                                            loc.address = Address(x.endLat + "," + x.endLong);
                                            if (loc.address != "")
                                            {
                                                loc.area = area(loc.address);
                                            }
                                            else
                                            {
                                                loc.area = "";
                                            }

                                            loc.IsOffline = true;
                                            loc.CreatedDate = DateTime.Now;
                                            loc.EmployeeType = "L";
                                            db.Locations.Add(loc);
                                            _IsOutSync = true;
                                            _IsInSync = false;
                                        }

                                        await db.SaveChangesAsync();
                                    }
                                    else
                                    {
                                        var OutTime = await db.Daily_Attendances.Where(a => a.userId == x.userId && a.startTime == x.startTime && a.daDate == x.daDate && a.EmployeeType == "L").OrderByDescending(m => m.daID).FirstOrDefaultAsync();

                                        if (OutTime != null && OutTime.endTime == "11:50 PM")
                                        {
                                            OutTime.userId = x.userId;
                                            OutTime.startLat = x.startLat;
                                            OutTime.startLong = x.startLong;
                                            OutTime.startTime = x.startTime;
                                            OutTime.daDate = x.daDate;
                                            OutTime.vehicleNumber = x.vehicleNumber;
                                            OutTime.vtId = x.vtId;
                                            OutTime.EmployeeType = "L";
                                            if (x.daEndDate.Equals(DateTime.MinValue))
                                            {
                                                OutTime.daEndDate = null;
                                                OutTime.endTime = (string.IsNullOrEmpty(x.endTime) ? "" : x.endTime);
                                            }
                                            else
                                            {
                                                if (x.daEndDate == x.daDate)
                                                {
                                                    OutTime.daEndDate = x.daEndDate;
                                                    OutTime.endTime = (string.IsNullOrEmpty(x.endTime) ? "" : x.endTime);
                                                }
                                                else
                                                {
                                                    OutTime.daEndDate = x.daDate;
                                                    OutTime.endTime = "11:50 PM";
                                                }

                                            }

                                            OutTime.endLat = x.endLat;
                                            OutTime.endLong = x.endLong;
                                            //OutTime.endTime = (string.IsNullOrEmpty(x.endTime) ? "" : x.endTime); //x.endTime;
                                            OutTime.daStartNote = x.daStartNote;
                                            OutTime.daEndNote = x.daEndNote;
                                            OutTime.batteryStatus = x.batteryStatus;

                                            //db.Daily_Attendance.Add(attendance);
                                            _IsInSync = true;

                                        }
                                        else
                                        {
                                            attendance.userId = x.userId;
                                            attendance.startLat = x.startLat;
                                            attendance.startLong = x.startLong;
                                            attendance.startTime = x.startTime;
                                            attendance.daDate = x.daDate;
                                            attendance.vehicleNumber = x.vehicleNumber;
                                            attendance.vtId = x.vtId;
                                            attendance.EmployeeType = "L";
                                            if (x.daEndDate.Equals(DateTime.MinValue))
                                            {
                                                attendance.daEndDate = null;
                                                attendance.endTime = (string.IsNullOrEmpty(x.endTime) ? "" : x.endTime);
                                            }
                                            else
                                            {
                                                if (x.daEndDate == x.daDate)
                                                {
                                                    attendance.daEndDate = x.daEndDate;
                                                    attendance.endTime = (string.IsNullOrEmpty(x.endTime) ? "" : x.endTime);

                                                }
                                                else
                                                {
                                                    attendance.daEndDate = x.daDate;
                                                    attendance.endTime = "11:50 PM";
                                                }
                                            }

                                            attendance.endLat = x.endLat;
                                            attendance.endLong = x.endLong;
                                            //attendance.endTime = (string.IsNullOrEmpty(x.endTime) ? "" : x.endTime); //x.endTime;
                                            attendance.daStartNote = x.daStartNote;
                                            attendance.daEndNote = x.daEndNote;
                                            attendance.batteryStatus = x.batteryStatus;
                                            if (OutTime != null)
                                            {
                                                if (OutTime.endTime == "" || OutTime.endTime == null)
                                                {
                                                    db.Daily_Attendances.Add(attendance);
                                                }
                                                OutTime.endTime = (string.IsNullOrEmpty(x.endTime) ? "" : x.endTime);
                                            }
                                            if (OutTime == null)
                                            {
                                                db.Daily_Attendances.Add(attendance);
                                            }
                                            _IsInSync = true;
                                            //  db.SaveChanges();
                                            //if ((!string.IsNullOrEmpty(x.endLat))  && (!string.IsNullOrEmpty(x.endLong)))
                                            //{
                                            //    string Time2 = x.endTime;
                                            //    DateTime date2 = DateTime.Parse(Time2, System.Globalization.CultureInfo.CurrentCulture);
                                            //    string t2 = date2.ToString("hh:mm:ss tt");
                                            //    string dt2 = Convert.ToDateTime(x.daEndDate).ToString("MM/dd/yyyy");
                                            //    DateTime? edate = Convert.ToDateTime(dt2 + " " + t2);

                                            //    Location loc = new Location();
                                            //    loc.userId = x.userId;
                                            //    loc.datetime = edate;
                                            //    loc.lat = x.endLat;
                                            //    loc.@long = x.endLong;
                                            //    loc.batteryStatus = x.batteryStatus;
                                            //    loc.address = Address(x.endLat + "," + x.endLong);
                                            //    if (loc.address != "")
                                            //    {
                                            //        loc.area = area(loc.address);
                                            //    }
                                            //    else
                                            //    {
                                            //        loc.area = "";
                                            //    }

                                            //    loc.IsOffline = true;
                                            //    loc.CreatedDate = DateTime.Now;

                                            //    db.Locations.Add(loc);
                                            //    _IsOutSync = true;
                                            //}

                                            //db.SaveChanges();

                                        }
                                        if ((!string.IsNullOrEmpty(x.endLat)) && (!string.IsNullOrEmpty(x.endLong)) && IsSameRecordLocation == null)
                                        {
                                            string Time2 = x.endTime;
                                            DateTime date2 = DateTime.Parse(Time2, System.Globalization.CultureInfo.CurrentCulture);
                                            string t2 = date2.ToString("hh:mm:ss tt");
                                            string dt2 = Convert.ToDateTime(x.daEndDate).ToString("MM/dd/yyyy");
                                            DateTime? edate = Convert.ToDateTime(dt2 + " " + t2);

                                            Location loc = new Location();
                                            loc.userId = x.userId;
                                            loc.datetime = edate;
                                            loc.lat = x.endLat;
                                            loc._long = x.endLong;
                                            loc.batteryStatus = x.batteryStatus;
                                            loc.address = Address(x.endLat + "," + x.endLong);
                                            if (loc.address != "")
                                            {
                                                loc.area = area(loc.address);
                                            }
                                            else
                                            {
                                                loc.area = "";
                                            }

                                            loc.IsOffline = true;
                                            loc.CreatedDate = DateTime.Now;
                                            loc.EmployeeType = "L";
                                            db.Locations.Add(loc);
                                            _IsOutSync = true;
                                        }

                                        if ((!string.IsNullOrEmpty(x.startLat)) && (!string.IsNullOrEmpty(x.startLong)) && IsSameRecordLocation == null)
                                        {
                                            string Time2 = x.startTime;
                                            DateTime date2 = DateTime.Parse(Time2, System.Globalization.CultureInfo.CurrentCulture);
                                            string t2 = date2.ToString("hh:mm:ss tt");
                                            string dt2 = Convert.ToDateTime(x.daDate).ToString("MM/dd/yyyy");
                                            DateTime? sdate = Convert.ToDateTime(dt2 + " " + t2);

                                            Location loc = new Location();
                                            loc.userId = x.userId;
                                            loc.datetime = sdate;
                                            loc.lat = x.startLat;
                                            loc._long = x.startLong;
                                            loc.batteryStatus = x.batteryStatus;
                                            loc.address = Address(x.startLat + "," + x.startLong);
                                            if (loc.address != "")
                                            {
                                                loc.area = area(loc.address);
                                            }
                                            else
                                            {
                                                loc.area = "";
                                            }

                                            loc.IsOffline = true;
                                            loc.CreatedDate = DateTime.Now;
                                            loc.EmployeeType = "L";
                                            db.Locations.Add(loc);
                                            _IsOutSync = false;
                                        }
                                        await db.SaveChangesAsync();
                                    }

                                    result.Add(new SyncResult1()
                                    {
                                        ID = x.OfflineID,
                                        status = "success",
                                        message = "Shift started Successfully",
                                        messageMar = "शिफ्ट सुरू",
                                        IsInSync = _IsInSync,
                                        IsOutSync = _IsOutSync,
                                        EmpType = "L",

                                    });
                                }
                                else
                                {
                                    result.Add(new SyncResult1()
                                    {
                                        ID = x.OfflineID,
                                        status = "success",
                                        message = "Shift started Successfully",
                                        messageMar = "शिफ्ट सुरू",
                                        IsInSync = true,
                                        IsOutSync = true,
                                        EmpType = "L",
                                    });
                                }
                            }

                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex.ToString(), ex);
                            result.Add(new SyncResult1()
                            {
                                ID = x.OfflineID,
                                status = "error",
                                message = "Something is wrong,Try Again.. ",
                                messageMar = "काहीतरी चुकीचे आहे, पुन्हा प्रयत्न करा..",
                                IsInSync = false,
                                IsOutSync = false,
                                EmpType = "L",
                            });
                            return result;
                        }

                    }

                }
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString(), ex);
                return result;
            }
        }
        public async Task<List<SyncResult1>> SaveUserAttendenceOfflineForStreetAsync(List<SBUserAttendence> obj, int AppId, string cdate, string EmpType)
        {
            List<SyncResult1> result = new List<SyncResult1>();
            try
            {
                using (DevICTSBMChildEntities db = new DevICTSBMChildEntities(AppId))
                {
                    Daily_Attendance attendance = new Daily_Attendance();

                    foreach (var x in obj)
                    {
                        try
                        {
                            DateTime Datee = Convert.ToDateTime(cdate);
                            var IsSameRecordLocation = await db.Locations.Where(c => c.userId == x.userId && c.datetime == Datee && c.type == null && c.EmployeeType == "S").FirstOrDefaultAsync();

                            bool _IsInSync = false, _IsOutSync = false;
                            var user = await db.UserMasters.Where(c => c.userId == x.userId && c.EmployeeType == "S").FirstOrDefaultAsync();

                            if (user.isActive == true)
                            {

                                var IsSameRecord = await db.Daily_Attendances.Where(
                                       c => c.userId == x.userId &&
                                       //c.startLat == x.startLat &&
                                       //c.startLong == x.startLong &&
                                       //c.endLat == x.endLat &&
                                       //c.endLong == x.endLong &&
                                       c.startTime == x.startTime &&
                                       c.endTime == x.endTime &&
                                       EF.Functions.DateDiffDay(c.daDate, x.daDate) == 0 &&
                                       EF.Functions.DateDiffDay(c.daEndDate, x.daEndDate) == 0 &&
                                       c.EmployeeType == "S"
                                     ).FirstOrDefaultAsync();

                                if (IsSameRecord == null)
                                {

                                    var objdata = await db.Daily_Attendances.Where(c => EF.Functions.DateDiffDay(c.daDate, x.daDate) == 0 && c.userId == x.userId && (c.endTime == "" || c.endTime == null) && c.EmployeeType == "S").FirstOrDefaultAsync();
                                    if (objdata != null && x.endTime == null)
                                    {
                                        objdata.endTime = x.startTime;
                                        objdata.daEndDate = x.daDate;
                                        objdata.endLat = x.startLat;
                                        objdata.endLong = x.startLong;
                                        objdata.OutbatteryStatus = x.batteryStatus;
                                        objdata.totalKm = x.totalKm;
                                        objdata.EmployeeType = "S";
                                        await db.SaveChangesAsync();
                                    }
                                    if (objdata != null)
                                    {

                                        objdata.userId = x.userId;
                                        objdata.startLat = x.startLat;
                                        objdata.startLong = x.startLong;
                                        objdata.startTime = x.startTime;
                                        objdata.daDate = x.daDate;
                                        objdata.vehicleNumber = x.vehicleNumber;
                                        objdata.vtId = x.vtId;
                                        objdata.EmployeeType = "S";
                                        //objdata.daEndDate = x.daEndDate;

                                        if (x.daEndDate.Equals(DateTime.MinValue))
                                        {
                                            objdata.daEndDate = null;
                                            objdata.endTime = (string.IsNullOrEmpty(x.endTime) ? "" : x.endTime);
                                        }
                                        else
                                        {
                                            //objdata.daEndDate = x.daEndDate;
                                            if (x.daEndDate == x.daDate)
                                            {
                                                objdata.daEndDate = x.daEndDate;
                                                objdata.endTime = (string.IsNullOrEmpty(x.endTime) ? "" : x.endTime);
                                            }
                                            else
                                            {
                                                objdata.daEndDate = x.daDate;
                                                objdata.endTime = "11:50 PM";
                                            }
                                        }

                                        objdata.endLat = x.endLat;
                                        objdata.endLong = x.endLong;
                                        //objdata.endTime = (string.IsNullOrEmpty(x.endTime) ? "" : x.endTime); //x.endTime;
                                        objdata.daStartNote = x.daStartNote;
                                        objdata.daEndNote = x.daEndNote;
                                        objdata.OutbatteryStatus = x.batteryStatus;
                                        //  objdata.batteryStatus = x.batteryStatus;
                                        if (objdata != null && x.endTime == null)
                                        {
                                            db.Daily_Attendances.Add(objdata);
                                        }
                                        _IsInSync = true;

                                        if (x.endLat != null && x.endLong != null && IsSameRecordLocation == null)
                                        {
                                            string Time2 = x.endTime;
                                            DateTime date2 = DateTime.Parse(Time2, System.Globalization.CultureInfo.CurrentCulture);
                                            string t2 = date2.ToString("hh:mm:ss tt");
                                            string dt2 = Convert.ToDateTime(x.daEndDate).ToString("MM/dd/yyyy");
                                            DateTime? edate = Convert.ToDateTime(dt2 + " " + t2);

                                            Location loc = new Location();
                                            loc.userId = x.userId;
                                            loc.datetime = edate;
                                            loc.lat = x.endLat;
                                            loc._long = x.endLong;
                                            loc.batteryStatus = x.batteryStatus;
                                            loc.address = Address(x.endLat + "," + x.endLong);
                                            if (loc.address != "")
                                            {
                                                loc.area = area(loc.address);
                                            }
                                            else
                                            {
                                                loc.area = "";
                                            }

                                            loc.IsOffline = true;
                                            loc.CreatedDate = DateTime.Now;
                                            loc.EmployeeType = "S";
                                            db.Locations.Add(loc);
                                            _IsOutSync = true;
                                            _IsInSync = false;
                                        }

                                        await db.SaveChangesAsync();
                                    }
                                    else
                                    {
                                        var OutTime = await db.Daily_Attendances.Where(a => a.userId == x.userId && a.startTime == x.startTime && a.daDate == x.daDate && a.EmployeeType == "S").OrderByDescending(m => m.daID).FirstOrDefaultAsync();

                                        if (OutTime != null && OutTime.endTime == "11:50 PM")
                                        {
                                            OutTime.userId = x.userId;
                                            OutTime.startLat = x.startLat;
                                            OutTime.startLong = x.startLong;
                                            OutTime.startTime = x.startTime;
                                            OutTime.daDate = x.daDate;
                                            OutTime.vehicleNumber = x.vehicleNumber;
                                            OutTime.vtId = x.vtId;
                                            OutTime.EmployeeType = "S";
                                            if (x.daEndDate.Equals(DateTime.MinValue))
                                            {
                                                OutTime.daEndDate = null;
                                                OutTime.endTime = (string.IsNullOrEmpty(x.endTime) ? "" : x.endTime);
                                            }
                                            else
                                            {
                                                if (x.daEndDate == x.daDate)
                                                {
                                                    OutTime.daEndDate = x.daEndDate;
                                                    OutTime.endTime = (string.IsNullOrEmpty(x.endTime) ? "" : x.endTime);
                                                }
                                                else
                                                {
                                                    OutTime.daEndDate = x.daDate;
                                                    OutTime.endTime = "11:50 PM";
                                                }

                                            }

                                            OutTime.endLat = x.endLat;
                                            OutTime.endLong = x.endLong;
                                            //OutTime.endTime = (string.IsNullOrEmpty(x.endTime) ? "" : x.endTime); //x.endTime;
                                            OutTime.daStartNote = x.daStartNote;
                                            OutTime.daEndNote = x.daEndNote;
                                            OutTime.batteryStatus = x.batteryStatus;

                                            //db.Daily_Attendance.Add(attendance);
                                            _IsInSync = true;

                                        }
                                        else
                                        {
                                            attendance.userId = x.userId;
                                            attendance.startLat = x.startLat;
                                            attendance.startLong = x.startLong;
                                            attendance.startTime = x.startTime;
                                            attendance.daDate = x.daDate;
                                            attendance.vehicleNumber = x.vehicleNumber;
                                            attendance.vtId = x.vtId;
                                            attendance.EmployeeType = "S";
                                            if (x.daEndDate.Equals(DateTime.MinValue))
                                            {
                                                attendance.daEndDate = null;
                                                attendance.endTime = (string.IsNullOrEmpty(x.endTime) ? "" : x.endTime);
                                            }
                                            else
                                            {
                                                if (x.daEndDate == x.daDate)
                                                {
                                                    attendance.daEndDate = x.daEndDate;
                                                    attendance.endTime = (string.IsNullOrEmpty(x.endTime) ? "" : x.endTime);

                                                }
                                                else
                                                {
                                                    attendance.daEndDate = x.daDate;
                                                    attendance.endTime = "11:50 PM";
                                                }
                                            }

                                            attendance.endLat = x.endLat;
                                            attendance.endLong = x.endLong;
                                            //attendance.endTime = (string.IsNullOrEmpty(x.endTime) ? "" : x.endTime); //x.endTime;
                                            attendance.daStartNote = x.daStartNote;
                                            attendance.daEndNote = x.daEndNote;
                                            attendance.batteryStatus = x.batteryStatus;
                                            if (OutTime != null)
                                            {
                                                if (OutTime.endTime == "" || OutTime.endTime == null)
                                                {
                                                    db.Daily_Attendances.Add(attendance);
                                                }
                                                OutTime.endTime = (string.IsNullOrEmpty(x.endTime) ? "" : x.endTime);
                                            }
                                            if (OutTime == null)
                                            {
                                                db.Daily_Attendances.Add(attendance);
                                            }
                                            _IsInSync = true;
                                            //  db.SaveChanges();
                                            //if ((!string.IsNullOrEmpty(x.endLat))  && (!string.IsNullOrEmpty(x.endLong)))
                                            //{
                                            //    string Time2 = x.endTime;
                                            //    DateTime date2 = DateTime.Parse(Time2, System.Globalization.CultureInfo.CurrentCulture);
                                            //    string t2 = date2.ToString("hh:mm:ss tt");
                                            //    string dt2 = Convert.ToDateTime(x.daEndDate).ToString("MM/dd/yyyy");
                                            //    DateTime? edate = Convert.ToDateTime(dt2 + " " + t2);

                                            //    Location loc = new Location();
                                            //    loc.userId = x.userId;
                                            //    loc.datetime = edate;
                                            //    loc.lat = x.endLat;
                                            //    loc.@long = x.endLong;
                                            //    loc.batteryStatus = x.batteryStatus;
                                            //    loc.address = Address(x.endLat + "," + x.endLong);
                                            //    if (loc.address != "")
                                            //    {
                                            //        loc.area = area(loc.address);
                                            //    }
                                            //    else
                                            //    {
                                            //        loc.area = "";
                                            //    }

                                            //    loc.IsOffline = true;
                                            //    loc.CreatedDate = DateTime.Now;

                                            //    db.Locations.Add(loc);
                                            //    _IsOutSync = true;
                                            //}

                                            //db.SaveChanges();

                                        }
                                        if ((!string.IsNullOrEmpty(x.endLat)) && (!string.IsNullOrEmpty(x.endLong)) && IsSameRecordLocation == null)
                                        {
                                            string Time2 = x.endTime;
                                            DateTime date2 = DateTime.Parse(Time2, System.Globalization.CultureInfo.CurrentCulture);
                                            string t2 = date2.ToString("hh:mm:ss tt");
                                            string dt2 = Convert.ToDateTime(x.daEndDate).ToString("MM/dd/yyyy");
                                            DateTime? edate = Convert.ToDateTime(dt2 + " " + t2);

                                            Location loc = new Location();
                                            loc.userId = x.userId;
                                            loc.datetime = edate;
                                            loc.lat = x.endLat;
                                            loc._long = x.endLong;
                                            loc.batteryStatus = x.batteryStatus;
                                            loc.address = Address(x.endLat + "," + x.endLong);
                                            if (loc.address != "")
                                            {
                                                loc.area = area(loc.address);
                                            }
                                            else
                                            {
                                                loc.area = "";
                                            }

                                            loc.IsOffline = true;
                                            loc.CreatedDate = DateTime.Now;
                                            loc.EmployeeType = "S";
                                            db.Locations.Add(loc);
                                            _IsOutSync = true;
                                        }

                                        if ((!string.IsNullOrEmpty(x.startLat)) && (!string.IsNullOrEmpty(x.startLong)) && IsSameRecordLocation == null)
                                        {
                                            string Time2 = x.startTime;
                                            DateTime date2 = DateTime.Parse(Time2, System.Globalization.CultureInfo.CurrentCulture);
                                            string t2 = date2.ToString("hh:mm:ss tt");
                                            string dt2 = Convert.ToDateTime(x.daDate).ToString("MM/dd/yyyy");
                                            DateTime? sdate = Convert.ToDateTime(dt2 + " " + t2);

                                            Location loc = new Location();
                                            loc.userId = x.userId;
                                            loc.datetime = sdate;
                                            loc.lat = x.startLat;
                                            loc._long = x.startLong;
                                            loc.batteryStatus = x.batteryStatus;
                                            loc.address = Address(x.startLat + "," + x.startLong);
                                            if (loc.address != "")
                                            {
                                                loc.area = area(loc.address);
                                            }
                                            else
                                            {
                                                loc.area = "";
                                            }

                                            loc.IsOffline = true;
                                            loc.CreatedDate = DateTime.Now;
                                            loc.EmployeeType = "S";
                                            db.Locations.Add(loc);
                                            _IsOutSync = false;
                                        }
                                        await db.SaveChangesAsync();
                                    }

                                    result.Add(new SyncResult1()
                                    {
                                        ID = x.OfflineID,
                                        status = "success",
                                        message = "Shift started Successfully",
                                        messageMar = "शिफ्ट सुरू",
                                        IsInSync = _IsInSync,
                                        IsOutSync = _IsOutSync,
                                        EmpType = "S",

                                    });
                                }
                                else
                                {
                                    result.Add(new SyncResult1()
                                    {
                                        ID = x.OfflineID,
                                        status = "success",
                                        message = "Shift started Successfully",
                                        messageMar = "शिफ्ट सुरू",
                                        IsInSync = true,
                                        IsOutSync = true,
                                        EmpType = "S",
                                    });
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex.ToString(), ex);
                            result.Add(new SyncResult1()
                            {
                                ID = x.OfflineID,
                                status = "error",
                                message = "Something is wrong,Try Again.. ",
                                messageMar = "काहीतरी चुकीचे आहे, पुन्हा प्रयत्न करा..",
                                IsInSync = false,
                                IsOutSync = false,
                                EmpType = "S",
                            });
                            return result;
                        }


                    }

                }
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString(), ex);
                return result;
            }
        }
        public async Task<List<SyncResult1>> SaveUserAttendenceOfflineForDumpAsync(List<SBUserAttendence> obj, int AppId, string cdate, string EmpType)
        {
            List<SyncResult1> result = new List<SyncResult1>();
            try
            {
                using (DevICTSBMChildEntities db = new DevICTSBMChildEntities(AppId))
                {
                    Daily_Attendance attendance = new Daily_Attendance();

                    foreach (var x in obj)
                    {
                        try
                        {
                            var dy = await db.DumpYardDetails.Where(c => c.ReferanceId == x.ReferanceId).FirstOrDefaultAsync();
                            if (dy != null)
                            {
                                DateTime Datee = Convert.ToDateTime(cdate);
                                var IsSameRecordLocation = await db.Locations.Where(c => c.userId == x.userId && c.datetime == Datee && c.type == null && c.EmployeeType == "D").FirstOrDefaultAsync();

                                bool _IsInSync = false, _IsOutSync = false;
                                var user = await db.UserMasters.Where(c => c.userId == x.userId && c.EmployeeType == "D" || c.EmployeeType == null).FirstOrDefaultAsync();
                                if ((user.EmployeeType == "D" && AppId >= 4000) || (user.EmployeeType == "D" && AppId == 3098))
                                {
                                    if (user.isActive == true)
                                    {

                                        var IsSameRecord = await db.Daily_Attendances.Where(
                                               c => c.userId == x.userId &&
                                               //c.startLat == x.startLat &&
                                               //c.startLong == x.startLong &&
                                               //c.endLat == x.endLat &&
                                               //c.endLong == x.endLong &&
                                               c.startTime == x.startTime &&
                                               c.endTime == x.endTime &&
                                               EF.Functions.DateDiffDay(c.daDate, x.daDate) == 0 &&
                                               EF.Functions.DateDiffDay(c.daEndDate, x.daEndDate) == 0
                                               && c.EmployeeType == "D"
                                             ).FirstOrDefaultAsync();

                                        if (IsSameRecord == null)
                                        {

                                            var objdata = await db.Daily_Attendances.Where(c => EF.Functions.DateDiffDay(c.daDate, x.daDate) == 0 && c.userId == x.userId && (c.endTime == "" || c.endTime == null) && c.EmployeeType == "D").FirstOrDefaultAsync();
                                            if (objdata != null && x.endTime == null)
                                            {
                                                objdata.endTime = x.startTime;
                                                objdata.daEndDate = x.daDate;
                                                objdata.endLat = x.startLat;
                                                objdata.endLong = x.startLong;
                                                objdata.OutbatteryStatus = x.batteryStatus;
                                                objdata.totalKm = x.totalKm;
                                                if (dy != null)
                                                {
                                                    objdata.dyid = dy.dyId;
                                                }
                                                objdata.vehicleNumber = "1";
                                                objdata.vtId = "1";
                                                objdata.EmployeeType = "D";
                                                await db.SaveChangesAsync();
                                            }
                                            if (objdata != null)
                                            {

                                                objdata.userId = x.userId;
                                                objdata.startLat = x.startLat;
                                                objdata.startLong = x.startLong;
                                                objdata.startTime = x.startTime;
                                                objdata.daDate = x.daDate;
                                                objdata.vehicleNumber = x.vehicleNumber;
                                                objdata.vtId = x.vtId;
                                                if (dy != null)
                                                {
                                                    objdata.dyid = dy.dyId;
                                                }
                                                objdata.EmployeeType = "D";
                                                //objdata.daEndDate = x.daEndDate;

                                                if (x.daEndDate.Equals(DateTime.MinValue))
                                                {
                                                    objdata.daEndDate = null;
                                                    objdata.endTime = (string.IsNullOrEmpty(x.endTime) ? "" : x.endTime);
                                                }
                                                else
                                                {
                                                    //objdata.daEndDate = x.daEndDate;
                                                    if (x.daEndDate == x.daDate)
                                                    {
                                                        objdata.daEndDate = x.daEndDate;
                                                        objdata.endTime = (string.IsNullOrEmpty(x.endTime) ? "" : x.endTime);
                                                    }
                                                    else
                                                    {
                                                        objdata.daEndDate = x.daDate;
                                                        objdata.endTime = "11:50 PM";
                                                    }
                                                }

                                                objdata.endLat = x.endLat;
                                                objdata.endLong = x.endLong;
                                                //objdata.endTime = (string.IsNullOrEmpty(x.endTime) ? "" : x.endTime); //x.endTime;
                                                objdata.daStartNote = x.daStartNote;
                                                objdata.daEndNote = x.daEndNote;
                                                objdata.OutbatteryStatus = x.batteryStatus;
                                                objdata.vehicleNumber = "1";
                                                objdata.vtId = "1";
                                                //  objdata.batteryStatus = x.batteryStatus;
                                                if (objdata != null && x.endTime == null)
                                                {
                                                    db.Daily_Attendances.Add(objdata);
                                                }
                                                _IsInSync = true;

                                                if (x.endLat != null && x.endLong != null && IsSameRecordLocation == null)
                                                {
                                                    string Time2 = x.endTime;
                                                    DateTime date2 = DateTime.Parse(Time2, System.Globalization.CultureInfo.CurrentCulture);
                                                    string t2 = date2.ToString("hh:mm:ss tt");
                                                    string dt2 = Convert.ToDateTime(x.daEndDate).ToString("MM/dd/yyyy");
                                                    DateTime? edate = Convert.ToDateTime(dt2 + " " + t2);

                                                    Location loc = new Location();
                                                    loc.userId = x.userId;
                                                    loc.datetime = edate;
                                                    loc.lat = x.endLat;
                                                    loc._long = x.endLong;
                                                    loc.batteryStatus = x.batteryStatus;
                                                    loc.address = Address(x.endLat + "," + x.endLong);
                                                    if (loc.address != "")
                                                    {
                                                        loc.area = area(loc.address);
                                                    }
                                                    else
                                                    {
                                                        loc.area = "";
                                                    }

                                                    loc.IsOffline = true;
                                                    loc.CreatedDate = DateTime.Now;
                                                    loc.EmployeeType = "D";
                                                    db.Locations.Add(loc);
                                                    _IsOutSync = true;
                                                    _IsInSync = false;
                                                }

                                                await db.SaveChangesAsync();
                                            }
                                            else
                                            {
                                                var OutTime = await db.Daily_Attendances.Where(a => a.userId == x.userId && a.startTime == x.startTime && a.daDate == x.daDate && a.EmployeeType == "D").OrderByDescending(m => m.daID).FirstOrDefaultAsync();

                                                if (OutTime != null && OutTime.endTime == "11:50 PM")
                                                {
                                                    OutTime.userId = x.userId;
                                                    OutTime.startLat = x.startLat;
                                                    OutTime.startLong = x.startLong;
                                                    OutTime.startTime = x.startTime;
                                                    OutTime.daDate = x.daDate;
                                                    OutTime.vehicleNumber = x.vehicleNumber;
                                                    OutTime.vtId = x.vtId;
                                                    OutTime.EmployeeType = "D";
                                                    if (dy != null)
                                                    {
                                                        OutTime.dyid = dy.dyId;
                                                    }
                                                    if (x.daEndDate.Equals(DateTime.MinValue))
                                                    {
                                                        OutTime.daEndDate = null;
                                                        OutTime.endTime = (string.IsNullOrEmpty(x.endTime) ? "" : x.endTime);
                                                    }
                                                    else
                                                    {
                                                        if (x.daEndDate == x.daDate)
                                                        {
                                                            OutTime.daEndDate = x.daEndDate;
                                                            OutTime.endTime = (string.IsNullOrEmpty(x.endTime) ? "" : x.endTime);
                                                        }
                                                        else
                                                        {
                                                            OutTime.daEndDate = x.daDate;
                                                            OutTime.endTime = "11:50 PM";
                                                        }

                                                    }

                                                    OutTime.endLat = x.endLat;
                                                    OutTime.endLong = x.endLong;
                                                    //OutTime.endTime = (string.IsNullOrEmpty(x.endTime) ? "" : x.endTime); //x.endTime;
                                                    OutTime.daStartNote = x.daStartNote;
                                                    OutTime.daEndNote = x.daEndNote;
                                                    OutTime.batteryStatus = x.batteryStatus;

                                                    //db.Daily_Attendance.Add(attendance);
                                                    _IsInSync = true;

                                                }
                                                else
                                                {
                                                    attendance.userId = x.userId;
                                                    attendance.startLat = x.startLat;
                                                    attendance.startLong = x.startLong;
                                                    attendance.startTime = x.startTime;
                                                    attendance.daDate = x.daDate;
                                                    attendance.vehicleNumber = "1";
                                                    attendance.vtId = "1";
                                                    attendance.EmployeeType = "D";
                                                    if (x.daEndDate.Equals(DateTime.MinValue))
                                                    {
                                                        attendance.daEndDate = null;
                                                        attendance.endTime = (string.IsNullOrEmpty(x.endTime) ? "" : x.endTime);
                                                    }
                                                    else
                                                    {
                                                        if (x.daEndDate == x.daDate)
                                                        {
                                                            attendance.daEndDate = x.daEndDate;
                                                            attendance.endTime = (string.IsNullOrEmpty(x.endTime) ? "" : x.endTime);

                                                        }
                                                        else
                                                        {
                                                            attendance.daEndDate = x.daDate;
                                                            attendance.endTime = "11:50 PM";
                                                        }
                                                    }

                                                    attendance.endLat = x.endLat;
                                                    attendance.endLong = x.endLong;
                                                    //attendance.endTime = (string.IsNullOrEmpty(x.endTime) ? "" : x.endTime); //x.endTime;
                                                    attendance.daStartNote = x.daStartNote;
                                                    attendance.daEndNote = x.daEndNote;
                                                    attendance.batteryStatus = x.batteryStatus;
                                                    if (OutTime != null)
                                                    {
                                                        if (OutTime.endTime == "" || OutTime.endTime == null)
                                                        {
                                                            db.Daily_Attendances.Add(attendance);
                                                        }
                                                        OutTime.endTime = (string.IsNullOrEmpty(x.endTime) ? "" : x.endTime);
                                                    }
                                                    if (dy != null)
                                                    {
                                                        attendance.dyid = dy.dyId;
                                                    }
                                                    if (OutTime == null)
                                                    {
                                                        db.Daily_Attendances.Add(attendance);
                                                    }
                                                    _IsInSync = true;

                                                }
                                                if ((!string.IsNullOrEmpty(x.endLat)) && (!string.IsNullOrEmpty(x.endLong)) && IsSameRecordLocation == null)
                                                {
                                                    string Time2 = x.endTime;
                                                    DateTime date2 = DateTime.Parse(Time2, System.Globalization.CultureInfo.CurrentCulture);
                                                    string t2 = date2.ToString("hh:mm:ss tt");
                                                    string dt2 = Convert.ToDateTime(x.daEndDate).ToString("MM/dd/yyyy");
                                                    DateTime? edate = Convert.ToDateTime(dt2 + " " + t2);

                                                    Location loc = new Location();
                                                    loc.userId = x.userId;
                                                    loc.datetime = edate;
                                                    loc.lat = x.endLat;
                                                    loc._long = x.endLong;
                                                    loc.batteryStatus = x.batteryStatus;
                                                    loc.address = Address(x.endLat + "," + x.endLong);
                                                    if (loc.address != "")
                                                    {
                                                        loc.area = area(loc.address);
                                                    }
                                                    else
                                                    {
                                                        loc.area = "";
                                                    }

                                                    loc.IsOffline = true;
                                                    loc.CreatedDate = DateTime.Now;
                                                    loc.EmployeeType = "D";
                                                    db.Locations.Add(loc);
                                                    _IsOutSync = true;
                                                }

                                                if ((!string.IsNullOrEmpty(x.startLat)) && (!string.IsNullOrEmpty(x.startLong)) && IsSameRecordLocation == null)
                                                {
                                                    string Time2 = x.startTime;
                                                    DateTime date2 = DateTime.Parse(Time2, System.Globalization.CultureInfo.CurrentCulture);
                                                    string t2 = date2.ToString("hh:mm:ss tt");
                                                    string dt2 = Convert.ToDateTime(x.daDate).ToString("MM/dd/yyyy");
                                                    DateTime? sdate = Convert.ToDateTime(dt2 + " " + t2);

                                                    Location loc = new Location();
                                                    loc.userId = x.userId;
                                                    loc.datetime = sdate;
                                                    loc.lat = x.startLat;
                                                    loc._long = x.startLong;
                                                    loc.batteryStatus = x.batteryStatus;
                                                    loc.address = Address(x.startLat + "," + x.startLong);
                                                    if (loc.address != "")
                                                    {
                                                        loc.area = area(loc.address);
                                                    }
                                                    else
                                                    {
                                                        loc.area = "";
                                                    }

                                                    loc.IsOffline = true;
                                                    loc.CreatedDate = DateTime.Now;
                                                    loc.EmployeeType = "D";
                                                    db.Locations.Add(loc);
                                                    _IsOutSync = false;
                                                }
                                                await db.SaveChangesAsync();
                                            }

                                            result.Add(new SyncResult1()
                                            {
                                                ID = x.OfflineID,
                                                status = "success",
                                                message = "Shift started Successfully",
                                                messageMar = "शिफ्ट सुरू",
                                                IsInSync = _IsInSync,
                                                IsOutSync = _IsOutSync,
                                                EmpType = "D",

                                            });
                                        }
                                        else
                                        {
                                            result.Add(new SyncResult1()
                                            {
                                                ID = x.OfflineID,
                                                status = "success",
                                                message = "Shift started Successfully",
                                                messageMar = "शिफ्ट सुरू",
                                                IsInSync = true,
                                                IsOutSync = true,
                                                EmpType = "D",
                                            });
                                        }
                                    }
                                }
                                if (user.EmployeeType == null)
                                {
                                    if (user.isActive == true)
                                    {

                                        var IsSameRecord = db.Daily_Attendances.Where(
                                               c => c.userId == x.userId &&
                                               //c.startLat == x.startLat &&
                                               //c.startLong == x.startLong &&
                                               //c.endLat == x.endLat &&
                                               //c.endLong == x.endLong &&
                                               c.startTime == x.startTime &&
                                               c.endTime == x.endTime &&
                                               EF.Functions.DateDiffDay(c.daDate, x.daDate) == 0 &&
                                               EF.Functions.DateDiffDay(c.daEndDate, x.daEndDate) == 0
                                               && c.EmployeeType == null
                                             ).FirstOrDefault();

                                        if (IsSameRecord == null)
                                        {

                                            var objdata = await db.Daily_Attendances.Where(c => EF.Functions.DateDiffDay(c.daDate, x.daDate) == 0 && c.userId == x.userId && (c.endTime == "" || c.endTime == null) && c.EmployeeType == null).FirstOrDefaultAsync();
                                            if (objdata != null && x.endTime == null)
                                            {
                                                objdata.endTime = x.startTime;
                                                objdata.daEndDate = x.daDate;
                                                objdata.endLat = x.startLat;
                                                objdata.endLong = x.startLong;
                                                objdata.OutbatteryStatus = x.batteryStatus;
                                                objdata.totalKm = x.totalKm;
                                                if (dy != null)
                                                {
                                                    objdata.dyid = dy.dyId;
                                                }
                                                objdata.vehicleNumber = "1";
                                                objdata.vtId = "1";
                                                objdata.EmployeeType = null;
                                                await db.SaveChangesAsync();
                                            }
                                            if (objdata != null)
                                            {

                                                objdata.userId = x.userId;
                                                objdata.startLat = x.startLat;
                                                objdata.startLong = x.startLong;
                                                objdata.startTime = x.startTime;
                                                objdata.daDate = x.daDate;
                                                objdata.vehicleNumber = x.vehicleNumber;
                                                objdata.vtId = x.vtId;
                                                if (dy != null)
                                                {
                                                    objdata.dyid = dy.dyId;
                                                }
                                                objdata.EmployeeType = null;
                                                //objdata.daEndDate = x.daEndDate;

                                                if (x.daEndDate.Equals(DateTime.MinValue))
                                                {
                                                    objdata.daEndDate = null;
                                                    objdata.endTime = (string.IsNullOrEmpty(x.endTime) ? "" : x.endTime);
                                                }
                                                else
                                                {
                                                    //objdata.daEndDate = x.daEndDate;
                                                    if (x.daEndDate == x.daDate)
                                                    {
                                                        objdata.daEndDate = x.daEndDate;
                                                        objdata.endTime = (string.IsNullOrEmpty(x.endTime) ? "" : x.endTime);
                                                    }
                                                    else
                                                    {
                                                        objdata.daEndDate = x.daDate;
                                                        objdata.endTime = "11:50 PM";
                                                    }
                                                }

                                                objdata.endLat = x.endLat;
                                                objdata.endLong = x.endLong;
                                                //objdata.endTime = (string.IsNullOrEmpty(x.endTime) ? "" : x.endTime); //x.endTime;
                                                objdata.daStartNote = x.daStartNote;
                                                objdata.daEndNote = x.daEndNote;
                                                objdata.OutbatteryStatus = x.batteryStatus;
                                                objdata.vehicleNumber = "1";
                                                objdata.vtId = "1";
                                                //  objdata.batteryStatus = x.batteryStatus;
                                                if (objdata != null && x.endTime == null)
                                                {
                                                    db.Daily_Attendances.Add(objdata);
                                                }
                                                _IsInSync = true;

                                                if (x.endLat != null && x.endLong != null && IsSameRecordLocation == null)
                                                {
                                                    string Time2 = x.endTime;
                                                    DateTime date2 = DateTime.Parse(Time2, System.Globalization.CultureInfo.CurrentCulture);
                                                    string t2 = date2.ToString("hh:mm:ss tt");
                                                    string dt2 = Convert.ToDateTime(x.daEndDate).ToString("MM/dd/yyyy");
                                                    DateTime? edate = Convert.ToDateTime(dt2 + " " + t2);

                                                    Location loc = new Location();
                                                    loc.userId = x.userId;
                                                    loc.datetime = edate;
                                                    loc.lat = x.endLat;
                                                    loc._long = x.endLong;
                                                    loc.batteryStatus = x.batteryStatus;
                                                    loc.address = Address(x.endLat + "," + x.endLong);
                                                    if (loc.address != "")
                                                    {
                                                        loc.area = area(loc.address);
                                                    }
                                                    else
                                                    {
                                                        loc.area = "";
                                                    }

                                                    loc.IsOffline = true;
                                                    loc.CreatedDate = DateTime.Now;
                                                    loc.EmployeeType = null;
                                                    db.Locations.Add(loc);
                                                    _IsOutSync = true;
                                                    _IsInSync = false;
                                                }

                                                await db.SaveChangesAsync();
                                            }
                                            else
                                            {
                                                var OutTime = db.Daily_Attendances.Where(a => a.userId == x.userId && a.startTime == x.startTime && a.daDate == x.daDate && a.EmployeeType == null).OrderByDescending(m => m.daID).FirstOrDefault();

                                                if (OutTime != null && OutTime.endTime == "11:50 PM")
                                                {
                                                    OutTime.userId = x.userId;
                                                    OutTime.startLat = x.startLat;
                                                    OutTime.startLong = x.startLong;
                                                    OutTime.startTime = x.startTime;
                                                    OutTime.daDate = x.daDate;
                                                    OutTime.vehicleNumber = x.vehicleNumber;
                                                    OutTime.vtId = x.vtId;
                                                    OutTime.EmployeeType = null;
                                                    if (dy != null)
                                                    {
                                                        OutTime.dyid = dy.dyId;
                                                    }
                                                    if (x.daEndDate.Equals(DateTime.MinValue))
                                                    {
                                                        OutTime.daEndDate = null;
                                                        OutTime.endTime = (string.IsNullOrEmpty(x.endTime) ? "" : x.endTime);
                                                    }
                                                    else
                                                    {
                                                        if (x.daEndDate == x.daDate)
                                                        {
                                                            OutTime.daEndDate = x.daEndDate;
                                                            OutTime.endTime = (string.IsNullOrEmpty(x.endTime) ? "" : x.endTime);
                                                        }
                                                        else
                                                        {
                                                            OutTime.daEndDate = x.daDate;
                                                            OutTime.endTime = "11:50 PM";
                                                        }

                                                    }

                                                    OutTime.endLat = x.endLat;
                                                    OutTime.endLong = x.endLong;
                                                    //OutTime.endTime = (string.IsNullOrEmpty(x.endTime) ? "" : x.endTime); //x.endTime;
                                                    OutTime.daStartNote = x.daStartNote;
                                                    OutTime.daEndNote = x.daEndNote;
                                                    OutTime.batteryStatus = x.batteryStatus;

                                                    //db.Daily_Attendance.Add(attendance);
                                                    _IsInSync = true;

                                                }
                                                else
                                                {
                                                    attendance.userId = x.userId;
                                                    attendance.startLat = x.startLat;
                                                    attendance.startLong = x.startLong;
                                                    attendance.startTime = x.startTime;
                                                    attendance.daDate = x.daDate;
                                                    attendance.vehicleNumber = "1";
                                                    attendance.vtId = "1";
                                                    attendance.EmployeeType = null;
                                                    if (x.daEndDate.Equals(DateTime.MinValue))
                                                    {
                                                        attendance.daEndDate = null;
                                                        attendance.endTime = (string.IsNullOrEmpty(x.endTime) ? "" : x.endTime);
                                                    }
                                                    else
                                                    {
                                                        if (x.daEndDate == x.daDate)
                                                        {
                                                            attendance.daEndDate = x.daEndDate;
                                                            attendance.endTime = (string.IsNullOrEmpty(x.endTime) ? "" : x.endTime);

                                                        }
                                                        else
                                                        {
                                                            attendance.daEndDate = x.daDate;
                                                            attendance.endTime = "11:50 PM";
                                                        }
                                                    }

                                                    attendance.endLat = x.endLat;
                                                    attendance.endLong = x.endLong;
                                                    //attendance.endTime = (string.IsNullOrEmpty(x.endTime) ? "" : x.endTime); //x.endTime;
                                                    attendance.daStartNote = x.daStartNote;
                                                    attendance.daEndNote = x.daEndNote;
                                                    attendance.batteryStatus = x.batteryStatus;
                                                    if (OutTime != null)
                                                    {
                                                        if (OutTime.endTime == "" || OutTime.endTime == null)
                                                        {
                                                            db.Daily_Attendances.Add(attendance);
                                                        }
                                                        OutTime.endTime = (string.IsNullOrEmpty(x.endTime) ? "" : x.endTime);
                                                    }
                                                    if (dy != null)
                                                    {
                                                        attendance.dyid = dy.dyId;
                                                    }
                                                    if (OutTime == null)
                                                    {
                                                        db.Daily_Attendances.Add(attendance);
                                                    }
                                                    _IsInSync = true;

                                                }
                                                if ((!string.IsNullOrEmpty(x.endLat)) && (!string.IsNullOrEmpty(x.endLong)) && IsSameRecordLocation == null)
                                                {
                                                    string Time2 = x.endTime;
                                                    DateTime date2 = DateTime.Parse(Time2, System.Globalization.CultureInfo.CurrentCulture);
                                                    string t2 = date2.ToString("hh:mm:ss tt");
                                                    string dt2 = Convert.ToDateTime(x.daEndDate).ToString("MM/dd/yyyy");
                                                    DateTime? edate = Convert.ToDateTime(dt2 + " " + t2);

                                                    Location loc = new Location();
                                                    loc.userId = x.userId;
                                                    loc.datetime = edate;
                                                    loc.lat = x.endLat;
                                                    loc._long = x.endLong;
                                                    loc.batteryStatus = x.batteryStatus;
                                                    loc.address = Address(x.endLat + "," + x.endLong);
                                                    if (loc.address != "")
                                                    {
                                                        loc.area = area(loc.address);
                                                    }
                                                    else
                                                    {
                                                        loc.area = "";
                                                    }

                                                    loc.IsOffline = true;
                                                    loc.CreatedDate = DateTime.Now;
                                                    loc.EmployeeType = null;
                                                    db.Locations.Add(loc);
                                                    _IsOutSync = true;
                                                }

                                                if ((!string.IsNullOrEmpty(x.startLat)) && (!string.IsNullOrEmpty(x.startLong)) && IsSameRecordLocation == null)
                                                {
                                                    string Time2 = x.startTime;
                                                    DateTime date2 = DateTime.Parse(Time2, System.Globalization.CultureInfo.CurrentCulture);
                                                    string t2 = date2.ToString("hh:mm:ss tt");
                                                    string dt2 = Convert.ToDateTime(x.daDate).ToString("MM/dd/yyyy");
                                                    DateTime? sdate = Convert.ToDateTime(dt2 + " " + t2);

                                                    Location loc = new Location();
                                                    loc.userId = x.userId;
                                                    loc.datetime = sdate;
                                                    loc.lat = x.startLat;
                                                    loc._long = x.startLong;
                                                    loc.batteryStatus = x.batteryStatus;
                                                    loc.address = Address(x.startLat + "," + x.startLong);
                                                    if (loc.address != "")
                                                    {
                                                        loc.area = area(loc.address);
                                                    }
                                                    else
                                                    {
                                                        loc.area = "";
                                                    }

                                                    loc.IsOffline = true;
                                                    loc.CreatedDate = DateTime.Now;
                                                    loc.EmployeeType = null;
                                                    db.Locations.Add(loc);
                                                    _IsOutSync = false;
                                                }
                                                await db.SaveChangesAsync();
                                            }

                                            result.Add(new SyncResult1()
                                            {
                                                ID = x.OfflineID,
                                                status = "success",
                                                message = "Shift started Successfully",
                                                messageMar = "शिफ्ट सुरू",
                                                IsInSync = _IsInSync,
                                                IsOutSync = _IsOutSync,
                                                EmpType = null,

                                            });
                                        }
                                        else
                                        {
                                            result.Add(new SyncResult1()
                                            {
                                                ID = x.OfflineID,
                                                status = "success",
                                                message = "Shift started Successfully",
                                                messageMar = "शिफ्ट सुरू",
                                                IsInSync = true,
                                                IsOutSync = true,
                                                EmpType = "D",
                                            });
                                        }
                                    }
                                }


                            }
                            else
                            {
                                result.Add(new SyncResult1()
                                {
                                    ID = x.OfflineID,
                                    status = "error",
                                    message = "Inavlid Dumpyard Id.. ",
                                    messageMar = "अवैध डम्पयार्ड आयडी..",
                                    IsInSync = false,
                                    IsOutSync = false,
                                    EmpType = "D",
                                });

                                return result;
                            }
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex.ToString(), ex);
                            result.Add(new SyncResult1()
                            {
                                ID = x.OfflineID,
                                status = "error",
                                message = "Something is wrong,Try Again.. ",
                                messageMar = "काहीतरी चुकीचे आहे, पुन्हा प्रयत्न करा..",
                                IsInSync = false,
                                IsOutSync = false,
                                EmpType = "D",
                            });
                            return result;
                        }
                    }

                }
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString(), ex);
                return result;
            }
        }



        public async Task<List<SBVehicleType>> GetVehicleAsync(int appId)
        {
            List<SBVehicleType> obj = new List<SBVehicleType>();
            try
            {
                using (DevICTSBMChildEntities db = new DevICTSBMChildEntities(appId))
                {
                    var data = await db.VehicleTypes.Where(c => c.isActive == true).ToListAsync();
                    foreach (var x in data)
                    {
                        string des = "", desmar = ""; ;
                        if (x.description != null)
                        {
                            des = checkNull(x.description.Trim());
                        }
                        if (x.descriptionMar != null)
                        {
                            desmar = checkNull(x.descriptionMar.Trim());
                        }
                        obj.Add(new SBVehicleType()
                        {

                            vtId = x.vtId,
                            description = des,
                            descriptionMar = desmar
                        });
                    }

                }
                return obj;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString(), ex);
                return obj;
            }
        }
        public async Task<List<SBWorkDetails>> GetUserWorkAsync(int userId, int year, int month, int appId, string EmpType)
        {
            List<SBWorkDetails> obj = new List<SBWorkDetails>();
            if (EmpType == "N")
            {
                obj = await GetUserWorkForNormalAsync(userId, year, month, appId);
            }
            if (EmpType == "L")
            {
                obj = await GetUserWorkForLiquidAsync(userId, year, month, appId);
            }
            if (EmpType == "S")
            {
                obj = await GetUserWorkForStreetAsync(userId, year, month, appId);
            }
            if (EmpType == "D")
            {
                obj = await GetUserWorkForDumpAsync(userId, year, month, appId);
            }
            return obj;
        }

        public async Task<List<LatLongD>> GetLatLong(int appId, int userid, DateTime date)
        {
            List<LatLongD> obj = new List<LatLongD>();
            using (DevICTSBMChildEntities db = new DevICTSBMChildEntities(appId))
            {
                var data = await db.HouseMasters.Where(c => c.userId == userid && EF.Functions.DateDiffDay(c.modified, date) == 0).OrderByDescending(c => c.modified).ToListAsync();
                foreach (var x in data)
                {

                    obj.Add(new LatLongD()
                    {
                        RefferenceId = x.ReferanceId,
                        Lat = x.houseLat,
                        Long = x.houseLong
                    });
                }

            }
            return obj;
        }


        public async Task<List<SBWorkDetails>> GetUserWorkForNormalAsync(int userId, int year, int month, int appId)
        {
            List<SBWorkDetails> obj = new List<SBWorkDetails>();
            try
            {
                using (DevICTSBMChildEntities db = new DevICTSBMChildEntities(appId))
                {
                    //var data = db.GetAttendenceDetailsTotal(userId, year, month).ToList();
                    List<SqlParameter> parms = new List<SqlParameter>
                                                {
                                                    // Create parameter(s)    
                                                    new SqlParameter { ParameterName = "@userId", Value = userId },
                                                    new SqlParameter { ParameterName = "@year", Value = year },
                                                    new SqlParameter { ParameterName = "@month", Value = month }
                                                };
                    var data = await db.GetAttendenceDetailsTotal_Results.FromSqlRaw<GetAttendenceDetailsTotal_Result>("EXEC GetAttendenceDetailsTotal @userId,@year,@month", parms.ToArray()).ToListAsync();

                    foreach (var x in data)
                    {

                        obj.Add(new SBWorkDetails()
                        {
                            date = Convert.ToDateTime(x.day).ToString("MM-dd-yyy"),
                            houseCollection = checkIntNull(x.HouseCollection.ToString()),
                            pointCollection = checkIntNull(x.PointCollection.ToString()),
                            DumpYardCollection = checkIntNull(x.DumpYardCollection.ToString()),
                        });
                    }

                }
                return obj;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString(), ex);
                return obj;
            }
        }
        public async Task<List<SBWorkDetails>> GetUserWorkForLiquidAsync(int userId, int year, int month, int appId)
        {
            List<SBWorkDetails> obj = new List<SBWorkDetails>();
            try
            {
                using (DevICTSBMChildEntities db = new DevICTSBMChildEntities(appId))
                {
                    //var data = db.GetAttendenceDetailsTotalLiquid(userId, year, month).ToList();
                    List<SqlParameter> parms = new List<SqlParameter>
                                                {
                                                    // Create parameter(s)    
                                                    new SqlParameter { ParameterName = "@userId", Value = userId },
                                                    new SqlParameter { ParameterName = "@year", Value = year },
                                                    new SqlParameter { ParameterName = "@month", Value = month }
                                                };
                    var data = await db.GetAttendenceDetailsTotalLiquid_Results.FromSqlRaw<GetAttendenceDetailsTotalLiquid_Result>("EXEC GetAttendenceDetailsTotalLiquid @userId,@year,@month", parms.ToArray()).ToListAsync();

                    foreach (var x in data)
                    {

                        obj.Add(new SBWorkDetails()
                        {
                            date = Convert.ToDateTime(x.day).ToString("MM-dd-yyy"),
                            LiquidCollection = checkIntNull(x.LiquidCollection.ToString()),
                            DumpYardCollection = checkIntNull(x.DumpYardCollection.ToString()),

                        });
                    }

                }
                return obj;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString(), ex);
                return obj;

            }
        }


        public async Task<List<SBWorkDetails>> GetUserWorkForStreetAsync(int userId, int year, int month, int appId)
        {
            List<SBWorkDetails> obj = new List<SBWorkDetails>();
            try
            {
                using (DevICTSBMChildEntities db = new DevICTSBMChildEntities(appId))
                {
                    //var data = db.GetAttendenceDetailsTotalStreet(userId, year, month).ToList();
                    List<SqlParameter> parms = new List<SqlParameter>
                                                {
                                                    // Create parameter(s)    
                                                    new SqlParameter { ParameterName = "@userId", Value = userId },
                                                    new SqlParameter { ParameterName = "@year", Value = year },
                                                    new SqlParameter { ParameterName = "@month", Value = month }
                                                };
                    var data = await db.GetAttendenceDetailsTotalStreet_Results.FromSqlRaw<GetAttendenceDetailsTotalStreet_Result>("EXEC GetAttendenceDetailsTotalStreet @userId,@year,@month", parms.ToArray()).ToListAsync();

                    foreach (var x in data)
                    {

                        obj.Add(new SBWorkDetails()
                        {
                            date = Convert.ToDateTime(x.day).ToString("MM-dd-yyy"),
                            StreetCollection = checkIntNull(x.StreetCollection.ToString()),
                            DumpYardCollection = checkIntNull(x.DumpYardCollection.ToString()),
                        });
                    }

                }
                return obj;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString(), ex);
                return obj;
            }
        }

        public async Task<List<SBWorkDetails>> GetUserWorkForDumpAsync(int userId, int year, int month, int appId)
        {
            List<SBWorkDetails> obj = new List<SBWorkDetails>();
            try
            {
                using (DevICTSBMChildEntities db = new DevICTSBMChildEntities(appId))
                {
                    //var data = db.GetAttendenceDetailsTotalDump(userId, year, month).ToList();
                    List<SqlParameter> parms = new List<SqlParameter>
                                                {
                                                    // Create parameter(s)    
                                                    new SqlParameter { ParameterName = "@userId", Value = userId },
                                                    new SqlParameter { ParameterName = "@year", Value = year },
                                                    new SqlParameter { ParameterName = "@month", Value = month }
                                                };
                    var data = await db.GetAttendenceDetailsTotalDump_Results.FromSqlRaw<GetAttendenceDetailsTotalDump_Result>("EXEC GetAttendenceDetailsTotalDump @userId,@year,@month", parms.ToArray()).ToListAsync();

                    foreach (var x in data)
                    {
                        obj.Add(new SBWorkDetails()
                        {
                            date = Convert.ToDateTime(x.day).ToString("MM-dd-yyy"),
                            DumpYardPlantCollection = checkIntNull(x.DumpYardPlantCollection.ToString()),
                        });
                    }

                }
                return obj;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString(), ex);
                return obj;
            }
        }

        public async Task<List<SBWorkDetailsHistory>> GetUserWorkDetailsAsync(DateTime date, int appId, int userId, int languageId)
        {

            List<SBWorkDetailsHistory> obj = new List<SBWorkDetailsHistory>();
            try
            {
                using (DevICTSBMChildEntities db = new DevICTSBMChildEntities(appId))
                {

                    var data = await db.GarbageCollectionDetails.Where(c => EF.Functions.DateDiffDay(c.gcDate, date) == 0 && c.userId == userId).OrderByDescending(c => c.gcDate).ToListAsync();
                    foreach (var x in data)
                    {
                        string housnum = "", area = "", Name = "", vNumber = "";
                        var att = await db.Daily_Attendances.Where(c => EF.Functions.DateDiffDay(c.daDate, date) == 0 && c.userId == userId).FirstOrDefaultAsync();
                        if (x.gcType == 1)
                        {
                            try
                            {
                                var house = await db.HouseMasters.Where(c => c.houseId == x.houseId).FirstOrDefaultAsync();
                                housnum = checkNull(house.ReferanceId);

                                if (languageId == 1)
                                {
                                    Name = checkNull(house.houseOwner);
                                    var tery = await db.TeritoryMasters.Where(c => c.Id == house.AreaId).FirstOrDefaultAsync();
                                    if (tery != null)
                                    {
                                        area = tery.Area;
                                    }

                                }
                                else
                                {
                                    Name = checkNull(house.houseOwnerMar);
                                    var tery = await db.TeritoryMasters.Where(c => c.Id == house.AreaId).FirstOrDefaultAsync();
                                    if (tery != null)
                                    {
                                        area = tery.AreaMar;
                                    }
                                }

                            }
                            catch
                            {
                                //housnum = "";
                                //area = "";
                            }


                        }
                        if (x.gcType == 2)
                        {
                            try
                            {
                                var point = await db.GarbagePointDetails.Where(c => c.gpId == x.gpId).FirstOrDefaultAsync();
                                if (point != null)
                                {
                                    housnum = point.ReferanceId;
                                    if (languageId == 1)
                                    {
                                        Name = checkNull(point.gpName);
                                        var tery = await db.TeritoryMasters.Where(c => c.Id == point.areaId).FirstOrDefaultAsync();
                                        if (tery != null)
                                        {
                                            area = tery.Area;
                                        }
                                        // housnum = point.gpId.ToString();
                                    }
                                    else
                                    {
                                        Name = checkNull(point.gpNameMar);
                                        var tery = await db.TeritoryMasters.Where(c => c.Id == point.areaId).FirstOrDefaultAsync();
                                        if (tery != null)
                                        {
                                            area = tery.AreaMar;
                                        }
                                        //       housnum = point.gpId.ToString();
                                    }
                                }


                            }
                            catch
                            {
                                //housnum = "";
                                //area = "";
                            }

                        }

                        if (x.gcType == 3)
                        {
                            try
                            {
                                var dump = await db.DumpYardDetails.Where(c => c.dyId == x.dyId).FirstOrDefaultAsync();
                                if (dump != null)
                                {
                                    housnum = dump.ReferanceId;
                                    if (languageId == 1)
                                    {
                                        Name = checkNull(dump.dyName);
                                        var tery = await db.TeritoryMasters.Where(c => c.Id == dump.areaId).FirstOrDefaultAsync();
                                        if (tery != null)
                                        {
                                            area = tery.Area;
                                        }
                                        // housnum = point.gpId.ToString();
                                    }
                                    else
                                    {
                                        Name = checkNull(dump.dyNameMar);
                                        var tery = await db.TeritoryMasters.Where(c => c.Id == dump.areaId).FirstOrDefaultAsync();
                                        if (tery != null)
                                        {
                                            area = tery.AreaMar;
                                        }
                                        //       housnum = point.gpId.ToString();
                                    }
                                }


                            }
                            catch
                            {
                                //housnum = "";
                                //area = "";
                            }

                        }

                        if (x.gcType == 4)
                        {
                            try
                            {
                                var Liquid = await db.LiquidWasteDetails.Where(c => c.LWId == x.LWId).FirstOrDefaultAsync();
                                if (Liquid != null)
                                {
                                    housnum = Liquid.ReferanceId;
                                    if (languageId == 1)
                                    {
                                        Name = checkNull(Liquid.LWName);
                                        var tery = await db.TeritoryMasters.Where(c => c.Id == Liquid.areaId).FirstOrDefaultAsync();
                                        if (tery != null)
                                        {
                                            area = tery.Area;
                                        }
                                        // housnum = point.gpId.ToString();
                                    }
                                    else
                                    {
                                        Name = checkNull(Liquid.LWNameMar);
                                        var tery = await db.TeritoryMasters.Where(c => c.Id == Liquid.areaId).FirstOrDefaultAsync();
                                        if (tery != null)
                                        {
                                            area = tery.AreaMar;
                                        }
                                        //       housnum = point.gpId.ToString();
                                    }

                                }


                            }
                            catch
                            {
                                //housnum = "";
                                //area = "";
                            }

                        }

                        if (x.gcType == 5)
                        {
                            try
                            {
                                var Street = await db.StreetSweepingDetails.Where(c => c.SSId == x.SSId).FirstOrDefaultAsync();
                                if (Street != null)
                                {
                                    housnum = Street.ReferanceId;
                                    if (languageId == 1)
                                    {
                                        Name = checkNull(Street.SSName);
                                        var tery = await db.TeritoryMasters.Where(c => c.Id == Street.areaId).FirstOrDefaultAsync();
                                        if (tery != null)
                                        {
                                            area = tery.Area;

                                        }
                                        // housnum = point.gpId.ToString();
                                    }
                                    else
                                    {
                                        Name = checkNull(Street.SSNameMar);
                                        var tery = await db.TeritoryMasters.Where(c => c.Id == Street.areaId).FirstOrDefaultAsync();
                                        if (tery != null)
                                        {
                                            area = tery.AreaMar;
                                        }

                                        //       housnum = point.gpId.ToString();
                                    }

                                }

                            }
                            catch
                            {
                                //housnum = "";
                                //area = "";
                            }

                        }

                        if (x.gcType == 6)
                        {
                            try
                            {
                                int vtype = 0;
                                var vehical = await db.Vehical_QR_Masters.Where(c => c.vqrId == x.vqrid).FirstOrDefaultAsync();
                                if (vehical != null)
                                {
                                    housnum = vehical.ReferanceId;
                                    if (languageId == 1)
                                    {

                                        vtype = Convert.ToInt32(vehical.VehicalType);
                                        var vehicleType = await db.VehicleTypes.Where(c => c.vtId == vtype).FirstOrDefaultAsync();
                                        if (vehicleType != null)
                                        {
                                            Name = checkNull(vehicleType.description);
                                            //   area = db.TeritoryMasters.Where(c => c.Id == Street.areaId).FirstOrDefault().Area;
                                            //   area= checkNull(vehical.VehicalNumber);
                                            vNumber = checkNull(vehical.VehicalNumber);
                                            // housnum = point.gpId.ToString();

                                        }

                                    }
                                    else
                                    {
                                        vtype = Convert.ToInt32(vehical.VehicalType);
                                        var vehicleType = await db.VehicleTypes.Where(c => c.vtId == vtype).FirstOrDefaultAsync();
                                        if (vehicleType != null)
                                        {
                                            Name = checkNull(vehicleType.description);
                                            //   area = db.TeritoryMasters.Where(c => c.Id == Street.areaId).FirstOrDefault().AreaMar;
                                            vNumber = checkNull(vehical.VehicalNumber);
                                            //       housnum = point.gpId.ToString();
                                        }

                                    }
                                }


                            }
                            catch (Exception ex)
                            {
                                //housnum = "";
                                //area = "";
                            }

                        }
                        if (x.gcType == 6)
                        {
                            obj.Add(new SBWorkDetailsHistory()
                            {
                                time = Convert.ToDateTime(x.gcDate).ToString("hh:mm tt"),
                                Refid = housnum,
                                name = Name,
                                vehicleNumber = vNumber,
                                areaName = area,
                                type = Convert.ToInt32(x.gcType),
                            });
                        }
                        else
                        {
                            obj.Add(new SBWorkDetailsHistory()
                            {
                                time = Convert.ToDateTime(x.gcDate).ToString("hh:mm tt"),
                                Refid = housnum,
                                name = Name,
                                vehicleNumber = checkNull(x.vehicleNumber),
                                areaName = area,
                                type = Convert.ToInt32(x.gcType),
                            });
                        }

                    }

                    //// Bad Response
                    ///
                    //List<SqlParameter> parms = new List<SqlParameter>
                    //                            {
                    //                                // Create parameter(s)    
                    //                                new SqlParameter { ParameterName = "@userId", Value = userId },
                    //                                new SqlParameter { ParameterName = "@date", Value =  Convert.ToDateTime(date).ToString("MM/dd/yyyy") }
                    //                            };
                    //var Newdata = await db.GetEmployeeWorkHistoryDetails_Results.FromSqlRaw<GetEmployeeWorkHistoryDetails_Result>("EXEC GetEmployeeWorkHistoryDetails @userId,@date", parms.ToArray()).ToListAsync();
                    //foreach (var c in Newdata)
                    //{
                    //    obj.Add(new SBWorkDetailsHistory()
                    //    {

                    //        time = Convert.ToDateTime(c.gcDate).ToString("hh:mm tt"),
                    //        Refid = c.ReferanceId,
                    //        name = c.Name,
                    //        vehicleNumber = checkNull(c.vehicleNumber),
                    //        areaName = c.Area,
                    //        type = Convert.ToInt32(c.gcType),
                    //    });
                    //}

                }
                //return obj.OrderByDescending(c => c.id).ToList(); 
                return obj.ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString(), ex);
                return obj;
            }
        }


        public async Task<List<CMSBZoneVM>> GetZoneAsync(int AppId, string SearchString)
        {
            List<CMSBZoneVM> data = new List<CMSBZoneVM>();
            try
            {
                using (var db = new DevICTSBMChildEntities(AppId))
                {
                    data = await db.ZoneMasters.Select(x => new CMSBZoneVM
                    {
                        zoneId = x.zoneId,
                        name = x.name
                    }).ToListAsync();

                    foreach (var item in data)
                    {

                        if (item.name == null || item.name == "")
                            item.name = "";
                    }
                    if (!string.IsNullOrEmpty(SearchString))
                    {
                        var model = data.Where(c => c.name.ToUpper().ToString().Contains(SearchString.ToUpper())).ToList();

                        data = model.ToList();
                    }
                    return data.OrderByDescending(c => c.zoneId).ToList();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString(), ex);
                return data;
            }
        }
        public async Task<List<CMSBWardZoneVM>> GetWardZoneListAsync(int AppId)
        {
            List<CMSBWardZoneVM> data = new List<CMSBWardZoneVM>();
            try
            {
                using (var db = new DevICTSBMChildEntities(AppId))
                {
                    data = await db.WardNumbers.Select(x => new CMSBWardZoneVM
                    {
                        WardID = x.Id,
                        WardNo = x.WardNo,
                        zoneId = x.zoneId,
                        Zone = db.ZoneMasters.Where(c => c.zoneId == x.zoneId).Select(c => c.name).FirstOrDefault()
                    }).ToListAsync();

                    return data.OrderByDescending(c => c.WardID).ToList();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString(), ex);
                return data;
            }


        }
        public async Task<List<CMSBAreaVM>> GetAreaListAsync(int AppId, string SearchString)
        {
            List<CMSBAreaVM> data = new List<CMSBAreaVM>();
            try
            {
                using (var db = new DevICTSBMChildEntities(AppId))
                {
                    data = await db.TeritoryMasters.Select(x => new CMSBAreaVM
                    {
                        id = x.Id,
                        area = x.Area,
                        areaMar = x.AreaMar,
                        wardId = x.wardId,
                        Wardno = db.WardNumbers.Where(v => v.Id == x.wardId).Select(v => v.WardNo).FirstOrDefault()
                    }).ToListAsync();

                    foreach (var item in data)
                    {
                        item.area = checkNull(item.area);
                        item.areaMar = checkNull(item.areaMar);
                        item.Wardno = checkNull(item.Wardno);
                        string zone = string.Empty;
                        if (!string.IsNullOrEmpty(item.Wardno))
                        {
                            int wa = Convert.ToInt32(await db.WardNumbers.Where(c => c.WardNo == item.Wardno).Select(c => c.zoneId).FirstOrDefaultAsync());
                            zone = await db.ZoneMasters.Where(c => c.zoneId == wa).Select(c => c.name).FirstOrDefaultAsync();
                            item.Wardno = item.Wardno + " (" + zone + ")";
                        }
                        else
                        {
                            item.Wardno = item.Wardno + " (" + zone + ")";
                        }

                    }
                    if (!string.IsNullOrEmpty(SearchString))
                    {
                        var model = data.Where(c => c.area.ToUpper().ToString().Contains(SearchString.ToUpper()) || c.areaMar.ToString().ToUpper().ToString().Contains(SearchString.ToUpper()) ||
                         c.Wardno.ToString().Contains(SearchString.ToUpper())).ToList();

                        data = model.ToList();
                    }
                    return data.OrderByDescending(c => c.id).ToList();
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString(), ex);
                return data;

            }


        }

        public async Task<List<SBArea>> GetCollectionAreaAsync(int AppId, int type, string EmpType)
        {
            List<SBArea> obj = new List<SBArea>();

            if (EmpType == "N")
            {
                obj = await GetCollectionAreaForNormalAsync(AppId, type);
            }
            if (EmpType == "L")
            {
                obj = await GetCollectionAreaForLiquidAsync(AppId, type);
            }
            if (EmpType == "S")
            {
                obj = await GetCollectionAreaForStreetAsync(AppId, type);
            }
            return obj;

        }
        public async Task<List<SBArea>> GetCollectionAreaForNormalAsync(int AppId, int type)
        {
            List<SBArea> obj = new List<SBArea>();
            try
            {
                using (DevICTSBMChildEntities db = new DevICTSBMChildEntities(AppId))
                {
                    //var data = db.CollecctionArea(type).ToList();
                    List<SqlParameter> parms = new List<SqlParameter>
                                                {
                                                    // Create parameter(s)    
                                                    new SqlParameter { ParameterName = "@type", Value = type }
                                                };
                    var data = await db.CollecctionArea_Results.FromSqlRaw<CollecctionArea_Result>("EXEC CollecctionArea @type", parms.ToArray()).ToListAsync();



                    foreach (var x in data)
                    {

                        obj.Add(new SBArea()
                        {
                            id = x.Id,
                            area = checkNull(x.Area).Trim(),
                            areaMar = checkNull(x.AreaMar).Trim()
                        });
                    }

                }
                return obj;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString(), ex);
                return obj;
            }
        }
        public async Task<List<SBArea>> GetCollectionAreaForLiquidAsync(int AppId, int type)
        {
            List<SBArea> obj = new List<SBArea>();
            try
            {
                using (DevICTSBMChildEntities db = new DevICTSBMChildEntities(AppId))
                {
                    //var data = db.CollecctionAreaForLiquid(type).ToList();
                    List<SqlParameter> parms = new List<SqlParameter>
                                                {
                                                    // Create parameter(s)    
                                                    new SqlParameter { ParameterName = "@type", Value = type }
                                                };
                    var data = await db.CollecctionAreaForLiquid_Results.FromSqlRaw<CollecctionAreaForLiquid_Result>("EXEC CollecctionAreaForLiquid @type", parms.ToArray()).ToListAsync();

                    foreach (var x in data)
                    {

                        obj.Add(new SBArea()
                        {
                            id = x.Id,
                            area = checkNull(x.Area).Trim(),
                            areaMar = checkNull(x.AreaMar).Trim()
                        });
                    }

                }
                return obj;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString(), ex);
                return obj;

            }
        }

        public async Task<List<SBArea>> GetCollectionAreaForStreetAsync(int AppId, int type)
        {
            List<SBArea> obj = new List<SBArea>();
            try
            {
                using (DevICTSBMChildEntities db = new DevICTSBMChildEntities(AppId))
                {
                    //var data = db.CollecctionAreaForStreet(type).ToList();

                    List<SqlParameter> parms = new List<SqlParameter>
                                                {
                                                    // Create parameter(s)    
                                                    new SqlParameter { ParameterName = "@type", Value = type }
                                                };
                    var data = await db.CollecctionAreaForStreet_Results.FromSqlRaw<CollecctionAreaForStreet_Result>("EXEC CollecctionAreaForStreet @type", parms.ToArray()).ToListAsync();


                    foreach (var x in data)
                    {

                        obj.Add(new SBArea()
                        {
                            id = x.Id,
                            area = checkNull(x.Area).Trim(),
                            areaMar = checkNull(x.AreaMar).Trim()
                        });
                    }

                }
                return obj;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString(), ex);
                return obj;

            }
        }

        public async Task<List<HouseDetailsVM>> GetAreaHouseAsync(int AppId, int areaId, string EmpType)
        {

            List<HouseDetailsVM> obj = new List<HouseDetailsVM>();
            if (EmpType == "N")
            {
                obj = await GetAreaHouseForNormalAsync(AppId, areaId);
            }
            if (EmpType == "L")
            {
                obj = await GetAreaHouseForLiquidAsync(AppId, areaId);
            }
            if (EmpType == "S")
            {
                obj = await GetAreaHousForStreetAsync(AppId, areaId);
            }

            if (EmpType == "D")
            {
                obj = await GetDumpListAsync(AppId);
            }

            if (EmpType == "V")
            {
                obj = await GetVehicleListAsync(AppId);
            }


            return obj;

        }


        public async Task<List<HouseDetailsVM>> GetAreaHouseForNormalAsync(int AppId, int areaId)
        {
            List<HouseDetailsVM> obj = new List<HouseDetailsVM>();
            try
            {
                using (DevICTSBMChildEntities db = new DevICTSBMChildEntities(AppId))
                {
                    var data = await db.Vw_GetHouseNumbers.Where(c => c.AreaId == areaId).ToListAsync();
                    if (AppId == 1003)
                    {
                        foreach (var x in data)
                        {

                            obj.Add(new HouseDetailsVM()
                            {
                                houseid = x.ReferanceId,
                                houseNumber = x.ReferanceId,
                            });
                        }
                    }
                    else
                    {

                        foreach (var x in data)
                        {
                            string HouseN = "";
                            if (x.houseNumber == null || x.houseNumber == "")
                            {
                                HouseN = x.ReferanceId;
                            }
                            else { HouseN = x.houseNumber; }
                            obj.Add(new HouseDetailsVM()
                            {
                                houseid = x.ReferanceId,
                                houseNumber = HouseN,

                            });
                        }
                    }

                }
                return obj;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString(), ex);
                return obj;
            }

        }

        public async Task<List<HouseDetailsVM>> GetAreaHouseForLiquidAsync(int AppId, int areaId)
        {
            List<HouseDetailsVM> obj = new List<HouseDetailsVM>();
            try
            {
                using (DevICTSBMChildEntities db = new DevICTSBMChildEntities(AppId))
                {
                    var data = await db.Vw_GetLiquidNumbers.Where(c => c.AreaId == areaId).ToListAsync();
                    if (AppId == 1003)
                    {
                        foreach (var x in data)
                        {

                            obj.Add(new HouseDetailsVM()
                            {
                                houseid = x.ReferanceId,
                                houseNumber = x.ReferanceId,
                            });
                        }
                    }
                    else
                    {

                        foreach (var x in data)
                        {
                            string HouseN = "";
                            //if (x.houseNumber == null || x.houseNumber == "")
                            //{
                            //    HouseN = x.ReferanceId;
                            //}
                            //else { HouseN = x.houseNumber; }
                            obj.Add(new HouseDetailsVM()
                            {
                                houseid = x.ReferanceId,
                                houseNumber = x.ReferanceId,

                            });
                        }
                    }

                }
                return obj;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString(), ex);
                return obj;
            }
        }


        public async Task<List<HouseDetailsVM>> GetAreaHousForStreetAsync(int AppId, int areaId)
        {
            List<HouseDetailsVM> obj = new List<HouseDetailsVM>();
            try
            {
                using (DevICTSBMChildEntities db = new DevICTSBMChildEntities(AppId))
                {
                    var data = await db.Vw_GetStreetNumbers.Where(c => c.AreaId == areaId).ToListAsync();
                    if (AppId == 1003)
                    {
                        foreach (var x in data)
                        {

                            obj.Add(new HouseDetailsVM()
                            {
                                houseid = x.ReferanceId,
                                houseNumber = x.ReferanceId,
                            });
                        }
                    }
                    else
                    {

                        foreach (var x in data)
                        {
                            string HouseN = "";
                            //if (x.houseNumber == null || x.houseNumber == "")
                            //{
                            //    HouseN = x.ReferanceId;
                            //}
                            //else { HouseN = x.houseNumber; }
                            obj.Add(new HouseDetailsVM()
                            {
                                houseid = x.ReferanceId,
                                houseNumber = x.ReferanceId,

                            });
                        }
                    }

                }
                return obj;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString(), ex);
                return obj;

            }

        }
        public async Task<List<HouseDetailsVM>> GetDumpListAsync(int AppId)
        {
            List<HouseDetailsVM> obj = new List<HouseDetailsVM>();
            try
            {

                using (DevICTSBMChildEntities db = new DevICTSBMChildEntities(AppId))
                {
                    var data = await db.DumpYardDetails.ToListAsync();


                    foreach (var x in data)
                    {

                        obj.Add(new HouseDetailsVM()
                        {
                            houseid = x.ReferanceId,
                            houseNumber = x.ReferanceId,

                        });
                    }


                }
                return obj;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString(), ex);
                return obj;

            }

        }

        public async Task<List<HouseDetailsVM>> GetVehicleListAsync(int AppId)
        {
            List<HouseDetailsVM> obj = new List<HouseDetailsVM>();
            try
            {
                using (DevICTSBMChildEntities db = new DevICTSBMChildEntities(AppId))
                {
                    var data = await db.Vehical_QR_Masters.Where(c => c.VehicalType != null && c.VehicalNumber != null).ToListAsync();


                    foreach (var x in data)
                    {

                        obj.Add(new HouseDetailsVM()
                        {
                            houseid = x.ReferanceId,
                            houseNumber = x.ReferanceId,

                        });
                    }


                }
                return obj;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString(), ex);
                return obj;

            }

        }


        public async Task<CollectionAppAreaLatLong> GetAppAreaLatLongAsync(int appId)
        {
            CollectionAppAreaLatLong obj = new CollectionAppAreaLatLong();
            try
            {
                using (DevICTSBMMainEntities db = new DevICTSBMMainEntities())
                {
                    var data = await db.AppDetails.Where(c => c.AppId == appId).FirstOrDefaultAsync();

                    obj.AppId = appId;
                    obj.AppAreaLatLong = data.AppAreaLatLong;
                    obj.IsAreaActive = data.IsAreaActive;

                }
                return obj;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString(), ex);
                return obj;

            }


        }




        public async Task<Result> SendSMSToHOuseAsync(int area, int AppId)
        {

            Result res = new Result();
            try
            {
                using (DevICTSBMChildEntities db = new DevICTSBMChildEntities(AppId))
                using (DevICTSBMMainEntities dbMain = new DevICTSBMMainEntities())
                {
                    var appdetails = await dbMain.AppDetails.Where(c => c.AppId == AppId).FirstOrDefaultAsync();
                    //var data = db.HouseMasters.Where(c => c.AreaId == area).ToList();
                    //foreach (var x in data)
                    //{
                    //    string msg;
                    //    if (AppId == 1)
                    //    {
                    //        msg = "नमस्कार! घंटागाडीचे आगमन आपल्या क्षेत्रात झालेले आहे. आपल्या घरातून लवकरच कचरा संकलित करण्यात येईल. आपणास विनंती आहे कि कृपया आपल्या घरातील ओला व सुका असा वर्गीकृत कचरा आमच्या सफाई सेवकास सुपूर्द करावा. कचरा संकलन न झाल्यास कृपया खालील दिलेल्या क्रमांकावर संपर्क करून तक्रार/सूचना नोंदवावी. धन्यवाद. " + appdetails.yoccContact + " आपल्या सेवेशी" + appdetails.AppName_mar + ".";
                    //    }
                    //    else {
                    //        msg = "नमस्कार! घंटागाडीचे आगमन आपल्या क्षेत्रात झालेले आहे. आपल्या घरातून लवकरच कचरा संकलित करण्यात येईल. आपणास विनंती आहे कि कृपया आपल्या घरातील ओला व सुका असा वर्गीकृत कचरा आमच्या सफाई सेवकास सुपूर्द करावा. धन्यवाद. " + appdetails.yoccContact + " आपल्या सेवेशी" + appdetails.AppName_mar + ".";
                    //    }
                    //    sendSMS(msg, x.houseOwnerMobile);

                    //}

                    //string mob = db.GetMobile(Convert.ToInt32(area)).FirstOrDefault().mob;
                    List<SqlParameter> parms = new List<SqlParameter>
                                                {
                                                    // Create parameter(s)    
                                                    new SqlParameter { ParameterName = "@areaId", Value = area }
                                                };
                    var ListMob = await db.GetMobile_Results.FromSqlRaw<GetMobile_Result>("EXEC GetMobile @areaId", parms.ToArray()).ToListAsync();
                    string mob = ListMob == null ? "" : ListMob.FirstOrDefault().mob;

                    string msg, msgMar;



                    if (appdetails.LanguageId == 3)
                    {
                        //  msg = "प्रिय नागरिक, कन्नड नगरपरिषदेची घंटागाडी १५ मिनिटाच्या आत आपल्या घरासमोर कचरा संकलनासाठी येत आहे. तरी आपण ओला व सुका कचरा वेगवेगळा करून गाडीत टाकावा. आपली सौ स्वातीताई संतोषभाऊ कोल्हे" + appdetails.yoccContact + " आपल्या सेवेशी" + appdetails.AppName_mar + ".";
                        msg = "" + appdetails.MsgForBroadcast + " " + appdetails.yoccContact + " आपकी सेवा में " + appdetails.AppName_mar + "|";
                        sendSMSmar(msg, mob);
                    }
                    if (appdetails.LanguageId == 4)
                    {
                        //  msg = "प्रिय नागरिक, कन्नड नगरपरिषदेची घंटागाडी १५ मिनिटाच्या आत आपल्या घरासमोर कचरा संकलनासाठी येत आहे. तरी आपण ओला व सुका कचरा वेगवेगळा करून गाडीत टाकावा. आपली सौ स्वातीताई संतोषभाऊ कोल्हे" + appdetails.yoccContact + " आपल्या सेवेशी" + appdetails.AppName_mar + ".";
                        msg = "" + appdetails.MsgForBroadcast + " " + appdetails.yoccContact + " आपल्या सेवेशी" + appdetails.AppName_mar + ".";
                        sendSMSmar(msg, mob);
                    }
                    else if (AppId == 1 && appdetails.LanguageId == 4)
                    {
                        // msgMar = "नमस्कार! घंटागाडीचे आगमन आपल्या क्षेत्रात झालेले आहे. आपल्या घरातून लवकरच कचरा संकलित करण्यात येईल. आपणास विनंती आहे कि कृपया आपल्या घरातील ओला व सुका असा वर्गीकृत कचरा आमच्या सफाई सेवकास सुपूर्द करावा. कचरा संकलन न झाल्यास कृपया खालील दिलेल्या क्रमांकावर संपर्क करून तक्रार/सूचना नोंदवावी. धन्यवाद. " + appdetails.yoccContact + " आपल्या सेवेशी" + appdetails.AppName_mar + ".";

                        //msg = "Dear citizen, garbage collection van will arrive in your area very shortly. Kindly keep the waste in dry and wet segregated form ready and submit it to the sanitary worker. Thank you " + appdetails.AppName + ".";

                        //   msg= "Dear citizen, garbage collection van will arrive in your area very shortly.Kindly keep the dry and wet segregated waste ready & submit it to the sanitary worker.Thank you " + appdetails.AppName + ".";

                        msg = "" + appdetails.MsgForBroadcast + " " + appdetails.AppName + ".";
                        sendSMS(msg, mob);
                    }


                    else
                    {
                        // msgMar = "नमस्कार! घंटागाडीचे आगमन आपल्या क्षेत्रात झालेले आहे. आपल्या घरातून लवकरच कचरा संकलित करण्यात येईल. आपणास विनंती आहे कि कृपया आपल्या घरातील ओला व सुका असा वर्गीकृत कचरा आमच्या सफाई सेवकास सुपूर्द करावा. धन्यवाद. " + appdetails.yoccContact + " आपल्या सेवेशी" + appdetails.AppName_mar + ".";

                        //  msg = "Dear citizen, garbage collection van will arrive in your area very shortly.Kindly keep the dry and wet segregated waste ready & submit it to the sanitary worker.Thank you " + appdetails.AppName + ".";
                        if (appdetails.LanguageId == 1)
                        {
                            msg = "" + appdetails.MsgForBroadcast + " " + appdetails.AppName + ".";
                        }
                        else
                        {
                            msg = "" + appdetails.MsgForBroadcast + " " + appdetails.AppName + ".";
                            sendSMS(msg, mob);
                        }
                    }


                    var FCM = from h in db.HouseMasters
                              join d in db.DeviceDetails on h.ReferanceId equals d.ReferenceID
                              select new { FCMID = d };

                    //var FCM = db.HouseMasters
                    //  .Where(y => y.AreaId == area & y.FCMID != null)
                    //  .ToList();

                    List<String> ArrayList = new List<String>();
                    if (FCM != null)
                    {
                        foreach (var x in FCM)
                        {
                            ArrayList.Add(x.FCMID.FCMID);
                        }
                    }
                    PushNotificationMessageBroadCast(msg, ArrayList, appdetails.AppName, appdetails.Android_GCM_pushNotification_Key);

                }
                res.status = "success";
                return res;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString(), ex);
                return res;
            }



        }


        public async Task<List<GarbagePointDetailsVM>> GetAreaPointAsync(int AppId, int areaId)
        {

            List<GarbagePointDetailsVM> obj = new List<GarbagePointDetailsVM>();
            try
            {
                using (DevICTSBMChildEntities db = new DevICTSBMChildEntities(AppId))
                {
                    var data = await db.GarbagePointDetails.Where(c => c.areaId == areaId).ToListAsync();
                    if (data != null && data.Count > 0)
                    {
                        foreach (var x in data)
                        {
                            string HouseN = "";
                            if (x.gpName == null || x.gpName == "")
                            {
                                HouseN = x.ReferanceId;
                            }
                            else { HouseN = x.gpName; }
                            obj.Add(new GarbagePointDetailsVM()
                            {
                                gpId = x.ReferanceId,
                                gpName = HouseN,

                            });
                        }
                    }


                }
                return obj;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString(), ex);
                return obj;
            }

        }

        public async Task<List<DumpYardPointDetailsVM>> GetDumpYardAreaAsync(int AppId, int areaId)
        {

            List<DumpYardPointDetailsVM> obj = new List<DumpYardPointDetailsVM>();
            try
            {
                using (DevICTSBMChildEntities db = new DevICTSBMChildEntities(AppId))
                {
                    var data = await db.DumpYardDetails.Where(c => c.areaId == areaId).ToListAsync();
                    if (data != null && data.Count > 0)
                    {
                        foreach (var x in data)
                        {
                            string HouseN = "";
                            if (x.dyName == null || x.dyName == "")
                            {
                                HouseN = x.ReferanceId;
                            }
                            else { HouseN = x.dyName; }
                            obj.Add(new DumpYardPointDetailsVM()
                            {
                                dyId = x.ReferanceId,
                                dyName = HouseN,

                            });
                        }
                    }


                }
                return obj;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString(), ex);
                return obj;

            }

        }

        public async Task<SyncResult2> GetUserMobileIdentificationAsync(int appId, int userId, bool isSync, int batteryStatus, string imeinos)
        {
            SyncResult2 result = new SyncResult2();
            try
            {
                using (DevICTSBMChildEntities db = new DevICTSBMChildEntities(appId))
                {
                    Daily_Attendance attendance = new Daily_Attendance();
                    var mob = await db.UserMasters.Where(c => c.userId == userId).FirstOrDefaultAsync();
                    if (mob != null)
                    {
                        string imei1 = mob.imoNo;
                        string imei2 = mob.imoNo2;
                        if (mob.imoNo != null && mob.imoNo2 != null)
                        {
                            if (mob.imoNo == imeinos)
                            {
                                mob.imoNo = null;
                                result.IsInSync = true;
                                result.UserId = userId;
                                result.batterystatus = batteryStatus;
                                result.imei = imei1;
                                mob.imoNo = mob.imoNo2;
                                mob.imoNo2 = null;
                                await db.SaveChangesAsync();
                            }
                            else
                            {
                                result.IsInSync = false;
                                result.UserId = userId;
                                result.batterystatus = batteryStatus;
                                result.imei = imei2;
                            }
                        }
                        else
                        {
                            result.IsInSync = false;
                            result.UserId = userId;
                            result.batterystatus = batteryStatus;
                            result.imei = imei1;
                        }
                    }


                }
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString(), ex);
                return result;

            }

        }
        public async Task<SBUser> CheckSupervisorUserLoginAsync(string userName, string password, string EmpType)
        {
            SBUser user = new SBUser();
            try
            {
                if (EmpType == "A")
                {
                    user = await SupervisorLoginAsync(userName, password, EmpType);
                }
                else
                {
                    user.name = "";
                    user.userId = 0;
                    user.userLoginId = "";
                    user.userPassword = "";
                    user.status = "error";
                    user.gtFeatures = false;
                    user.EmpType = "";
                    user.imiNo = "";
                    user.message = "Employee Type Does not Match.";
                    user.messageMar = "कर्मचारी प्रकार जुळत नाही.";
                }
                return user;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString(), ex);
                return user;
            }

        }
        public async Task<List<NameULB>> GetUlbAsync(int userId, string Status)
        {
            List<NameULB> obj = new List<NameULB>();
            try
            {
                using (DevICTSBMMainEntities dbMain = new DevICTSBMMainEntities())
                {
                    var ids = await dbMain.EmployeeMasters.Where(c => c.EmpId == userId).Select(c => c.isActiveULB).FirstOrDefaultAsync();
                    if (ids != null)
                    {

                        string[] authorsList = ids.Split(',');
                        foreach (string author in authorsList)
                        {
                            if (Status == "false")
                            {
                                var data = await dbMain.AppDetails.Where(c => c.AppId.ToString().ToLower().Contains(author.Replace(" ", "").ToLower())).ToListAsync();
                                foreach (var x in data)
                                {
                                    obj.Add(new NameULB()
                                    {
                                        ulb = (x.AppName.ToString()),
                                        appid = (x.AppId),
                                        faq = x.FAQ,
                                    });
                                }
                            }
                            else if (Status == "true")
                            {
                                var data = await dbMain.AppDetails.Where(c => c.AppId.ToString().ToLower().Contains(author.Replace(" ", "").ToLower()) && c.FAQ != "0").ToListAsync();
                                foreach (var x in data)
                                {
                                    obj.Add(new NameULB()
                                    {
                                        ulb = (x.AppName.ToString()),
                                        appid = (x.AppId),
                                        faq = x.FAQ,
                                    });
                                }
                            }
                        }
                    }
                    else
                    {
                        if (Status == "false")
                        {
                            var data = await dbMain.AppDetails.Where(c => c.IsActive == true && c.AppId != 3088).ToListAsync();
                            foreach (var x in data)
                            {
                                obj.Add(new NameULB()
                                {
                                    ulb = (x.AppName.ToString()),
                                    appid = (x.AppId),
                                    faq = x.FAQ,
                                });
                            }
                        }
                        else if (Status == "true")
                        {
                            var data = await dbMain.AppDetails.Where(c => c.IsActive == true && c.FAQ != "0" && c.AppId != 3088).ToListAsync();
                            foreach (var x in data)
                            {
                                obj.Add(new NameULB()
                                {
                                    ulb = (x.AppName.ToString()),
                                    appid = (x.AppId),
                                    faq = x.FAQ,
                                });
                            }
                        }


                    }
                }
                return obj.OrderBy(c => c.ulb).ToList();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString(), ex);
                return obj;
            }

        }
        public async Task<HSDashboard> GetSelectedUlbDataAsync(int userId, string EmpType, int appId)
        {
            HSDashboard model = new HSDashboard();
            try
            {
                using (DevICTSBMChildEntities db = new DevICTSBMChildEntities(appId))
                using (DevICTSBMMainEntities dbMain = new DevICTSBMMainEntities())
                {
                    var appdetails = await dbMain.AppDetails.Where(c => c.AppId == appId).FirstOrDefaultAsync();
                    //var data = db.SP_HouseScanifyDetails(appId).First();
                    List<SqlParameter> parms = new List<SqlParameter>
                                                {
                                                    // Create parameter(s)    
                                                    new SqlParameter { ParameterName = "@appId", Value = appId }
                                                };
                    var Listdata = await db.SP_HouseScanifyDetails_Results.FromSqlRaw<SP_HouseScanifyDetails_Result>("EXEC SP_HouseScanifyDetails @appId", parms.ToArray()).ToListAsync();
                    var data = Listdata == null ? null : Listdata.FirstOrDefault();


                    if (data != null)
                    {
                        model.AppId = appdetails.AppId;
                        model.AppName = appdetails.AppName;
                        model.TotalHouse = data.TotalHouse;
                        model.TotalHouseUpdated = data.TotalHouseUpdated;
                        model.TotalHouseUpdated_CurrentDay = data.TotalHouseUpdated_CurrentDay;
                        model.TotalPoint = data.TotalPoint;
                        model.TotalPointUpdated = data.TotalPointUpdated;
                        model.TotalPointUpdated_CurrentDay = data.TotalPointUpdated_CurrentDay;
                        model.TotalDump = data.TotalDump;
                        model.TotalDumpUpdated = data.TotalDumpUpdated;
                        model.TotalDumpUpdated_CurrentDay = data.TotalDumpUpdated_CurrentDay;

                        model.TotalLiquid = data.TotalLiquid;
                        model.TotalLiquidUpdated = data.TotalLiquidUpdated;
                        model.TotalLiquidUpdated_CurrentDay = data.TotalLiquidUpdated_CurrentDay;

                        model.TotalStreet = data.TotalStreet;
                        model.TotalStreetUpdated = data.TotalStreetUpdated;
                        model.TotalStreetUpdated_CurrentDay = data.TotalStreetUpdated_CurrentDay;

                        return model;
                    }

                    else
                    {
                        return model;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString(), ex);
                return model;
            }
        }
        public async Task<List<HSEmployee>> GetQREmployeeListAsync(int userId, string EmpType, int appId)
        {
            List<HSEmployee> obj = new List<HSEmployee>();
            try
            {
                using (DevICTSBMChildEntities db = new DevICTSBMChildEntities(appId))
                {
                    {
                        var data = await db.QrEmployeeMasters.Where(c => c.isActive == true).ToListAsync();
                        if (data != null && data.Count > 0)
                        {
                            foreach (var x in data)
                            {
                                obj.Add(new HSEmployee()
                                {
                                    EmployeeName = (x.qrEmpName.ToString()),
                                    EmployeeID = (x.qrEmpId),
                                });
                            }

                        }

                    }
                    if (obj != null && obj.Count > 0)
                        return obj.OrderBy(c => c.EmployeeName).ToList();
                    else
                        return obj;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString(), ex);
                return obj;
            }

        }
        public async Task<List<HouseScanifyEmployeeDetails>> GetQREmployeeDetailsListAsync(int userId, string EmpType, int appId, int QrEmpID, bool status)
        {
            List<HouseScanifyEmployeeDetails> obj = new List<HouseScanifyEmployeeDetails>();
            try
            {
                using (DevICTSBMChildEntities db = new DevICTSBMChildEntities(appId))
                {
                    {
                        if (QrEmpID != 0)
                        {
                            var data = await db.QrEmployeeMasters.Where(c => c.qrEmpId == QrEmpID).ToListAsync();
                            if (data != null && data.Count > 0)
                            {
                                foreach (var x in data)
                                {
                                    obj.Add(new HouseScanifyEmployeeDetails()
                                    {
                                        qrEmpId = x.qrEmpId,
                                        qrEmpName = x.qrEmpName.ToString(),
                                        qrEmpLoginId = x.qrEmpLoginId,
                                        qrEmpPassword = x.qrEmpPassword,
                                        qrEmpMobileNumber = x.qrEmpMobileNumber,
                                        qrEmpAddress = x.qrEmpAddress,
                                        type = x.type,
                                        typeId = x.typeId,
                                        imoNo = x.imoNo,
                                        bloodGroup = x.bloodGroup,
                                        isActive = x.isActive,
                                        target = x.target,
                                        lastModifyDate = x.lastModifyDate,
                                    });
                                }
                            }
                        }
                        else
                        {
                            var data = await db.QrEmployeeMasters.Where(c => c.isActive == status).ToListAsync();
                            if (data != null && data.Count > 0)
                            {
                                foreach (var x in data)
                                {
                                    obj.Add(new HouseScanifyEmployeeDetails()
                                    {
                                        qrEmpId = x.qrEmpId,
                                        qrEmpName = x.qrEmpName.ToString(),
                                        qrEmpLoginId = x.qrEmpLoginId,
                                        qrEmpPassword = x.qrEmpPassword,
                                        qrEmpMobileNumber = x.qrEmpMobileNumber,
                                        qrEmpAddress = x.qrEmpAddress,
                                        type = x.type,
                                        typeId = x.typeId,
                                        imoNo = x.imoNo,
                                        bloodGroup = x.bloodGroup,
                                        isActive = x.isActive,
                                        target = x.target,
                                        lastModifyDate = x.lastModifyDate,
                                    });
                                }
                            }
                        }

                    }
                    if (obj != null && obj.Count > 0)
                        return obj.OrderByDescending(c => c.qrEmpId).ToList();
                    else
                        return obj;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString(), ex);
                return obj;
            }

        }
        public async Task<IEnumerable<HouseScanifyDetailsGridRow>> GetHouseScanifyDetailsAsync(int qrEmpId, DateTime FromDate, DateTime Todate, int appId)
        {
            List<HouseScanifyDetailsGridRow> obj = new List<HouseScanifyDetailsGridRow>();
            try
            {
                using (DevICTSBMChildEntities db = new DevICTSBMChildEntities(appId))
                {

                    //var data = db.SP_HouseScanify(FromDate, Todate, qrEmpId).ToList();
                    List<SqlParameter> parms = new List<SqlParameter>
                                                {
                                                    // Create parameter(s)    
                                                    new SqlParameter { ParameterName = "@fdate", Value = FromDate },
                                                    new SqlParameter { ParameterName = "@tdate", Value = Todate },
                                                    new SqlParameter { ParameterName = "@userid", Value = qrEmpId }
                                                };
                    var data = await db.SP_HouseScanify_Results.FromSqlRaw<SP_HouseScanify_Result>("EXEC SP_HouseScanify @fdate,@tdate,@userid", parms.ToArray()).ToListAsync();



                    if (data != null && data.Count > 0)
                    {
                        foreach (var x in data)
                        {
                            var data1 = await db.Qr_Employee_Daily_Attendances.Where(c => c.qrEmpId == x.qrEMpId && EF.Functions.DateDiffDay(c.startDate, FromDate) <= 0 && EF.Functions.DateDiffDay(c.startDate, Todate) >= 0).ToListAsync();

                            if (data1 != null && data1.Count != 0)
                            {
                                obj.Add(new HouseScanifyDetailsGridRow()
                                {
                                    qrEmpId = x.qrEMpId,
                                    qrEmpName = x.qrEmpName,
                                    qrEmpNameMar = x.qrEmpNameMar,
                                    qrEmpMobileNumber = x.qrEmpMobileNumber,
                                    qrEmpAddress = x.qrEmpAddress,
                                    qrEmpLoginId = x.qrEmpLoginId,
                                    qrEmpPassword = x.qrEmpPassword,
                                    isActive = x.isActive,
                                    bloodGroup = x.bloodGroup,
                                    lastModifyDate = x.lastModifyDate,
                                    HouseCount = x.HouseCount,
                                    PointCount = x.PointCount,
                                    DumpCount = x.DumpCount,
                                    LiquidCount = x.LiquidCount,
                                    StreetCount = x.StreetCount,
                                });
                            }


                        }
                    }

                    if (obj != null && obj.Count > 0)
                        return obj.OrderByDescending(c => c.LiquidCount).ThenByDescending(c => c.HouseCount).ThenByDescending(c => c.StreetCount);
                    else
                        return obj;


                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString(), ex);
                return obj;
            }

        }

        public async Task<SBUser> SupervisorLoginAsync(string userName, string password, string EmpType)
        {
            SBUser user = new SBUser();
            try
            {
                using (DevICTSBMMainEntities dbMain = new DevICTSBMMainEntities())
                {
                    var obj = await dbMain.EmployeeMasters.Where(c => c.LoginId == userName & c.Password == password & c.isActive == true).FirstOrDefaultAsync();
                    if (obj == null)
                    {
                        user.name = "";
                        user.userId = 0;
                        user.userLoginId = "";
                        user.userPassword = "";
                        user.status = "error";
                        user.gtFeatures = false;
                        user.EmpType = "";
                        user.imiNo = "";
                        user.message = "UserName or Passward not Match.";
                        user.messageMar = "वापरकर्ता नाव किंवा पासवर्ड जुळत नाही.";
                        user.hsusertoken = "";
                    }
                    else if (obj != null && obj.LoginId == userName && obj.Password == password)
                    {

                        user.name = obj.EmpName;
                        user.userId = obj.EmpId;
                        user.userLoginId = obj.LoginId;
                        user.userPassword = obj.Password;
                        user.EmpType = checkNull(obj.type); ;
                        user.imiNo = "";
                        user.type = "";
                        user.gtFeatures = true;
                        user.status = "success"; user.message = "Login Successfully"; user.messageMar = "लॉगिन यशस्वी";

                        var authClaims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name,user.name),
                             new Claim("userLoginId",user.userLoginId),
                             new Claim("userPassword",user.userPassword),
                            new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                        };
                        var authSigninkey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["JWT:Secret"]));
                        var token = new JwtSecurityToken(
                            issuer: _configuration["JWT:ValidIssuer"],
                            audience: _configuration["JWT:ValidAudience"],
                            expires: DateTime.Now.AddYears(1),
                            claims: authClaims,
                            signingCredentials: new SigningCredentials(authSigninkey, SecurityAlgorithms.HmacSha256Signature));
                        user.hsusertoken = new JwtSecurityTokenHandler().WriteToken(token);
                    }

                    return user;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString(), ex);
                return user;

            }
        }

        public async Task<IEnumerable<HSAttendanceGrid>> GetAttendanceDetailsAsync(int userId, DateTime FromDate, DateTime Todate, int appId)
        {
            List<HSAttendanceGrid> obj = new List<HSAttendanceGrid>();
            try
            {
                using (DevICTSBMChildEntities db = new DevICTSBMChildEntities(appId))
                {

                    var data = await (from t1 in db.Qr_Employee_Daily_Attendances
                                      join t2 in db.QrEmployeeMasters on t1.qrEmpId equals t2.qrEmpId
                                      select new
                                      {
                                          t1.qrEmpDaId,
                                          t1.qrEmpId,
                                          t1.startDate,
                                          t1.endDate,
                                          t1.startTime,
                                          t1.endTime,
                                          t1.startLat,
                                          t1.startLong,
                                          t1.endLat,
                                          t1.endLong,
                                          t1.startNote,
                                          t1.endNote,
                                          t2.qrEmpName,

                                      }).OrderByDescending(c => c.startDate).ThenByDescending(c => c.startTime).ToListAsync();

                    //return obj.OrderBy(c => c.Date).ThenByDescending(c => c.StartTime);

                    //var data = db.Qr_Employee_Daily_Attendance.OrderByDescending(c => c.qrEmpDaId).ToList();

                    if (data != null && data.Count > 0)
                    {
                        if (Convert.ToDateTime(FromDate).ToString("dd/MM/yyyy") == Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy"))
                        {
                            data = data.OrderByDescending(c => c.qrEmpDaId).Where(c => (c.startDate == FromDate || c.endDate == FromDate || c.endTime == "")).ToList();
                        }
                        else
                        {

                            data = data.OrderByDescending(c => c.qrEmpDaId).Where(c => (c.startDate >= FromDate && c.startDate <= Todate) || (c.startDate >= FromDate && c.startDate <= Todate)).ToList();
                        }

                        if (userId > 0)
                        {
                            var model = data.OrderByDescending(c => c.qrEmpDaId).Where(c => c.qrEmpId == userId).ToList();

                            data = model.ToList();
                        }

                        foreach (var x in data)
                        {

                            DateTime cDate = DateTime.Now;

                            TimeSpan timespan = new TimeSpan(00, 00, 00);
                            DateTime time = DateTime.Now.Add(timespan);
                            string displayTime = time.ToString("hh:mm tt");


                            string displayTime1 = Convert.ToDateTime(x.startDate).ToString("MM/dd/yyyy");
                            string sTime = Convert.ToDateTime(x.startTime).ToString("HH:mm:ss");

                            var a = (Convert.ToDateTime(x.startDate).ToString("MM/dd/yyyy"));
                            var b = x.endDate == null ? Convert.ToDateTime(cDate).ToString("MM/dd/yyyy") : Convert.ToDateTime(x.endDate).ToString("MM/dd/yyyy");

                            string Time1 = (x.startTime).ToString();
                            string Time2 = ((x.endTime == "" ? displayTime : x.endTime).ToString());

                            DateTime startDate = Convert.ToDateTime(a + " " + Time1);
                            DateTime endDate = Convert.ToDateTime(b + " " + Time2);
                            var houseCount = await db.HouseMasters.Where(c => c.modified >= startDate && c.modified <= endDate && c.userId == x.qrEmpId).CountAsync();
                            var liquidCount = await db.LiquidWasteDetails.Where(c => c.lastModifiedDate >= startDate && c.lastModifiedDate <= endDate && c.userId == x.qrEmpId).CountAsync();
                            var streetCount = await db.StreetSweepingDetails.Where(c => c.lastModifiedDate >= startDate && c.lastModifiedDate <= endDate && c.userId == x.qrEmpId).CountAsync();
                            var dumpyardcount = await db.DumpYardDetails.Where(c => c.lastModifiedDate >= startDate && c.lastModifiedDate <= endDate && c.userId == x.qrEmpId).CountAsync();

                            string endate = "";
                            if (x.endDate == null)
                            {
                                endate = "";
                            }
                            else
                            {
                                endate = Convert.ToDateTime(x.endDate).ToString("dd/MM/yyyy");
                            }
                            obj.Add(new HSAttendanceGrid()
                            {
                                qrEmpDaId = x.qrEmpDaId,
                                qrEmpId = Convert.ToInt32(x.qrEmpId),
                                userName = x.qrEmpName,
                                startDate = Convert.ToDateTime(x.startDate).ToString("dd/MM/yyyy"),
                                endDate = endate,
                                startTime = checkNull(x.startTime),
                                endTime = checkNull(x.endTime),
                                startLat = checkNull(x.startLat),
                                startLong = checkNull(x.startLong),
                                endLat = checkNull(x.endLat),
                                endLong = checkNull(x.endLong),
                                startNote = checkNull(x.startNote),
                                endNote = checkNull(x.endNote),
                                CompareDate = x.startDate,
                                HouseCount = houseCount,
                                LiquidCount = liquidCount,
                                StreetCount = streetCount,
                                DumpYardCount = dumpyardcount,
                                daDateTIme = (displayTime1 + " " + sTime)



                            });
                        }


                    }
                    if (obj != null && obj.Count > 0)
                        return obj.OrderByDescending(c => c.qrEmpDaId).ToList();
                    else
                        return obj;
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString(), ex);
                return obj;
            }

        }

        public async Task<IEnumerable<HSHouseDetailsGrid>> GetHouseDetailsAsync(int userId, DateTime FromDate, DateTime Todate, int appId, string ReferanceId)
        {
            List<HSHouseDetailsGrid> obj = new List<HSHouseDetailsGrid>();
            try
            {
                using (DevICTSBMChildEntities db = new DevICTSBMChildEntities(appId))
                {
                    List<SqlParameter> parms = new List<SqlParameter>
                                                {
                                                    // Create parameter(s)    
                                                    new SqlParameter { ParameterName = "@fdate", Value = FromDate },
                                                    new SqlParameter { ParameterName = "@tdate", Value = Todate },
                                                    new SqlParameter { ParameterName = "@userid", Value = userId },
                                                    new SqlParameter { ParameterName = "@referanceId", Value = ReferanceId }
                                                };
                    var data = await db.SP_HouseDetailsApp_Results.FromSqlRaw<SP_HouseDetailsApp_Result>("EXEC SP_HouseDetailsApp @fdate,@tdate,@userid,@referanceId", parms.ToArray()).ToListAsync();
                    if (data != null && data.Count > 0)
                    {
                        obj = data.Select(x => new HSHouseDetailsGrid
                        {
                            houseId = x.houseId,
                            Name = x.qrEmpName,
                            Lat = x.houseLat,
                            Long = x.houseLong,
                            QRCodeImage = x.QRCodeImage,
                            ReferanceId = x.ReferanceId,
                            modifiedDate = x.modified.HasValue ? Convert.ToDateTime(x.modified).ToString("dd/MM/yyyy hh:mm tt") : "",
                            QRStatus = x.QRStatus,


                        }).ToList();
                    }

                    return obj;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString(), ex);
                return obj;

            }
        }
        public async Task<IEnumerable<HSDumpYardDetailsGrid>> GetDumpYardDetailsAsync(int userId, DateTime FromDate, DateTime Todate, int appId)
        {
            List<HSDumpYardDetailsGrid> obj = new List<HSDumpYardDetailsGrid>();
            try
            {
                using (DevICTSBMChildEntities db = new DevICTSBMChildEntities(appId))
                {

                    List<SqlParameter> parms = new List<SqlParameter>
                                                {
                                                    // Create parameter(s)    
                                                    new SqlParameter { ParameterName = "@fdate", Value = FromDate },
                                                    new SqlParameter { ParameterName = "@tdate", Value = Todate },
                                                    new SqlParameter { ParameterName = "@userid", Value = userId }
                                                };
                    var data = await db.SP_DumpYardDetailsApp_Results.FromSqlRaw<SP_DumpYardDetailsApp_Result>("EXEC SP_DumpYardDetailsApp @fdate,@tdate,@userid", parms.ToArray()).ToListAsync();
                    if (data != null && data.Count > 0)
                    {
                        obj = data.Select(x => new HSDumpYardDetailsGrid
                        {
                            dyId = x.dyId,
                            Name = x.qrEmpName,
                            Lat = x.dyLat,
                            Long = x.dyLong,
                            QRCodeImage = x.QRCodeImage,
                            ReferanceId = x.ReferanceId,
                            modifiedDate = x.lastModifiedDate.HasValue ? Convert.ToDateTime(x.lastModifiedDate).ToString("dd/MM/yyyy hh:mm tt") : "",
                            QRStatus = x.QRStatus,


                        }).ToList();
                    }

                    return obj;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString(), ex);
                return obj;
            }
        }

        public async Task<IEnumerable<HSLiquidDetailsGrid>> GetLiquidDetailsAsync(int userId, DateTime FromDate, DateTime Todate, int appId)
        {
            List<HSLiquidDetailsGrid> obj = new List<HSLiquidDetailsGrid>();
            try
            {
                using (DevICTSBMChildEntities db = new DevICTSBMChildEntities(appId))
                {
                    List<SqlParameter> parms = new List<SqlParameter>
                                                {
                                                    // Create parameter(s)    
                                                    new SqlParameter { ParameterName = "@fdate", Value = FromDate },
                                                    new SqlParameter { ParameterName = "@tdate", Value = Todate },
                                                    new SqlParameter { ParameterName = "@userid", Value = userId }
                                                };
                    var data = await db.SP_LiquidDetailsApp_Results.FromSqlRaw<SP_LiquidDetailsApp_Result>("EXEC SP_LiquidDetailsApp @fdate,@tdate,@userid", parms.ToArray()).ToListAsync();

                    if (data != null && data.Count > 0)
                    {
                        obj = data.Select(x => new HSLiquidDetailsGrid
                        {
                            LWId = x.LWId,
                            Name = x.qrEmpName,
                            Lat = x.LWLat,
                            Long = x.LWLong,
                            QRCodeImage = x.QRCodeImage,
                            ReferanceId = x.ReferanceId,
                            modifiedDate = x.lastModifiedDate.HasValue ? Convert.ToDateTime(x.lastModifiedDate).ToString("dd/MM/yyyy hh:mm tt") : "",
                            QRStatus = x.QRStatus,

                        }).ToList();
                    }

                    return obj;

                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString(), ex);
                return obj;
            }
        }


        public async Task<IEnumerable<HSStreetDetailsGrid>> GetStreetDetailsAsync(int userId, DateTime FromDate, DateTime Todate, int appId)
        {
            List<HSStreetDetailsGrid> obj = new List<HSStreetDetailsGrid>();
            try
            {
                using (DevICTSBMChildEntities db = new DevICTSBMChildEntities(appId))
                {

                    List<SqlParameter> parms = new List<SqlParameter>
                                                {
                                                    // Create parameter(s)    
                                                    new SqlParameter { ParameterName = "@fdate", Value = FromDate },
                                                    new SqlParameter { ParameterName = "@tdate", Value = Todate },
                                                    new SqlParameter { ParameterName = "@userid", Value = userId }
                                                };
                    var data = await db.SP_StreetDetailsApp_Results.FromSqlRaw<SP_StreetDetailsApp_Result>("EXEC SP_StreetDetailsApp @fdate,@tdate,@userid", parms.ToArray()).ToListAsync();

                    if (data != null && data.Count > 0)
                    {
                        obj = data.Select(x => new HSStreetDetailsGrid
                        {
                            SSId = x.SSId,
                            Name = x.qrEmpName,
                            Lat = x.SSLat,
                            Long = x.SSLong,
                            QRCodeImage = x.QRCodeImage,
                            ReferanceId = x.ReferanceId,
                            modifiedDate = x.lastModifiedDate.HasValue ? Convert.ToDateTime(x.lastModifiedDate).ToString("dd/MM/yyyy hh:mm tt") : "",
                            QRStatus = x.QRStatus,

                        }).ToList();
                    }

                    return obj;
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString(), ex);
                return obj;
            }
        }
        public async Task<List<UserRoleDetails>> UserRoleListAsync(int userId, string EmpType, bool status, int EmpId)
        {
            List<UserRoleDetails> obj = new List<UserRoleDetails>();
            try
            {
                using (DevICTSBMMainEntities dbMain = new DevICTSBMMainEntities())
                {
                    if (EmpId != 0)
                    {
                        var data = await dbMain.EmployeeMasters.Where(c => c.isActive == status && c.EmpId == EmpId).ToListAsync();
                        if (data != null && data.Count > 0)
                        {
                            foreach (var x in data)
                            {
                                obj.Add(new UserRoleDetails()
                                {
                                    EmpId = x.EmpId,
                                    EmpName = x.EmpName.ToString(),
                                    LoginId = x.LoginId,
                                    Password = x.Password,
                                    EmpMobileNumber = x.EmpMobileNumber,
                                    EmpAddress = x.EmpAddress,
                                    type = x.type,
                                    isActive = x.isActive,
                                    isActiveULB = x.isActiveULB,
                                    LastModifyDateEntry = Convert.ToDateTime(x.lastModifyDateEntry).ToString("dd-MM-yyyy hh:mm"),
                                });
                            }
                        }

                    }
                    else
                    {
                        var data = await dbMain.EmployeeMasters.Where(c => c.isActive == status).ToListAsync();
                        if (data != null && data.Count > 0)
                        {

                            foreach (var x in data)
                            {
                                obj.Add(new UserRoleDetails()
                                {
                                    EmpId = x.EmpId,
                                    EmpName = x.EmpName.ToString(),
                                    LoginId = x.LoginId,
                                    Password = x.Password,
                                    EmpMobileNumber = x.EmpMobileNumber,
                                    EmpAddress = x.EmpAddress,
                                    type = x.type,
                                    isActive = x.isActive,
                                    isActiveULB = x.isActiveULB,
                                    LastModifyDateEntry = Convert.ToDateTime(x.lastModifyDateEntry).ToString("dd-MM-yyyy hh:mm"),
                                });
                            }
                        }

                    }

                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString(), ex);
                return obj;
            }
            if (obj != null && obj.Count > 0)
                return obj.OrderByDescending(c => c.EmpId).ToList();
            else
                return obj;
        }


        public async Task<CollectionSyncResult> SaveAddEmployeeAsync(HouseScanifyEmployeeDetails obj, int AppId)
        {
            CollectionSyncResult result = new CollectionSyncResult();
            QrEmployeeMaster objdata = new QrEmployeeMaster();
            using (DevICTSBMChildEntities db = new DevICTSBMChildEntities(AppId))
            {
                try
                {
                    if (obj.qrEmpId != 0)
                    {
                        var model = await db.QrEmployeeMasters.Where(c => c.qrEmpId == obj.qrEmpId).FirstOrDefaultAsync();
                        if (model != null)
                        {

                            //var isrecord = db.QrEmployeeMasters.Where(x => x.qrEmpName == obj.qrEmpName && x.isActive == true).FirstOrDefault();
                            //if (isrecord == null)
                            //{

                            //var isrecord1 = db.QrEmployeeMasters.Where(x => x.qrEmpLoginId == obj.qrEmpLoginId && x.isActive == true).FirstOrDefault();
                            //var isrecord2 = db.UserMasters.Where(x => x.userLoginId == obj.qrEmpLoginId && x.isActive == true).FirstOrDefault();
                            //if (isrecord1 == null && isrecord2 == null)
                            //{

                            model.qrEmpId = obj.qrEmpId;
                            model.qrEmpName = obj.qrEmpName;
                            // model.qrEmpLoginId = obj.qrEmpLoginId;
                            model.qrEmpPassword = obj.qrEmpPassword;
                            model.qrEmpMobileNumber = obj.qrEmpMobileNumber;
                            model.qrEmpAddress = obj.qrEmpAddress;
                            model.type = "Employee";
                            model.typeId = 1;
                            model.imoNo = obj.imoNo;
                            model.bloodGroup = "0";
                            model.isActive = obj.isActive;

                            await db.SaveChangesAsync();
                            result.status = "success";
                            result.message = "Employee Details Updated successfully";
                            result.messageMar = "कर्मचारी तपशील यशस्वीरित्या अद्यतनित केले";
                            //}
                            //else
                            //{
                            //    result.status = "Error";
                            //    result.message = "This LoginId Is Already Exist !";
                            //    result.messageMar = "हे लॉगिनआयडी आधीच अस्तित्वात आहे !";
                            //    return result;
                            //}
                            //}
                            //else
                            //{
                            //    result.status = "Error";
                            //    result.message = "Name Already Exist";
                            //    result.messageMar = "नाव आधीपासून अस्तित्वात आहे..";
                            //    return result;
                            //}

                        }
                        else
                        {
                            result.message = "This Employee Not Available.";
                            result.messageMar = "कर्मचारी उपलब्ध नाही.";
                            result.status = "error";
                            return result;

                        }

                    }
                    else
                    {
                        var isrecord = await db.QrEmployeeMasters.Where(x => x.qrEmpName == obj.qrEmpName && x.isActive == true).FirstOrDefaultAsync();
                        if (isrecord == null)
                        {
                            var isrecord1 = await db.QrEmployeeMasters.Where(x => x.qrEmpLoginId == obj.qrEmpLoginId && x.isActive == true).FirstOrDefaultAsync();
                            var isrecord2 = await db.UserMasters.Where(x => x.userLoginId == obj.qrEmpLoginId && x.isActive == true).FirstOrDefaultAsync();
                            if (isrecord1 == null && isrecord2 == null)
                            {


                                objdata.qrEmpName = obj.qrEmpName;
                                objdata.qrEmpLoginId = obj.qrEmpLoginId;
                                objdata.qrEmpPassword = obj.qrEmpPassword;
                                objdata.qrEmpMobileNumber = obj.qrEmpMobileNumber;
                                objdata.qrEmpAddress = obj.qrEmpAddress;
                                objdata.type = "Employee";
                                objdata.typeId = 1;
                                //objdata.imoNo = obj.imoNo;
                                objdata.imoNo = null;
                                objdata.bloodGroup = "0";
                                objdata.isActive = obj.isActive;

                                db.QrEmployeeMasters.Add(objdata);
                                await db.SaveChangesAsync();
                                result.status = "success";
                                result.message = "Employee Details Added successfully";
                                result.messageMar = "कर्मचारी तपशील यशस्वीरित्या जोडले";
                                return result;
                            }
                            else
                            {
                                result.status = "Error";
                                result.message = "This LoginId Is Already Exist !";
                                result.messageMar = "हे लॉगिनआयडी आधीच अस्तित्वात आहे !";
                                return result;
                            }


                        }
                        else
                        {
                            result.status = "Error";
                            result.message = "Name Already Exist";
                            result.messageMar = "नाव आधीपासून अस्तित्वात आहे..";
                            return result;
                        }
                    }

                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.ToString(), ex);
                    result.message = "Something is wrong,Try Again.. ";
                    result.messageMar = "काहीतरी चुकीचे आहे, पुन्हा प्रयत्न करा..";
                    result.status = "error";
                    return result;
                }


            }

            return result;
        }


        public async Task<CollectionSyncResult> SaveAddUserRoleAsync(UserRoleDetails obj)
        {
            CollectionSyncResult result = new CollectionSyncResult();
            EmployeeMaster objdata = new EmployeeMaster();

            try
            {
                using (DevICTSBMMainEntities dbMain = new DevICTSBMMainEntities())
                {
                    if (obj.EmpId != 0)
                    {
                        var model = await dbMain.EmployeeMasters.Where(c => c.EmpId == obj.EmpId).FirstOrDefaultAsync();
                        if (model != null)
                        {
                            model.EmpId = obj.EmpId;
                            model.EmpName = obj.EmpName;
                            model.LoginId = obj.LoginId;
                            model.Password = obj.Password;
                            model.EmpMobileNumber = obj.EmpMobileNumber;
                            model.EmpAddress = obj.EmpAddress;
                            model.type = obj.type;
                            model.isActive = obj.isActive;
                            model.isActiveULB = obj.isActiveULB;
                            model.lastModifyDateEntry = DateTime.Now;


                            await dbMain.SaveChangesAsync();
                            result.status = "success";
                            result.message = "User Role Details Updated successfully";
                            result.messageMar = "वापरकर्ता भूमिका तपशील यशस्वीरित्या अद्यतनित केले";
                        }
                        else
                        {
                            result.message = "This User Role Not Available.";
                            result.messageMar = "वापरकर्ता भूमिका उपलब्ध नाही.";
                            result.status = "error";
                            return result;

                        }

                    }
                    else
                    {
                        objdata.EmpName = obj.EmpName;
                        objdata.LoginId = obj.LoginId;
                        objdata.Password = obj.Password;
                        objdata.EmpMobileNumber = obj.EmpMobileNumber;
                        objdata.EmpAddress = obj.EmpAddress;
                        objdata.type = obj.type;
                        objdata.isActive = obj.isActive;
                        objdata.isActiveULB = obj.isActiveULB;
                        objdata.lastModifyDateEntry = DateTime.Now;

                        dbMain.EmployeeMasters.Add(objdata);
                        await dbMain.SaveChangesAsync();
                        result.status = "success";
                        result.message = "User Role Added successfully";
                        result.messageMar = "वापरकर्ता भूमिका तपशील यशस्वीरित्या जोडले";
                        return result;
                    }

                }


            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString(), ex);
                result.message = "Something is wrong,Try Again.. ";
                result.messageMar = "काहीतरी चुकीचे आहे, पुन्हा प्रयत्न करा..";
                result.status = "error";
                return result;
            }


            return result;
        }


        public async Task<CollectionQRStatusResult> UpdateQRstatusAsync(HSHouseDetailsGrid obj, int AppId)
        {
            CollectionQRStatusResult result = new CollectionQRStatusResult();
            HouseMaster objdata = new HouseMaster();
            using (DevICTSBMChildEntities db = new DevICTSBMChildEntities(AppId))
            {
                try
                {
                    if (obj.ReferanceId != null)
                    {
                        var model = await db.HouseMasters.Where(c => c.ReferanceId == obj.ReferanceId).FirstOrDefaultAsync();
                        if (model != null)
                        {

                            model.QRStatus = obj.QRStatus;
                            model.QRStatusDate = DateTime.Now;
                            await db.SaveChangesAsync();
                            result.ReferanceId = obj.ReferanceId;
                            result.status = "success";
                            result.message = "Record Updated successfully";
                            result.messageMar = "रेकॉर्ड यशस्वीरित्या अद्यतनित केले";
                            return result;
                        }
                        else
                        {
                            var dump = await db.DumpYardDetails.Where(c => c.ReferanceId == obj.ReferanceId).FirstOrDefaultAsync();
                            if (dump != null)
                            {

                                dump.QRStatus = obj.QRStatus;
                                dump.QRStatusDate = DateTime.Now;
                                await db.SaveChangesAsync();
                                result.ReferanceId = obj.ReferanceId;
                                result.status = "success";
                                result.message = "Record Updated successfully";
                                result.messageMar = "रेकॉर्ड यशस्वीरित्या अद्यतनित केले";
                                return result;
                            }
                            else
                            {
                                var street = await db.StreetSweepingDetails.Where(c => c.ReferanceId == obj.ReferanceId).FirstOrDefaultAsync();
                                if (street != null)
                                {

                                    street.QRStatus = obj.QRStatus;
                                    street.QRStatusDate = DateTime.Now;
                                    await db.SaveChangesAsync();
                                    result.ReferanceId = obj.ReferanceId;
                                    result.status = "success";
                                    result.message = "Record Updated successfully";
                                    result.messageMar = "रेकॉर्ड यशस्वीरित्या अद्यतनित केले";
                                    return result;
                                }
                                else
                                {
                                    var liquid = await db.LiquidWasteDetails.Where(c => c.ReferanceId == obj.ReferanceId).FirstOrDefaultAsync();
                                    if (liquid != null)
                                    {

                                        liquid.QRStatus = obj.QRStatus;
                                        liquid.QRStatusDate = DateTime.Now;
                                        await db.SaveChangesAsync();
                                        result.ReferanceId = obj.ReferanceId;
                                        result.status = "success";
                                        result.message = "Record Updated successfully";
                                        result.messageMar = "रेकॉर्ड यशस्वीरित्या अद्यतनित केले";
                                        return result;
                                    }
                                    else
                                    {
                                        result.ReferanceId = obj.ReferanceId;
                                        result.message = "This Record Not Available.";
                                        result.messageMar = "रेकॉर्ड उपलब्ध नाही.";
                                        result.status = "error";
                                        return result;

                                    }
                                }
                            }
                        }


                    }

                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.ToString(), ex);
                    result.ReferanceId = obj.ReferanceId;
                    result.message = "Something is wrong,Try Again.. ";
                    result.messageMar = "काहीतरी चुकीचे आहे, पुन्हा प्रयत्न करा..";
                    result.status = "error";
                    return result;
                }


            }

            return result;
        }

        public async Task<Result> SaveSupervisorAttendenceAsync(BigVQREmployeeAttendenceVM obj, int type)
        {
            Result result = new Result();

            using (DevICTSBMMainEntities dbMain = new DevICTSBMMainEntities())
            {
                if (type == 0)
                {

                    HSUR_Daily_Attendance data = dbMain.HSUR_Daily_Attendances.Where(c => c.userId == obj.qrEmpId && c.ip_address == null && c.login_device == null && (c.endTime == null || c.endTime == "")).FirstOrDefault();
                    if (data != null)
                    {
                        data.endTime = obj.startTime;
                        data.daEndDate = obj.startDate;
                        data.endLat = obj.startLat;
                        data.endLong = obj.startLong;
                        data.OutbatteryStatus = obj.batteryStatus;
                        await dbMain.SaveChangesAsync();
                    }
                    try
                    {
                        HSUR_Daily_Attendance objdata = new HSUR_Daily_Attendance();

                        var isActive = dbMain.EmployeeMasters.Where(c => c.EmpId == obj.qrEmpId && c.isActive == true && c.type == "SA").FirstOrDefault();
                        if (isActive != null)
                        {

                            objdata.userId = obj.qrEmpId;
                            objdata.daDate = obj.startDate;
                            objdata.endLat = "";
                            objdata.startLat = obj.startLat;
                            objdata.startLong = obj.startLong;
                            objdata.startTime = obj.startTime;
                            objdata.endTime = "";
                            objdata.EmployeeType = obj.EmployeeType;
                            objdata.batteryStatus = obj.batteryStatus;
                            dbMain.HSUR_Daily_Attendances.Add(objdata);
                            UR_Location loc = new UR_Location();
                            loc.empId = obj.qrEmpId;
                            loc.datetime = DateTime.Now;
                            loc.lat = obj.startLat;
                            loc._long = obj.startLong;
                            loc.batteryStatus = obj.batteryStatus;
                            loc.type = 1;
                            loc.address = Address(obj.endLat + "," + obj.endLong);
                            if (loc.address != "")
                            { loc.area = area(loc.address); }
                            else
                            {
                                loc.area = "";
                            }
                            dbMain.UR_Locations.Add(loc);
                            await dbMain.SaveChangesAsync();
                            result.status = "success";
                            result.message = "Shift started Successfully";
                            result.messageMar = "शिफ्ट सुरू";
                            result.isAttendenceOn = true;
                            result.isAttendenceOff = false;
                            return result;
                        }
                        else
                        {
                            result.status = "Error";
                            result.message = "Contact To Administrator";
                            result.messageMar = "प्रशासकाशी संपर्क साधा";
                            return result;
                        }
                    }

                    catch (Exception ex)
                    {
                        _logger.LogError(ex.ToString(), ex);
                        result.status = "error";
                        result.message = "Something is wrong,Try Again.. ";
                        result.messageMar = "काहीतरी चुकीचे आहे, पुन्हा प्रयत्न करा..";
                        return result;
                    }


                }
                else
                {

                    try
                    {
                        HSUR_Daily_Attendance objdata = await dbMain.HSUR_Daily_Attendances.Where(c => EF.Functions.DateDiffDay(c.daDate, obj.endDate) == 0 && c.userId == obj.qrEmpId && c.ip_address == null && c.login_device == null && (c.endTime == "" || c.endTime == null)).FirstOrDefaultAsync();
                        if (objdata != null)
                        {

                            objdata.userId = obj.qrEmpId;
                            objdata.daEndDate = obj.endDate;
                            objdata.endLat = obj.endLat;
                            objdata.endLong = obj.endLong;
                            objdata.endTime = obj.endTime;
                            objdata.EmployeeType = obj.EmployeeType;
                            objdata.OutbatteryStatus = obj.batteryStatus;
                            //objdata.endAddress = Address(objdata.endLat + "," + objdata.endLong);

                            ///////////////////////////////////////////////////////////////////

                            UR_Location loc = new UR_Location();
                            loc.empId = obj.qrEmpId;
                            loc.datetime = DateTime.Now;
                            loc.lat = obj.endLat;
                            loc._long = obj.endLong;
                            loc.batteryStatus = obj.batteryStatus;
                            loc.type = 1;
                            loc.address = Address(obj.endLat + "," + obj.endLong);
                            if (loc.address != "")
                            { loc.area = area(loc.address); }
                            else
                            {
                                loc.area = "";
                            }
                            dbMain.UR_Locations.Add(loc);

                            ///////////////////////////////////////////////////////////////////

                            await dbMain.SaveChangesAsync();
                            result.status = "success";
                            result.message = "Shift ended successfully";
                            result.messageMar = "शिफ्ट संपले";
                            result.isAttendenceOn = false;
                            result.isAttendenceOff = true;

                            return result;
                        }


                        else
                        {
                            HSUR_Daily_Attendance objdata2 = await dbMain.HSUR_Daily_Attendances.Where(c => c.userId == obj.qrEmpId && c.ip_address == null && c.login_device == null && (c.endTime == "" || c.endTime == null)).OrderByDescending(c => c.daID).FirstOrDefaultAsync();
                            objdata2.userId = obj.qrEmpId;
                            objdata2.daEndDate = obj.endDate;
                            objdata2.endLat = obj.endLat;
                            objdata2.endLong = obj.endLong;
                            objdata2.endTime = obj.endTime;
                            objdata2.EmployeeType = obj.EmployeeType;
                            objdata2.OutbatteryStatus = obj.batteryStatus;

                            //       objdata.endAddress = Address(objdata.endLat + "," + objdata.endLong);

                            ///////////////////////////////////////////////////////////////////

                            UR_Location loc = new UR_Location();
                            loc.empId = obj.qrEmpId;
                            loc.datetime = DateTime.Now;
                            loc.lat = obj.endLat;
                            loc._long = obj.endLong;
                            loc.batteryStatus = obj.batteryStatus;
                            loc.type = 1;
                            loc.address = Address(obj.endLat + "," + obj.endLong);
                            if (loc.address != "")
                            { loc.area = area(loc.address); }
                            else
                            {
                                loc.area = "";
                            }
                            dbMain.UR_Locations.Add(loc);

                            ///////////////////////////////////////////////////////////////////

                            await dbMain.SaveChangesAsync();
                            result.status = "success";
                            result.message = "Shift ended successfully";
                            result.messageMar = "शिफ्ट संपले";
                            result.isAttendenceOn = false;
                            result.isAttendenceOff = true;
                            return result;
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex.ToString(), ex);
                        result.status = "error";
                        result.message = "Something is wrong,Try Again.. ";
                        result.messageMar = "काहीतरी चुकीचे आहे, पुन्हा प्रयत्न करा..";
                        return result;
                    }
                }
            }


        }

        public async Task<Result> CheckSupervisorAttendenceAsync(BigVQREmployeeAttendenceVM obj)
        {
            Result result = new Result();
            try
            {
                using (DevICTSBMMainEntities dbMain = new DevICTSBMMainEntities())
                {
                    HSUR_Daily_Attendance objdata = new HSUR_Daily_Attendance();
                    var isActive = await dbMain.EmployeeMasters.Where(c => c.EmpId == obj.qrEmpId && c.isActive == true && c.type == "SA").FirstOrDefaultAsync();
                    var AttenIn = await dbMain.HSUR_Daily_Attendances.Where(c => EF.Functions.DateDiffDay(c.daDate, obj.startDate) == 0 && c.userId == obj.qrEmpId && c.ip_address == null && c.login_device == null && (c.endTime == null || c.endTime == "")).FirstOrDefaultAsync();
                    if (isActive != null)
                    {
                        if (AttenIn != null)
                        {
                            result.status = "Success";
                            result.message = "Shift On";
                            result.messageMar = "शिफ्ट सुरू";
                            result.isAttendenceOn = true;
                            result.isAttendenceOff = false;
                            return result;
                        }
                        else
                        {
                            result.status = "Success";
                            result.message = "Shift Off";
                            result.messageMar = "शिफ्ट बंद";
                            result.isAttendenceOn = false;
                            result.isAttendenceOff = true;
                            return result;
                        }
                    }
                    else
                    {
                        result.status = "Error";
                        result.message = "Contact To Administrator";
                        result.messageMar = "प्रशासकाशी संपर्क साधा";
                        return result;
                    }

                }

            }

            catch (Exception ex)
            {
                _logger.LogError(ex.ToString(), ex);
                result.status = "error";
                result.message = "Something is wrong,Try Again.. ";
                result.messageMar = "काहीतरी चुकीचे आहे, पुन्हा प्रयत्न करा..";
                return result;
            }
        }


        public async Task<List<HouseDetailsVM>> GetHouseListAsync(int appId, string EmpType)
        {
            List<HouseDetailsVM> obj = new List<HouseDetailsVM>();
            obj = await GetHouseForNormalAsync(appId, EmpType);
            return obj;

        }


        public async Task<List<HouseDetailsVM>> GetHouseForNormalAsync(int AppId, string EmpType)
        {
            List<HouseDetailsVM> obj = new List<HouseDetailsVM>();
            try
            {
                using (DevICTSBMChildEntities db = new DevICTSBMChildEntities(AppId))
                {
                    var house = await db.HouseMasters.Where(x => x.ReferanceId.Contains(EmpType) && x.houseLat != null && x.houseLong != null).Select(x => new { x.ReferanceId, x.houseNumber }).ToListAsync();
                    if (house != null && house.Count > 0)
                    {
                        foreach (var x in house)
                        {
                            string HouseN = "";
                            if (x.houseNumber == null || x.houseNumber == "")
                            {
                                HouseN = x.ReferanceId;
                            }
                            else { HouseN = x.houseNumber; }
                            obj.Add(new HouseDetailsVM()
                            {
                                houseid = x.ReferanceId,
                                houseNumber = HouseN,

                            });
                        }
                    }


                    var dump = await db.DumpYardDetails.Where(x => x.ReferanceId.Contains(EmpType) && x.dyLat != null && x.dyLong != null).Select(x => new { x.ReferanceId }).ToListAsync();
                    if (dump != null && dump.Count > 0)
                    {
                        foreach (var x in dump)
                        {

                            obj.Add(new HouseDetailsVM()
                            {
                                houseid = x.ReferanceId,
                                houseNumber = x.ReferanceId,

                            });
                        }

                    }


                    var LW = db.LiquidWasteDetails.Where(x => x.ReferanceId.Contains(EmpType) && x.LWLat != null && x.LWLong != null).Select(x => new { x.ReferanceId }).ToList();
                    if (LW != null && LW.Count > 0)
                    {
                        foreach (var x in LW)
                        {
                            obj.Add(new HouseDetailsVM()
                            {
                                houseid = x.ReferanceId,
                                houseNumber = x.ReferanceId,

                            });
                        }
                    }

                    var SSD = db.StreetSweepingDetails.Where(x => x.ReferanceId.Contains(EmpType) && x.SSLat != null && x.SSLong != null).Select(x => new { x.ReferanceId }).ToList();
                    if (SSD != null && SSD.Count > 0)
                    {
                        foreach (var x in SSD)
                        {

                            obj.Add(new HouseDetailsVM()
                            {
                                houseid = x.ReferanceId,
                                houseNumber = x.ReferanceId,

                            });
                        }
                    }

                }
                return obj;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString(), ex);
                return obj;
            }
        }


        public async Task<List<HSHouseDetailsGrid>> GetHDSLListAsync(int appId, string EmpType, string ReferanceId)
        {
            List<HSHouseDetailsGrid> objDetail = new List<HSHouseDetailsGrid>();
            if (EmpType == "H")
            {
                objDetail = await GetHouseDetailsByIdAsync(appId, ReferanceId);
            }
            if (EmpType == "L")
            {
                objDetail = await GetLiquidDetailsByIdAsync(appId, ReferanceId);
            }
            if (EmpType == "S")
            {
                objDetail = await GetStreetDetailsByIdAsync(appId, ReferanceId);
            }

            if (EmpType == "D")
            {
                objDetail = await GetDumpYardDetailsByIdAsync(appId, ReferanceId);
            }


            return objDetail;

        }


        public async Task<List<HSHouseDetailsGrid>> GetHouseDetailsByIdAsync(int appId, string ReferanceId)
        {
            List<HSHouseDetailsGrid> obj = new List<HSHouseDetailsGrid>();
            try
            {
                using (DevICTSBMChildEntities db = new DevICTSBMChildEntities(appId))
                {
                    var data = await db.HouseMasters.Where(x => x.ReferanceId == ReferanceId).ToListAsync();
                    if (data != null && data.Count > 0)
                    {
                        foreach (var x in data)
                        {
                            var name = await db.QrEmployeeMasters.Where(c => c.qrEmpId == x.userId).Select(c => new { c.qrEmpName }).FirstOrDefaultAsync();
                            if (name != null)
                            {
                                string QRCodeImage = "";
                                var BQI = await db.HouseMasters.Where(c => c.ReferanceId == ReferanceId).Select(c => new { c.BinaryQrCodeImage }).FirstOrDefaultAsync();
                                if (BQI.BinaryQrCodeImage != null)
                                {
                                    QRCodeImage = Convert.ToBase64String(x.BinaryQrCodeImage);
                                }
                                if (string.IsNullOrEmpty(QRCodeImage))
                                {
                                    QRCodeImage = "/Images/default_not_upload.png";
                                }
                                else
                                {
                                    QRCodeImage = "data:image/jpeg;base64," + QRCodeImage;
                                }
                                obj.Add(new HSHouseDetailsGrid()
                                {
                                    houseId = x.houseId,
                                    Name = name.qrEmpName,
                                    Lat = x.houseLat,
                                    Long = x.houseLong,
                                    QRCodeImage = QRCodeImage,
                                    ReferanceId = x.ReferanceId,
                                    modifiedDate = x.modified.HasValue ? Convert.ToDateTime(x.modified).ToString("dd/MM/yyyy hh:mm tt") : "",
                                    QRStatus = x.QRStatus,


                                });
                            }
                            else
                            {
                                obj.Add(new HSHouseDetailsGrid()
                                {
                                    houseId = x.houseId,
                                    Name = "",
                                    Lat = x.houseLat,
                                    Long = x.houseLong,
                                    QRCodeImage = "/Images/default_not_upload.png",
                                    ReferanceId = x.ReferanceId,
                                    modifiedDate = x.modified.HasValue ? Convert.ToDateTime(x.modified).ToString("dd/MM/yyyy hh:mm tt") : "",
                                    QRStatus = x.QRStatus,
                                });
                            }

                        }

                    }
                    return obj;
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString(), ex);
                return obj;

            }
        }
        public async Task<List<HSHouseDetailsGrid>> GetLiquidDetailsByIdAsync(int appId, string ReferanceId)
        {
            List<HSHouseDetailsGrid> obj = new List<HSHouseDetailsGrid>();
            try
            {
                using (DevICTSBMChildEntities db = new DevICTSBMChildEntities(appId))
                {

                    var data = await db.LiquidWasteDetails.Where(x => x.ReferanceId == ReferanceId).ToListAsync();
                    if (data != null && data.Count > 0)
                    {
                        foreach (var x in data)
                        {
                            var name = await db.QrEmployeeMasters.Where(c => c.qrEmpId == x.userId).Select(c => new { c.qrEmpName }).FirstOrDefaultAsync();
                            if (name != null)
                            {
                                string QRCodeImage = "";
                                var BQI = await db.LiquidWasteDetails.Where(c => c.ReferanceId == ReferanceId).Select(c => new { c.BinaryQrCodeImage }).FirstOrDefaultAsync();
                                if (BQI.BinaryQrCodeImage != null)
                                {
                                    QRCodeImage = Convert.ToBase64String(x.BinaryQrCodeImage);
                                }
                                if (string.IsNullOrEmpty(QRCodeImage))
                                {
                                    QRCodeImage = "/Images/default_not_upload.png";
                                }
                                else
                                {
                                    QRCodeImage = "data:image/jpeg;base64," + QRCodeImage;
                                }
                                obj.Add(new HSHouseDetailsGrid()
                                {
                                    houseId = x.LWId,
                                    Name = name.qrEmpName,
                                    Lat = x.LWLat,
                                    Long = x.LWLong,
                                    QRCodeImage = QRCodeImage,
                                    ReferanceId = x.ReferanceId,
                                    modifiedDate = x.lastModifiedDate.HasValue ? Convert.ToDateTime(x.lastModifiedDate).ToString("dd/MM/yyyy hh:mm tt") : "",
                                    QRStatus = x.QRStatus,
                                });
                            }
                            else
                            {
                                obj.Add(new HSHouseDetailsGrid()
                                {
                                    houseId = x.LWId,
                                    Name = "",
                                    Lat = x.LWLat,
                                    Long = x.LWLong,
                                    QRCodeImage = "/Images/default_not_upload.png",
                                    ReferanceId = x.ReferanceId,
                                    modifiedDate = x.lastModifiedDate.HasValue ? Convert.ToDateTime(x.lastModifiedDate).ToString("dd/MM/yyyy hh:mm tt") : "",
                                    QRStatus = x.QRStatus,
                                });
                            }
                        }


                    }
                    return obj;
                }


            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString(), ex);
                return obj;
            }
        }


        public async Task<List<HSHouseDetailsGrid>> GetStreetDetailsByIdAsync(int appId, string ReferanceId)
        {
            List<HSHouseDetailsGrid> obj = new List<HSHouseDetailsGrid>();
            try
            {
                using (DevICTSBMChildEntities db = new DevICTSBMChildEntities(appId))
                {

                    var data = await db.StreetSweepingDetails.Where(x => x.ReferanceId == ReferanceId).ToListAsync();

                    if (data != null && data.Count > 0)
                    {
                        foreach (var x in data)
                        {
                            var name = await db.QrEmployeeMasters.Where(c => c.qrEmpId == x.userId).Select(c => new { c.qrEmpName }).FirstOrDefaultAsync();
                            if (name != null)
                            {
                                string QRCodeImage = "";
                                var BQI = await db.StreetSweepingDetails.Where(c => c.ReferanceId == ReferanceId).Select(c => new { c.BinaryQrCodeImage }).FirstOrDefaultAsync();
                                if (BQI.BinaryQrCodeImage != null)
                                {
                                    QRCodeImage = Convert.ToBase64String(x.BinaryQrCodeImage);
                                }
                                if (string.IsNullOrEmpty(QRCodeImage))
                                {
                                    QRCodeImage = "/Images/default_not_upload.png";
                                }
                                else
                                {
                                    QRCodeImage = "data:image/jpeg;base64," + QRCodeImage;
                                }
                                obj.Add(new HSHouseDetailsGrid()
                                {
                                    houseId = x.SSId,
                                    Name = name.qrEmpName,
                                    Lat = x.SSLat,
                                    Long = x.SSLong,
                                    QRCodeImage = QRCodeImage,
                                    ReferanceId = x.ReferanceId,
                                    modifiedDate = x.lastModifiedDate.HasValue ? Convert.ToDateTime(x.lastModifiedDate).ToString("dd/MM/yyyy hh:mm tt") : "",
                                    QRStatus = x.QRStatus,
                                });
                            }
                            else
                            {
                                obj.Add(new HSHouseDetailsGrid()
                                {
                                    houseId = x.SSId,
                                    Name = "",
                                    Lat = x.SSLat,
                                    Long = x.SSLong,
                                    QRCodeImage = "/Images/default_not_upload.png",
                                    ReferanceId = x.ReferanceId,
                                    modifiedDate = x.lastModifiedDate.HasValue ? Convert.ToDateTime(x.lastModifiedDate).ToString("dd/MM/yyyy hh:mm tt") : "",
                                    QRStatus = x.QRStatus,
                                });
                            }
                        }
                    }

                    return obj;
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString(), ex);
                return obj;
            }
        }


        public async Task<List<HSHouseDetailsGrid>> GetDumpYardDetailsByIdAsync(int appId, string ReferanceId)
        {
            List<HSHouseDetailsGrid> obj = new List<HSHouseDetailsGrid>();

            try
            {
                using (DevICTSBMChildEntities db = new DevICTSBMChildEntities(appId))
                {
                    var data = await db.DumpYardDetails.Where(x => x.ReferanceId == ReferanceId).ToListAsync();
                    if (data != null && data.Count > 0)
                    {
                        foreach (var x in data)
                        {
                            var name = await db.QrEmployeeMasters.Where(c => c.qrEmpId == x.userId).Select(c => new { c.qrEmpName }).FirstOrDefaultAsync();
                            if (name != null)
                            {
                                string QRCodeImage = "";
                                var BQI = await db.DumpYardDetails.Where(c => c.ReferanceId == ReferanceId).Select(c => new { c.BinaryQrCodeImage }).FirstOrDefaultAsync();
                                if (BQI.BinaryQrCodeImage != null)
                                {
                                    QRCodeImage = Convert.ToBase64String(x.BinaryQrCodeImage);
                                }
                                if (string.IsNullOrEmpty(QRCodeImage))
                                {
                                    QRCodeImage = "/Images/default_not_upload.png";
                                }
                                else
                                {
                                    QRCodeImage = "data:image/jpeg;base64," + QRCodeImage;
                                }
                                obj.Add(new HSHouseDetailsGrid()
                                {
                                    houseId = x.dyId,
                                    Name = name.qrEmpName,
                                    Lat = x.dyLat,
                                    Long = x.dyLong,
                                    QRCodeImage = QRCodeImage,
                                    ReferanceId = x.ReferanceId,
                                    modifiedDate = x.lastModifiedDate.HasValue ? Convert.ToDateTime(x.lastModifiedDate).ToString("dd/MM/yyyy hh:mm tt") : "",
                                    QRStatus = x.QRStatus,

                                });
                            }
                            else
                            {
                                obj.Add(new HSHouseDetailsGrid()
                                {
                                    houseId = x.dyId,
                                    Name = "",
                                    Lat = x.dyLat,
                                    Long = x.dyLong,
                                    QRCodeImage = "/Images/default_not_upload.png",
                                    ReferanceId = x.ReferanceId,
                                    modifiedDate = x.lastModifiedDate.HasValue ? Convert.ToDateTime(x.lastModifiedDate).ToString("dd/MM/yyyy hh:mm tt") : "",
                                    QRStatus = x.QRStatus,
                                });
                            }
                        }

                    }
                    return obj;
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString(), ex);
                return obj;

            }
        }

        public async Task<IEnumerable<UREmployeeAttendence>> UserRoleAttendanceAsync(int userid, DateTime FromDate, DateTime Todate, bool type)
        {
            List<UREmployeeAttendence> obj = new List<UREmployeeAttendence>();
            try
            {
                using (DevICTSBMMainEntities dbMain = new DevICTSBMMainEntities())
                {
                    if (type == true)
                    {
                        if (userid == 0)
                        {
                            var data = await dbMain.HSUR_Daily_Attendances.Where(c => c.startLat != null && c.daDate >= FromDate && c.daDate <= Todate).ToListAsync();
                            if (data != null && data.Count > 0)
                            {
                                foreach (var x in data)
                                {
                                    obj.Add(new UREmployeeAttendence()
                                    {
                                        qrEmpDaId = x.daID,
                                        qrEmpId = x.userId,
                                        startLat = x.startLat,
                                        startLong = x.startLong,
                                        endLat = x.endLat,
                                        endLong = x.endLong,
                                        startTime = x.startTime,
                                        endTime = x.endTime,
                                        startDate = x.daDate == null ? null : Convert.ToDateTime(x.daDate).ToString("dd/MM/yyyy"),
                                        endDate = x.daEndDate == null ? null : Convert.ToDateTime(x.daEndDate).ToString("dd/MM/yyyy"),
                                        batteryStatus = x.batteryStatus,
                                        OutbatteryStatus = x.batteryStatus,
                                        EmployeeType = x.EmployeeType,
                                        IpAddress = x.ip_address,
                                        LoginDevice = x.login_device,
                                        HostName = x.HostName,
                                        EmployeeName = await dbMain.EmployeeMasters.Where(c => c.EmpId == x.userId).Select(s => s.EmpName).FirstOrDefaultAsync(),
                                    });
                                }
                            }

                        }
                        else
                        {
                            var data = await dbMain.HSUR_Daily_Attendances.Where(c => c.startLat != null && c.daDate >= FromDate && c.daDate <= Todate && c.userId == userid).ToListAsync();
                            if (data != null && data.Count > 0)
                            {
                                foreach (var x in data)
                                {
                                    obj.Add(new UREmployeeAttendence()
                                    {
                                        qrEmpDaId = x.daID,
                                        qrEmpId = x.userId,
                                        startLat = x.startLat,
                                        startLong = x.startLong,
                                        endLat = x.endLat,
                                        endLong = x.endLong,
                                        startTime = x.startTime,
                                        endTime = x.endTime,
                                        startDate = x.daDate == null ? null : Convert.ToDateTime(x.daDate).ToString("dd/MM/yyyy"),
                                        endDate = x.daEndDate == null ? null : Convert.ToDateTime(x.daEndDate).ToString("dd/MM/yyyy"),
                                        batteryStatus = x.batteryStatus,
                                        OutbatteryStatus = x.batteryStatus,
                                        EmployeeType = x.EmployeeType,
                                        IpAddress = x.ip_address,
                                        LoginDevice = x.login_device,
                                        HostName = x.HostName,
                                        EmployeeName = await dbMain.EmployeeMasters.Where(c => c.EmpId == x.userId).Select(s => s.EmpName).FirstOrDefaultAsync(),
                                    });
                                }

                            }

                        }

                    }
                    else
                    {
                        if (userid == 0)
                        {
                            var data = await dbMain.HSUR_Daily_Attendances.Where(c => c.startLat == null && c.daDate >= FromDate && c.daDate <= Todate).ToListAsync();
                            if (data != null && data.Count > 0)
                            {
                                foreach (var x in data)
                                {
                                    obj.Add(new UREmployeeAttendence()
                                    {
                                        qrEmpDaId = x.daID,
                                        qrEmpId = x.userId,
                                        startLat = x.startLat,
                                        startLong = x.startLong,
                                        endLat = x.endLat,
                                        endLong = x.endLong,
                                        startTime = x.startTime,
                                        endTime = x.endTime,
                                        startDate = x.daDate == null ? null : Convert.ToDateTime(x.daDate).ToString("dd/MM/yyyy"),
                                        endDate = x.daEndDate == null ? null : Convert.ToDateTime(x.daEndDate).ToString("dd/MM/yyyy"),
                                        batteryStatus = x.batteryStatus,
                                        OutbatteryStatus = x.batteryStatus,
                                        EmployeeType = x.EmployeeType,
                                        IpAddress = x.ip_address,
                                        LoginDevice = x.login_device,
                                        HostName = x.HostName,
                                        EmployeeName = await dbMain.EmployeeMasters.Where(c => c.EmpId == x.userId).Select(s => s.EmpName).FirstOrDefaultAsync(),
                                    });
                                }
                            }

                        }
                        else
                        {
                            var data = await dbMain.HSUR_Daily_Attendances.Where(c => c.startLat == null && c.daDate >= FromDate && c.daDate <= Todate && c.userId == userid).ToListAsync();
                            if (data != null && data.Count > 0)
                            {
                                foreach (var x in data)
                                {
                                    obj.Add(new UREmployeeAttendence()
                                    {
                                        qrEmpDaId = x.daID,
                                        qrEmpId = x.userId,
                                        startLat = x.startLat,
                                        startLong = x.startLong,
                                        endLat = x.endLat,
                                        endLong = x.endLong,
                                        startTime = x.startTime,
                                        endTime = x.endTime,
                                        startDate = x.daDate == null ? null : Convert.ToDateTime(x.daDate).ToString("dd/MM/yyyy"),
                                        endDate = x.daEndDate == null ? null : Convert.ToDateTime(x.daEndDate).ToString("dd/MM/yyyy"),
                                        batteryStatus = x.batteryStatus,
                                        OutbatteryStatus = x.batteryStatus,
                                        EmployeeType = x.EmployeeType,
                                        IpAddress = x.ip_address,
                                        LoginDevice = x.login_device,
                                        HostName = x.HostName,
                                        EmployeeName = await dbMain.EmployeeMasters.Where(c => c.EmpId == x.userId).Select(s => s.EmpName).FirstOrDefaultAsync(),
                                    });
                                }
                            }

                        }

                    }
                }



            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString(), ex);
                return obj;
            }
            if (obj != null && obj.Count > 0)
                return obj.OrderByDescending(c => c.qrEmpDaId).ToList();
            else
                return obj;
        }


        public string checkNull(string str)
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
        public int checkIntNull(string str)
        {
            int result = 0;
            if (str == null || str == "")
            {
                result = 0;
                return result;
            }
            else
            {
                result = Convert.ToInt32(str);
                return result;
            }
        }
        public string ImagePathCMS(string FolderName, string Image, AppDetail objmain)
        {
            string ImageUrl;
            if (Image == null || Image == "")
            {
                ImageUrl = "";
                return ImageUrl;
            }
            else
            {
                var AppDetailURL = objmain.baseImageUrlCMS + objmain.basePath + FolderName + "/";
                ImageUrl = AppDetailURL + Image;
                return ImageUrl;
            }
        }

        private async Task<List<String>> DeviceDetailsFCM(string houseId, int AppId)
        {
            using (DevICTSBMChildEntities db = new DevICTSBMChildEntities(AppId))
            {
                var FCM = await db.DeviceDetails.Where(x => x.ReferenceID == houseId & x.FCMID != null).ToListAsync();

                List<String> ArrayList = new List<String>();

                foreach (var x in FCM)
                {
                    ArrayList.Add(x.FCMID);
                }

                return ArrayList;
            }

        }
        public void sendSMS(string sms, string MobilNumber)
        {
            try
            {
                //HttpWebRequest myReq = (HttpWebRequest)WebRequest.Create("https://www.smsjust.com/sms/user/urlsms.php?username=ycagent&pass=yocc@5095&senderid=YOCCAG&dest_mobileno=" + MobilNumber + "&msgtype=UNI&message=" + sms + "&response=Y");
                //HttpWebRequest myReq = (HttpWebRequest)WebRequest.Create("https://www.smsjust.com/sms/user/urlsms.php?username=ycagent&pass=yocc@5095&senderid=YOCCAG&dest_mobileno=" + MobilNumber + "&message=" + sms + "&response=Y");
                //HttpWebRequest myReq = (HttpWebRequest)WebRequest.Create("https://www.smsjust.com/sms/user/urlsms.php?username=artiyocc&pass=123456&senderid=BIGVCL&dest_mobileno=" + MobilNumber + "&msgtype=UNI&message="+ sms + "%20&response=Y");

                HttpWebRequest myReq = (HttpWebRequest)WebRequest.Create("https://www.smsjust.com/sms/user/urlsms.php?username=ycagent&pass=yocc@5095&senderid=BIGVCL&dest_mobileno=" + MobilNumber + "&message=" + sms + "%20&response=Y");

                //  HttpWebRequest myReq = (HttpWebRequest)WebRequest.Create("https://www.smsjust.com/sms/user/urlsms.php?username=artiyocc&pass=123456&senderid=BIGVCL&dest_mobileno=" + MobilNumber + "&message=" + sms + "%20&response=Y");

                //Get response from Ozeki NG SMS Gateway Server and read the answer
                HttpWebResponse myResp = (HttpWebResponse)myReq.GetResponse();
                System.IO.StreamReader respStreamReader = new System.IO.StreamReader(myResp.GetResponseStream());
                string responseString = respStreamReader.ReadToEnd();
                respStreamReader.Close();
                myResp.Close();
            }
            catch { }

        }
        public void sendSMSmar(string sms, string MobilNumber)
        {
            try
            {
                //HttpWebRequest myReq = (HttpWebRequest)WebRequest.Create("https://www.smsjust.com/sms/user/urlsms.php?username=ycagent&pass=yocc@5095&senderid=YOCCAG&dest_mobileno=" + MobilNumber + "&msgtype=UNI&message=" + sms + "&response=Y");
                //HttpWebRequest myReq = (HttpWebRequest)WebRequest.Create("https://www.smsjust.com/sms/user/urlsms.php?username=ycagent&pass=yocc@5095&senderid=YOCCAG&dest_mobileno=" + MobilNumber + "&message=" + sms + "&response=Y");
                //HttpWebRequest myReq = (HttpWebRequest)WebRequest.Create("https://www.smsjust.com/sms/user/urlsms.php?username=artiyocc&pass=123456&senderid=BIGVCL&dest_mobileno=" + MobilNumber + "&msgtype=UNI&message="+ sms + "%20&response=Y");
                HttpWebRequest myReq = (HttpWebRequest)WebRequest.Create(" https://www.smsjust.com/sms/user/urlsms.php?username=ycagent&pass=yocc@5095&senderid=BIGVCL&dest_mobileno=" + MobilNumber + "&msgtype=UNI&message=" + sms + "%20&response=Y");
                //Get response from Ozeki NG SMS Gateway Server and read the answer
                HttpWebResponse myResp = (HttpWebResponse)myReq.GetResponse();
                System.IO.StreamReader respStreamReader = new System.IO.StreamReader(myResp.GetResponseStream());
                string responseString = respStreamReader.ReadToEnd();
                respStreamReader.Close();
                myResp.Close();
            }
            catch { }

        }
        public void PushNotificationMessageBroadCast(string message, List<String> FCMID, string title, string ApiKey)
        {

            string authKey = ApiKey; //"AIzaSyDIBhuq26awLm3qQ8yuTYuoFx5WTPOuA7I";

            try
            {
                var result = "-1";
                var webAddr = _configuration["AppSettings:FCMUrl"]; // "https://fcm.googleapis.com/fcm/send";
                string senderId = _configuration["AppSettings:senderId"]; //"858685861086";

                string regID = (FCMID.Count > 1 ? (string.Join("\",\"", FCMID)) : (string.Join("\"\"", FCMID)));

                //string.Join("\",\"", FCMID);

                var httpWebRequest = (HttpWebRequest)WebRequest.Create(webAddr);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Headers.Add("Authorization:key=" + authKey);
                httpWebRequest.Headers.Add(string.Format("Sender:id=" + senderId));
                httpWebRequest.Method = "POST";


                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    string json = "{\"registration_ids\": [\"" + regID + "\"],\"data\": {\"title\": \"" + title + " \",\"message\": \"" + message + "\",\"body\": \"GG\"},\"priority\":10,\"timestamp\": \"" + DateTime.Now + "\"}";

                    //registration_ids, array of strings -  to, single recipient
                    streamWriter.Write(json);
                    streamWriter.Flush();
                }

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    result = streamReader.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
            }

        }

        public async Task<DataTable> SqlHelperExecSP(string spName, List<SqlParameter> parms, int AppID)
        {

            var data = new DataTable();
            try
            {
                using (var db = new DevICTSBMChildEntities(AppID))
                {
                    var conn = db.Database.GetDbConnection();
                    var connectionState = conn.State;
                    try
                    {
                        if (connectionState != ConnectionState.Open) conn.Open();
                        using (var cmd = conn.CreateCommand())
                        {
                            cmd.CommandText = spName;
                            cmd.CommandType = CommandType.StoredProcedure;
                            if (parms != null)
                            {
                                foreach (var parm in parms)
                                {
                                    cmd.Parameters.Add(parm);
                                }
                            }
                            using (var reader = await cmd.ExecuteReaderAsync())
                            {
                                data.Load(reader);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        // error handling
                        return data;
                    }
                    finally
                    {
                        if (connectionState != ConnectionState.Closed) conn.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                return data;
            }
            return data;
        }

        public async Task<Qr_Location> FillLocationDetailsAsync(BigVQRHPDVM obj, int AppId, bool IsOffline)
        {
            var distCount = "";
            string addre = string.Empty, batteryStatus = string.Empty;
            Qr_Location loc = new Qr_Location();
            using (DevICTSBMChildEntities db = new DevICTSBMChildEntities(AppId))
            using (DevICTSBMMainEntities dbMain = new DevICTSBMMainEntities())
            {
                try
                {
                    //var locc = dbMain2.SP_UserLatLongDetailMain(obj.userId, 0).FirstOrDefault();
                    List<SqlParameter> parms = new List<SqlParameter>
                                                {
                                                    // Create parameter(s)    
                                                    new SqlParameter { ParameterName = "@userid", Value = obj.userId },
                                                    new SqlParameter { ParameterName = "@typeId", Value = 0 }
                                                };
                    var Listlocc = await dbMain.SP_UserLatLongDetailMain_Results.FromSqlRaw<SP_UserLatLongDetailMain_Result>("EXEC SP_UserLatLongDetailMain @userid,@typeId", parms.ToArray()).ToListAsync();
                    var locc = Listlocc == null ? null : Listlocc.FirstOrDefault();

                    if (locc == null || locc.lat == "" || locc.@long == "")
                    {
                        string a = obj.Lat;
                        string b = obj.Long;

                        //var dist = db.SP_DistanceCount(Convert.ToDouble(a), Convert.ToDouble(b), Convert.ToDouble(obj.Lat), Convert.ToDouble(obj.Long)).FirstOrDefault();
                        List<SqlParameter> parms1 = new List<SqlParameter>
                                                    {
                                                        // Create parameter(s)    
                                                        new SqlParameter { ParameterName = "@sLat", Value = Convert.ToDouble(a) },
                                                        new SqlParameter { ParameterName = "@sLong", Value = Convert.ToDouble(b) },
                                                        new SqlParameter { ParameterName = "@dLat", Value = Convert.ToDouble(obj.Lat) },
                                                        new SqlParameter { ParameterName = "@dLong", Value = Convert.ToDouble(obj.Long) }
                                                    };
                        var Listdist = await dbMain.SP_DistanceCount_Main_Results.FromSqlRaw<SP_DistanceCount_Main_Result>("EXEC SP_DistanceCount @sLat,@sLong,@dLat,@dLong", parms1.ToArray()).ToListAsync();
                        var dist = Listdist == null ? null : Listdist.FirstOrDefault();
                        distCount = dist.Distance_in_KM.ToString();
                    }
                    else
                    {
                        //var dist = db.SP_DistanceCount(Convert.ToDouble(locc.lat), Convert.ToDouble(locc.@long), Convert.ToDouble(obj.Lat), Convert.ToDouble(obj.Long)).FirstOrDefault();
                        List<SqlParameter> parms1 = new List<SqlParameter>
                                                    {
                                                        // Create parameter(s)    
                                                        new SqlParameter { ParameterName = "@sLat", Value = Convert.ToDouble(locc.lat) },
                                                        new SqlParameter { ParameterName = "@sLong", Value = Convert.ToDouble(locc.@long) },
                                                        new SqlParameter { ParameterName = "@dLat", Value = Convert.ToDouble(obj.Lat) },
                                                        new SqlParameter { ParameterName = "@dLong", Value = Convert.ToDouble(obj.Long) }
                                                    };
                        var Listdist = await dbMain.SP_DistanceCount_Main_Results.FromSqlRaw<SP_DistanceCount_Main_Result>("EXEC SP_DistanceCount @sLat,@sLong,@dLat,@dLong", parms1.ToArray()).ToListAsync();
                        var dist = Listdist == null ? null : Listdist.FirstOrDefault();
                        distCount = dist.Distance_in_KM.ToString();
                    }

                    loc.Distnace = Convert.ToDecimal(distCount);
                    obj.Address = addre;
                    loc.datetime = obj.date; //DateTime.Now;
                    loc.lat = obj.Lat;
                    loc._long = obj.Long;
                    loc.address = obj.Address;
                    //loc.batteryStatus = batteryStatus;
                    loc.area = (obj.Address != "") ? area(loc.address) : "";
                    loc.empId = obj.userId;
                    loc.type = 1;
                    loc.ReferanceID = obj.ReferanceId;
                    loc.IsOffline = (IsOffline == true ? true : false);
                    loc.CreatedDate = DateTime.Now;


                }

                catch (Exception ex)
                {
                    _logger.LogError(ex.ToString(), ex);
                    return loc;
                }
            }

            return loc;
        }

        public bool IsPointInPolygon(List<coordinates> poly, coordinates p)
        {
            bool inside = false;

            if (poly != null && poly.Count > 0)
            {
                double minX = poly[0].lat ?? 0;
                double maxX = poly[0].lat ?? 0;
                double minY = poly[0].lng ?? 0;
                double maxY = poly[0].lng ?? 0;

                for (int i = 1; i < poly.Count; i++)
                {
                    coordinates q = poly[i];
                    minX = Math.Min(q.lat ?? 0, minX);
                    maxX = Math.Max(q.lat ?? 0, maxX);
                    minY = Math.Min(q.lng ?? 0, minY);
                    maxY = Math.Max(q.lng ?? 0, maxY);
                }


                if (p.lat < minX || p.lat > maxX || p.lng < minY || p.lng > maxY)
                {
                    return false;
                }
                for (int i = 0, j = poly.Count - 1; i < poly.Count; j = i++)
                {
                    if ((poly[i].lng > p.lng) != (poly[j].lng > p.lng) &&
                         p.lat < (poly[j].lat - poly[i].lat) * (p.lng - poly[i].lng) / (poly[j].lng - poly[i].lng) + poly[i].lat)
                    {
                        inside = !inside;
                    }
                }
                return inside;

            }
            return inside;
        }

        public AppAreaMapVM GetEmpBeatMapByAppId(int appId)
        {
            AppAreaMapVM empBeatMap = new AppAreaMapVM();
            try
            {
                using (var db = new DevSwachhBharatMainEntities())
                {
                    if (appId > 0)
                    {
                        var model = db.AppDetails.Where(x => x.AppId == appId).FirstOrDefault();
                        if (model != null)
                        {
                            empBeatMap = fillEmpBeatMapVM(model);
                        }
                        else
                        {

                        }
                    }
                    else
                    {


                    }

                }
            }
            catch (Exception ex)
            {
                return empBeatMap;
            }

            return empBeatMap;
        }

        private AppAreaMapVM fillEmpBeatMapVM(AppDetail data)
        {
            AppAreaMapVM model = new AppAreaMapVM();
            model.AppId = data.AppId;
            model.AppName = data.AppName;
            model.AppAreaLatLong = ConvertStringToLatLong(data.AppAreaLatLong);

            return model;
        }
        public List<List<coordinates>> ConvertStringToLatLong(string strCord)
        {
            List<List<coordinates>> lstCord = new List<List<coordinates>>();
            string[] lstPoly = strCord.Split(':');
            if (lstPoly.Length > 0)
            {
                for (var i = 0; i < lstPoly.Length; i++)
                {
                    List<coordinates> poly = new List<coordinates>();
                    string[] lstLatLong = lstPoly[i].Split(';');
                    if (lstLatLong.Length > 0)
                    {
                        for (var j = 0; j < lstLatLong.Length; j++)
                        {
                            coordinates cord = new coordinates();
                            string[] strLatLong = lstLatLong[j].Split(',');
                            if (strLatLong.Length == 2)
                            {
                                cord.lat = Convert.ToDouble(strLatLong[0]);
                                cord.lng = Convert.ToDouble(strLatLong[1]);
                            }
                            poly.Add(cord);
                        }

                    }
                    lstCord.Add(poly);
                }
            }
            return lstCord;
        }

        public async Task<CollectionSyncResult>SaveSurveyDetails(SurveyFormDetails obj, int AppId)
        {
            CollectionSyncResult result = new CollectionSyncResult();
            SurveyFormDetail objdata = new SurveyFormDetail();

            using (DevICTSBMChildEntities db = new DevICTSBMChildEntities(AppId))
            {
                try
                {
                    if (obj.ReferanceId != null)
                    {
                        var model = await db.SurveyFormDetails.Where(c => c.ReferanceId == obj.ReferanceId).FirstOrDefaultAsync();
                        if (model != null)
                        {

                            model.ReferanceId = obj.ReferanceId;
                            model.HouseId = model.HouseId;
                            model.Name = obj.name;

                            model.HouseLat = obj.houseLat;
                            model.HouseLong = obj.houseLong;
                            model.MobileNumber = obj.mobileNumber;
                            model.Age = obj.age;

                            model.DateOfBirth = Convert.ToDateTime(obj.dateOfBirth);
                            model.Gender = obj.gender;

                            model.BloodGroup = obj.bloodGroup;
                            model.Qualification = obj.qualification;
                            model.Occupation = obj.occupation;
                            model.MaritalStatus = obj.maritalStatus;
                            if (obj.marriageDate != "" && obj.marriageDate != "0000-00-00")
                            {
                                model.MarriageDate = Convert.ToDateTime(obj.marriageDate);
                            }


                            model.LivingStatus = obj.livingStatus;

                            model.TotalAdults = obj.totalAdults;
                            model.TotalChildren = obj.totalChildren;
                            model.TotalSrCitizen = obj.totalSrCitizen;
                            model.TotalMember = obj.totalMember;

                            model.WillingStart = obj.willingStart;
                            model.ResourcesAvailable = obj.resourcesAvailable;

                            model.MemberJobOtherCity = obj.memberJobOtherCity;

                            model.NoOfVehicle = obj.noOfVehicle;
                            model.VehicleType = obj.vehicleType;
                            model.TwoWheelerQty = obj.twoWheelerQty;
                            //model.threeWheelerQty = obj.threeWheelerQty;
                            model.FourWheelerQty = obj.fourWheelerQty;
                            model.NoPeopleVote = obj.noPeopleVote;
                            model.SocialMedia = obj.socialMedia;
                            model.OnlineShopping = obj.onlineShopping;
                            model.PaymentModePrefer = obj.paymentModePrefer;
                            model.OnlinePayApp = obj.onlinePayApp;
                            model.Insurance = obj.insurance;

                            model.UnderInsurer = obj.underInsurer;
                            model.AyushmanBeneficiary = obj.ayushmanBeneficiary;
                            model.BoosterShot = obj.boosterShot;
                            model.MemberDivyang = obj.memberDivyang;

                            //model.createUserId = obj.createUserId;
                            //model.createDate = DateTime.Now;

                            model.UpdateUserId = obj.updateUserId;
                            model.UpdateDate = DateTime.Now;


                            db.SaveChanges();
                            result.houseId = obj.ReferanceId;
                            result.status = "success";
                            result.message = "Survey Details Updated Successfully";
                            result.messageMar = "सर्वेक्षण तपशील यशस्वीरित्या अद्यतनित केले";
                            return result;
                        }
                        else
                        {
                            var Housemodel = db.HouseMasters.Where(c => c.ReferanceId == obj.ReferanceId).FirstOrDefault();
                            if (Housemodel != null)
                            {
                                objdata.ReferanceId = obj.ReferanceId;
                                objdata.HouseId = Housemodel.houseId;
                                objdata.Name = obj.name;

                                objdata.HouseLat = obj.houseLat;
                                objdata.HouseLong = obj.houseLong;
                                objdata.MobileNumber = obj.mobileNumber;
                                objdata.Age = obj.age;

                                objdata.DateOfBirth = Convert.ToDateTime(obj.dateOfBirth);
                                objdata.Gender = obj.gender;

                                objdata.BloodGroup = obj.bloodGroup;
                                objdata.Qualification = obj.qualification;
                                objdata.Occupation = obj.occupation;
                                objdata.MaritalStatus = obj.maritalStatus;
                                if (obj.marriageDate != "" && obj.marriageDate != "0000-00-00")
                                {
                                    objdata.MarriageDate = Convert.ToDateTime(obj.marriageDate);
                                }



                                objdata.LivingStatus = obj.livingStatus;

                                objdata.TotalAdults = obj.totalAdults;
                                objdata.TotalChildren = obj.totalChildren;
                                objdata.TotalSrCitizen = obj.totalSrCitizen;
                                objdata.TotalMember = obj.totalMember;

                                objdata.WillingStart = obj.willingStart;
                                objdata.ResourcesAvailable = obj.resourcesAvailable;

                                objdata.MemberJobOtherCity = obj.memberJobOtherCity;

                                objdata.NoOfVehicle = obj.noOfVehicle;
                                objdata.VehicleType = obj.vehicleType;
                                objdata.TwoWheelerQty = obj.twoWheelerQty;
                                //objdata.threeWheelerQty = obj.threeWheelerQty;
                                objdata.FourWheelerQty = obj.fourWheelerQty;
                                objdata.NoPeopleVote = obj.noPeopleVote;
                                objdata.SocialMedia = obj.socialMedia;
                                objdata.OnlineShopping = obj.onlineShopping;
                                objdata.PaymentModePrefer = obj.paymentModePrefer;
                                objdata.OnlinePayApp = obj.onlinePayApp;
                                objdata.Insurance = obj.insurance;

                                objdata.UnderInsurer = obj.underInsurer;
                                objdata.AyushmanBeneficiary = obj.ayushmanBeneficiary;
                                objdata.BoosterShot = obj.boosterShot;
                                objdata.MemberDivyang = obj.memberDivyang;

                                objdata.CreateUserId = obj.createUserId;
                                objdata.CreateDate = DateTime.Now;

                                //objdata.updateUserId = obj.updateUserId;
                                //objdata.updateDate = DateTime.Now;


                                db.SurveyFormDetails.Add(objdata);
                                db.SaveChanges();

                                result.status = "success";
                                result.houseId = obj.ReferanceId;
                                result.message = "Survey Details Added Successfully";
                                result.messageMar = "सर्वेक्षण तपशील यशस्वीरित्या जोडले";
                                return result;
                            }
                            else
                            {
                                result.houseId = obj.ReferanceId;
                                result.message = "Invalid House ID.. ";
                                result.messageMar = "अवैध घर आयडी..";
                                result.status = "error";
                                return result;
                            }

                        }

                    }
                    else
                    {

                        result.message = "House Id Is Empty.. ";
                        result.messageMar = "काहीतरी चुकीचे आहे, पुन्हा प्रयत्न करा..";
                        result.status = "error";
                        return result;

                    }

                }
                catch (Exception ex)
                {
                    result.message = "Something is wrong,Try Again.. ";
                    result.messageMar = "काहीतरी चुकीचे आहे, पुन्हा प्रयत्न करा..";
                    result.status = "error";
                    return result;
                }

            }

            return result;
        }

        public async Task<List<SurveyFormDetail>>GetSurveyDetailsById(int AppId, string ReferanceId)
        {
            List<SurveyFormDetail> obj = new List<SurveyFormDetail>();
            using (DevICTSBMChildEntities db = new DevICTSBMChildEntities(AppId))
            {
                var data = await db.SurveyFormDetails.Where(c => c.ReferanceId == ReferanceId).ToListAsync();
                foreach (var x in data)
                {

                    obj.Add(new SurveyFormDetail()
                    {
                        ReferanceId = x.ReferanceId,
                        HouseId = x.HouseId,
                        Name = x.Name,
                        HouseLat = x.HouseLat,
                        HouseLong = x.HouseLong,
                        MobileNumber = x.MobileNumber,
                        DateOfBirth = (x.DateOfBirth),
                        Age = x.Age,
                        Gender = x.Gender,
                        BloodGroup = x.BloodGroup,
                        Qualification = x.Qualification,
                        Occupation = x.Occupation,
                        MaritalStatus = x.MaritalStatus,
                        MarriageDate = (x.MarriageDate),
                        LivingStatus = x.LivingStatus,
                        TotalMember = x.TotalMember,
                        TotalAdults = x.TotalAdults,
                        TotalChildren = x.TotalChildren,
                        TotalSrCitizen = x.TotalSrCitizen,
                        WillingStart = x.WillingStart,
                        ResourcesAvailable = x.ResourcesAvailable,
                        MemberJobOtherCity = x.MemberJobOtherCity,
                        NoOfVehicle = x.NoOfVehicle,
                        TwoWheelerQty = x.TwoWheelerQty,
                        FourWheelerQty = x.FourWheelerQty,
                        NoPeopleVote = x.NoPeopleVote,
                        SocialMedia = x.SocialMedia,
                        OnlineShopping = x.OnlineShopping,
                        PaymentModePrefer = x.PaymentModePrefer,
                        OnlinePayApp = x.OnlinePayApp,
                        Insurance = x.Insurance,
                        UnderInsurer = x.UnderInsurer,
                        AyushmanBeneficiary = x.AyushmanBeneficiary,
                        BoosterShot = x.BoosterShot,
                        MemberDivyang = x.MemberDivyang,
                        CreateUserId = x.CreateUserId,
                        CreateDate = x.CreateDate,
                        UpdateUserId = x.UpdateUserId,
                        UpdateDate = x.UpdateDate,

                    });
                }

            }
            return obj;
        }
    }
}
