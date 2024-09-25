namespace superecommere.Errors
{
    public class ApiErrorResponse(int statusCode, string message,string? details)
    {
        //public ApiErrorResponse(int statusCode,string message=null)
        //{
        //    StatusCode = statusCode;
        //    Message = message??GetDefaultMessageForStatusCode(statusCode);
        //}

        public int StatusCode { get; set; } = statusCode;
        public string Message { get; set; } = message;
        public string? Details { get; set; } = details;

        private string GetDefaultMessageForStatusCode(int statusCode)
        {
            return statusCode switch
            {
                400 => "A bad request ,you have made",
                401 => "Authorize, you are not",
                404 => "Resorce found, it was not",
                500 => "Errors are the path to dark side.Errors lead to anger",
                _ => null
            };
        }

    }
}
