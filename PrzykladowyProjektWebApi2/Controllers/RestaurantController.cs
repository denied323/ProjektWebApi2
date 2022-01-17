using Microsoft.AspNetCore.Mvc;
using PrzykladowyProjektWebApi2.Entities;
using PrzykladowyProjektWebApi2.IServices;
using PrzykladowyProjektWebApi2.Models;
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
            if (list is null)
            {
                return NotFound(list);
            }
            return Ok(list);
        }

        [HttpGet("{id}")]
        public ActionResult<Restaurant> GetById([FromRoute] int id)
        {
            var restaurant = _restaurantService.GetById(id);
            if (restaurant is null)
            {
                return NotFound(restaurant);
            }
            return Ok(restaurant);
        }

        [HttpPost]
        public ActionResult CreateRestaurant([FromBody] CreateRestaurantDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var id = _restaurantService.CreateRestaurant(dto);
            return Created($"/api/restaurant/{id}", null);
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteRestaurant([FromRoute] int id)
        {
            var isDeleted = _restaurantService.DeleteById(id);
            if(isDeleted == false)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpPut("{id}")]
        public ActionResult EditPartiallyRestaurant([FromRoute] int id, [FromBody] EditPartiallyRestaurantDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (_restaurantService.EditPartiallyRestaurant(id, dto) == true)
            {
                return Ok();
            }
            return NoContent();
        }


    }
}
