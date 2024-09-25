
using Microsoft.EntityFrameworkCore;
using superecommere.Data;
using superecommere.Models.Products;
using superecommere.Repositories.Interface;


namespace superecommere.Repositories.Implementation
{
    public class ProductRepository(ApplicationDbContext context) : IProductRepository
    {
        
      

        public void AddProduct(TblProducts product)
        {
            context.Products.Add(product);
        }

        public void DeleteProduct(TblProducts product)
        {
            context.Products.Remove(product);
        }

        public async Task<IReadOnlyList<ProductBrand>> GetBrandsAsync()
        {
            return await context.ProductBrands.ToListAsync();
        }

        public async Task<TblProducts> GetProductByIdAsync(int id)
        {
            return await context.Products.FindAsync(id);
        }

        public async Task<IReadOnlyList<TblProducts>> GetProductsAsync(int? brandID, int? typeID,string ?sort)
        {
            var quary = context.Products.AsQueryable();
            if (!string.IsNullOrWhiteSpace(brandID.ToString()))
            {
                quary = quary.Where(x => x.ProductBrandId == brandID);
            }
            if (!string.IsNullOrWhiteSpace(typeID.ToString()))
            {
                quary = quary.Where(x => x.ProductTypeId == typeID);
            }

            quary = sort switch
            {
                "priceAsc" => quary.OrderBy(x => x.Price),
                "priceDesc" => quary.OrderByDescending(x => x.Price),
                 _ => quary.OrderBy(x => x.Title)

             };
            
            return await quary.Skip(2).Take(1).ToListAsync();
        }

        public async Task<TblProducts> GetProductsByIdAsync(int id)
        {
            return await context.Products.FindAsync(id);
        }

        public async Task<IReadOnlyList<ProductType>> GetTypesAsync()
        {
            return await context.ProductTypes.ToListAsync();
        }

        public bool ProductExists(int id)
        {
            return context.Products.Any(x=>x.Id==id);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await context.SaveChangesAsync() > 0;
        }

        public void UpdateProduct(TblProducts product)
        {
            context.Entry(product).State= EntityState.Modified;
        }
    }
}
