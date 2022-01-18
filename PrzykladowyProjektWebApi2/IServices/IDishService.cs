using PrzykladowyProjektWebApi2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrzykladowyProjektWebApi2.IServices
{
    public interface IDishService
    {
        int Create(int restaurantId, CreateDishDto dto);
        DishDto GetById(int restaurantId, int dishId);
        List<DishDto> Get(int restaurantId);
        bool RemoveAll(int restaurantId);

    }
}
