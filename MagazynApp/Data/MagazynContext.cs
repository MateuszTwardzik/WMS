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
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<ProductType> ProductType { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().HasKey(x => x.Id);
            modelBuilder.Entity<User>().HasKey(x => x.Id);
            modelBuilder.Entity<ProductType>().HasKey(x => x.Id);
            modelBuilder.Entity<Enrollment>().HasKey(x => x.EnrollmentID);



            modelBuilder.Entity<Product>().HasOne<ProductType>(pt => pt.Type)
                .WithMany(p => p.Product)
                .HasForeignKey(pt => pt.TypeId);

            base.OnModelCreating(modelBuilder);
          
        }
    }
}
