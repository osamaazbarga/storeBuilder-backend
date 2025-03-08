using superecommere.Models.Products;
using System.Linq;

namespace superecommere.Repositories.Specification
{
    public class ProductSpecification:BaseSpecification<TblProducts>
    {
        public ProductSpecification(ProductSpecParams specParams):base(x=>
        (string.IsNullOrEmpty(specParams.Search)||x.Title.ToLower().Contains(specParams.Search))
        //&&
        //(!specParams.Brands.Any() || specParams.Brands.Contains(x.ProductBrandId.ToString())/*x.ProductBrandId == brandId*/)&&
        //(!specParams.Types.Any() || specParams.Types.Contains(x.ProductTypeId.ToString()))
        )
        {
            ApplyPaging(specParams.PageSize * (specParams.PageIndex - 1), specParams.PageSize);
            switch (specParams.Sort)
            {
                case "priceAsc":
                    AddOrderBy(x=>x.Price); break;
                case "priceDesc":
                    AddOrderByDescendig(x => x.Price); break;
                default:
                    AddOrderBy(x => x.Title); break;
            }
        }
    }
}
