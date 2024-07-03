using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace superecommere.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        [HttpGet("Products")]
        public IActionResult Products()
        {
            return Ok(new JsonResult( new { message="only users can view products" }) );
        }
    }
}
