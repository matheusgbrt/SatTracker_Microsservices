using Authentication.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Authentication.Infrastructure.Configuration.EntityTypeConfigurations
{
    public class UserRoleconfiguration : IEntityTypeConfiguration<UserRole>
    {
        public void Configure(EntityTypeBuilder<UserRole> builder)
        {
            builder.HasKey(ur => new { ur.RoleId, ur.UserId });
            builder.HasOne(ur => ur.User).WithMany(ur => ur.UserRoles).HasForeignKey(ur => ur.UserId);
            builder.HasOne(ur => ur.Role).WithMany(ur => ur.UserRoles).HasForeignKey(ur => ur.RoleId);
        }
    }
}
