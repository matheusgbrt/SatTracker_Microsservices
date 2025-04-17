namespace Authentication.Domain.Exceptions
{
    public class UserAlreadyExistsException(string username) : Exception($"O usuário '{username}' já está cadastrado.")
    {
    }
}
