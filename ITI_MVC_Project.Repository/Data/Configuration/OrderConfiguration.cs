using ITI_MVC_Project.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITI_MVC_Project.Repository.Data.Configuration
{
	public class OrderConfiguration : IEntityTypeConfiguration<Order>
	{
		public void Configure(EntityTypeBuilder<Order> builder)
		{
			builder.HasKey(o => o.OrderId);

			builder.HasIndex(o => o.OrderNumber)
				.IsUnique();

			builder.Property(o => o.TotalAmount)
				.HasColumnType("decimal(18,2)");

			builder.HasOne(o => o.User)
				.WithMany(u => u.Orders)
				.HasForeignKey(o => o.UserId)
				.OnDelete(DeleteBehavior.Restrict);

			builder.HasOne(o => o.ShippingAddress)
				.WithMany(a => a.Orders)
				.HasForeignKey(o => o.ShippingAddressId)
				.OnDelete(DeleteBehavior.Restrict);
		}
	}
}
