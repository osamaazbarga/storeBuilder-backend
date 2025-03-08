using Microsoft.AspNetCore.Mvc;
using superecommere.Data;
using superecommere.Errors;
using superecommere.Models.DTO.Product;
using superecommere.Models.Products;

namespace superecommere.Controllers
{
  
    public class BuggyController:BaseApiController
    {
        
        private readonly ApplicationDbContext _context;

        public BuggyController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet("unauhorized")]
        public IActionResult GetUnauhorized()
        {
            return Unauthorized();
        }

        [HttpGet("notfound")]
        public IActionResult GetNotFound()
        {
            var thing = _context.Products.Find(42);
            if(thing == null)
            {
                return NotFound();
            }
            return Ok();
        }

        [HttpGet("internalerror")]
        public IActionResult GetInternalError()
        {
            throw new Exception("This is a test exeception");
        }

        [HttpPost("validationerror")]
        public IActionResult GetValidationError(ProductDetailsDto products)
        {
            return Ok();
        }

        [HttpGet("servererror")]
        public ActionResult GetServerError()
        {
            var thing = _context.Products.Find(42);
            var thingToReturn = thing?.ToString();
            return Ok();
        }

        [HttpGet("badrequest")]
        public IActionResult GetBadRequest()
        {
            return BadRequest();
        }

        [HttpGet("badrequest/{id}")]
        public ActionResult GetNotFoundRequest(int id)
        {
            return Ok();
        }
    }
}
