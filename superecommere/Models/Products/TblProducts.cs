using superecommere.Models.Domain;

namespace superecommere.Models.Products
{
    public class TblProducts:BaseEntity
    {
        public required string Title { get; set; }
        public required string Description { get; set; }
        public decimal Price { get; set; }
        public string? PictureUrl { get; set; }
        //public ProductType ProductType { get; set; }
        public int? ProductTypeId { get; set; }
        //public ProductBrand ProductBrand { get; set; }
        public int? ProductBrandId { get; set;}
        public int? Quantity { get; set; }
        public TblUser User { get; set; }
        public string CreatedBy { get; set; }// Foreign key
        public DateTime ModefiedDate { get; set; }
        public DateTime CreateDate { get; set; } = DateTime.UtcNow;






    }
}
