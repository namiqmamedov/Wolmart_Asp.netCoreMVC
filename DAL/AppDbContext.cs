using Microsoft.EntityFrameworkCore;
using Wolmart.Ecommerce.Models;

namespace Wolmart.Ecommerce.DAL
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<Brand> Brands { get; set; }
    }
}
