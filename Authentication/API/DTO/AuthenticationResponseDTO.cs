namespace Authentication.API.DTO
{
    public class AuthenticationResponseDTO
    {
        public required string token { get; set; }
        public required DateTime expires { get; set; }
    }
}
