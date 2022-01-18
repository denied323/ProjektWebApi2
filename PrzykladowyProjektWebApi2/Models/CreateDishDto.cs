using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PrzykladowyProjektWebApi2.Models
{
    public class CreateDishDto
    {
        [Required(ErrorMessage = "Pole z nazwą dania jest wymagane")]
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int RestaurantId { get; set; }

    }
}
