using Microsoft.AspNetCore.Authorization;
using PrzykladowyProjektWebApi2.Migrations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PrzykladowyProjektWebApi2.Authorization
{
    public class CreatedMultipleRestaurantsRequirementHandler : AuthorizationHandler<CreatedMultipleRestaurantsRequirement>
    {
        private readonly RestaurantDbContext _context;
        public CreatedMultipleRestaurantsRequirementHandler(RestaurantDbContext context)
        {
            _context = context;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, CreatedMultipleRestaurantsRequirement requirement)
        {
            if (context.User is null) // dodałem żeby nie było błędów jak ktoś niezalogowany 
            {
                return Task.CompletedTask;
            }

            var userId = int.Parse(context.User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value);

            var restaurantCount = _context.Restaurants.Count(r => r.CreatedById == userId);

            if(restaurantCount >= requirement.CountRestaurant)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }


    }
}
