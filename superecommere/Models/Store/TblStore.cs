using superecommere.Models.Domain;
using System.ComponentModel.DataAnnotations;

namespace superecommere.Models.Store
{
    public class TblStore:BaseEntity
    {
        public string Name { get; set; }
        public string Link { get; set; }
        public string Subdomain { get; set; }
        public string CustomDomain { get; set; }
        public string DomainVerificationStatus { get; set; } = "Unverified";
        public Guid DomainVerificationCode { get; set; } = Guid.NewGuid();
        public int OwnerUserId { get; set; }
        public string Kind { get; set; }
        public string Category { get; set; }
        public string Logo { get; set; }
        public string Description { get; set; }
        public TblUser User { get; set; }
        public string UserId { get; set; }// Foreign key
        public DateTime ModefiedDate { get; set; }
        public DateTime CreateDate { get; set; } = DateTime.UtcNow;
        public Boolean IsActive { get; set; } = true;

        




    }
}
