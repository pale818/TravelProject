using System.ComponentModel.DataAnnotations;

namespace Travel.Web.Models
{
    public class UpdateUser
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Username is required")]
        [StringLength(50, ErrorMessage = "Username can't be longer than 50 characters")]
        [Display(Name = "Username")]
        public string UserName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        [StringLength(50, ErrorMessage = "First name can't be longer than 50 characters")]
        [Display(Name = "First Name")]
        public string? FirstName { get; set; }

        [StringLength(50, ErrorMessage = "Last name can't be longer than 50 characters")]
        [Display(Name = "Last Name")]
        public string? LastName { get; set; }

        [Phone(ErrorMessage = "Invalid phone number")]
        [StringLength(20, ErrorMessage = "Phone number can't be longer than 20 characters")]
        [Display(Name = "Phone Number")]
        public string? PhoneNumber { get; set; }
    }
}
