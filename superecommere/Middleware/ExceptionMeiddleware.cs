using superecommere.Errors;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace superecommere.Middleware
{
    public class ExceptionMeiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMeiddleware> _logger;
        private readonly IHostEnvironment _env;

        public ExceptionMeiddleware(RequestDelegate next,ILogger<ExceptionMeiddleware> logger,IHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }


        public async Task InvokeAsync(HttpContext context) 
        {
            try
            {
                await _next(context);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                context.Response.ContentType= "application/json";
                context.Response.StatusCode=(int)HttpStatusCode.InternalServerError;
                var response = _env.IsDevelopment()
                    ? new ApiException((int)HttpStatusCode.InternalServerError, ex.Message, ex.StackTrace.ToString())
                    : new ApiResponse((int)HttpStatusCode.InternalServerError);
                var option = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
                var json=JsonSerializer.Serialize(response, option);
                await context.Response.WriteAsync(json);
            }
        }
    }
}
