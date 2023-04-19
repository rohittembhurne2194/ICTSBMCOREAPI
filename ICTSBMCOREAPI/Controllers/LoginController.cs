using ICTSBMCOREAPI.SwachhBharat.API.Bll.Repository.Repository;
using ICTSBMCOREAPI.SwachhBhart.API.Bll.ViewModels.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ICTSBMCOREAPI.Controllers
{
    [Authorize]
    [Route("api/Account")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ILogger<LoginController> _logger;
        private readonly IRepository objRep;

        public LoginController(ILogger<LoginController> logger,IRepository repository)
        {
            _logger = logger;
            objRep = repository;
        }

        [HttpPost("getToken")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromHeader] string AppId)
        {
            //string encodedStr = AppId.ToString();
            //string encodedStr = Convert.ToBase64String(Encoding.UTF8.GetBytes(AppId_New));

            string inputStr = Encoding.UTF8.GetString(Convert.FromBase64String(AppId));

            string[] inputStr2 = inputStr.Split('@');

            string[] abc = inputStr2[2].Split('.');

            string inputStr3 = inputStr2[1].Trim() + abc[1].Trim();
          

            int AppId_New = Convert.ToInt32(inputStr3);
            var result = await objRep.LoginAsync(AppId_New);
            if (string.IsNullOrEmpty(result))
            {
                return Unauthorized();
            }

            return Ok(result);
        }
        [HttpPost("Login")]
        public async Task<ActionResult<SBUser>> GetLogin([FromHeader] int AppId, [FromBody]SBUser objlogin)
        {
            //Request.Headers.TryGetValue("appId", out var id);
            //int AppId = int.Parse(id);
            //Request.Headers.TryGetValue("EmpType", out var EmpType);
            var jti = HttpContext.User.Claims.ToList().First(claim => claim.Type == "AppId").Value;


            //var stream = authorization.Replace("Bearer ", string.Empty);
            //var handler = new JwtSecurityTokenHandler();
            //var jsonToken = handler.ReadToken(stream);
            //var tokenS = jsonToken as JwtSecurityToken;
            //var jti = tokenS.Claims.First(claim => claim.Type == "AppId").Value;


            var Auth_AppId = Convert.ToInt32(jti);

            if (Auth_AppId == AppId)
            {
                SBUser objresponse = await objRep.CheckUserLoginAsync(objlogin.userLoginId, objlogin.userPassword, objlogin.imiNo, AppId, objlogin.EmpType);
                return objresponse;
            }
            else
            {
                _logger.LogError("Error : Invalid AppId="+ AppId+" / Correct AppId="+ Auth_AppId);
                return Unauthorized();
            }
          
        }

        [HttpPost("GisLogin")]
        [EnableCors("MyCorsPolicy")]
        [AllowAnonymous]
        public async Task<ActionResult<GisLoginResult>> LoginGis([FromBody] GisUsers loginobj)
        {
            GisLoginResult objDetail = new GisLoginResult();

            var result = await objRep.GisLoginAsync(loginobj.userLoginId, loginobj.userPassword);

            objDetail.code = result.code;
            objDetail.status = result.status;
            objDetail.message = result.message;
            objDetail.timestamp = result.timestamp;
            objDetail.data = result.data;
           
            return Ok(objDetail);
        }

    }
}
