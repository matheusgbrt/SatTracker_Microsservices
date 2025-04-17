
using System.ComponentModel.DataAnnotations;

namespace Authentication.API.DTO
{
    public class AuthenticationRequestDTO
    {
        [MaxLength(100)]
        public required string login { get; set; }
        [MaxLength(100)]
        public required string password { get; set; }

    }
}
