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
using superecommere.Services;


namespace superecommere.Controllers
{

    public class ProductsController(ApplicationDbContext context, TranslationService translationService, IMapper mapper,IGenericRepository<TblProducts> repo, IGenericRepository<ProductTranslation> repoTrans, IGenericRepository<ProductBrand> repoBrand, IGenericRepository<ProductType> repoType, IProductRepository repoPro) : BaseApiController
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
                return NotFound(/*new ApiErrorResponse(404)*/);
            }
            return product;
        }

        [HttpPost]
        public async Task<ActionResult<TblProducts>> CreateProduct(TblProducts product)
        {
            repo.Add(product);
            if (await repo.SaveAllAsync()) { 
                return CreatedAtAction("GetProduct", new {id=product.Id},product);
            }
            return BadRequest("problem Creating Product"); 
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> UpdateProduct(int id,TblProducts product)
        {
            if (product.Id != id || !ProductExists(id)) return BadRequest("Cannot update this product");
            repo.Update(product);
            if (await repo.SaveAllAsync())
            {
                var translations = await context.ProductTranslation
                .Where(t => t.ProductId == id).ToListAsync();
                foreach (var item in translations)
                {
                    item.IsTranslateChanged = true;
                    repoTrans.Update(item);

                };
                return NoContent();
            }

            return BadRequest("problem Updating Product");

        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteProduct(int id) {
            var product = await repo.GetByIdAsync(id);
            if (product == null) return NotFound();
            repo.Remove(product);
            if (await repo.SaveAllAsync())
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
                //.Include(p => p.ProductType)
                //.Include(p => p.ProductBrand)
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
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]

        public async Task<ActionResult<ProductDetailsDto>> GetProductsByIdAsync(int id)
        {

            var product = await context.Products
                //.Include(p => p.ProductType)
                //.Include(p => p.ProductBrand)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
            {
                return NotFound(/*new ApiErrorResponse(404)*/);
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


        //public async Task AddProductWithTranslationsAsync(TblProducts product, string language)
        //{



        //    // ترجمة النصوص إلى اللغات المختلفة
        //    //string englishText = product.Title;  // مثال على اسم المنتج
        //    //string arabicTranslation = await translationService.TranslateTextAsync(englishText, language);
        //    //string hebrewTranslation = await translationService.TranslateTextAsync(englishText, "he");

        //    // تخزين الترجمات في قاعدة البيانات
        //    var translations = new ProductTranslation
        //    {
        //        Title = product.Title,
        //        Description = product.Description,
        //        TranslatedTitle = await translationService.TranslateTextAsync(product.Title, language),
        //        TranslatedDescription = await translationService.TranslateTextAsync(product.Description, language),
        //        ProductId = product.Id,
        //        Language = language,

        //    };

        //    // إضافة الترجمات لقاعدة البيانات
        //    context.ProductTranslation.AddRange(translations);
        //    await context.SaveChangesAsync();
        //}

        //public async Task UpdateProductWithTranslationsAsync(TblProducts product, ProductTranslation productTranslation, string language)
        //{

        //    if (productTranslation.Title != product.Title)
        //    {
        //        productTranslation.TranslatedTitle = await translationService.TranslateTextAsync(product.Title, language);
        //        productTranslation.Title = product.Title;
        //    }
        //    if (productTranslation.Description != product.Description)
        //    {
        //        productTranslation.TranslatedDescription = await translationService.TranslateTextAsync(product.Description, language);
        //        productTranslation.Description = product.Description;
        //    }
        //    productTranslation.IsTranslateChanged = false;
        //    productTranslation.ModefiedDate = DateTime.Now;

        //    // إضافة الترجمات لقاعدة البيانات
        //    repoTrans.Update(productTranslation);
        //    await repoTrans.SaveAllAsync();
        //}

        //public async Task<string> GetProductTranslationAsync(int productId, string language)
        //{
        //    var product = await repo.GetByIdAsync(productId);
        //    if (product == null)
        //    {
        //        return "Product not found";
        //    }
        //    var translation = await context.ProductTranslation
        //        .FirstOrDefaultAsync(t => t.ProductId == productId && t.Language == language);
        //    if (translation==null)
        //    {
        //        await AddProductWithTranslationsAsync(product, language);
        //    }
        //    if (translation.IsTranslateChanged)
        //    {
        //        if (product.Id != productId || !ProductExists(productId)) return "Cannot update this product";
        //       // await UpdateProductWithTranslationsAsync(product, translation, language);
        //    }

        //    return translation?.Title ?? "Translation not found";
        //}


    }
}
