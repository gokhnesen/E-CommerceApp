
using ECommerceApp.Application.Errors;
using ECommerceApp.Application.Interfaces;
using ECommerceApp.Application.IProductRepository;
using ECommerceApp.Application.Middleware;
using ECommerceApp.Infrastructure.Extensions;
using ECommerceApp.Persistance.Context;
using ECommerceApp.Persistance.Repositories;
using ECommerceApp.Persistance.SeedData;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ECommerceApp.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddApplicationServices(builder.Configuration);
           
            var app = builder.Build();

            app.UseMiddleware<ExceptionMiddleware>();

            app.UseStatusCodePagesWithReExecute("/errors/{0}");
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseCors("CorsPolicy");
            app.UseAuthorization();


            app.MapControllers();

            // Migrate Database Automatically

            using var scope = app.Services.CreateScope();
            var services = scope.ServiceProvider;
            var context = services.GetRequiredService<StoreContext>();
            var logger = services.GetRequiredService<ILogger<Program>>();
            try
            {
                 context.Database.MigrateAsync();
                 AppDdInitialer.Seed(services);
            }
            catch (Exception ex)
            {

                logger.LogError(ex, "An error occured during migration");
            }

            app.Run();
        }
    }
}