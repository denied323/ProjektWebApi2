﻿using Microsoft.EntityFrameworkCore;
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
            "Server=localhost\\SQLEXPRESS;Database=RestaurantDb;Trusted_Connection=True;";


        public DbSet<Restaurant> Restaurant { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Dish> Dishes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Restaurant>()
                .Property(r => r.Name)
                .IsRequired()
                .HasMaxLength(25);

            modelBuilder.Entity<Dish>()
                .Property(r => r.Name)
                .IsRequired();

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);
        }


    }
}