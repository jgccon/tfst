using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TheFullStackTeam.Application.Auth.Models;
using TheFullStackTeam.Common.Configuration;
using TheFullStackTeam.Domain.Entities;
using TheFullStackTeam.Infrastructure.Persistence.Sql;

namespace TheFullStackTeam.Application.Auth.Services;
public class TokenService(IOptions<JwtSettings> jwtOptions, ApplicationDbContext context) : ITokenService
{
    private readonly JwtSettings _jwtSettings = jwtOptions.Value;
    private readonly ApplicationDbContext _context = context;

    public TokenResponse GenerateTokens(Account account)
    {
        var selectedProfile = account.Profiles.FirstOrDefault(p => p.IsPrimary) ?? throw new Exception("No primary profile found.");

        // Add custom claims for profile info
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, account.Email),
            new Claim(ClaimTypes.NameIdentifier, account.Id.ToString()),
            new Claim("ProfileId", selectedProfile?.Id.ToString() ?? string.Empty),
            new Claim("DisplayName", selectedProfile?.DisplayName ?? string.Empty),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };

        // Agregar claims de roles
        claims.AddRange(account.Roles.Select(role => new Claim(ClaimTypes.Role, role)));

        var key = Encoding.ASCII.GetBytes(_jwtSettings.Key ?? throw new InvalidOperationException("JWT key not configured."));
        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenLifetime = _jwtSettings.TokenLifetimeInMinutes;

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(tokenLifetime),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            Issuer = _jwtSettings.Issuer,
            Audience = _jwtSettings.Audience
        };

        var accessToken = tokenHandler.CreateToken(tokenDescriptor);
        var accessTokenString = tokenHandler.WriteToken(accessToken);

        // Generar el refresh token
        var refreshToken = new RefreshToken
        {
            JwtId = accessToken.Id,
            IsUsed = false,
            IsRevoked = false,
            AccountId = account.Id,
            ExpireDate = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenLifetimeInDays),
            Token = $"{GenerateRandomTokenCharacters(35)}{Guid.NewGuid()}"
        };

        _context.RefreshTokens!.Add(refreshToken);
        _context.SaveChanges();

        return new TokenResponse
        {
            AccessToken = accessTokenString,
            RefreshToken = refreshToken.Token
        };
    }

    private string GenerateRandomTokenCharacters(int length)
    {
        var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        var random = new Random();
        return new string(Enumerable.Range(0, length)
                                .Select(_ => chars[random.Next(chars.Length)])
                                .ToArray());
    }
}
