using Authentication.API.DTO;
using Authentication.Domain.Interfaces;

namespace Authentication.Domain.Model
{
    public class User
    {
        protected User() { }
        public User(string username, string password)
        {
            this.Name = username;
            this.Password = password;
        }

        public int Id { get; private set; }
        public string Name { get; private set; }
        public string Password { get; private set; }
        public ICollection<UserRole> UserRoles { get; private set; }


        public static User Create(string username, string rawPassword, IPasswordService passwordService)
        {
            var hashedPassword = passwordService.HashPassword(rawPassword);
            return new User(username, hashedPassword);
        }

        public bool Authenticate(string rawPassword, IPasswordService passwordService)
        {
            return passwordService.VerifyPassword(rawPassword, Password);
        }

        public User Update(string rawPassword, IPasswordService passwordService)
        {
            this.Password = passwordService.HashPassword(rawPassword);
            return this;
        }

        public UserDTO ConvertToDTO()
        {
            UserDTO userDTO = new()
            { username = this.Name, roles = this.UserRoles.Select(x => x.Role.Name).ToList() };

            return userDTO;
        }

    }
}
