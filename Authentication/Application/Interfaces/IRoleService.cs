using Authentication.Domain.Model;

namespace Authentication.Application.Interfaces
{
    public interface IRoleService
    {
        Task CreateRole(string roleName);
        Task DeleteRole(string roleName);
        Task<Role?> GetRoleByName(string roleName);

        Task<List<Role>> GetAllRoles();
        Task<Role?> GetRoleByNameWithIncludes(string roleName);
    }
}
