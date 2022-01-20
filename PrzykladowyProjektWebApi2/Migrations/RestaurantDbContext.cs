using Microsoft.EntityFrameworkCore;
using PrzykladowyProjektWebApi2.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrzykladowyProjektWebApi2.Migrations
{
    public class RestaurantDbContext : DbContext
    {
        private string _connectionString =
            "Server=DESKTOP-PL5608Q\\SQLEXPRESS;Database=RestaurantDb;Trusted_Connection=True;";


        public DbSet<Restaurant> Restaurants { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Dish> Dishes { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .Property(u => u.Email)
                .IsRequired();

            modelBuilder.Entity<Role>()
                .Property(u => u.Name)
                .IsRequired();



            modelBuilder.Entity<Restaurant>()
                .Property(r => r.Name)
                .IsRequired()
                .HasMaxLength(25);

            modelBuilder.Entity<Dish>()
                .Property(r => r.Name)
                .IsRequired();

            modelBuilder.Entity<Address>()
                .Property(r => r.City)
                .IsRequired()
                .HasMaxLength(50);

            modelBuilder.Entity<Address>()
                .Property(r => r.Street)
                .IsRequired()
                .HasMaxLength(50);


        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);
        }


    }
}
