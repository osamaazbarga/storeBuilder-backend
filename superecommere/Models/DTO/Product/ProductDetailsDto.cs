using superecommere.Models.Products;

namespace superecommere.Models.DTO.Product
{
    public class ProductDetailsDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string PictureUrl { get; set; }
        public ProductType ProductType { get; set; }
        //public string ProductType { get; set; }

        public ProductBrand ProductBrand { get; set; }
        //public string ProductBrand { get; set; }

    }
}
