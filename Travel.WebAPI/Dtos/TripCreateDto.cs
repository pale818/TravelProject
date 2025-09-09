using System.ComponentModel.DataAnnotations;

namespace Travel.WebAPI.Dtos
{
    public class TripCreateDto
    {
        [Required(ErrorMessage = "Trip name is required")]
        [StringLength(100, ErrorMessage = "Trip name cannot be longer than 100 characters")]
        public string Name { get; set; }

     
        public string? Description { get; set; }
        
        [Range(0, double.MaxValue, ErrorMessage = "Price must be non-negative")]
        [Required(ErrorMessage = "Trip price is required")]
        public decimal Price { get; set; }
        
    }
}
