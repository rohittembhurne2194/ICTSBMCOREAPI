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
    [Route("api")]
    [ApiController]
    public class TypeController : ControllerBase
    {
        private readonly ILogger<TypeController> _logger;
        private readonly IRepository objRep;
        public TypeController(ILogger<TypeController> logger, IRepository repository)
        {
            _logger = logger;
            objRep = repository;
        }

        [HttpGet]
        [Route("Get/VehicleType")]
        //api/BookATable/GetBookAtableList
        public async Task<List<SBVehicleType>> GetComplaintType([FromHeader] int AppId)
        {
           
            List<SBVehicleType> objDetail = new List<SBVehicleType>();
            objDetail = await objRep.GetVehicleAsync(AppId);
            return objDetail;
        }

    }
}
