using Microsoft.EntityFrameworkCore;
using PrzykladowyProjektWebApi2.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrzykladowyProjektWebApi2.Migrations
{
    public class RestaurantDbSeeder
    {

        readonly RestaurantDbContext _dbContext;

        public RestaurantDbSeeder(RestaurantDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        public void Seed()
        {
            if (_dbContext.Database.CanConnect()) //jeżeli może połączyć
            {
                //migracje które nie zostały zaaplikoane:
                var pendingMigrations = _dbContext.Database.GetPendingMigrations(); 
                if(pendingMigrations is not null && pendingMigrations.Any())
                {
                    _dbContext.Database.Migrate();
                }

                /*
                if (!_dbContext.Users.Any())
                {

                }*/
                if (!_dbContext.Roles.Any())
                {
                    var roles = GetRoles();
                    _dbContext.Roles.AddRange(roles);
                    _dbContext.SaveChanges();
                }

                if (!_dbContext.Restaurants.Any()) //jeżeli nie ma niczego w Restaurant
                {
                    var restaurants = GetRestaurants();
                    _dbContext.Restaurants.AddRange(restaurants);
                    _dbContext.SaveChanges();
                }
                

            }

        }

        private IEnumerable<Role> GetRoles()
        {
            var roles = new List<Role>()
            {
                new Role()
                {
                    Name = "User"
                },
                new Role()
                {
                    Name = "Manager"
                },
                new Role()
                {
                    Name = "Administrator"
                },
            };
            return roles;
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
