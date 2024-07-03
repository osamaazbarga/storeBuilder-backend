using System.ComponentModel.DataAnnotations;

namespace Core.Models.DTO
{
    public class ConfirmEmailDto
    {
        [Required]
        [RegularExpression("^([0-9a-zA-Z]+[-._+&amp;])*[0-9a-zA-Z]+@([-0-9a-zA-Z]+[.])+[a-zA-Z]{2,6}$", ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }
        [Required]
        
        public string Token { get; set; }
    }
}
