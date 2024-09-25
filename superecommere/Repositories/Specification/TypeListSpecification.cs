using superecommere.Models.Products;

namespace superecommere.Repositories.Specification
{
    public class TypeListSpecification : BaseSpecification<ProductType, string>
    {
        public TypeListSpecification()
        {
            AddSelect(x => x.Name);
            ApplyDistinct();
        }
    }
}
