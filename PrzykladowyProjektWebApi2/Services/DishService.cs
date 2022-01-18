using AutoMapper;
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
    public class DishService : IDishService
    {
        private readonly RestaurantDbContext _context;
        private readonly IMapper _mapper;

        public DishService(RestaurantDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public int Create(int restaurantId, CreateDishDto dto)
        {
            var restaurant = _context.Restaurants.FirstOrDefault(a => a.Id == restaurantId);
            if(restaurant is null)
            {
                return -1;
            }

            dto.RestaurantId = restaurantId;
            var dishEntity = _mapper.Map<Dish>(dto);

            _context.Dishes.Add(dishEntity);
            _context.SaveChanges();
            return dishEntity.Id;
        }

        public List<DishDto> Get(int restaurantId)
        {
            var restaurant = _context.
                Restaurants
                .Include(r => r.Dishes)
                .FirstOrDefault(r => r.Id == restaurantId);
            return _mapper.Map<List<DishDto>>(restaurant.Dishes);
        }

        public DishDto GetById(int restaurantId, int dishId)
        {
            var dish = _context.Dishes.FirstOrDefault(a => a.Id == dishId && a.RestaurantId == restaurantId);
            return _mapper.Map<DishDto>(dish);
        }

        public bool RemoveAll(int restaurantId)
        {
            var restaurant = _context.
                Restaurants
                .Include(r => r.Dishes)
                .FirstOrDefault(r => r.Id == restaurantId);

            if (restaurant is null)
            {
                return false;
            }

            _context.RemoveRange(restaurant.Dishes);
            _context.SaveChanges();
            return true;
        }
    }
}
