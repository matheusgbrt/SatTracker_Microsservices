using Authentication.Application.Interfaces;
using Authentication.Domain.Exceptions;
using Authentication.Domain.Interfaces;
using Authentication.Domain.Model;
using Authentication.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Authentication.Application.Services
{
    public class UserService(AuthenticationDBContext dbContext, IPasswordService passwordService, IRoleService roleService) : IUserService
    {
        private readonly AuthenticationDBContext _dbContext = dbContext;
        private readonly IPasswordService _passwordService = passwordService;
        private readonly IRoleService _roleService = roleService;

        public Task<bool> UserExistsByUsername(string username)
        {
            return _dbContext.Users.AnyAsync(usr => usr.Name == username);
        }

        public Task<bool> UserExistsById(int id)
        {
            return _dbContext.Users.AnyAsync(usr => usr.Id == id);
        }

        public Task<User?> GetUserByUsername(string username)
        {
            return _dbContext.Users.Where(usr => usr.Name == username).Include(usr => usr.UserRoles).ThenInclude(usr => usr.Role).FirstOrDefaultAsync();
        }

        public Task<User?> GetUserById(int id)
        {
            return _dbContext.Users.Where(usr => usr.Id == id).Include(usr => usr.UserRoles).ThenInclude(usr => usr.Role).FirstOrDefaultAsync();
        }


        public async Task<bool> CreateUser(string username, string password)
        {
            User? user = await GetUserByUsername(username);
            if (user != null)
                throw new UserAlreadyExistsException(username);

            user = User.Create(username, password, _passwordService);

            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();
            return true;
        }


        public async Task DeleteUser(string username)
        {
            User? user = await GetUserByUsername(username) ?? throw new UserNotFoundException(username);

            _dbContext.Users.Remove(user);
            _dbContext.SaveChanges();
        }

        public async Task PatchUser(string username, string password)
        {
            User? user = await GetUserByUsername(username) ?? throw new UserNotFoundException(username);

            user.Update(password, _passwordService);

            _dbContext.Users.Update(user);
            _dbContext.SaveChanges();
        }

        public async Task DeleteUserRoles(string username)
        {
            await _dbContext.UserRoles.Where(ur => ur.User.Name == username).ExecuteDeleteAsync();
            _dbContext.SaveChanges();
        }

        public async Task AddUserRoles(string username, List<string> roleNames)
        {
            var user = await GetUserByUsername(username) ?? throw new UserNotFoundException(username);

            foreach (var roleName in roleNames)
            {
                var role = await _roleService.GetRoleByName(roleName) ?? throw new RoleNotFoundException(roleName);

                if (user.UserRoles.Count > 0)
                    throw new InvalidOperationException(message: $"O usuário {username} já possui roles.");


                user.UserRoles.Add(new UserRole { User = user, Role = role });
            }

            await _dbContext.SaveChangesAsync();
        }

        public async Task AddMissingUserRoles(string username, List<string> roleNames)
        {
            var user = await GetUserByUsername(username) ?? throw new UserNotFoundException(username);

            foreach (var roleName in roleNames)
            {
                var role = await _roleService.GetRoleByName(roleName) ?? throw new RoleNotFoundException(roleName);

                if (user.UserRoles.Select(ur => ur.Role).Contains(role))
                    throw new InvalidOperationException(message: $"O usuário {username} já possuiroles");


                user.UserRoles.Add(new UserRole { User = user, Role = role });
            }

            await _dbContext.SaveChangesAsync();
        }


    }
}
