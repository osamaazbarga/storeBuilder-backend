using System.ComponentModel.DataAnnotations;

namespace superecommere.Models.Store
{
    public class DomainVerifyRequest:BaseEntity
    {
        [Required]
        [Url]
        public string Domain { get; set; }
    }
}
