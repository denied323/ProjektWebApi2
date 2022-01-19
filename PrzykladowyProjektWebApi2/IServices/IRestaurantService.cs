using Microsoft.AspNetCore.Mvc;
using PrzykladowyProjektWebApi2.Entities;
using PrzykladowyProjektWebApi2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PrzykladowyProjektWebApi2.IServices
{
    public interface IRestaurantService
    {
        IEnumerable<RestaurantDto> GetAll();
        RestaurantDto GetById(int id);
        int CreateRestaurant(CreateRestaurantDto dto);
        void DeleteById(int id);
        void UpdateRestaurant(int id, EditPartiallyRestaurantDto dto);


    }
}
