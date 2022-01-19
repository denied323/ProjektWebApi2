using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PrzykladowyProjektWebApi2.Authorization;
using PrzykladowyProjektWebApi2.Entities;
using PrzykladowyProjektWebApi2.IServices;
using PrzykladowyProjektWebApi2.Migrations;
using PrzykladowyProjektWebApi2.Models;
using RestaurantAPI.Exceptions;
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
        private readonly IUserContextService _userContextService;

        public RestaurantService(RestaurantDbContext context, IMapper mapper, IAuthorizationService authorizationService, IUserContextService userContextService)
        {
            _context = context;
            _mapper = mapper;
            _authorizationService = authorizationService;
            _userContextService = userContextService;
        }

        public int CreateRestaurant(CreateRestaurantDto dto)
        {
            var restaurant = _mapper.Map<Restaurant>(dto);

            restaurant.CreatedById = _userContextService.GetUserId;

            _context.Restaurants.Add(restaurant);
            _context.SaveChanges();

            return restaurant.Id;
        }

        public void DeleteById(int id)
        {
            var restaurant = _context.Restaurants.FirstOrDefault(a => a.Id == id);

            if(restaurant is null)
            {
                throw new NotFoundException("Restaurant not found");
            }

            var authorizationResult = _authorizationService
                .AuthorizeAsync(_userContextService.User, restaurant, new ResourceOperationRequirement(ResourceOperation.Update))
                .Result;
            
            if (!authorizationResult.Succeeded)
            {
                throw new ForbidException();
            }

            _context.Remove(restaurant);
            _context.SaveChanges();
        }

        public void UpdateRestaurant(int id, EditPartiallyRestaurantDto dto)
        {
            var restaurant = _context.Restaurants.FirstOrDefault(a => a.Id == id);

            if (restaurant is null)
            {
                throw new NotFoundException("Restaurant not found");
            }

            var authorizationResult = _authorizationService
                .AuthorizeAsync(_userContextService.User, restaurant, new ResourceOperationRequirement(ResourceOperation.Update))
                .Result;

            if (!authorizationResult.Succeeded)
            {
                throw new ForbidException();
            }

            restaurant.Name = dto.Name;
            restaurant.Description = dto.Description;
            restaurant.HasDelivery = dto.HasDelivery;

            _context.SaveChanges();
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

            if (restaurant is null) 
            { 
                throw new NotFoundException("Restaurant not found");
            }
                
            var restaurantDto = _mapper.Map<RestaurantDto>(restaurant);
            return restaurantDto;
        }
    }
}
