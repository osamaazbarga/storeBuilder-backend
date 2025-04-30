using System.ComponentModel.DataAnnotations;

namespace superecommere.Models.Store
{
    public class DomainConnectRequest:BaseEntity
    {
        [Required]
        public int StoreId { get; set; }

        [Required]
        [Url]
        public string Domain { get; set; }
    }
}
