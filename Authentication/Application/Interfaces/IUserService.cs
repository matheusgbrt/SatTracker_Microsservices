using Authentication.Domain.Model;

namespace Authentication.Application.Interfaces
{
    public interface IUserService
    {
        Task<bool> UserExistsByUsername(string username);
        Task<bool> UserExistsById(int id);
        Task<User?> GetUserByUsername(string username);
        Task<User?> GetUserById(int id);
        Task<bool> CreateUser(string username, string password);
        Task DeleteUser(string username);
        Task AddUserRoles(string username, List<string> roleNames);
        Task AddMissingUserRoles(string username, List<string> roleNames);
        Task DeleteUserRoles(string username);
        Task PatchUser(string username, string password);
    }
}
