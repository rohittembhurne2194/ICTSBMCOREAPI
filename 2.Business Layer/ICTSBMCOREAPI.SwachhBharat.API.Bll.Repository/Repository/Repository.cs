using ICTSBMCOREAPI.Dal.DataContexts.Models.DB.ChildModels;
using ICTSBMCOREAPI.Dal.DataContexts.Models.DB.MainModels;
using ICTSBMCOREAPI.SwachhBhart.API.Bll.ViewModels.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ICTSBMCOREAPI.SwachhBharat.API.Bll.Repository.Repository
{
    public class Repository:IRepository
    {
        private readonly DevSwachhBharatMainEntities dbMain;
        private readonly IConfiguration _configuration;
        private readonly ILogger<Repository> _logger;

        public Repository(DevSwachhBharatMainEntities devSwachhBharatMainEntities, IConfiguration configuration,ILogger<Repository> logger)
        {
            dbMain = devSwachhBharatMainEntities;
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
                var token = new JwtSecurityToken(
                    issuer: _configuration["JWT:ValidIssuer"],
                    audience: _configuration["JWT:ValidAudience"],
                    expires: DateTime.Now.AddMinutes(10),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigninkey, SecurityAlgorithms.HmacSha256Signature));
                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            catch(Exception ex)
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
                using (DevSwachhBharatNagpurEntities db = new DevSwachhBharatNagpurEntities(AppId))
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
                using (DevSwachhBharatNagpurEntities db = new DevSwachhBharatNagpurEntities(AppId))
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
            catch(Exception ex)
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
                using (DevSwachhBharatNagpurEntities db = new DevSwachhBharatNagpurEntities(AppId))
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
            catch(Exception ex)
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
                using (DevSwachhBharatNagpurEntities db = new DevSwachhBharatNagpurEntities(AppId))
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
    }
}
