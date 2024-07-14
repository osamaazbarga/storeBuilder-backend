﻿namespace superecommere.Errors
{
    public class ApiResponse
    {
        public ApiResponse(int statusCode,string message=null)
        {
            StatusCode = statusCode;
            Message = message??GetDefaultMessageForStatusCode(statusCode);
        }

        public int StatusCode { get; set; }
        public string Message { get; set; }

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
