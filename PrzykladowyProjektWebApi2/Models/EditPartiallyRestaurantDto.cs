using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PrzykladowyProjektWebApi2.Models
{
    public class EditPartiallyRestaurantDto
    {
        [Required(ErrorMessage = "Nazwa restauracji jest wymagana")]
        [MaxLength(25, ErrorMessage = "Nazwa restauracji nie może mieć więcej niż 25 znaków.")]
        public string Name { get; set; }
        public string Description { get; set; }
        public bool HasDelivery { get; set; }

    }
}
