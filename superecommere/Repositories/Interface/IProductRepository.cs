using superecommere.Models.Products;

namespace superecommere.Repositories.Interface
{
    public interface IProductRepository
    {
        Task<TblProducts> GetProductByIdAsync(int id);
        Task<IReadOnlyList<TblProducts>> GetProductsAsync(int? brandID,int?typeID,string?sort);
        Task<IReadOnlyList<ProductBrand>> GetBrandsAsync();
        Task<IReadOnlyList<ProductType>> GetTypesAsync();

        void AddProduct(TblProducts product);
        void UpdateProduct(TblProducts product);
        void DeleteProduct(TblProducts product);
        bool ProductExists(int id);
        Task<bool> SaveChangesAsync();



    }
}
