using MagazynApp.Models;
using Microsoft.EntityFrameworkCore;

namespace MagazynApp.Data
{
    public class MagazynContext : DbContext
    {
        public MagazynContext(DbContextOptions<MagazynContext> options) : base(options)
        {

        }
        public DbSet<User> User { get; set; }
        public DbSet<Client> Client { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<ProductType> ProductType { get; set; }
        public DbSet<Order> Order { get; set; }
        public DbSet<OrderDetail> OrderDetail { get; set; }
        public DbSet<ShoppingCartItem> ShoppingCartItems { get; set; }
        public DbSet<ShoppingCart> ShoppingCart { get; set; }
        public DbSet<MissingOrderedProduct> MissingOrderedProduct { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().HasKey(x => x.Id);
            modelBuilder.Entity<User>().HasKey(x => x.Id);
            modelBuilder.Entity<ProductType>().HasKey(x => x.Id);
            modelBuilder.Entity<Enrollment>().HasKey(x => x.EnrollmentID);
            modelBuilder.Entity<Client>().HasKey(x => x.Id);
            modelBuilder.Entity<Order>().HasKey(x => x.Id);
            modelBuilder.Entity<OrderState>().HasKey(x => x.Id);
            modelBuilder.Entity<OrderDetail>().HasKey(x => x.Id);
            modelBuilder.Entity<MissingOrderedProduct>().HasKey(x => x.Id);



            modelBuilder.Entity<Product>().HasOne<ProductType>(pt => pt.Type)
                .WithMany(p => p.Product)
                .HasForeignKey(pt => pt.TypeId);

            modelBuilder.Entity<Order>().HasOne<OrderState>(os => os.State)
                .WithMany(o => o.Order)
                .HasForeignKey(os => os.StateId);

            modelBuilder.Entity<Order>().HasOne<Client>(oc => oc.Client)
                .WithMany(o => o.Order)
                .HasForeignKey(oc => oc.ClientId);

            modelBuilder.Entity<Order>().HasMany<OrderDetail>(od => od.OrderLines)
                .WithOne(o => o.Order)
                .HasForeignKey(od => od.OrderId);

            modelBuilder.Entity<OrderDetail>().HasOne<Product>(odp => odp.Product)
                .WithMany(o => o.OrderLines)
                .HasForeignKey(odp => odp.ProductId);

            modelBuilder.Entity<OrderDetail>().HasOne<Order>(odo => odo.Order)
                .WithMany(o => o.OrderLines)
                .HasForeignKey(odo => odo.OrderId);

            //modelBuilder.Entity<Order>().HasMany<MissingOrderedProduct>(om => om.MissingOrderedProducts)
            //    .WithOne(o => o.Order)
            //    .HasForeignKey(od => od.OrderId);

            //modelBuilder.Entity<MissingOrderedProduct>().HasOne<Product>(odp => odp.Product)
            //    .WithMany(o => o.MissingOrderedProducts)
            //    .HasForeignKey(odp => odp.ProductId);

            //modelBuilder.Entity<MissingOrderedProduct>().HasOne<Order>(odp => odp.Order)
            //    .WithMany(o => o.MissingOrderedProducts)
            //    .HasForeignKey(odp => odp.OrderId);

            //modelBuilder.Entity<OrderDetail>().HasOne<Order>(od => od.Order)
            //    .WithMany(o => o.OrderLines)
            //    .HasForeignKey(od => od.OrderId);


        }


    }
}
