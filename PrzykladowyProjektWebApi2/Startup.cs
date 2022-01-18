using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using PrzykladowyProjektWebApi2.Entities;
using PrzykladowyProjektWebApi2.IServices;
using PrzykladowyProjektWebApi2.Migrations;
using PrzykladowyProjektWebApi2.Models;
using PrzykladowyProjektWebApi2.Models.Validators;
using PrzykladowyProjektWebApi2.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrzykladowyProjektWebApi2
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            

            //autentyfikacja JWT
            var authenticationSettings = new AuthenticationSettings();
            services.AddSingleton(authenticationSettings);
            Configuration.GetSection("Authentication").Bind(authenticationSettings);
            services.AddAuthentication(option =>
            {
                option.DefaultAuthenticateScheme = "Bearer";
                option.DefaultScheme = "Bearer";
                option.DefaultChallengeScheme = "Bearer";
            }).AddJwtBearer(cfg =>
            {
                cfg.RequireHttpsMetadata = false; //nie wymuszamy klientowi https
                cfg.SaveToken = true; // token powinien zostaæ zapisany po stronie serwera do autentyfikacji
                cfg.TokenValidationParameters = new TokenValidationParameters // do sprawdzania czy dany token jest poprawny
                {
                    ValidIssuer = authenticationSettings.JwtIssue, // wydawca tokenu
                    ValidAudience = authenticationSettings.JwtIssue, //jakie podmioty mog¹ u¿ywaæ tokenu (ta sama wartoœæ bo my bêdziemy tylko dla siebie robiæ)
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authenticationSettings.JwtKey)) //klucz wygenerowany po kluczu z appsettings
                };
            });



            services.AddControllers().AddFluentValidation(); //walidacje
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "PrzykladowyProjektWebApi2", Version = "v1" });
            });

            //dbContext:
            services.AddDbContext<RestaurantDbContext>();
            //seeder:
            services.AddScoped<RestaurantDbSeeder>();

            //automapper:
            services.AddAutoMapper(this.GetType().Assembly); //przeszuka wszystkie mappery samo

            //hasowanie hase³:
            services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>(); //hashowanie hase³

            //walidacje:
            services.AddValidatorsFromAssemblyContaining<RegisterUserDtoValidator>();
            services.AddValidatorsFromAssemblyContaining<LoginDtoValidator>();

            //serwisy:
            services.AddScoped<IWeatherForecastService, WeatherForecastService>();
            services.AddScoped<IRestaurantService, RestaurantService>();
            services.AddScoped<IDishService, DishService>();
            services.AddScoped<IAccountService, AccountService>();
            


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, RestaurantDbSeeder seeder)
        {
            //seedowanie danych przyk³adowych
            seeder.Seed();

            //autentyfikacja:
            app.UseAuthentication();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "PrzykladowyProjektWebApi2 v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
