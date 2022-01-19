using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PrzykladowyProjektWebApi2.Authorization;
using PrzykladowyProjektWebApi2.Entities;
using PrzykladowyProjektWebApi2.IServices;
using PrzykladowyProjektWebApi2.Migrations;
using PrzykladowyProjektWebApi2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PrzykladowyProjektWebApi2.Services
{
    public class RestaurantService : IRestaurantService
    {
        private readonly RestaurantDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthorizationService _authorizationService;

        public RestaurantService(RestaurantDbContext context, IMapper mapper, IAuthorizationService authorizationService)
        {
            _context = context;
            _mapper = mapper;
            _authorizationService = authorizationService;
        }

        public int CreateRestaurant(CreateRestaurantDto dto, int userId)
        {
            var rest = _mapper.Map<Restaurant>(dto);
            rest.CreatedById = userId;

            _context.Restaurants.Add(rest);
            _context.SaveChanges();
            return rest.Id;
        }

        public int DeleteById(int id, ClaimsPrincipal user)
        {
            var restaurant = _context.Restaurants.FirstOrDefault(a => a.Id == id);

            if(restaurant is null)
            {
                return -2;
            }
            var authorizationResult = _authorizationService.AuthorizeAsync(user, restaurant, new ResourceOperationRequirement(ResourceOperation.Update)).Result;
            if (!authorizationResult.Succeeded)
            {
                return -1;
            }

            _context.Remove(restaurant);
            _context.SaveChanges();
            return 0;
        }

        public int EditPartiallyRestaurant(int id, EditPartiallyRestaurantDto dto, ClaimsPrincipal user)
        {
            

            var restaurant = _context.Restaurants.FirstOrDefault(a => a.Id == id);
            if(restaurant is not null)
            {
                var authorizationResult = _authorizationService.AuthorizeAsync(user, restaurant, new ResourceOperationRequirement(ResourceOperation.Update)).Result;
                if (!authorizationResult.Succeeded)
                {
                    return -1;
                }

                restaurant.Name = dto.Name;
                restaurant.Description = dto.Description;
                restaurant.HasDelivery = dto.HasDelivery;
                _context.SaveChanges();
                return 0;
            }
            return -2;
        }

        public IEnumerable<RestaurantDto> GetAll()
        {
            var list = _context.Restaurants
                .Include(a => a.Address)
                .Include(a => a.Dishes)
                .ToList();

            var listDto = _mapper.Map<List<RestaurantDto>>(list);
            return listDto;
        }

        public RestaurantDto GetById(int id)
        {
            var restaurant = _context.Restaurants
                .Include(a => a.Address)
                .Include(a => a.Dishes)
                .FirstOrDefault(a => a.Id == id);

            var restaurantDto = _mapper.Map<RestaurantDto>(restaurant);
            return restaurantDto;
        }
    }
}
