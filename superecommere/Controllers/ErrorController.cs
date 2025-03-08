using Microsoft.AspNetCore.Mvc;
using superecommere.Errors;

namespace superecommere.Controllers
{
    
    [Route("errors/{code}")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ErrorController:BaseApiController
    {
        public IActionResult Error(int code)
        {

            return new ObjectResult(new ApiErrorResponse(code,null,null));
        }
    }
}
