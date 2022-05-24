using Exchange_API.Model;
using Microsoft.EntityFrameworkCore;

namespace Exchange_API.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<User> User { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<ProductImage> ProductImage { get; set; }
        public DbSet<Comment> Comment { get; set; }
        public DbSet<Fav> Fav { get; set; }
    }
}
