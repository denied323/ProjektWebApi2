using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PrzykladowyProjektWebApi2.Entities;
using PrzykladowyProjektWebApi2.IServices;
using PrzykladowyProjektWebApi2.Models;
using System.Collections.Generic;
using System.Security.Claims;

namespace PrzykladowyProjektWebApi2.Controllers
{
    [ApiController]
    [Route("api/restaurant")]
    [Authorize]
    public class RestaurantController : ControllerBase
    {
        private readonly IRestaurantService _restaurantService;

        public RestaurantController(IRestaurantService restaurantService)
        {
            _restaurantService = restaurantService;
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult<IEnumerable<Restaurant>> GetAll()
        {
            var list = _restaurantService.GetAll();
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
        [Authorize(Roles = "Administrator,Manager")]
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
        [Authorize(Roles = "Administrator,Manager")]
        public ActionResult DeleteById([FromRoute] int id)
        {
            _restaurantService.DeleteById(id);
            return NoContent();
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Administrator,Manager")]
        public ActionResult UpdateRestaurant([FromRoute] int id, [FromBody] EditPartiallyRestaurantDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _restaurantService.UpdateRestaurant(id, dto);
            return Ok();
        }






        [HttpGet("min2restaurants")]
        [Authorize(Policy = "CreatedAtleast2Restaurants")]
        public ActionResult<IEnumerable<Restaurant>> GetAllMin2Restaurants()
        {
            var lista1 = _restaurantService.GetAll();
            return Ok(lista1);
        }

        [HttpGet("nation")]
        [Authorize(Policy = "HasNationality")]
        public ActionResult<IEnumerable<Restaurant>> GetAllNation()
        {
            var list2 = _restaurantService.GetAll();
            return Ok(list2);
        }

        [HttpGet("min20")]
        [Authorize(Policy = "Atleast20")]
        public ActionResult<IEnumerable<Restaurant>> GetAllMin20()
        {
            var list3 = _restaurantService.GetAll();
            return Ok(list3);
        }

    }
}
