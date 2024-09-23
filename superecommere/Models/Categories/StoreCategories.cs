using superecommere.Models.Domain;

namespace superecommere.Models.Categories
{
    public class StoreCategories
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        ICollection<SubStoreCategory> Subcategories { get; set; }
    }
}
