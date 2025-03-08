using Microsoft.AspNetCore.Mvc;
using superecommere.Helpers;
using superecommere.Models;
using superecommere.Repositories.Interface;

namespace superecommere.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BaseApiController:ControllerBase
    {
        protected async Task<ActionResult> CreatePagedResult<T>(IGenericRepository<T> repo,
            ISpecification<T> spec, int pageIndex,int pageSize)where T : BaseEntity
        {
            var items = await repo.ListAsync(spec);
            var count=await repo.CountAsync(spec);
            var pagination = new Pagination<T>(pageIndex, pageSize, count, items);
            return Ok(pagination);
        }
    }
}
