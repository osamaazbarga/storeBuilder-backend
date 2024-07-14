namespace superecommere.Errors
{
    public class ApiValidationErrorResponce : ApiResponse
    {
        public ApiValidationErrorResponce() : base(400)
        {
        }

        public IEnumerable<string> Errors { get; set; }
    }
}
