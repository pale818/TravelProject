using System.ComponentModel.DataAnnotations;

namespace Travel.Web.Models
{
    public class ApplicationUser
    {
        [Required(ErrorMessage = "Username is required")]
        [MinLength(3, ErrorMessage = "Username must be at least 3 characters")]
        [Display(Name = "Username")]
        public string UserName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Email must be valid")]
        [Display(Name = "Email Address")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        [MinLength(3, ErrorMessage = "Password must be at least 3 characters")]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [StringLength(50)]
        [Display(Name = "First Name")]
        public string? FirstName { get; set; }

        [StringLength(50)]
        [Display(Name = "Last Name")]
        public string? LastName { get; set; }

        [Phone]
        [Display(Name = "Phone Number")]
        [DataType(DataType.PhoneNumber)]
        public string? PhoneNumber { get; set; }

        [Display(Name = "Administrator")]
        public bool? IsAdmin { get; set; }
    }
}
