namespace Authentication.Domain.Exceptions
{
    public class UserNotFoundException(string username) : Exception($"O usuário '{username}' não foi encontrado.")
    {
    }
}
