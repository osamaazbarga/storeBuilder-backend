using superecommere.Models.Products;
using superecommere.Models.Store;

namespace superecommere.Models.Categories
{
    public class StoreCategoryContainer:BaseEntity
    {
        public int SubCategoryId { get; set; }
        public SubStoreCategory SubCategory { get; set; }
        public int StoreId { get; set; }
        public TblStore Store { get; set; } // Many-to-One relationship
    }
}
