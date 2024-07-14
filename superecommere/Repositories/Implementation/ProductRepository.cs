
using Microsoft.EntityFrameworkCore;
using superecommere.Data;
using superecommere.Models.Products;
using superecommere.Repositories.Interface;


namespace superecommere.Repositories.Implementation
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _Context;
        public ProductRepository(ApplicationDbContext Context)
        { 
            _Context = Context;
        }

        public async Task<IReadOnlyList<TblProducts>> GetProductsAsync()
        {
            return await _Context.Products.ToListAsync();
        }

        public async Task<TblProducts> GetProductsByIdAsync(int id)
        {
            return await _Context.Products.FindAsync(id);
        }
    }
}
