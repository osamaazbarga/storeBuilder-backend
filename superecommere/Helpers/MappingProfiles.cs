using AutoMapper;
using superecommere.Models.DTO.Product;
using superecommere.Models.Products;

namespace superecommere.Helpers
{
    public class MappingProfiles:Profile
    {
        public MappingProfiles()
        {
            CreateMap<TblProducts, ProductDetailsDto>()
                .ForMember(d => d.PictureUrl, o => o.MapFrom<ProductUrlResolver>());
            //if i want to return the brand name and type name 
                //.ForMember(d => d.ProductBrand, o => o.MapFrom(s => s.ProductBrand.Name))
                //.ForMember(d => d.ProductType, o => o.MapFrom(s => s.ProductType.Name));

        }
    }
}
