using ECommerceApp.Domain.Entities.Identity;
using ECommerceApp.Persistance.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceApp.Persistance.SeedData
{
    public class AppIdentityDbContextSeed
    {
        public static async Task SeedUsersAsync(UserManager<AppUser> userManager)
        {
            var testUser = new AppUser
            {
                DisplayName = "Gökhan",
                Address = new Address
                {
                    FirstName = "Gökhan",
                    LastName = "Esen",
                    City = "İstanbul",
                    Street = "Filiz",
                    ZipCode = "12312",
                }
            };

            await userManager.CreateAsync(testUser,"gizlisifre");

        }
    }
}
