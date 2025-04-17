using System.ComponentModel.DataAnnotations;

namespace Authentication.API.DTO
{
    public class RoleDTO()
    {
        [Required]
        public required string roleName { get; set; }
    }
}
