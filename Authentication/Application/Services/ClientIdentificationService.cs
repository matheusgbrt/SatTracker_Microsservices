namespace Authentication.Application.Services
{
    public static class ClientIdentificationService
    {
        public static string GetClientIp(HttpContext context)
        {
            string? clientIp = context.Request.Headers["X-Forwarded-For"].FirstOrDefault();

            if (string.IsNullOrEmpty(clientIp))
            {
                clientIp = context.Request.Headers["X-Real-IP"].FirstOrDefault();
            }

            return clientIp ?? context.Connection.RemoteIpAddress?.ToString() ?? "unknown_ip";
        }
    }
}
