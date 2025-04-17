using System.ComponentModel.DataAnnotations;

namespace Authentication.API.DTO
{
    public class UserUpdateDTO
    {
        [MaxLength(100)]
        public required string password { get; set; }
    }
}
