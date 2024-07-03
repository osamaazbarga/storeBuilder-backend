using System.ComponentModel.DataAnnotations;

namespace superecommere.Models.DTO
{
    public class ResetPasswordDto
    {
        [Required]
        [RegularExpression("^([0-9a-zA-Z]+[-._+&amp;])*[0-9a-zA-Z]+@([-0-9a-zA-Z]+[.])+[a-zA-Z]{2,6}$", ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }
        [Required]

        public string Token { get; set; }
        [Required]
        [StringLength(15, MinimumLength = 6, ErrorMessage = "New Password name must be at least {2}, and maximum {1} charactors")]
        public string NewPassword { get; set; }
    }
}
