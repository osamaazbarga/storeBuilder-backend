using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using superecommere.Configurations;
using superecommere.Data;
using superecommere.Errors;
using superecommere.Models.Domain;
using superecommere.Repositories.Implementation;
using superecommere.Repositories.Interface;
using superecommere.Services;
using System.Security.Claims;
using System.Text;

namespace superecommere.Extensions
{
    public static class ApplicationServicesExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services,IConfiguration config)
        {

            services.Configure<JwtConfig>(config.GetSection(key: "JwtConfig"));


           

            services.AddHttpClient();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(config.GetConnectionString("DefaultConnection")));
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<JWTService>();
            services.AddScoped<EmailService>();
            services.AddScoped<ContextSeedService>();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = actionContext =>
                {
                    var errors = actionContext.ModelState
                        .Where(e => e.Value.Errors.Count > 0)
                        .SelectMany(x => x.Value.Errors)
                        .Select(x => x.ErrorMessage).ToArray();
                    var errorResponse = new ApiValidationErrorResponce
                    {
                        Errors = errors
                    };
                    return new BadRequestObjectResult(errorResponse);
                };
            });


            services.AddIdentityCore<TblUser>(options =>
            {
                options.Password.RequiredLength = 6;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.User.AllowedUserNameCharacters =
                            "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+ ";

                options.SignIn.RequireConfirmedEmail = true;

            })
            .AddRoles<IdentityRole>()
            .AddRoleManager<RoleManager<IdentityRole>>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddSignInManager<SignInManager<TblUser>>()
            .AddUserManager<UserManager<TblUser>>()
            .AddDefaultTokenProviders();

            services.AddDefaultIdentity<TblUser>(configureOptions: Options => Options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<ApplicationDbContext>();
            services.AddAuthentication(Options =>
            {
                Options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                Options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                Options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(jwt =>
            {
#pragma warning disable CS8604 // Possible null reference argument.
                byte[] key = Encoding.ASCII.GetBytes(config.GetSection(key: "JwtConfig:secret").Value);
#pragma warning restore CS8604 // Possible null reference argument.

                jwt.SaveToken = true;
                jwt.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidIssuer = config["JwtConfig:Issuer"],
                    ValidateIssuer = true,
                    ValidateAudience = false,
                    RequireExpirationTime = false,
                    ValidateLifetime = false

                };

            });

            services.AddCors(options => options.AddPolicy(name: "SuperEcommereOrigins",
                policy =>
                {
                    policy.WithOrigins("http://localhost:4200","https://localhost:4200").AllowAnyMethod().AllowAnyHeader();
                }));

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = ActionContext =>
                {
                    var errors = ActionContext.ModelState
                    .Where(x => x.Value.Errors.Count > 0)
                    .SelectMany(x => x.Value.Errors)
                    .Select(x => x.ErrorMessage).ToArray();
                    var toReturn = new
                    {
                        Errors = errors
                    };
                    return new BadRequestObjectResult(toReturn);
                };
            });

            services.AddAuthorization(opt =>
            {
                opt.AddPolicy("AdminPolicy", policy => policy.RequireRole("Admin"));
                opt.AddPolicy("ManagerPolicy", policy => policy.RequireRole("Manager"));
                opt.AddPolicy("CustomerPolicy", policy => policy.RequireRole("Customer"));

                opt.AddPolicy("AdminOrMangerPolicy", policy => policy.RequireRole("Admin", "Manager"));
                opt.AddPolicy("AdminAndMangerPolicy", policy => policy.RequireRole("Admin").RequireRole("Manager"));
                opt.AddPolicy("AllRolesPolicy", policy => policy.RequireRole("Admin", "Manager", "Customer"));

                opt.AddPolicy("AdminEmailPolicy", policy => policy.RequireClaim(ClaimTypes.Email, "admin@test.com"));
                opt.AddPolicy("ManagerEmailAndOsama2SurnamePolicy", policy => policy.RequireClaim(ClaimTypes.Surname, "osama2").RequireClaim(ClaimTypes.Email, "manager@test.com"));

                opt.AddPolicy("osama3Policy", policy => policy.RequireClaim(ClaimTypes.Surname, "osama3"));
                opt.AddPolicy("VIPPolicy", policy => policy.RequireAssertion(context => SD.VIPPolicy(context)));
                //opt.AddPolicy("AdminEmailPolicy", policy => policy.RequireClaim(ClaimTypes.Email, "Admin@test.com"));




            });

            services.AddScoped<ITblUserRepository, TblUserRepository>();

            return services;
        }
    }
}
