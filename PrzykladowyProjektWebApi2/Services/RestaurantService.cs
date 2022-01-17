using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PrzykladowyProjektWebApi2.Entities;
using PrzykladowyProjektWebApi2.IServices;
using PrzykladowyProjektWebApi2.Migrations;
using PrzykladowyProjektWebApi2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrzykladowyProjektWebApi2.Services
{
    public class RestaurantService : IRestaurantService
    {
        private readonly RestaurantDbContext _context;
        private readonly IMapper _mapper;

        public RestaurantService(RestaurantDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public int CreateRestaurant(CreateRestaurantDto dto)
        {
            var rest = _mapper.Map<Restaurant>(dto);
            _context.Restaurants.Add(rest);
            _context.SaveChanges();
            return rest.Id;
        }

        public bool DeleteById(int id)
        {
            var restaurant = _context.Restaurants.FirstOrDefault(a => a.Id == id);

            if(restaurant is null)
            {
                return false;
            }

            _context.Remove(restaurant);
            _context.SaveChanges();
            return true;
        }

        public bool EditPartiallyRestaurant(int id, EditPartiallyRestaurantDto dto)
        {
            var restaurant = _context.Restaurants.FirstOrDefault(a => a.Id == id);
            if(restaurant is not null)
            {
                restaurant.Name = dto.Name;
                restaurant.Description = dto.Description;
                restaurant.HasDelivery = dto.HasDelivery;
                _context.SaveChanges();
                return true;
            }
            return false;
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
