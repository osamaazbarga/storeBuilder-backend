using Amazon.S3;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using superecommere;
using superecommere.Configurations;
using superecommere.Data;
using superecommere.Errors;
using superecommere.Extensions;
using superecommere.Middleware;
using superecommere.Models.Domain;
using superecommere.Repositories.Implementation;
using superecommere.Repositories.Interface;
using superecommere.Services;
using System.Security.Claims;
using System.Text;
using Amazon.S3;
using Amazon.Extensions.NETCore.Setup;



var builder = WebApplication.CreateBuilder(args);

//builder.Services.Configure<JwtConfig>(builder.Configuration.GetSection(key:"JwtConfig"));


//Add services to the container.
builder.Services.AddControllers();

builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddDefaultAWSOptions(builder.Configuration.GetAWSOptions());
builder.Services.AddAWSService<IAmazonS3>();
var app = builder.Build();





//app.UseStatusCodePagesWithReExecute("/errors/{0}");
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI();
//}


app.UseHttpsRedirection();

app.UseCors("SuperEcommereOrigins");

app.UseAuthentication();

app.UseAuthorization();

//is going to look for index.html and serving out api application using index.html
app.UseDefaultFiles();

app.UseStaticFiles();

app.MapControllers();

app.MapFallbackToController("Index", "Fallback");

// Configure the HTTP request pipeline.
app.UseMiddleware<ExceptionMeiddleware>();

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



try
{

    using var scope2 = app.Services.CreateScope();
    var context = scope2.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    await context.Database.MigrateAsync();
    await SuperContextSeed.SeedAsync(context);
    


}
catch (Exception ex)
{
    var logger = scope.ServiceProvider.GetService<ILogger<Program>>();
#pragma warning disable CS8604 // Possible null reference argument.
    logger.LogError(ex.Message, "Failed to intialize and seed database");
#pragma warning restore CS8604 // Possible null reference argument.

}

app.Run();
