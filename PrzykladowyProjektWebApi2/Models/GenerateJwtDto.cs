using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrzykladowyProjektWebApi2.Models
{
    public class GenerateJwtDto
    {
        public string Jwt { get; set; }
        public bool isUserExist { get; set; }
        public bool isPasswordGood { get; set; }
    }
}
