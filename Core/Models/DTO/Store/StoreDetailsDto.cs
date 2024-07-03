using Core.Models.Domain;

namespace Core.Models.DTO.Store
{
    public class StoreDetailsDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Link { get; set; } = string.Empty;
        public string Kind { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string Logo { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public required TblUser User { get; set; }
        public DateTime CreateDate { get; set; } = DateTime.UtcNow;
    }
}
