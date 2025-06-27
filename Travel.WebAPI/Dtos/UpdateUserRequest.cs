using System.ComponentModel.DataAnnotations;

namespace Travel.API.Dtos
{
    public class UpdateUserRequest
    {
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Email must be a valid format.")]
        public string Email { get; set; } = string.Empty;

        [MaxLength(50)]
        public string? FirstName { get; set; }

        [MaxLength(50)]
        public string? LastName { get; set; }

        [Phone(ErrorMessage = "Phone number must be valid.")]
        public string? PhoneNumber { get; set; }

        [Required(ErrorMessage = "Password hash is required")]
        [MinLength(3, ErrorMessage = "Password must be at least 3 characters")]
        public string Password { get; set; } = string.Empty;

        public bool IsAdmin { get; set; } = false;
    }
}
