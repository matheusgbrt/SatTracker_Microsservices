using Authentication.Application.Interfaces;
using Authentication.Domain.Exceptions;
using Authentication.Domain.Interfaces;
using Authentication.Domain.Model;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Authentication.Application.Services
{
    public class AuthenticationService(IUserService userService, IPasswordService passwordService) : IAuthenticationService
    {
        IUserService _userService = userService;
        IPasswordService _passwordService = passwordService;

        public async Task<User> Authenticate(string username, string password)
        {
            User? user = await _userService.GetUserByUsername(username) ?? throw new UserNotFoundException(username);
            if (user.Authenticate(password, _passwordService))
            {
                return user;
            }
            else
            {
                throw new UnauthorizedAccessException("Credenciais de autenticação não são válidas.");
            }
        }

        public string GenerateJWT(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub,user.Name),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
            };
            claims.AddRange(user.UserRoles.Select(ur => new Claim(ClaimTypes.Role, ur.Role.Name)));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("JWT_TOKEN_SECRET_KEY") ?? ""));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expiryDate = DateTime.UtcNow.AddHours(2);

            var token = new JwtSecurityToken(
                issuer: Environment.GetEnvironmentVariable("JWT_TOKEN_ISSUER"),
                audience: Environment.GetEnvironmentVariable("JWT_TOKEN_AUDIENCE"),
                claims: claims,
                expires: expiryDate,
                signingCredentials: creds
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
