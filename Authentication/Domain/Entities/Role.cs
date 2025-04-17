using Authentication.API.DTO;

namespace Authentication.Domain.Model
{
    public class Role
    {
        public Role(string name)
        {
            this.Name = name;
        }
        protected Role() { }
        public int Id { get; private set; }
        public string Name { get; private set; }
        public ICollection<UserRole> UserRoles { get; private set; }


        public static Role Create(string name)
        {
            return new Role(name);
        }

        public static List<RoleDTO> ConvertToDTO(List<Role> roles)
        {
            return roles.Select(role => new RoleDTO { roleName = role.Name }).ToList();
        }

        public static RoleDTO ConvertToDTO(Role role)
        {
            return new RoleDTO { roleName = role.Name };
        }


    }

}
