using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PrzykladowyProjektWebApi2.Entities;
using PrzykladowyProjektWebApi2.IServices;
using PrzykladowyProjektWebApi2.Migrations;
using PrzykladowyProjektWebApi2.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace PrzykladowyProjektWebApi2.Services
{
    public class AccountService : IAccountService
    {
        private readonly RestaurantDbContext _dbContext;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly AuthenticationSettings _authenticationSettings;

        public AccountService(RestaurantDbContext dbContext, IPasswordHasher<User> passwordHasher, AuthenticationSettings authenticationSettings)
        {
            _dbContext = dbContext;
            _passwordHasher = passwordHasher;
            _authenticationSettings = authenticationSettings;
        }

        public GenerateJwtDto GenerateJwt(LoginDto dto)
        {
            var user = _dbContext.Users
                .Include(a => a.Role)
                .FirstOrDefault(a => a.Email == dto.Email);
            GenerateJwtDto generateJwtDto = new GenerateJwtDto();

            if (user is null)
            {
                generateJwtDto.isUserExist = false;
            }
            else
            {
                generateJwtDto.isUserExist = true;

                var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password);
                if(result == PasswordVerificationResult.Failed)
                {
                    generateJwtDto.isPasswordGood = false;
                }
                else
                {
                    generateJwtDto.isPasswordGood = true;

                    var claims = new List<Claim>()
                    {
                        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                        new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
                        new Claim(ClaimTypes.Role, $"{user.Role.Name}"),
                        new Claim("DateOfBirth", user.DateOfBirth.Value.ToString("yyyy-MM-dd")),
                    };


                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authenticationSettings.JwtKey));
                    var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                    var expires = DateTime.Now.AddDays(_authenticationSettings.JwtExpireDays);

                    var token = new JwtSecurityToken(_authenticationSettings.JwtIssuer,
                        _authenticationSettings.JwtIssuer,
                        claims,
                        expires: expires,
                        signingCredentials: cred);

                    var tokenHandler = new JwtSecurityTokenHandler();

                    generateJwtDto.Jwt = tokenHandler.WriteToken(token);
                }
            }
            return generateJwtDto;
        }

        public void RegisterUser(RegisterUserDto dto)
        {
            var newUser = new User()
            {
                Email = dto.Email,
                DateOfBirth = dto.DateOfBirth,
                Nationality = dto.Nationality,
                RoleId = dto.RoleId
            };
            var hashedPassword = _passwordHasher.HashPassword(newUser, dto.Password);

            newUser.PasswordHash = hashedPassword;
            _dbContext.Users.Add(newUser);
            _dbContext.SaveChanges();
        }



    }
}
