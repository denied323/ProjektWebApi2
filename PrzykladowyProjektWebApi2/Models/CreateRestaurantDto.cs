using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PrzykladowyProjektWebApi2.Models
{
    public class CreateRestaurantDto
    {
        [Required(ErrorMessage = "Nazwa restauracji jest wymagana")]
        [MaxLength(25, ErrorMessage = "Nazwa restauracji nie może mieć więcej niż 25 znaków.")]
        public string Name { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public bool HasDelivery { get; set; }
        [EmailAddress(ErrorMessage = "Musisz podać adres email")]
        public string ContactEmail { get; set; }
        [Phone(ErrorMessage = "Musisz podać numer telefonu")]
        public string ContactNumber { get; set; }
        [Required(ErrorMessage = "Nazwa miasta jest wymagana")]
        [MaxLength(50, ErrorMessage = "Miasto nie może mieć więcej niż 50 znaków.")]
        public string City { get; set; }
        [Required(ErrorMessage = "Nazwa ulicy jest wymagana")]
        [MaxLength(50, ErrorMessage = "Miasto nie może mieć więcej niż 50 znaków.")]
        public string Street { get; set; }
        public string PostalCode { get; set; }



    }
}
