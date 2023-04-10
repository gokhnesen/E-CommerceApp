
using ECommerceApp.Application.IProductRepository;
using ECommerceApp.Persistance.Context;
using ECommerceApp.Persistance.Repositories;
using ECommerceApp.Persistance.SeedData;
using Microsoft.EntityFrameworkCore;

namespace ECommerceApp.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddDbContext<StoreContext>(opt =>
            {
                opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            builder.Services.AddScoped<IProductRepository, ProductRepository>();
            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

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
                 StoreContextSeed.SeedAsync(context);
            }
            catch (Exception ex)
            {

                logger.LogError(ex, "An error occured during migration");
            }

            app.Run();
        }
    }
}