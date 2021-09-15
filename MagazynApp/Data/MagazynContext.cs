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
        public DbSet<ProductType> ProductType { get; set; }
        public DbSet<Order> Order { get; set; }
        public DbSet<OrderDetail> OrderDetail { get; set; }
        public DbSet<OrderState> OrderState { get; set; }
        public DbSet<ShoppingCartItem> ShoppingCartItems { get; set; }
        public DbSet<ShoppingCart> ShoppingCart { get; set; }
        public DbSet<MissingOrderedProduct> MissingOrderedProduct { get; set; }
        public DbSet<Supply> Supply { get; set; }
        public DbSet<SupplyState> SupplyState { get; set; }
        public DbSet<Socket> Socket { get; set; }
        public DbSet<Shelf> Shelf { get; set; }
        public DbSet<Alley> Alley { get; set; }
        public DbSet<Sector> Sector { get; set; }
        public DbSet<SocketProduct> SocketProduct { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().HasKey(x => x.Id);
            modelBuilder.Entity<ProductType>().HasKey(x => x.Id);


            modelBuilder.Entity<Client>().HasKey(x => x.Id);

            modelBuilder.Entity<Order>().HasKey(x => x.Id);
            modelBuilder.Entity<OrderState>().HasKey(x => x.Id);
            modelBuilder.Entity<OrderDetail>().HasKey(x => x.Id);

            modelBuilder.Entity<MissingOrderedProduct>().HasKey(x => x.Id);

            modelBuilder.Entity<Supply>().HasKey(x => x.Id);
            modelBuilder.Entity<SupplyState>().HasKey(x => x.Id);

            modelBuilder.Entity<ShoppingCartItem>().HasKey(x => x.ShoppingCartItemId);

            modelBuilder.Entity<Socket>().HasKey(x => x.Id);
            modelBuilder.Entity<SocketProduct>().HasKey(x => x.Id);
            //modelBuilder.Entity<SocketProduct>().HasKey(sp => new {sp.SocketId, sp.ProductId});
            
            modelBuilder.Entity<SocketProduct>().HasOne<Socket>(sp => sp.Socket)
                .WithMany(s => s.SocketProduct)
                .HasForeignKey(sp => sp.SocketId);
            
            modelBuilder.Entity<SocketProduct>().HasOne<Product>(sp => sp.Product)
                .WithMany(s => s.SocketProduct)
                .HasForeignKey(sp => sp.ProductId);

            modelBuilder.Entity<Shelf>().HasKey(x => x.Id);

            modelBuilder.Entity<Alley>().HasKey(x => x.Id);

            modelBuilder.Entity<Sector>().HasKey(x => x.Id);

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


            modelBuilder.Entity<OrderDetail>().HasOne<Product>(odp => odp.Product)
                .WithMany(o => o.OrderLines)
                .HasForeignKey(odp => odp.ProductId);

            modelBuilder.Entity<OrderDetail>().HasOne<Order>(odo => odo.Order)
                .WithMany(o => o.OrderLines)
                .HasForeignKey(odo => odo.OrderId);


            modelBuilder.Entity<Supply>().HasOne<SupplyState>(ss => ss.State)
                .WithMany(s => s.Supply)
                .HasForeignKey(ss => ss.StateId);


            modelBuilder.Entity<ShoppingCart>().HasMany<ShoppingCartItem>(sdp => sdp.ShoppingCartItems)
                .WithOne(s => s.ShoppingCart)
                .HasForeignKey(sdp => sdp.ShoppingCartId);


            modelBuilder.Entity<MissingOrderedProduct>().HasOne<Product>(odp => odp.Product)
                .WithMany(o => o.MissingOrderedProducts)
                .HasForeignKey(odp => odp.ProductId);

            modelBuilder.Entity<MissingOrderedProduct>().HasOne<Order>(odp => odp.Order)
                .WithMany(o => o.MissingOrderedProducts)
                .HasForeignKey(odp => odp.OrderId);

            modelBuilder.Entity<Socket>().HasOne<Shelf>(s => s.Shelf)
                .WithMany(s => s.Sockets)
                .HasForeignKey(s => s.ShelfId);

            modelBuilder.Entity<Shelf>().HasOne<Alley>(s => s.Alley)
                .WithMany(s => s.Shelves)
                .HasForeignKey(s => s.AlleyId);

            modelBuilder.Entity<Alley>().HasOne<Sector>(s => s.Sector)
                .WithMany(s => s.Alleys)
                .HasForeignKey(s => s.SectorId);
            
        }
    }
}