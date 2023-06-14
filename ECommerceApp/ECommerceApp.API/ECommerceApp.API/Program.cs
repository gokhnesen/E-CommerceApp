
using ECommerceApp.Application.Errors;
using ECommerceApp.Application.Interfaces;
using ECommerceApp.Application.IProductRepository;
using ECommerceApp.Application.Middleware;
using ECommerceApp.Domain.Entities.Identity;
using ECommerceApp.Infrastructure.Extensions;
using ECommerceApp.Persistance.Context;
using ECommerceApp.Persistance.Repositories;
using ECommerceApp.Persistance.SeedData;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ECommerceApp.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddApplicationServices(builder.Configuration);
            builder.Services.AddIdentityServices(builder.Configuration);
            builder.Services.AddSwaggerDocumentation();
            var app = builder.Build();

            app.UseMiddleware<ExceptionMiddleware>();

            app.UseStatusCodePagesWithReExecute("/errors/{0}");
   
            app.UseCors("CorsPolicy");
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSwaggerDocumentation();

            app.MapControllers();

            // Migrate Database Automatically 

            using var scope = app.Services.CreateScope();
            var services = scope.ServiceProvider;
            var context = services.GetRequiredService<StoreContext>();
            var identityContext = services.GetRequiredService<AppIdentityDbContext>();
            var userManager = services.GetRequiredService<UserManager<AppUser>>();
            var logger = services.GetRequiredService<ILogger<Program>>();
            
            try
            {
                await context.Database.MigrateAsync();
                //await identityContext.Database.MigrateAsync();
                await AppDdInitialer.Seed(services);
                //AppIdentityDbContextSeed.SeedUsersAsync(userManager).GetAwaiter().GetResult();

            }
            catch (Exception ex)
            {

                logger.LogError(ex, "An error occured during migration");
            }

            app.Run();
        }
    }
}