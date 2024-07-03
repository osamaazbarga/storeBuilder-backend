using superecommere.Models.Domain;
using System.ComponentModel.DataAnnotations;

namespace superecommere.Models.Store
{
    public class TblStore
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Link { get; set; } = string.Empty;
        public string Kind { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string Logo { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public TblUser User { get; set; } = null!;
        public required string UserId { get; set; }// Foreign key
        public DateTime CreateDate { get; set; } = DateTime.UtcNow;



    }
}
