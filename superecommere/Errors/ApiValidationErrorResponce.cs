namespace superecommere.Errors
{
    public class ApiValidationErrorResponce : ApiErrorResponse
    {
        public ApiValidationErrorResponce() : base(400,null,null)
        {
        }

        public IEnumerable<string> Errors { get; set; }
    }
}
