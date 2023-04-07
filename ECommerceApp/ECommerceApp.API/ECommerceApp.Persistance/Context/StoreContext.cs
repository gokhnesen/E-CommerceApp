using ECommerceApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceApp.Persistance.Context
{
    public class StoreContext : DbContext
    {
        public StoreContext(DbContextOptions options):base(options)
        {
            
        }

        public DbSet<Product> Products { get; set; }
    }
}
