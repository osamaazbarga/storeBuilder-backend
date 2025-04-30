using Mailjet.Client.Resources;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using superecommere.Data;
using superecommere.Helpers;
using superecommere.Models.Domain;
using superecommere.Models.DTO.Store;
using superecommere.Models.Products;
using superecommere.Models.Store;
using superecommere.Repositories.Interface;
using superecommere.Services;
using System.Reflection.Metadata;

namespace superecommere.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StoreController(IGenericRepository<TblStore> repo, ApplicationDbContext context) : ControllerBase
    {
        //private readonly ApplicationDbContext _Context;
        //private readonly JWTService _jwtService;
        //private readonly SignInManager<TblUser> _signInManager;
        //private readonly UserManager<TblUser> _userManager;
        //private readonly EmailService _emailService;
        //private readonly IConfiguration _config;





        //public StoreController(JWTService jwtService,
        //    SignInManager<TblUser> signInManager,
        //    UserManager<TblUser> userManager,
        //    ApplicationDbContext Context,
        //    EmailService emailService,
        //    IConfiguration config)
        //{

        //    _Context = Context;
        //    _jwtService = jwtService;
        //    _signInManager = signInManager;
        //    _userManager = userManager;
        //    _emailService = emailService;
        //    _config = config;
        //}
        [HttpGet("get-stores")]

        public async Task<ActionResult<TblProducts>> GetStores()
        {
            List<StoreDetailsDto> stores = new List<StoreDetailsDto>();
            var storesData = await context.Stores.ToListAsync();
            foreach (var store in storesData)
            {
                var storeToAdd = new StoreDetailsDto
                {
                    Id = store.Id,
                    Name = store.Name,
                    Link = store.Link,
                    Category = store.Category,
                    Logo = store.Logo,
                    Description = store.Description,
                    User = store.User,
                    CreateDate = store.CreateDate,
                };
                stores.Add(storeToAdd);
            }

            return Ok(stores);
        }

        [HttpGet("get-store/{id}")]
        public async Task<ActionResult<StoreAddEditDto>> GetStore(int id)
        {

            //var storeData = await context.Stores
            //   .Where(x => x.Id == id).FirstOrDefaultAsync();

            var storeData = await repo.GetByIdAsync(id);
            if (storeData == null)
            {
                return NotFound(/*new ApiErrorResponse(404)*/);
            }

            var store = new StoreAddEditDto
            {
                Id = storeData.Id,
                Name = storeData.Name,
                Link= storeData.Link,
                Category = storeData.Category,
                Logo = storeData.Logo,
                Description = storeData.Description,
                User = storeData.User,
            };




            return Ok(store);
        }

        [HttpGet("get-store-link/{link}")]
        public async Task<ActionResult<StoreAddEditDto>> GetStoreByLink(string link)
        {

            var storeData = await context.Stores
               .Where(x => x.Link == link).FirstOrDefaultAsync();
            if (storeData == null)
            {
                return Ok();
            }
                var store = new StoreAddEditDto
            {
                Id = storeData.Id,
                Name = storeData.Name,
                Link = storeData.Link,
                Category = storeData.Category,
                Logo = storeData.Logo,
                Description = storeData.Description,
                User = storeData.User,
            };




            return Ok(store);
        }

        [HttpGet("check-availble-link/{link}")]
        public async Task<ActionResult<bool>> GetLinkAvailble(string link)
        {

            var storeData = await context.Stores
               .Where(x => x.Link == link).FirstOrDefaultAsync();
            if(storeData == null)
            {
                return true;
            }




            return false;
        }

        [HttpPost("add-edit-store")]
        public async Task<IActionResult> AddEditStore(StoreAddEditDto model)
        {
            var getStore=await context.Stores.AnyAsync(u => u.Link == model.Link.ToLower());
            TblStore store;
            if (context.Stores.Any(s => s.Subdomain == model.Subdomain))
                return BadRequest("Subdomain already exists");
            if (getStore)
            {
                return BadRequest($"An existing Store is using {model.Link},Link address. please try with another Link");
            }
            var user = await context.Users.FirstOrDefaultAsync(x => x.Id == model.UserId);
            //var user = await _userManager.Users
            //    .Where(x => x.UserName != SD.AdminUserName && x.Id == id).FirstOrDefaultAsync();
            //add a new Store
            store = new TblStore
            {
                 Name = model.Name,
                    Link = model.Link,
                    Category = model.Category,
                    Kind = model.Kind,
                    Logo = model.Logo,
                    Description = model.Description,
                    UserId= user.Id,
                    Subdomain=model.Subdomain,
                    User = user,
                };
                //context.Stores.Add(store);
                repo.Add(store);
            if (await repo.SaveAllAsync())
            {
                return Ok(store);
            }
            return BadRequest("problem Creating Product"); ;
            //return Ok(store);

            return CreatedAtAction(nameof(GetStore), new { id = store.Id }, store);

            //return CreatedAtAction(nameof(GetStore), new { id = store.Id }, store);
            //var result = await _userManager.CreateAsync(user, model.Password);
            //if (!result.Succeeded) { return BadRequest(result.Errors); }


            //else
            //{
            //    //editing an existing member


            //    store = await _Context.Stores.Where(x => x.Id == model.Id).FirstOrDefaultAsync();
            //    if (store == null) return NotFound();
            //    store.Name = model.Name.ToLower();
            //    store.Link = model.Link.ToLower();
            //    store.Category = model.Category.ToLower();
            //    store.Logo = model.Logo.ToLower();
            //    store.Description = model.Description.ToLower();
            //    store.User = model.User;
            //}

            //var userRoles = await _userManager.GetRolesAsync(user);

            ////removing users existing role(s)
            //await _userManager.RemoveFromRolesAsync(user, userRoles);

            //foreach (var role in model.Roles.Split(',').ToArray())
            //{
            //    var roleToAdd = await _roleManager.Roles.FirstOrDefaultAsync(r => r.Name == role);
            //    if (roleToAdd != null)
            //    {
            //        await _userManager.AddToRoleAsync(user, role);
            //    }
            //}

            //if (model.Id==null)
            //{
            //    return Ok(new JsonResult(new { title = "Store Created", message = $"{model.Name} has been created" }));
            //}
            //else
            //{
            //    return Ok(new JsonResult(new { title = "Store Edited", message = $"{model.Name} has been updated" }));
            //}

        }


        [HttpPost("custom-domain")]
        public async Task<IActionResult> ConnectCustomDomain([FromBody] CustomDomainDto dto)
        {
            var store = context.Stores.FirstOrDefault(s => s.OwnerUserId == 1); // simulate auth
            if (store == null) return NotFound();

            store.CustomDomain = dto.CustomDomain;
            store.DomainVerificationStatus = "Pending";

            await context.SaveChangesAsync();

            return Ok(new { success = true, status = store.DomainVerificationStatus });
        }


        [HttpGet("by-subdomain/{subdomain}")]
        public IActionResult GetStoreBySubdomain(string subdomain)
        {
            var store = context.Stores.FirstOrDefault(s => s.Subdomain == subdomain);

            if (store == null)
                return NotFound();

            return Ok(store);
        }

        [HttpPut("{id}/custom-domain")]
        public async Task<IActionResult> UpdateCustomDomain(int id, [FromBody] CustomDomainDto dto)
        {
            var store = await context.Stores.FindAsync(id);
            if (store == null) return NotFound();

            store.CustomDomain = dto.CustomDomain;
            store.DomainVerificationStatus = "Unverified";
            store.DomainVerificationCode = Guid.NewGuid(); // trigger re-verification

            await context.SaveChangesAsync();
            return Ok(store);
        }

        // POST: api/stores/{id}/verify-domain
        [HttpPost("{id}/verify-domain")]
        public async Task<IActionResult> VerifyDomain(int id)
        {
            var store = await context.Stores.FindAsync(id);
            if (store == null) return NotFound();

            // In real setup you'd verify DNS here
            store.DomainVerificationStatus = "Verified";

            await context.SaveChangesAsync();
            return Ok(new { status = "Verified" });
        }

        // PUT: api/stores/{id}/deactivate
        [HttpPut("{id}/deactivate")]
        public async Task<IActionResult> DeactivateStore(int id)
        {
            var store = await context.Stores.FindAsync(id);
            if (store == null) return NotFound();

            store.IsActive = false;
            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPost("verify-domain")]
        public async Task<IActionResult> VerifyDomain([FromBody] DomainVerifyRequest request)
        {
            var store = await context.Stores.FirstOrDefaultAsync(s => s.CustomDomain == request.Domain);

            if (store == null) return NotFound();

            // Check TXT DNS record (or make a fetch request to a file/token endpoint)
            var dnsChecker = new DnsChecker();
            if (await dnsChecker.HasVerificationRecordAsync(request.Domain, store.DomainVerificationCode.ToString()))
            {
                store.DomainVerificationStatus = "Verified";
                await context.SaveChangesAsync();
                return Ok("Verified");
            }

            return BadRequest("Verification TXT record not found.");
        }


        [HttpPost("connect-custom-domain")]
        public async Task<IActionResult> ConnectCustomDomain([FromBody] DomainConnectRequest request)
        {
            var store = await context.Stores.FirstOrDefaultAsync(s => s.Id == request.StoreId);
            if (store == null) return NotFound();

            store.CustomDomain = request.Domain.ToLower();
            store.DomainVerificationCode = Guid.NewGuid(); // New verification code
            store.DomainVerificationStatus = "Unverified";

            await context.SaveChangesAsync();
            return Ok(new { VerificationCode = store.DomainVerificationCode });
        }




        //[HttpPut("lock-member/{id}")]
        //public async Task<IActionResult> LockMember(string id)
        //{
        //    var user = await _userManager.FindByIdAsync(id);
        //    if (user == null) return NotFound();

        //    if (IsAdminUserId(id))
        //    {
        //        return BadRequest(SD.SuperAdminChangeNotAllow);
        //    }
        //    await _userManager.SetLockoutEndDateAsync(user, DateTime.UtcNow.AddDays(5));
        //    return NoContent();
        //}

        //[HttpPut("unlock-member/{id}")]
        //public async Task<IActionResult> UnlockMember(string id)
        //{
        //    var user = await _userManager.FindByIdAsync(id);
        //    if (user == null) return NotFound();

        //    if (IsAdminUserId(id))
        //    {
        //        return BadRequest(SD.SuperAdminChangeNotAllow);
        //    }
        //    await _userManager.SetLockoutEndDateAsync(user, null);
        //    return NoContent();
        //}

        //[HttpDelete("delete-member/{id}")]
        //public async Task<IActionResult> DeleteMember(string id)
        //{
        //    var user = await _userManager.FindByIdAsync(id);
        //    if (user == null) return NotFound();

        //    if (IsAdminUserId(id))
        //    {
        //        return BadRequest(SD.SuperAdminChangeNotAllow);
        //    }
        //    await _userManager.DeleteAsync(user);
        //    return NoContent();
        //}

        //[HttpGet("get-application-roles")]
        //public async Task<ActionResult<string[]>> GetApplicationRoles()
        //{
        //    return Ok(await _roleManager.Roles.Select(x => x.Name).ToListAsync());
        //}

        //private bool IsAdminUserId(string userId)
        //{
        //    return _userManager.FindByIdAsync(userId).GetAwaiter().GetResult().UserName.Equals(SD.AdminUserName);
        //}

        public class CreateStoreDto
        {
            public string Subdomain { get; set; } = "";
        }

        public class CustomDomainDto
        {
            public string CustomDomain { get; set; } = "";
        }

    }
}
