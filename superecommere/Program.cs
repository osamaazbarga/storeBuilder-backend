using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using superecommere;
using superecommere.Configurations;
using superecommere.Data;
using superecommere.Models.Domain;
using superecommere.Repositories.Implementation;
using superecommere.Repositories.Interface;
using superecommere.Services;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<JwtConfig>(builder.Configuration.GetSection(key:"JwtConfig"));


// Add services to the container.
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
});
builder.Services.AddHttpClient();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<JWTService>();
builder.Services.AddScoped<EmailService>();
builder.Services.AddScoped<ContextSeedService>();



builder.Services.AddIdentityCore<TblUser>(options =>
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

builder.Services.AddDefaultIdentity<TblUser>(configureOptions: Options => Options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddAuthentication(Options =>
{
    Options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    Options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    Options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

}).AddJwtBearer(jwt =>
{
#pragma warning disable CS8604 // Possible null reference argument.
    byte[] key = Encoding.ASCII.GetBytes(builder.Configuration.GetSection(key: "JwtConfig:secret").Value);
#pragma warning restore CS8604 // Possible null reference argument.

    jwt.SaveToken = true;
    jwt.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidIssuer = builder.Configuration["JwtConfig:Issuer"],
        ValidateIssuer = true,
        ValidateAudience = false,
        RequireExpirationTime = false,
        ValidateLifetime = false

    };

});

builder.Services.AddCors(options => options.AddPolicy(name: "SuperEcommereOrigins",
    policy =>
    {
        policy.WithOrigins("https://localhost:4200").AllowAnyMethod().AllowAnyHeader();
    }));

builder.Services.Configure<ApiBehaviorOptions>(options =>
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

builder.Services.AddAuthorization(opt =>
{
    opt.AddPolicy("AdminPolicy", policy => policy.RequireRole("Admin"));
    opt.AddPolicy("ManagerPolicy", policy => policy.RequireRole("Manager"));
    opt.AddPolicy("CustomerPolicy", policy => policy.RequireRole("Customer"));

    opt.AddPolicy("AdminOrMangerPolicy", policy => policy.RequireRole("Admin", "Manager"));
    opt.AddPolicy("AdminAndMangerPolicy", policy => policy.RequireRole("Admin").RequireRole("Manager"));
    opt.AddPolicy("AllRolesPolicy", policy => policy.RequireRole("Admin", "Manager","Customer"));

    opt.AddPolicy("AdminEmailPolicy", policy => policy.RequireClaim(ClaimTypes.Email, "admin@test.com"));
    opt.AddPolicy("ManagerEmailAndOsama2SurnamePolicy", policy => policy.RequireClaim(ClaimTypes.Surname, "osama2").RequireClaim(ClaimTypes.Email, "manager@test.com"));

    opt.AddPolicy("osama3Policy", policy => policy.RequireClaim(ClaimTypes.Surname, "osama3"));
    opt.AddPolicy("VIPPolicy", policy => policy.RequireAssertion(context => SD.VIPPolicy(context)));
    //opt.AddPolicy("AdminEmailPolicy", policy => policy.RequireClaim(ClaimTypes.Email, "Admin@test.com"));




});

builder.Services.AddScoped<ITblUserRepository, TblUserRepository>();

var app = builder.Build();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("SuperEcommereOrigins");

app.UseAuthentication();

app.UseAuthorization();

//is going to look for index.html and serving out api application using index.html
app.UseDefaultFiles();

app.UseStaticFiles();

app.MapControllers();

app.MapFallbackToController("Index", "Fallback");

app.MapControllers();

#region ContextSeed
using var scope = app.Services.CreateScope();
try
{
    var contextSeedService = scope.ServiceProvider.GetService<ContextSeedService>();
    await contextSeedService.IntializeCintextAsync();

} 
catch(Exception ex)
{
    var logger=scope.ServiceProvider.GetService<ILogger<Program>>();
#pragma warning disable CS8604 // Possible null reference argument.
    logger.LogError(ex.Message,"Failed to intialize and seed database");
#pragma warning restore CS8604 // Possible null reference argument.

}
#endregion

app.Run();
