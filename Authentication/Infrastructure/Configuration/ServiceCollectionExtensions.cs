using Authentication.Application.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using System.Threading.RateLimiting;
using Authentication.Infrastructure.Configuration.Swagger;

namespace Authentication.Infrastructure.Configuration
{
    public static class ServiceCollectionExtensions
    {

        public static IServiceCollection AddJwtAuthentication(this IServiceCollection services)
        {

            //This just adds basic Jwt authentication to this API.
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opt =>
            {
                opt.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = Environment.GetEnvironmentVariable("JWT_TOKEN_ISSUER"),
                    ValidAudience = Environment.GetEnvironmentVariable("JWT_TOKEN_AUDIENCE"),
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("JWT_TOKEN_SECRET_KEY") ?? "")),
                    RoleClaimType = ClaimTypes.Role
                };
            });

            return services;

        }


        //This has two possibilities:
        //Either the user is authenticated and we limit him to a token bucket of X with a refresh of Y tokens/timespan
        //Or the user is not authenticated and we limit him to X requests per IP per Y seconds.
        public static IServiceCollection AddCustomRateLimiters(this IServiceCollection services)
        {
            //Define the error code for rejections.
            services.AddRateLimiter(opt =>
            {
                opt.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
                opt.AddPolicy("TokenBasedLimit", context =>
                {
                    var user = context.User;
                    if (user.Identity?.IsAuthenticated == true)
                    {
                        var token = user.Identity.Name ?? "unknown_user";
                        return RateLimitPartition.GetTokenBucketLimiter(token, key => new TokenBucketRateLimiterOptions
                        {
                            TokenLimit = 100,
                            ReplenishmentPeriod = TimeSpan.FromSeconds(5),
                            TokensPerPeriod = 20,
                            QueueLimit = 0
                        });
                    }
                    var clientIP = ClientIdentificationService.GetClientIp(context);

                    return RateLimitPartition.GetFixedWindowLimiter(clientIP, key => new FixedWindowRateLimiterOptions
                    {
                        PermitLimit = 5,
                        Window = TimeSpan.FromSeconds(5)
                    });

                });

            });

            return services;
        }

        public static IServiceCollection ConfigureSwaggerGen(this IServiceCollection services)
        {
            services.AddSwaggerGen(opt =>
            {

                opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Add your JWT Token."
                });

                opt.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "JWT Authentication Service",
                    Description = "API to provide JWT authentication and user/role management."
                });


                //This is not good. It adds the "lock" in all endpoints in swagger UI.
                //opt.AddSecurityRequirement(new OpenApiSecurityRequirement
                //{
                //    {
                //    new OpenApiSecurityScheme
                //    {
                //        Reference = new OpenApiReference
                //        {
                //            Type = ReferenceType.SecurityScheme,
                //            Id="Bearer"
                //        }
                //    },Array.Empty<string>()

                //    }
                //});
                //This creates a filter to mark only the endpoints that are protected. Really weird piece of code, had to ask for help.
                opt.OperationFilter<AuthOperationFilter>();
                opt.DocumentFilter<HttpMethodOrder>();

                opt.EnableAnnotations();


            });
            return services;
        }
    }
}
