﻿using Microsoft.AspNetCore.Http;
using PrzykladowyProjektWebApi2.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PrzykladowyProjektWebApi2.Services
{
    public class UserContextService : IUserContextService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserContextService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public ClaimsPrincipal User => _httpContextAccessor.HttpContext?.User;

        public int? GetUserId =>
            User is null ? null : (int?)int.Parse(User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value);

    }
}
