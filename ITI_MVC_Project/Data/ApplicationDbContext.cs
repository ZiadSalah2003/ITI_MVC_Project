using ITI_MVC_Project.Models.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ITI_MVC_Project.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<CartItem> CartItems { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Order>()
                .HasOne(o => o.User)
                .WithMany(u => u.Orders)
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<OrderItem>()
                .HasOne(oi => oi.Order)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<OrderItem>()
                .HasOne(oi => oi.Product)
                .WithMany(p => p.OrderItems)
                .HasForeignKey(oi => oi.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<CartItem>()
                .HasOne(ci => ci.User)
                .WithMany(u => u.CartItems)
                .HasForeignKey(ci => ci.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<CartItem>()
                .HasOne(ci => ci.Product)
                .WithMany()
                .HasForeignKey(ci => ci.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<CartItem>()
                .HasIndex(ci => new { ci.UserId, ci.ProductId })
                .IsUnique();

            builder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Electronics", Description = "Electronic devices and gadgets" },
                new Category { Id = 2, Name = "Clothing", Description = "Apparel and fashion items" },
                new Category { Id = 3, Name = "Books", Description = "Books and publications" },
                new Category { Id = 4, Name = "Home & Garden", Description = "Home and garden supplies" }
            );

            builder.Entity<Product>().HasData(
                new Product { Id = 1, Name = "Laptop", Description = "High-performance laptop", Price = 999.99m, Stock = 50, CategoryId = 1, ImageUrl = "/images/laptop.jpg" },
                new Product { Id = 2, Name = "Smartphone", Description = "Latest smartphone", Price = 699.99m, Stock = 100, CategoryId = 1, ImageUrl = "/images/phone.jpg" },
                new Product { Id = 3, Name = "T-Shirt", Description = "Cotton casual t-shirt", Price = 29.99m, Stock = 200, CategoryId = 2, ImageUrl = "/images/tshirt.jpg" },
                new Product { Id = 4, Name = "Jeans", Description = "Classic fit denim jeans", Price = 59.99m, Stock = 150, CategoryId = 2, ImageUrl = "/images/jeans.jpg" },
                new Product { Id = 5, Name = "C# in Depth", Description = "C# programming book", Price = 45.99m, Stock = 75, CategoryId = 3, ImageUrl = "/images/csharp.jpg" },
                new Product { Id = 6, Name = "Design Patterns", Description = "GoF design patterns", Price = 39.99m, Stock = 60, CategoryId = 3, ImageUrl = "/images/patterns.jpg" },
                new Product { Id = 7, Name = "Desk Lamp", Description = "LED desk lamp", Price = 34.99m, Stock = 80, CategoryId = 4, ImageUrl = "/images/lamp.jpg" },
                new Product { Id = 8, Name = "Plant Pot", Description = "Ceramic indoor pot", Price = 19.99m, Stock = 120, CategoryId = 4, ImageUrl = "/images/pot.jpg" }
            );
        }
    }
}