using ICTSBMCOREAPI.SwachhBharat.API.Bll.Repository.Repository;
using ICTSBMCOREAPI.SwachhBhart.API.Bll.ViewModels.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
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

        [HttpPost("getTocken")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromHeader] int AppId)
        {
            var result = await objRep.LoginAsync(AppId);
            if (string.IsNullOrEmpty(result))
            {
                return Unauthorized();
            }

            return Ok(result);
        }
        [HttpPost("Login")]
        public async Task<SBUser> GetLogin([FromHeader] int AppId, [FromHeader] string EmpType, [FromBody]SBUser objlogin)
        {
            //Request.Headers.TryGetValue("appId", out var id);
            //int AppId = int.Parse(id);
            //Request.Headers.TryGetValue("EmpType", out var EmpType);
            
            SBUser objresponse = await objRep.CheckUserLoginAsync(objlogin.userLoginId, objlogin.userPassword, objlogin.imiNo, AppId, EmpType);
            return objresponse;
        }


    }
}
