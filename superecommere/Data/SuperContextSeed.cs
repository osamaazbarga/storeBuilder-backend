using superecommere.Models.Products;
using System.Text.Json;

namespace superecommere.Data
{
    public class SuperContextSeed
    {
        public static async Task SeedAsync(ApplicationDbContext context)
        {
            if(!context.ProductBrands.Any())
            {
                var brandsData = File.ReadAllText("../superecommere/Data/SeedData/brands.json");
                var brands =JsonSerializer.Deserialize<List<ProductBrand>>(brandsData);
                var brandsList = new List<ProductBrand>();
                foreach (var brand in brands)
                {
                    var productBrand = new ProductBrand();
                    productBrand.Id = brand.Id;
                    productBrand.Name = brand.Name;
                    brandsList.Add(productBrand);
                }
                context.ProductBrands.AddRange(new ProductBrand { Id = 2, Name = "Brand1" },
                new ProductBrand {Id=1 ,Name = "Brand2" });
                context.ProductBrands.Add(new ProductBrand { Id = 2, Name = "Brand1" });
                await context.SaveChangesAsync();
            }

            if (!context.ProductTypes.Any())
            {
                var typesData = File.ReadAllText("../superecommere/Data/SeedData/types.json");
                var types = JsonSerializer.Deserialize<List<ProductType>>(typesData);
                context.ProductTypes.AddRange(types);
            }

            if (!context.Products.Any())
            {
                var productsData = File.ReadAllText("../superecommere/Data/SeedData/products.json");
                var products = JsonSerializer.Deserialize<List<TblProducts>>(productsData);
                context.Products.AddRange(products);
            }

            if(context.ChangeTracker.HasChanges())
            {
                await context.SaveChangesAsync();
            }

        }
    }
}
