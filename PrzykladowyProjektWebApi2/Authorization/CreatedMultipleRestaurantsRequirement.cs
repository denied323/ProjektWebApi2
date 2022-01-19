using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrzykladowyProjektWebApi2.Authorization
{
    public class CreatedMultipleRestaurantsRequirement : IAuthorizationRequirement
    {
        public int CountRestaurant { get; }

        public CreatedMultipleRestaurantsRequirement(int countRestaurant)
        {
            CountRestaurant = countRestaurant;
        }


    }
}
