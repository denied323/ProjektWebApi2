using Microsoft.AspNetCore.Mvc;
using PrzykladowyProjektWebApi2.Entities;
using PrzykladowyProjektWebApi2.IServices;
using System.Collections.Generic;

namespace PrzykladowyProjektWebApi2.Controllers
{
    [ApiController]
    [Route("api/restaurant")]
    public class RestaurantController : ControllerBase
    {
        private readonly IRestaurantService _restaurantService;

        public RestaurantController(IRestaurantService restaurantService)
        {
            _restaurantService = restaurantService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Restaurant>> GetAll()
        {
            var list = _restaurantService.GetAll();
            if (list is not null)
            {
                return Ok(list);
            }
            else
            {
                return NotFound(list);
            }
        }

        [HttpGet("{id}")]
        public ActionResult<Restaurant> GetById([FromRoute] int id)
        {
            var restaurant = _restaurantService.GetById(id);
            if (restaurant is not null)
            {
                return Ok(restaurant);
            }
            else
            {
                return NotFound(restaurant);
            }
        }


    }
}
