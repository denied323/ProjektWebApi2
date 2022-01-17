using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrzykladowyProjektWebApi2.Models
{
    public class TemperatureRequest
    {
        public int minTemp { get; set; }
        public int maxTemp { get; set; }
    }
}
