using Microsoft.AspNetCore.Mvc;
using PrzykladowyProjektWebApi2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrzykladowyProjektWebApi2.IServices
{
    public interface IWeatherForecastService
    {
        public IEnumerable<WeatherForecast> Get(int count, int minTemp, int maxTemp);


    }
}
