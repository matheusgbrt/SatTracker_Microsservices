namespace Authentication.Domain.Exceptions
{
    public class RoleNotFoundException(string username) : Exception($"A role '{username}' não foi encontrada.")
    {
    }
}
