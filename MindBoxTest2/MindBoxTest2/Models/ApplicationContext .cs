using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace MindBoxTest2.Models
{
    public class ProductDbContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }

        public ProductDbContext(DbContextOptions<ProductDbContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
