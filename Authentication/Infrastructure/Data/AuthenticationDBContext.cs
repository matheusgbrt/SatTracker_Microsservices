using Authentication.Domain.Model;
using Authentication.Infrastructure.Configuration.EntityTypeConfigurations;
using Microsoft.EntityFrameworkCore;

namespace Authentication.Infrastructure.Data
{
    public class AuthenticationDBContext(DbContextOptions<AuthenticationDBContext> options) : DbContext(options)
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //Call the configuration methods for all entities.
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new RoleConfiguration());
            modelBuilder.ApplyConfiguration(new UserRoleconfiguration());

        }
    }
}
