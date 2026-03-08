using ITI_MVC_Project.Consts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ITI_MVC_Project.Data.EntitiesConfigurations
{
    public class RoleConfiguration : IEntityTypeConfiguration<IdentityRole>
    {
        public void Configure(EntityTypeBuilder<IdentityRole> builder)
        {
            builder.HasData(new IdentityRole
            {
                Id = DefaultRoles.AdminRoleId,
                Name = DefaultRoles.Admin,
                NormalizedName = DefaultRoles.Admin.ToUpper(),
                ConcurrencyStamp = DefaultRoles.AdminRoleConcurrencyStamp
            },
            new IdentityRole
            {
                Id = DefaultRoles.CustomerRoleId,
                Name = DefaultRoles.Customer,
                NormalizedName = DefaultRoles.Customer.ToUpper(),
                ConcurrencyStamp = DefaultRoles.CustomerRoleConcurrencyStamp
            });
        }
    }
}