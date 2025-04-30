namespace superecommere.Models.UploadFile
{
    public class S3ObjectDto: BaseEntity
    {
        public string? Name { get; set; }
        public string? PresignedUrl { get; set; }
    }
}
