using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PrzykladowyProjektWebApi2.Authorization
{
    public class MinimumAgeRequirementHandler : AuthorizationHandler<MinimumAgeRequirement>
    {

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, MinimumAgeRequirement requirement)
        {
            if(!context.User.HasClaim(c => c.Type == "DateOfBirth")) // dodałem żeby nie było błędów jak ktoś niezalogowany bez DateOfBirth będzie próbował się dostać
            {
                return Task.CompletedTask;
            }

                var dateOfBirth = DateTime.Parse(context.User.FindFirst(c => c.Type == "DateOfBirth").Value);

                if (dateOfBirth.AddYears(requirement.MinimumAge) <= DateTime.Today)
                {
                    context.Succeed(requirement);
                }

                return Task.CompletedTask;

        }
    }
}
