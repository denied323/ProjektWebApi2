using Microsoft.AspNetCore.Mvc;
using PrzykladowyProjektWebApi2.IServices;
using PrzykladowyProjektWebApi2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrzykladowyProjektWebApi2.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        /// <summary>
        /// rejestracja
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost("register")]
        public ActionResult RegisterUser([FromBody] RegisterUserDto dto)
        {
            _accountService.RegisterUser(dto);
            return Ok();
        }

        /// <summary>
        /// logowanie
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost("login")]
        public ActionResult Login([FromBody] LoginDto dto)
        {
            GenerateJwtDto gjd = _accountService.GenerateJwt(dto);

            if(gjd.isUserExist == false)
            {
                return NotFound("user not found");
            }
            if(gjd.isPasswordGood == false)
            {
                return BadRequest("invalid username or password");
            }
            return Ok(gjd.Jwt);

        }



    }
}
