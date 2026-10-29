
using API_with_DOCKER.models;
using Microsoft.EntityFrameworkCore;

namespace API_with_DOCKER.DataBase
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options): base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Order>()
                .HasOne(order => order.Customer)
                .WithMany(customer => customer.Orders)
                .HasForeignKey(order => order.CustomerId).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<OrderItem>()
                .HasOne(item => item.Order)
                .WithMany(order => order.Items)
                .HasForeignKey(item => item.OrderId).OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<OrderItem>()
                .HasOne(item => item.Product)
                .WithMany()
                .HasForeignKey(item => item.ProductId).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Product>()
                .Property(product => product.Price);

            modelBuilder.Entity<Order>()
                .Property(order => order.Subtotal);

            modelBuilder.Entity<Order>()
                .Property(order => order.TotalCost);

            modelBuilder.Entity<OrderItem>()
                .Property(item => item.UnitPrice);

        }
    }
}
