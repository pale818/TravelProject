using System.ComponentModel.DataAnnotations;

namespace Travel.Web.Models
{
    public class ApplicationUser
    {
        [Required]
        [MinLength(3)]
        public string UserName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MinLength(6)]
        public string Password { get; set; } = string.Empty;

        [StringLength(50)]
        public string? FirstName { get; set; }

        [StringLength(50)]
        public string? LastName { get; set; }

        [Phone]
        public string? PhoneNumber { get; set; }

        // Usually this is set server side, so you might skip validation or hide in UI
        public bool? IsAdmin { get; set; }
    }
}
