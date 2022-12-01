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
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace ICTSBMCOREAPI.SwachhBharat.API.Bll.Repository.Repository
{
    public class Repository : IRepository
    {
        private readonly DevICTSBMMainEntities dbMain;
        private readonly IConfiguration _configuration;
        private readonly ILogger<Repository> _logger;

        public Repository(DevICTSBMMainEntities devICTSBMMainEntities, IConfiguration configuration, ILogger<Repository> logger)
        {
            dbMain = devICTSBMMainEntities;
            _configuration = configuration;
            _logger = logger;
        }
        public async Task<string> LoginAsync(int AppId)
        {
            try
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
              //  var Key = Base64UrlEncoder.DecodeBytes("9ST5hQe5dUNfAJOQZAtt19uiDhNtKKUt");
            //    SymmetricSecurityKey signingKey = new SymmetricSecurityKey(Key);
                var token = new JwtSecurityToken(
                    issuer: _configuration["JWT:ValidIssuer"],
                    audience: _configuration["JWT:ValidAudience"],
                    expires: DateTime.Now.AddMinutes(20),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigninkey, SecurityAlgorithms.Aes256Encryption));
                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString(), ex);
                return "Error Occured";
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
                        var objEmpMst2 = await db.UserMasters.Where(c => c.userLoginId == userName & c.userPassword == password).FirstOrDefaultAsync();
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
                                    data.VQRID = Vehicaldetail.vqrId;
                                    data.vehicleNumber = Vehicaldetail.VehicalNumber;
                                    data.vtId = Vehicaldetail.VehicalType;
                                }
                                else
                                {
                                    data.VQRID = null;
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
                                    objdata.VQRID = Vehicaldetail.vqrId;
                                    objdata.vehicleNumber = Vehicaldetail.VehicalNumber;
                                    objdata.vtId = Vehicaldetail.VehicalType;
                                }
                                else
                                {
                                    objdata.VQRID = null;
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
                                result.message = "Something is wrong,Try Again.. ";
                                result.messageMar = "काहीतरी चुकीचे आहे, पुन्हा प्रयत्न करा..";
                                result.emptype = "N";
                                return result;
                            }
                        }

                        result.status = "error";
                        result.message = "Please contact your administrator.. ";
                        result.messageMar = "कृपया आपल्या ऍडमिनिस्ट्रेटरशी संपर्क साधा..";
                        result.emptype = "N";
                        return result;

                    }
                    else
                    {

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
                                    objdata.VQRID = Vehicaldetail.vqrId;
                                    objdata.vehicleNumber = Vehicaldetail.VehicalNumber;
                                    objdata.vtId = Vehicaldetail.VehicalType;
                                }
                                else
                                {
                                    objdata.VQRID = null;
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
                                objdata2.batteryStatus = batteryStatus;
                                objdata2.EmployeeType = null;
                                if ((string.IsNullOrEmpty(obj.QrCodeImage)) == false)
                                {
                                    obj.QrCodeImage = obj.QrCodeImage.Replace("data:image/jpeg;base64,", "");
                                    objdata2.BinaryQrCodeImage = Convert.FromBase64String(obj.QrCodeImage);
                                }
                                if (Vehicaldetail != null)
                                {
                                    objdata2.VQRID = Vehicaldetail.vqrId;
                                    objdata2.vehicleNumber = Vehicaldetail.VehicalNumber;
                                    objdata2.vtId = Vehicaldetail.VehicalType;
                                }
                                else
                                {
                                    objdata2.VQRID = null;
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
                            result.message = "Something is wrong,Try Again.. ";
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
                                objdata.batteryStatus = batteryStatus;
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
                                objdata2.batteryStatus = batteryStatus;
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
                                objdata.batteryStatus = batteryStatus;
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
                                objdata2.batteryStatus = batteryStatus;
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
                                var locc = await dbMain.SP_UserLatLongDetailMain_Results.FromSqlRaw<SP_UserLatLongDetailMain_Result>("EXEC SP_UserLatLongDetailMain @userid,@typeId", parms).FirstOrDefaultAsync();


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
                                var dist = await dbMain.SP_DistanceCount_Main_Results.FromSqlRaw<SP_DistanceCount_Main_Result>("EXEC SP_DistanceCount @sLat,@sLong,@dLat,@dLong", parms1).FirstOrDefaultAsync();

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
                                var dist = await dbMain.SP_DistanceCount_Main_Results.FromSqlRaw<SP_DistanceCount_Main_Result>("EXEC SP_DistanceCount @sLat,@sLong,@dLat,@dLong", parms1).FirstOrDefaultAsync();

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
            try
            {
                string[] ad = address.Split(',');
                int l = ad.Length - 4;
                return ad[l];
            }
            catch
            {

                return "";
            }


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
                            if(parms != null)
                            {
                                foreach(var parm in parms)
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
    }
}
