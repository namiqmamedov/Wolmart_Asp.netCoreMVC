using Microsoft.EntityFrameworkCore;
using Wolmart.Ecommerce.Models;

namespace Wolmart.Ecommerce.DAL
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            
        }

        public DbSet<SKU> SKUs { get; set; }
    }
}
