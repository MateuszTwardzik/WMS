using MagazynApp.Models;
using Microsoft.EntityFrameworkCore;

namespace MagazynApp.Data
{
    public class MagazynContext : DbContext
    {
        public MagazynContext(DbContextOptions<MagazynContext> options) : base(options)
        {

        }
        public DbSet<Client> Client { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<ProductType> ProductType { get; set; }
        public DbSet<Order> Order { get; set; }
        public DbSet<OrderDetail> OrderDetail { get; set; }
        public DbSet<OrderState> OrderState { get; set; }
        public DbSet<ShoppingCartItem> ShoppingCartItems { get; set; }
        public DbSet<ShoppingCart> ShoppingCart { get; set; }
        public DbSet<MissingOrderedProduct> MissingOrderedProduct { get; set; }
        public DbSet<Supply> Supply { get; set; }
        public DbSet<SupplyDetail> SupplyDetail { get; set; }
        public DbSet<SupplyState> SupplyState { get; set; }
        public DbSet<StockForRelease> StockForRelease { get; set; }
        public DbSet<StockForReleaseState> StockForReleaseState { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().HasKey(x => x.Id);
            modelBuilder.Entity<ProductType>().HasKey(x => x.Id);

            modelBuilder.Entity<Enrollment>().HasKey(x => x.EnrollmentID);

            modelBuilder.Entity<Client>().HasKey(x => x.Id);

            modelBuilder.Entity<Order>().HasKey(x => x.Id);
            modelBuilder.Entity<OrderState>().HasKey(x => x.Id);
            modelBuilder.Entity<OrderDetail>().HasKey(x => x.Id);

            modelBuilder.Entity<MissingOrderedProduct>().HasKey(x => x.Id);

            modelBuilder.Entity<Supply>().HasKey(x => x.Id);
            modelBuilder.Entity<SupplyDetail>().HasKey(x => x.Id);
            modelBuilder.Entity<SupplyState>().HasKey(x => x.Id);

            modelBuilder.Entity<ShoppingCartItem>().HasKey(x => x.ShoppingCartItemId);

            modelBuilder.Entity<StockForRelease>().HasKey(x => x.Id);
            modelBuilder.Entity<StockForReleaseState>().HasKey(x => x.Id);


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

            modelBuilder.Entity<Order>().HasMany<MissingOrderedProduct>(om => om.MissingOrderedProducts)
                  .WithOne(o => o.Order)
                  .HasForeignKey(od => od.OrderId);

            modelBuilder.Entity<Order>().HasMany<StockForRelease>(om => om.StockForRelease)
                   .WithOne(o => o.Order)
                   .HasForeignKey(od => od.OrderId);




            modelBuilder.Entity<OrderDetail>().HasOne<Product>(odp => odp.Product)
                .WithMany(o => o.OrderLines)
                .HasForeignKey(odp => odp.ProductId);

            modelBuilder.Entity<OrderDetail>().HasOne<Order>(odo => odo.Order)
                .WithMany(o => o.OrderLines)
                .HasForeignKey(odo => odo.OrderId);





            modelBuilder.Entity<Supply>().HasMany<SupplyDetail>(sd => sd.SupplyDetails)
                .WithOne(s => s.Supply)
                .HasForeignKey(sd => sd.SupplyId);

            modelBuilder.Entity<Supply>().HasOne<SupplyState>(ss => ss.State)
                .WithMany(s => s.Supply)
                .HasForeignKey(ss => ss.StateId);



            modelBuilder.Entity<SupplyDetail>().HasOne<Product>(sdp => sdp.Product)
                .WithMany(s => s.SupplyDetails)
                .HasForeignKey(sdp => sdp.ProductId);




            modelBuilder.Entity<ShoppingCart>().HasMany<ShoppingCartItem>(sdp => sdp.ShoppingCartItems)
                .WithOne(s => s.ShoppingCart)
                .HasForeignKey(sdp => sdp.ShoppingCartId);





            modelBuilder.Entity<MissingOrderedProduct>().HasOne<Product>(odp => odp.Product)
                .WithMany(o => o.MissingOrderedProducts)
                .HasForeignKey(odp => odp.ProductId);

            modelBuilder.Entity<MissingOrderedProduct>().HasOne<Order>(odp => odp.Order)
                .WithMany(o => o.MissingOrderedProducts)
                .HasForeignKey(odp => odp.OrderId);



            modelBuilder.Entity<StockForRelease>().HasOne<StockForReleaseState>(os => os.State)
                .WithMany(o => o.StockForRelease)
                .HasForeignKey(os => os.StateId);

            modelBuilder.Entity<StockForRelease>().HasOne<Product>(od => od.Product)
                .WithMany(o => o.StockForRelease)
                .HasForeignKey(od => od.ProductId);

            modelBuilder.Entity<StockForRelease>().HasOne<Order>(od => od.Order)
                .WithMany(o => o.StockForRelease)
                .HasForeignKey(od => od.OrderId);
            //modelBuilder.Entity<OrderDetail>().HasOne<Order>(od => od.Order)
            //    .WithMany(o => o.OrderLines)
            //    .HasForeignKey(od => od.OrderId);


        }


    }
}
