using Authentication.Application.Interfaces;
using Authentication.Application.Services;
using Authentication.Domain.Interfaces;
using Authentication.Domain.Services;
using Authentication.Infrastructure.Configuration;
using Authentication.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;

namespace Authentication
{
    public class Program
    {
        public static void Main(string[] args)
        {

            var builder = WebApplication.CreateBuilder(args);
            //Nullable tratment so its possible to execute migrations using the connstring without injecting .env.
            var connectionString = Environment.GetEnvironmentVariable("POSTGRES_CONNECTION_STRING");


            //Custom method to add JwtAuth. Made just to remove the code from Program.cs
            builder.Services.AddJwtAuthentication();

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();

            //Custom Swagger config.
            builder.Services.ConfigureSwaggerGen();

            //Custom ratelimiting.
            builder.Services.AddCustomRateLimiters();

            //Add services.
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IRoleService, RoleService>();
            builder.Services.AddScoped<IPasswordService, PasswordService>();
            builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();

            //Add Postgres EF and configure.
            builder.Services.AddDbContext<AuthenticationDBContext>(options => options.UseNpgsql(connectionString));

            var app = builder.Build();

            if (args.Contains("migrate"))
            {
                using var scope = app.Services.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<AuthenticationDBContext>();
                db.Database.Migrate();
                return;
            }

            app.UseSwagger();
            app.UseSwaggerUI(opt =>
            {
                opt.DefaultModelExpandDepth(-1);
                opt.DocumentTitle = "Authentication API Template";
            });


            app.UseHttpsRedirection();
            app.UseRateLimiter();
            app.MapControllers().RequireRateLimiting("TokenBasedLimit");
            app.UseAuthorization();
            app.Run();
        }
    }
}
