using AutoMapper;
using Mailjet.Client.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using superecommere.Data;
using superecommere.Models.Categories;
using superecommere.Models.Products;
using superecommere.Repositories.Interface;
using superecommere.Repositories.Specification;

namespace superecommere.Controllers
{
    public class StoreCategoriesController(ApplicationDbContext context,IGenericRepository<StoreCategories> catRepo, IGenericRepository<SubStoreCategory> subCatRepo): BaseApiController
    {

        [HttpGet("category")]
        public async Task<ActionResult<IReadOnlyList<StoreCategories>>> GetCategories()
        {
            return Ok( await catRepo.ListAllAsync());   
        }
        [HttpGet("subcategory")]
        public async Task<ActionResult<IReadOnlyList<SubStoreCategory>>> GetSubCategories()
        {
            return Ok(await subCatRepo.ListAllAsync());
        }

        [HttpGet("category/{id:int}")]
        public async Task<ActionResult<StoreCategories>> GetCategory(int id)
        {
            var category = await catRepo.GetByIdAsync(id);
            if (category == null)
            {
                return NotFound(/*new ApiErrorResponse(404)*/);
            }
            return category;
        }
        [HttpGet("subcategory/{id:int}")]
        public async Task<ActionResult<SubStoreCategory>> GetSubCategory(int id)
        {
            var subCategory = await subCatRepo.GetByIdAsync(id);
            if (subCategory == null)
            {
                return NotFound(/*new ApiErrorResponse(404)*/);
            }
            return subCategory;
        }

        [HttpGet("subs-by-category/{id:int}")]
        public async Task<ActionResult<IReadOnlyList<SubStoreCategory>>> GetSubCategoriesByCategoryId(int id)
        {
            var category = await catRepo.GetByIdAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            var subCategories = await context.SubStoreCategories.Where(c => c.CategoryId == category.Id).ToListAsync();
            if (subCategories == null)
            {
                return NotFound(/*new ApiErrorResponse(404)*/);
            }
            return Ok(subCategories);
        }
        //[HttpGet("subcategory/{id:int}")]
        //public async Task<ActionResult<IReadOnlyList<SubStoreCategory>>> GetSubCategory(int id)
        //{
        //    var subCategory = await subCatRepo.GetByIdAsync(id);
        //    if (subCategory == null)
        //    {
        //        return NotFound(/*new ApiErrorResponse(404)*/);
        //    }
        //    return subCategory;
        //}

    }
}
