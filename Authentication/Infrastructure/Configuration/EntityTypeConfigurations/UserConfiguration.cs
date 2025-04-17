using Authentication.Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace Authentication.Infrastructure.Configuration.EntityTypeConfigurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<User> builder)
        {
            builder.HasKey(u => u.Id);
            builder.Property(u => u.Id).ValueGeneratedOnAdd();
            builder.Property(u => u.Name).IsRequired().HasMaxLength(128);
            builder.Property(u => u.Password).IsRequired().HasMaxLength(500);
            builder.HasMany(e => e.UserRoles)
                .WithOne(ur => ur.User)
                .HasForeignKey(ur => ur.UserId);
        }
    }
}
