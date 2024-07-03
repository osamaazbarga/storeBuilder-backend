using System.ComponentModel.DataAnnotations;

namespace superecommere.Models.DTO
{
    public class LoginDto
    {
        [Required(ErrorMessage = "Email Is Required")]
        public required string UserName { get; set; }
        [Required]
        public required string Password { get; set; }

    }
}
