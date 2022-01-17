using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PrzykladowyProjektWebApi2.IServices;
using PrzykladowyProjektWebApi2.Models;
using PrzykladowyProjektWebApi2.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrzykladowyProjektWebApi2.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly IWeatherForecastService _service;
        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IWeatherForecastService service)
        {
            _logger = logger;
            _service = service;
        }

        /// <summary>
        /// Przyjmuje 3 parametry: liczbe resultatów, minimalną wartość temperatury, max wartość temeraptury
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<WeatherForecast> Get(int count, int minTemp, int maxTemp)
        {
            return _service.Get(count, minTemp, maxTemp);
        }

        /// <summary>
        /// Parametr: liczba rezultatów, w ciele obiekt z wartością maksymalną i minimalną
        /// </summary>
        /// <returns></returns>
        [HttpPost("generate")]
        public ActionResult<IEnumerable<WeatherForecast>> Generate([FromQuery] int count, [FromBody] TemperatureRequest req )
        {
            if (count < 0 || req.minTemp > req.maxTemp)
            {
                return BadRequest();
            }

            var result = _service.Get(count, req.minTemp, req.maxTemp);
            return Ok(result);
        }








        /*

        /// <summary>
        /// zwraca wszystko, parametry fromroute i fromquery
        /// </summary>
        /// <param name="take"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        [HttpGet("currentDay/{max}")]
        public IEnumerable<WeatherForecast> Get2([FromQuery]int take, [FromRoute] int max)
        {
            return _service.Get();
        }

        /// <summary>
        /// Status kody
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult<string> Hello([FromBody]string name)
        {
            //HttpContext.Response.StatusCode = 401; //nieatuoryzowany kod
            //return StatusCode(401, $"Hello {name}");
            return Unauthorized($"Hello {name}");
        }


        */

    }
}
