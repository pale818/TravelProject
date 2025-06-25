using System.ComponentModel.DataAnnotations;

public class LoginRequest
{
    [Required(ErrorMessage = "Username is required")]
    [Display(Name = "Username")]
    public string UserName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password is required")]
    [MinLength(3, ErrorMessage = "Password must be at least 3 characters long")]
    [Display(Name = "Password")]
    public string Password { get; set; } = string.Empty;
}
