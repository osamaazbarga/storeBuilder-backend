namespace superecommere.Models.Categories
{
    public class SubStoreCategory:BaseEntity
    {
        public string Name { get; set; }
        public string ArName { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public StoreCategories Category { get; set; }
    }
}
