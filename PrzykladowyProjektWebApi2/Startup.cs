using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authorization;
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
using PrzykladowyProjektWebApi2.Authorization;
using PrzykladowyProjektWebApi2.Entities;
using PrzykladowyProjektWebApi2.IServices;
using PrzykladowyProjektWebApi2.Migrations;
using PrzykladowyProjektWebApi2.Models;
using PrzykladowyProjektWebApi2.Models.Validators;
using PrzykladowyProjektWebApi2.Services;
using RestaurantAPI.Middleware;
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

            Configuration.GetSection("Authentication").Bind(authenticationSettings);

            services.AddSingleton(authenticationSettings);
            services.AddAuthentication(option =>
            {
                option.DefaultAuthenticateScheme = "Bearer";
                option.DefaultScheme = "Bearer";
                option.DefaultChallengeScheme = "Bearer";
            }).AddJwtBearer(cfg =>
            {
                cfg.RequireHttpsMetadata = false;
                cfg.SaveToken = true;
                cfg.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = authenticationSettings.JwtIssuer,
                    ValidAudience = authenticationSettings.JwtIssuer,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authenticationSettings.JwtKey)),
                };
            });

            //autoryzacja po w³asnych claimach
            services.AddAuthorization(options =>
            {
                options.AddPolicy("HasNationality", builder => builder.RequireClaim("Nationality", "German", "Polish")); //sprawdza czy jest Nationality (przepuszcza jak jest german lub polish)
                options.AddPolicy("Atleast20", builder => builder.AddRequirements(new MinimumAgeRequirement(20))); //minimum 20 lat
                options.AddPolicy("CreatedAtleast2Restaurants", builder => builder.AddRequirements(new CreatedMultipleRestaurantsRequirement(2))); //min 2 restauracje
            });
            services.AddScoped<IAuthorizationHandler, MinimumAgeRequirementHandler>(); //minimum 20 lat
            services.AddScoped<IAuthorizationHandler, ResourceOperationRequirementHandler>(); //dany u¿ytkownik utworzy³ restauracja
            services.AddScoped<IAuthorizationHandler, CreatedMultipleRestaurantsRequirementHandler>(); //min 2 restauracje



            //walidacje
            services.AddControllers().AddFluentValidation(); //walidacje

            //swagger
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
            services.AddValidatorsFromAssemblyContaining<RestaurantQueryValidator>();


            //middleware:
            services.AddScoped<ErrorHandlingMiddleware>();
            services.AddScoped<RequestTimeMiddleware>();


            //serwisy:
            services.AddScoped<IWeatherForecastService, WeatherForecastService>();
            services.AddScoped<IRestaurantService, RestaurantService>();
            services.AddScoped<IDishService, DishService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IUserContextService, UserContextService>();


            


            //¿eby mo¿na by³o wstrzykn¹æ httpcontext do serwisu
            services.AddHttpContextAccessor();

            //CORS
            services.AddCors(options =>
            {
                options.AddPolicy("FrontEndClient", builder =>

                    builder.AllowAnyMethod() //mo¿e wys³aæ POST,PUT,DELETE,GET itd.
                        .AllowAnyHeader() //mo¿e wys³aæ headery
                        .WithOrigins(Configuration["AllowedOrigins"]) //domena z której bêdzie dostêp (appsettings)

                );
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, RestaurantDbSeeder seeder)
        {
            //CORS
            app.UseCors("FrontEndClient");

            //seedowanie danych przyk³adowych
            seeder.Seed();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "PrzykladowyProjektWebApi2 v1"));
            }

            //midleware:
            app.UseMiddleware<ErrorHandlingMiddleware>();
            app.UseMiddleware<RequestTimeMiddleware>();


            app.UseHttpsRedirection();

            app.UseRouting();

            //autentyfikacja:
            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
