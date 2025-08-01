﻿using System.ComponentModel.DataAnnotations;

namespace Travel.API.Dtos
{
    public class LoginRequest
    {
        [Required(ErrorMessage = "Username is required")]
        public string UserName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; } = string.Empty;
    }
}
