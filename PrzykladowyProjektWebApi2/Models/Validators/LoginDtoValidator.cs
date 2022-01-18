using FluentValidation;
using PrzykladowyProjektWebApi2.Migrations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrzykladowyProjektWebApi2.Models.Validators
{
    public class LoginDtoValidator : AbstractValidator<LoginDto>
    {
        private readonly RestaurantDbContext _dbContext;

        public LoginDtoValidator(RestaurantDbContext dbContext)
        {
            _dbContext = dbContext;

            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress();

            RuleFor(x => x.Password).NotEmpty();


        }
    }
}
