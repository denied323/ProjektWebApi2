using Microsoft.AspNetCore.Mvc;
using PrzykladowyProjektWebApi2.IServices;
using PrzykladowyProjektWebApi2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrzykladowyProjektWebApi2.Controllers
{
    [Route("api/restaurant/{restaurantId}/dish")]
    [ApiController]
    public class DishController : ControllerBase
    {
        private readonly IDishService _dishService;

        public DishController(IDishService dishService)
        {
            _dishService = dishService;
        }

        [HttpPost]
        public ActionResult Post([FromRoute] int restaurantId, [FromBody] CreateDishDto dto)
        {
            var id = _dishService.Create(restaurantId, dto);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id <= 0)
            {
                return NotFound($"Nie znaleziono Restauracji o ID {restaurantId}");
            }
            return Created($"/api/restaurant/{restaurantId}/dish/{id}", null);
        }

        [HttpGet("{dishId}")]
        public ActionResult<DishDto> GetById([FromRoute] int restaurantId, [FromRoute] int dishId)
        {
            var dish = _dishService.GetById(restaurantId, dishId);
            if(dish is null)
            {
                return NotFound(dish);
            }
            return Ok(dish);
        }

        [HttpGet]
        public ActionResult<List<DishDto>> Get([FromRoute] int restaurantId)
        {
            var dishes = _dishService.Get(restaurantId);
            if (dishes is null)
            {
                return NotFound(dishes);
            }
            return Ok(dishes);
        }

        [HttpDelete]
        public ActionResult Delete([FromRoute] int restaurantId)
        {
            var isDeleted = _dishService.RemoveAll(restaurantId);
            if (isDeleted == false)
            {
                return NotFound();
            }
            return NoContent();
        }



    }


}
