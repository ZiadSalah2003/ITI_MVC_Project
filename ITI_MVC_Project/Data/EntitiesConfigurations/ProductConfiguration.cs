using ITI_MVC_Project.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ITI_MVC_Project.Data.EntitiesConfigurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(300);

            builder.Property(p => p.Description)
                .HasMaxLength(2000);

            builder.Property(p => p.Price)
                .HasColumnType("decimal(18,2)");

            builder.Property(p => p.ImageUrl)
                .HasMaxLength(500);

            builder.HasIndex(p => p.Name)
                .IsUnique();

            builder.HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasData(
                new Product { Id = 1, Name = "Laptop", Description = "High-performance laptop", Price = 999.99m, Stock = 50, CategoryId = 1, ImageUrl = "/images/laptop.jpg", IsActive = true },
                new Product { Id = 2, Name = "Smartphone", Description = "Latest smartphone", Price = 699.99m, Stock = 100, CategoryId = 1, ImageUrl = "/images/phone.jpg", IsActive = true },
                new Product { Id = 3, Name = "T-Shirt", Description = "Cotton casual t-shirt", Price = 29.99m, Stock = 200, CategoryId = 2, ImageUrl = "/images/tshirt.jpg", IsActive = true },
                new Product { Id = 4, Name = "Jeans", Description = "Classic fit denim jeans", Price = 59.99m, Stock = 150, CategoryId = 2, ImageUrl = "/images/jeans.jpg", IsActive = true },
                new Product { Id = 5, Name = "C# in Depth", Description = "C# programming book", Price = 45.99m, Stock = 75, CategoryId = 3, ImageUrl = "/images/csharp.jpg", IsActive = true },
                new Product { Id = 6, Name = "Design Patterns", Description = "GoF design patterns", Price = 39.99m, Stock = 60, CategoryId = 3, ImageUrl = "/images/patterns.jpg", IsActive = true },
                new Product { Id = 7, Name = "Desk Lamp", Description = "LED desk lamp", Price = 34.99m, Stock = 80, CategoryId = 4, ImageUrl = "/images/lamp.jpg", IsActive = true },
                new Product { Id = 8, Name = "Plant Pot", Description = "Ceramic indoor pot", Price = 19.99m, Stock = 120, CategoryId = 4, ImageUrl = "/images/pot.jpg", IsActive = true }
            );
        }
    }
}