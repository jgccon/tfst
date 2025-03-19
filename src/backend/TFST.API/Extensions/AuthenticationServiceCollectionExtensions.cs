using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TFST.SharedKernel.Configuration;

namespace TFST.API.Extensions;

public static class AuthenticationServiceCollectionExtensions
{
    public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtSettings = configuration.GetSection("JwtSettings").Get<JwtSettings>() ?? throw new ArgumentNullException("JwtSettings not configured.");

        if (string.IsNullOrEmpty(jwtSettings.Key))
        {
            throw new ArgumentNullException(nameof(jwtSettings.Key), "JWT Key is not configured properly.");
        }

        var keyBytes = Encoding.ASCII.GetBytes(jwtSettings.Key);
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(keyBytes),

            ValidateIssuer = !string.IsNullOrEmpty(jwtSettings.Issuer),
            ValidateAudience = !string.IsNullOrEmpty(jwtSettings.Audience),

            ValidIssuer = jwtSettings.Issuer,
            ValidAudience = jwtSettings.Audience,

            ValidateLifetime = true,
            RequireExpirationTime = true,
            ClockSkew = TimeSpan.Zero
        };

        services.AddSingleton(tokenValidationParameters);

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.TokenValidationParameters = tokenValidationParameters;

                // If Identity Server uses introspection in the future, we can configure it here
                options.RequireHttpsMetadata = false;
            });

        return services;
    }
}
