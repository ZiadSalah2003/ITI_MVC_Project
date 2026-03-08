using ITI_MVC_Project.Consts;
using ITI_MVC_Project.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ITI_MVC_Project.Data.EntitiesConfigurations
{
    public class UserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.Property(u => u.FirstName).HasMaxLength(100);
            builder.Property(u => u.LastName).HasMaxLength(100);
            builder.Property(u => u.Address).HasMaxLength(500);
            builder.Property(u => u.City).HasMaxLength(100);

            var passwordHasher = new PasswordHasher<ApplicationUser>();

            builder.HasData(new ApplicationUser
            {
                Id = DefaultUsers.AdminId,
                FirstName = "Admin",
                LastName = "User",
                UserName = DefaultUsers.AdminEmail,
                NormalizedUserName = DefaultUsers.AdminEmail.ToUpper(),
                Email = DefaultUsers.AdminEmail,
                NormalizedEmail = DefaultUsers.AdminEmail.ToUpper(),
                EmailConfirmed = true,
                SecurityStamp = DefaultUsers.AdminSecurityStamp,
                ConcurrencyStamp = DefaultUsers.AdminConcurrencyStamp,
                Address = "Cairo, Egypt",
                City = "Cairo",
                PasswordHash = passwordHasher.HashPassword(null!, DefaultUsers.AdminPassword)
            });
        }
    }
}