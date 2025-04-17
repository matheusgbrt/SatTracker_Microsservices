namespace Authentication.API.DTO
{
    public record UserDTO
    {
        public required string username { get; set; }
        public required List<string> roles { get; set; }
    }
}
