using ITI_MVC_Project.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ITI_MVC_Project.Data.EntitiesConfigurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.Property(o => o.TotalAmount)
                .HasColumnType("decimal(18,2)");

            builder.Property(o => o.ShippingAddress)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(o => o.City)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(o => o.PhoneNumber)
                .HasMaxLength(20);

            builder.HasOne(o => o.User)
                .WithMany(u => u.Orders)
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}