using Authentication.Domain.Model;

namespace Authentication.Application.Interfaces
{
    public interface IAuthenticationService
    {
        Task<User> Authenticate(string username, string password);
        string GenerateJWT(User user);
    }
}
