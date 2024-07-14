using AutoMapper;
using superecommere.Models.DTO.Product;
using superecommere.Models.Products;

namespace superecommere.Helpers
{
    public class ProductUrlResolver : IValueResolver<TblProducts, ProductDetailsDto, string>
    {
        private readonly IConfiguration _config;

        public ProductUrlResolver(IConfiguration config)
        {
            this._config = config;
        }

        public string Resolve(TblProducts source, ProductDetailsDto destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.PictureUrl))
            {
                return _config["JwtConfig:Issuer"]+'/'+source.PictureUrl;
            }
            return null;
        }
    }
}
