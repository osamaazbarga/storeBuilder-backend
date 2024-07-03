using Core.Models.Domain;

namespace Core.Models.DTO.Store
{
    public class StoreAddEditDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Link { get; set; }
        public string Kind { get; set; }
        public string Category { get; set; }
        public string Logo { get; set; }
        public string Description { get; set; }
        public string UserId { get; set; }
        public required TblUser User { get; set; }
    }
}
