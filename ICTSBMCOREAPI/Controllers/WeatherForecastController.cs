using ICTSBMCOREAPI.Dal.DataContexts.Models.DB.ChildModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ICTSBMCOREAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private DevSwachhBharatNagpurEntities _devSwachhBharatNagpurEntities;
        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpGet("GetHouseCount")]
        [ApiConventionMethod(typeof(DefaultApiConventions),nameof(DefaultApiConventions.Get))]
        public int GetHouseCount(int AppId)
        {
            int houseCount = 0;
            try
            {

                using (_devSwachhBharatNagpurEntities = new DevSwachhBharatNagpurEntities(AppId))
                {
                    houseCount = _devSwachhBharatNagpurEntities.HouseMasters.Count();
                }
                //throw new Exception("exception occured........");
                return houseCount;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString(), ex);
                return houseCount;
            }

        }
    }
}
