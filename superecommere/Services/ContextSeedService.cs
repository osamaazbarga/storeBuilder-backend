using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using superecommere.Data;
using superecommere.Models.Domain;
using System.Security.Claims;

namespace superecommere.Services
{
    public class ContextSeedService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<TblUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        //private readonly ApplicationDbContext context;
        public ContextSeedService(ApplicationDbContext context,
            UserManager<TblUser> userManager,RoleManager<IdentityRole> roleManager)
        {
            this._context = context;
            this._userManager = userManager;
            this._roleManager = roleManager;
        }

        public async Task IntializeCintextAsync()
        {
            if (_context.Database.GetPendingMigrationsAsync().GetAwaiter().GetResult().Count() > 0)
            {
                //applies any pending migration into out datebase
                await _context.Database.MigrateAsync();
            }
            if (!_roleManager.Roles.Any())
            {
                await _roleManager.CreateAsync(new IdentityRole { Name = SD.AdminRole });
                await _roleManager.CreateAsync(new IdentityRole { Name = SD.ManagerRole });
                await _roleManager.CreateAsync(new IdentityRole { Name = SD.CustomerRole });

            }
            if (!_userManager.Users.AnyAsync().GetAwaiter().GetResult())
            {
                var admin = new TblUser
                {
                    FirstName = "admin",
                    LastName = "osama",
                    UserName = SD.AdminUserName,
                    Email = "admin@test.com",
                    EmailConfirmed = true
                };

                await _userManager.CreateAsync(admin,"123456");
                await _userManager.AddToRolesAsync(admin, new[] { SD.AdminRole, SD.ManagerRole, SD.CustomerRole });
                await _userManager.AddClaimsAsync(admin, new Claim[] 
                { 
                    new Claim(ClaimTypes.Email,admin.Email),
                    new Claim(ClaimTypes.Surname,admin.LastName)
                });


                var manger = new TblUser
                {
                    FirstName = "manager",
                    LastName = "osama2",
                    UserName = "manager@test.com",
                    Email = "manager@test.com",
                    EmailConfirmed = true
                };

                await _userManager.CreateAsync(manger, "123456");
                await _userManager.AddToRoleAsync(manger, SD.ManagerRole);
                await _userManager.AddClaimsAsync(manger, new Claim[]
                {
                    new Claim(ClaimTypes.Email,manger.Email),
                    new Claim(ClaimTypes.Surname,manger.LastName)
                });


                var customer = new TblUser
                {
                    FirstName = "customer",
                    LastName = "osama3",
                    UserName = "customer@test.com",
                    Email = "customer@test.com",
                    EmailConfirmed = true
                };

                await _userManager.CreateAsync(customer, "123456");
                await _userManager.AddToRoleAsync(customer, SD.CustomerRole);
                await _userManager.AddClaimsAsync(customer, new Claim[]
                {
                    new Claim(ClaimTypes.Email,customer.Email),
                    new Claim(ClaimTypes.Surname,customer.LastName)
                });

                var vipCustomer = new TblUser
                {
                    FirstName = "vipCustomer",
                    LastName = "osama4",
                    UserName = "vipcustomer@test.com",
                    Email = "vipcustomer@test.com",
                    EmailConfirmed = true
                };

                await _userManager.CreateAsync(vipCustomer, "123456");
                await _userManager.AddToRoleAsync(vipCustomer, SD.CustomerRole);
                await _userManager.AddClaimsAsync(vipCustomer, new Claim[]
                {
                    new Claim(ClaimTypes.Email,vipCustomer.Email),
                    new Claim(ClaimTypes.Surname,vipCustomer.LastName)
                });


            }
        }
    }
}
