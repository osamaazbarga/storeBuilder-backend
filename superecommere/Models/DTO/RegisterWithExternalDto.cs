using System.ComponentModel.DataAnnotations;

namespace superecommere.Models.DTO
{
    public class RegisterWithExternalDto
    {
       
        [Required]
        [StringLength(15, MinimumLength = 3, ErrorMessage = "First name must be at least {2}, and maximum {1} charactors")]
        public required string FirstName { get; set; }
        [Required]
        [StringLength(15, MinimumLength = 3, ErrorMessage = "Last name must be at least {2}, and maximum {1} charactors")]
        public required string LastName { get; set; }
        [Required]
        public string AccessToken { get; set; }
        [Required]
        public string UserId { get; set; }
        [Required]
        public string Provider { get; set; }
    }
}
