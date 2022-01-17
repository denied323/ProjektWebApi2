﻿using PrzykladowyProjektWebApi2.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrzykladowyProjektWebApi2.Migrations
{
    public class RestaurantDbSeeder
    {

        RestaurantDbContext _dbContext;

        public RestaurantDbSeeder(RestaurantDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        public void Seed()
        {
            if (_dbContext.Database.CanConnect()) //jeżeli może połączyć
            {
                if(!_dbContext.Restaurants.Any()) //jeżeli nie ma niczego w Restaurant
                {
                    var restaurants = GetRestaurants();
                    _dbContext.Restaurants.AddRange(restaurants);
                    _dbContext.SaveChanges();
                }
            }

        }

        private IEnumerable<Restaurant> GetRestaurants()
        {
            var restaurants = new List<Restaurant>()
            {
                new Restaurant()
                {
                    Name = "mc donald",
                    Category = "fast food",
                    Description = "jedzenie dobre pyszne smaczne",
                    ContactEmail = "a@wp.pl",
                    HasDelivery = true,
                    Dishes = new List<Dish>()
                    {
                        new Dish()
                        {
                            Name = "frytki",
                            Price = 4.65m
                        },
                        new Dish()
                        {
                            Name = "hamburgier",
                            Price = 7.30m
                        },
                    },
                    Address = new Address()
                    {
                        City = "Siedlce",
                        Street = "Ulica 1/25",
                        PostalCode = "08-110"
                    }
                },
                new Restaurant()
                {
                    Name = "kfc",
                    Category = "fast food",
                    Description = "jedzenie tez da sie zjesc",
                    ContactEmail = "b@wp.pl",
                    HasDelivery = true,
                    Dishes = new List<Dish>()
                    {
                        new Dish()
                        {
                            Name = "czisburgier",
                            Price = 5.30m
                        },
                        new Dish()
                        {
                            Name = "lody",
                            Price = 12.30m
                        },
                    },
                    Address = new Address()
                    {
                        City = "Warszawa",
                        Street = "Dłużna 1/25",
                        PostalCode = "08-115"
                    }
                },
            };

            return restaurants;
        }
    }
}