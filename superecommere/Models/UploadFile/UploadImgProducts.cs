using superecommere.Models.Domain;

namespace superecommere.Models.UploadFile
{
    public class UploadImgProducts: S3ObjectDto
    {
        public int ProductId { get; set; }
        public bool IsCover { get; set; }
        public TblUser User { get; set; }
        public string UserId { get; set; }// Foreign key
        public bool IsAvalible { get; set; }
        public DateTime CreateDate { get; set; } = DateTime.UtcNow;

    }
}
