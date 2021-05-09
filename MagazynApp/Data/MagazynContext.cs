using MagazynApp.Models;
using Microsoft.EntityFrameworkCore;

namespace MagazynApp.Data
{
    public class MagazynContext : DbContext
    {
        public MagazynContext(DbContextOptions<MagazynContext> options) : base(options)
        {

        }
        public DbSet<User> User{ get; set; }
        public DbSet<Product> Product { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().HasKey(x => x.Id);
         //   modelBuilder.Entity<Product>().ToTable("Product");

            base.OnModelCreating(modelBuilder);
           // modelBuilder.Entity<User>().ToTable("User");
          
        }
    }
}
