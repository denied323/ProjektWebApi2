using AutoMapper;
using PrzykladowyProjektWebApi2.Entities;
using PrzykladowyProjektWebApi2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrzykladowyProjektWebApi2.Profiles
{
    public class RestaurantMappingProfile : Profile
    {
        public RestaurantMappingProfile()
        {
            CreateMap<Restaurant, RestaurantDto>()
                .ForMember(d => d.City, c => c.MapFrom(s => s.Address.City))
                .ForMember(d => d.Street, c => c.MapFrom(s => s.Address.Street))
                .ForMember(d => d.PostalCode, c => c.MapFrom(s => s.Address.PostalCode));

            CreateMap<Dish, DishDto>();

            CreateMap<CreateRestaurantDto, Restaurant>()
                .ForMember(r => r.Address, c =>
                    c.MapFrom(dto => new Address()
                    {
                        City = dto.City,
                        Street = dto.Street,
                        PostalCode = dto.PostalCode
                    }
                    ));

            CreateMap<CreateDishDto, Dish>();


        }


    }
}
