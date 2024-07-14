using superecommere.Models.Products;

namespace superecommere.Repositories.Interface
{
    internal interface IProductRepository
    {
        Task<TblProducts> GetProductsByIdAsync(int id);
        Task<IReadOnlyList<TblProducts>> GetProductsAsync();

    }
}
