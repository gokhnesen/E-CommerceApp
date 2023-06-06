using ECommerceApp.Domain.Entities;
using ECommerceApp.Domain.Entities.Identity;
using ECommerceApp.Persistance.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceApp.Persistance.SeedData
{
    public class AppDdInitialer
    {
        public static async Task Seed(IServiceProvider serviceProvider)
        {
            using(var context = new StoreContext(serviceProvider.GetRequiredService<DbContextOptions<StoreContext>>()))
            {
                if(context.Products.Any()) 
                {
                    return;
                }
                context.Products.AddRange(
                    new Product
                    {
                        Name="iPhone 14",
                        Description="Phone",
                        Price=20000,
                        ProductBrandId=1,
                        ProductTypeId=1,
                        PictureUrl= "https://store.storeimages.cdn-apple.com/4668/as-images.apple.com/is/iphone-14-pro-model-unselect-gallery-2-202209_GEO_EMEA?wid=5120&hei=2880&fmt=p-jpg&qlt=80&.v=1660753617539",
                    },
                    new Product
                    {
                        Name="Asus Rog Laptop",
                        Description="Gaming Laptop",
                        Price=15000,
                        ProductBrandId=2,
                        ProductTypeId=2,
                        PictureUrl= "https://dlcdnwebimgs.asus.com/gain/A2E2B660-EFDE-4C0A-8035-7DD101C62103",
                    }
                   
            );
                context.ProductBrands.AddRange(
                    new ProductBrand
                    {
                        Name="Apple"
                    },
                    new ProductBrand
                    {
                        Name="Asus"
                    }

                    
            );
                context.ProductTypes.AddRange(
                    new ProductType
                    {
                        Name="Phone",

                    },
                    new ProductType
                    {
                        Name="Laptop"
                    }
                    
            );
                context.SaveChanges();
            }

        }
    }
}
