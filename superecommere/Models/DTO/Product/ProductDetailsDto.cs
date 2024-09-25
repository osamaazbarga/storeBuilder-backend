using superecommere.Models.Products;
using System.ComponentModel.DataAnnotations;

namespace superecommere.Models.DTO.Product
{
    public class ProductDetailsDto
    {
        //[Required]
        //public int Id { get; set; }
        [Required]
        public string Title { get; set; } = string.Empty;
        [Required]
        public string Description { get; set; } = string.Empty;
        [Range(0.01,double.MaxValue,ErrorMessage ="Price must be greater than 0")]
        public decimal Price { get; set; }
        [Required]
        public string PictureUrl { get; set; } = string.Empty;
        public ProductType ProductType { get; set; }


        public ProductBrand ProductBrand { get; set; }


        public int ProductTypeId { get; set; }
        public int ProductBrandId { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "Quantity in stock must be greater than 0")]
        public int Quantity { get; set; }

    }
}
