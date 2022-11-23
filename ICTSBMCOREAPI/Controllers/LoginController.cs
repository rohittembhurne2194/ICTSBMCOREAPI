using ICTSBMCOREAPI.SwachhBharat.API.Bll.Repository.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ICTSBMCOREAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ILogger<LoginController> _logger;
        private readonly IRepository _repository;

        public LoginController(ILogger<LoginController> logger,IRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromHeader] int AppId)
        {
            var result = await _repository.LoginAsync(AppId);
            if (string.IsNullOrEmpty(result))
            {
                return Unauthorized();
            }

            return Ok(result);
        }
    }
}
