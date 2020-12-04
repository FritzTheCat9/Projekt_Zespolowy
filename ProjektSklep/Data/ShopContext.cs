using Microsoft.EntityFrameworkCore;
using ProjektSklep.Models;

namespace ProjektSklep.Data
{
    public class ShopContext : DbContext
    {
        public ShopContext()
        {
        }

        public ShopContext(DbContextOptions<ShopContext> options) : base(options)
        {

        }

        public DbSet<DiscountCode> DiscountCodes { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<PageConfiguration> PageConfigurations { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<ShippingMethod> ShippingMethods { get; set; }
        public DbSet<PaymentMethod> PaymentMethods { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Attachment> Attachments { get; set; }
        public DbSet<Expert> Experts { get; set; }
        public DbSet<ProductOrder> ProductOrders { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=NowaBazaDanychSklepu;Trusted_Connection=True;");

            base.OnConfiguring(optionsBuilder);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DiscountCode>().ToTable("DiscountCode");
            modelBuilder.Entity<Address>().ToTable("Address");
            modelBuilder.Entity<Customer>().ToTable("Customer");
            modelBuilder.Entity<PageConfiguration>().ToTable("PageConfiguration");
            modelBuilder.Entity<Order>().ToTable("Order");
            modelBuilder.Entity<ShippingMethod>().ToTable("ShippingMethod");
            modelBuilder.Entity<PaymentMethod>().ToTable("PaymentMethod");
            modelBuilder.Entity<Product>().ToTable("Product");
            modelBuilder.Entity<Category>().ToTable("Category");
            modelBuilder.Entity<Attachment>().ToTable("Attachment");
            modelBuilder.Entity<Expert>().ToTable("Expert");
            modelBuilder.Entity<ProductOrder>().ToTable("ProductOrder");
        }
    }
}
