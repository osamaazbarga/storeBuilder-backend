using System.ComponentModel.DataAnnotations;

namespace superecommere.Models.DTO
{
    public class RegisterDto
    {
        [Required]
        //[StringLength(15,MinimumLength =3,ErrorMessage ="First name must be at least {2}, and maximum {1} charactors")]
        [RegularExpression("^([0-9a-zA-Z]+[-._+&amp;])*[0-9a-zA-Z]+@([-0-9a-zA-Z]+[.])+[a-zA-Z]{2,6}$", ErrorMessage ="Invalid Email Address")]
        public required string Email { get; set; }
        [Required]
        [StringLength(15, MinimumLength = 6, ErrorMessage = "Password name must be at least {2}, and maximum {1} charactors")]
        public required string Password { get; set; }
        [Required]
        [StringLength(15, MinimumLength = 3, ErrorMessage = "Merchant must be at least {2}, and maximum {1} charactors")]
        public required string Merchant { get; set; }
        [Required]
        [StringLength(15, MinimumLength = 3, ErrorMessage = "First name must be at least {2}, and maximum {1} charactors")]
        public required string FirstName { get; set; }
        [Required]
        [StringLength(15, MinimumLength = 3, ErrorMessage = "Last name must be at least {2}, and maximum {1} charactors")]
        public required string LastName { get; set; }
        [Required]
        [StringLength(15, MinimumLength = 10, ErrorMessage = "Phone number must be at least {2}, and maximum {1} charactors")]
        public required string Phone { get; set; }


    }
}
