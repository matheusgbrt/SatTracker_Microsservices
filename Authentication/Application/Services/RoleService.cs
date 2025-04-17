using Authentication.Application.Interfaces;
using Authentication.Domain.Exceptions;
using Authentication.Domain.Model;
using Authentication.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Authentication.Application.Services
{
    public class RoleService(AuthenticationDBContext dbContext) : IRoleService
    {
        private readonly AuthenticationDBContext _dbContext = dbContext;


        public Task<Role?> GetRoleByName(string roleName)
        {
            return _dbContext.Roles.Where(r => r.Name == roleName).FirstOrDefaultAsync();
        }

        public Task<Role?> GetRoleByNameWithIncludes(string roleName)
        {
            return _dbContext.Roles.Include(r => r.UserRoles).ThenInclude(usr => usr.User).Where(r => r.Name == roleName).FirstOrDefaultAsync();
        }

        public Task<List<Role>> GetAllRoles()
        {
            return _dbContext.Roles.ToListAsync();
        }

        public async Task CreateRole(string roleName)
        {
            Role? role = await GetRoleByName(roleName);
            if (role != null)
                throw new RoleAlreadyExistsException(roleName);

            role = Role.Create(roleName);
            _dbContext.Roles.Add(role);
            _dbContext.SaveChanges();

        }

        public async Task DeleteRole(string roleName)
        {
            Role? role = await GetRoleByNameWithIncludes(roleName) ?? throw new RoleNotFoundException(roleName);

            if (role.UserRoles.Count != 0)
                throw new InvalidOperationException($"Não é possível deletar a role {roleName} pois existem usuários atribuidos à ela.");

            _dbContext.Roles.Remove(role);
            _dbContext.SaveChanges();

        }
    }
}

