using Authentication.Domain.Interfaces;
using BC = BCrypt.Net.BCrypt;


namespace Authentication.Domain.Services
{
    public class PasswordService : IPasswordService
    {

        public string HashPassword(string password)
        {
            return BC.HashPassword(password);
        }

        public bool VerifyPassword(string password, string hashedPassword)
        {
            return BC.Verify(password, hashedPassword);
        }
    }
}
