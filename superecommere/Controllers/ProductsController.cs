using AutoMapper;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using superecommere.Data;
using superecommere.Errors;
using superecommere.Helpers;
using superecommere.Models.DTO.Product;
using superecommere.Models.Products;
using superecommere.Repositories.Interface;
using superecommere.Repositories.Specification;


namespace superecommere.Controllers
{

    public class ProductsController(ApplicationDbContext context,IMapper mapper,IGenericRepository<TblProducts> repo,IGenericRepository<ProductBrand> repoBrand, IGenericRepository<ProductType> repoType, IProductRepository repoPro) : BaseApiController
    {


        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<TblProducts>>> GetProducts(
            [FromQuery]ProductSpecParams specParams)
        {
            //return await context.Products.ToListAsync();
            //return Ok( await repoPro.GetProductsAsync(brandID, typeID,sort)); 
            var spec = new ProductSpecification(specParams);
            return await CreatePagedResult(repo,spec,specParams.PageIndex,specParams.PageSize);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<TblProducts>> GetProduct(int id)
        {
            var product=await repo.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound(new ApiResponse(404));
            }
            return product;
        }

        [HttpPost]
        public async Task<ActionResult<TblProducts>> CreateProduct(TblProducts product)
        {
            repo.Add(product);
            if (await repo.SacveAllAsync()) { 
                return CreatedAtAction("GetProduct", new {id=product.Id},product);
            }
            return BadRequest("problem Creating Product"); 
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> UpdateProduct(int id,TblProducts product)
        {
            if (product.Id != id || !ProductExists(id)) return BadRequest("Cannot update this product");
            repo.Update(product);
            if (await repo.SacveAllAsync())
            {
                return NoContent();
            }

            return BadRequest("problem Updating Product");

        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteProduct(int id) {
            var product = await repo.GetByIdAsync(id);
            if (product == null) return NotFound();
            repo.Remove(product);
            if (await repo.SacveAllAsync())
            {
                return NoContent();
            }
            return BadRequest("problem Deleting Product");
        }

        private bool ProductExists(int id)
        {
            return repo.Exists(id);
        }



        [HttpGet("async")]
        public async Task<ActionResult<IReadOnlyList<ProductDetailsDto>>> GetProductsAsync()
        {
            var products = await context.Products
                .Include(p => p.ProductType)
                .Include(p => p.ProductBrand)
                .ToListAsync();
            return Ok(mapper.Map<IReadOnlyList<TblProducts>, IReadOnlyList<ProductDetailsDto>>(products));
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

        [HttpGet("async/{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]

        public async Task<ActionResult<ProductDetailsDto>> GetProductsByIdAsync(int id)
        {

            var product = await context.Products
                .Include(p => p.ProductType)
                .Include(p => p.ProductBrand)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
            {
                return NotFound(new ApiResponse(404));
            }
            return mapper.Map<TblProducts, ProductDetailsDto>(product);
        }

        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetProductBrandsAsync()
        {
            var spec = new BrandListSpecification();
            return Ok(await repoBrand.ListAsync(spec));
            return Ok(await repoPro.GetBrandsAsync());
        }
        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<ProductType>>> GetProductTypesAsync()
        {
            var spec = new TypeListSpecification();
            return Ok(await repoType.ListAsync(spec));
            return Ok(await repoPro.GetTypesAsync());
        }
    }
}
