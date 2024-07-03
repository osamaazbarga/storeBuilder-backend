using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using superecommere.Data;
using superecommere.Models.Domain;
using superecommere.Models.DTO.Store;
using superecommere.Models.Store;
using superecommere.Services;
using System.Reflection.Metadata;

namespace superecommere.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StoreController : ControllerBase
    {
        private readonly ApplicationDbContext _Context;
        private readonly JWTService _jwtService;
        private readonly SignInManager<TblUser> _signInManager;
        private readonly UserManager<TblUser> _userManager;
        private readonly EmailService _emailService;
        private readonly IConfiguration _config;





        public StoreController(JWTService jwtService,
            SignInManager<TblUser> signInManager,
            UserManager<TblUser> userManager,
            ApplicationDbContext Context,
            EmailService emailService,
            IConfiguration config)
        {

            _Context = Context;
            _jwtService = jwtService;
            _signInManager = signInManager;
            _userManager = userManager;
            _emailService = emailService;
            _config = config;
        }
        [HttpGet("get-stores")]
        public async Task<ActionResult<IEnumerable<StoreDetailsDto>>> GetStores()
        {
            List<StoreDetailsDto> stores = new List<StoreDetailsDto>();
            var storesData = await _Context.Stores.ToListAsync();
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

            var storeData = await _Context.Stores
               .Where(x => x.Id == id).FirstOrDefaultAsync();

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

            var storeData = await _Context.Stores
               .Where(x => x.Link == link).FirstOrDefaultAsync();
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

        [HttpPost("add-edit-store")]
        public async Task<IActionResult> AddEditStore(StoreAddEditDto model)
        {
            var getStore=await _Context.Stores.AnyAsync(u => u.Link == model.Link.ToLower());
            TblStore store;
            if (getStore)
            {
                return BadRequest($"An existing Store is using {model.Link},Link address. please try with another Link");
            }
            var user = await _Context.Users.FirstOrDefaultAsync(x => x.Id == model.UserId);
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
                    User = user,
                };
                _Context.Stores.Add(store);
                await _Context.SaveChangesAsync();
            return Ok(store);

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

    }
}
