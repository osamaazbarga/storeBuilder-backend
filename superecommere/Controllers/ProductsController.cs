using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using superecommere.Data;
using superecommere.Errors;
using superecommere.Models.DTO.Product;
using superecommere.Models.Products;


namespace superecommere.Controllers
{
    //[Route("api/[controller]")]
    //[ApiController]
    public class ProductsController : BaseApiController
    {
        private readonly ApplicationDbContext _Context;
        private readonly IMapper _mapper;

        public ProductsController(ApplicationDbContext Context,
            IMapper mapper)
        {
            _Context = Context;
            _mapper = mapper;
        }

        [HttpGet("")]
        public async Task<ActionResult<IReadOnlyList<ProductDetailsDto>>> GetProductsAsync()
        {
            var products= await _Context.Products
                .Include(p => p.ProductType)
                .Include(p => p.ProductBrand)
                .ToListAsync();
            return Ok(_mapper.Map<IReadOnlyList<TblProducts>, IReadOnlyList<ProductDetailsDto>>(products));
            //return products.Select(product => new ProductDetailsDto
            //{
            //    Id = product.Id,
            //    Title = product.Title,
            //    Description = product.Description,
            //    PictureUrl = product.PictureUrl,
            //    Price = product.Price,
            //    ProductBrand = product.ProductBrand,
            //    ProductType = product.ProductType
            //}).ToList();
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse),StatusCodes.Status404NotFound)]

        public async Task<ActionResult<ProductDetailsDto>> GetProductsByIdAsync(int id)
        {

            var product= await _Context.Products
                .Include(p => p.ProductType)
                .Include(p => p.ProductBrand)
                .FirstOrDefaultAsync(p => p.Id == id);

            if(product == null)
            {
                return NotFound(new ApiResponse(404));
            }
            return _mapper.Map<TblProducts,ProductDetailsDto>(product);
        }

        [HttpGet("brands")]
        public async Task<ActionResult<List<ProductBrand>>> GetProductBrandsAsync()
        {
            return await _Context.ProductBrands.ToListAsync();
        }
        [HttpGet("types")]
        public async Task<ActionResult<List<ProductType>>> GetProductTypesAsync()
        {
            return await _Context.ProductTypes.ToListAsync();
        }
    }
}
