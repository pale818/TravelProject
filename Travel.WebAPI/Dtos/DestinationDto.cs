using System.ComponentModel.DataAnnotations;
using Travel.API.Models;

namespace Travel.API.Dtos
{
    public class DestinationDto
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Destination name is required")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Country is required")]
        public string Country { get; set; }
    }
}
