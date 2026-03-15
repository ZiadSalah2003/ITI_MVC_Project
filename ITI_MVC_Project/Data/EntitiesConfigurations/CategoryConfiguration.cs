using ITI_MVC_Project.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ITI_MVC_Project.Data.EntitiesConfigurations
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(c => c.Description)
                .HasMaxLength(1000);

            builder.HasIndex(c => c.Name)
                .IsUnique()
                .HasFilter("[IsDeleted] = 0");

            builder.HasData(
                new Category { Id = 1, Name = "Electronics", Description = "Electronic devices and gadgets", CreatedAt = new DateTime(2026, 3, 3, 1, 45, 26, 438, DateTimeKind.Utc).AddTicks(7004) },
                new Category { Id = 2, Name = "Clothing", Description = "Apparel and fashion items", CreatedAt = new DateTime(2026, 3, 3, 1, 45, 26, 438, DateTimeKind.Utc).AddTicks(7011) },
                new Category { Id = 3, Name = "Books", Description = "Books and publications", CreatedAt = new DateTime(2026, 3, 3, 1, 45, 26, 438, DateTimeKind.Utc).AddTicks(7012) },
                new Category { Id = 4, Name = "Home & Garden", Description = "Home and garden supplies", CreatedAt = new DateTime(2026, 3, 3, 1, 45, 26, 438, DateTimeKind.Utc).AddTicks(7013) }
            );
        }
    }
}