using System.ComponentModel.DataAnnotations;

namespace superecommere.Models.DTO
{
    public class TblUserDto
    {
        public string? Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        //[Required]
        //public string Email { get; set; } = string.Empty;
        //public string Username { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        //public override string PhoneNumber { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Region { get; set; } = string.Empty;
        public string PostalCode { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public int IsDeleted { get; set; }
        public int Plan { get; set; } = 0;
        public DateTime JoinDate { get; set; } = DateTime.UtcNow;
        public string JWT { get; set; }= string.Empty;


    }
}
