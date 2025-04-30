using superecommere.Models.Domain;

namespace superecommere.Models.Products
{
    public class ProductTranslation : BaseEntity
    {
        public required string Title { get; set; }
        public required string Description { get; set; }
        public required string TranslatedTitle { get; set; }
        public required string TranslatedDescription { get; set; }
        public required int ProductId { get; set; }
        public required string Language { get; set; }
        public DateTime ModefiedDate { get; set; }
        public bool IsTranslateChanged { get; set; }
        public DateTime CreateDate { get; set; } = DateTime.UtcNow;


    }
}
