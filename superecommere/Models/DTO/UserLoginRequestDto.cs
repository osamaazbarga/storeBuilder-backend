﻿using System.ComponentModel.DataAnnotations;

namespace superecommere.Models.DTO
{
    public class UserLoginRequestDto
    {
        [Required]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;

    }
}
