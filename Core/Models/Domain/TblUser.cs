


using Core.Models.Store;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Core.Models.Domain
{
    public class TblUser:IdentityUser
    {
        //public Guid Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Merchant { get; set; } = string.Empty;


        //[Required]
        //public string Email { get; set; } = string.Empty;


        //[Required]
        public string Phone { get; set; } = string.Empty;
        //public override string PhoneNumber { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Region { get; set; } = string.Empty;
        public string PostalCode { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public int IsDeleted { get; set; }
        public int Plan { get; set; } = 0;
        public DateTime JoinDate { get; set; }= DateTime.UtcNow;

        public string Provider { get; set; } = string.Empty;

        public ICollection<TblStore>? Stores { get; set; }
    }
}
