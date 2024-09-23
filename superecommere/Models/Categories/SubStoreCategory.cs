namespace superecommere.Models.Categories
{
    public class SubStoreCategory
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public StoreCategories Category { get; set; }
    }
}
