using superecommere.Models.Products;

namespace superecommere.Repositories.Specification
{
    public class BrandListSpecification:BaseSpecification<ProductBrand, /*ProductBrand*/string>
    {
        public BrandListSpecification()
        {
            AddSelect(x => x.Name);
            //AddSelect(x => x);

            ApplyDistinct();
        }
    }
}
