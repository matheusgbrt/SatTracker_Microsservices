namespace Authentication.Domain.Exceptions
{
    public class RoleAlreadyExistsException(string rolename) : Exception($"A role '{rolename}' já existe.")
    {
    }
}
