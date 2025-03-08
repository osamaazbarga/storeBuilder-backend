using superecommere.Models.Domain;

namespace superecommere.Models.Categories
{
    public class StoreCategories : BaseEntity
    {
        public string Name { get; set; }
        public string ArName { get; set; }
        public string Description { get; set; }
        ICollection<SubStoreCategory> Subcategories { get; set; }
    }
}
