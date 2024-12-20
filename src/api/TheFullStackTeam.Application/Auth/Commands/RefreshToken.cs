using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using TheFullStackTeam.Application.Auth.Models;
using TheFullStackTeam.Application.Auth.Services;
using TheFullStackTeam.Infrastructure.Persistence.Sql;

namespace TheFullStackTeam.Application.Auth.Commands;
public class RefreshTokenCommand(string token, string refreshToken) : IRequest<TokenResponse>
{
    public string Token { get; set; } = token;
    public string RefreshToken { get; set; } = refreshToken;
}

// TODO: Implement the Result pattern to handle errors
public class RefreshTokenCommandHandler(
    ApplicationDbContext context, TokenValidationParameters tokenValidationParameters, ITokenService tokenService
    ) : IRequestHandler<RefreshTokenCommand, TokenResponse>
{
    private readonly ApplicationDbContext _context = context;
    private readonly TokenValidationParameters _tokenValidationParameters = tokenValidationParameters;
    private readonly ITokenService _tokenService = tokenService;

    public async Task<TokenResponse> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var jwtTokenHandler = new JwtSecurityTokenHandler();
        var tokenValidationParamsClone = _tokenValidationParameters.Clone();
        tokenValidationParamsClone.ValidateLifetime = false;

        try
        {
            // validation: token format is correct
            var tokenVerification = jwtTokenHandler.ValidateToken(
                request.Token,
                tokenValidationParamsClone,
                out var validatedToken);

            // validation: Check encryption
            if (validatedToken is JwtSecurityToken jwtSecurityToken)
            {
                var result = jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                    StringComparison.InvariantCultureIgnoreCase);

                if (result == false) throw new Exception("The Token has encryption errors");
            }

            // validation: Check expiration date token
            var utcExpiryDateToken = long.Parse(tokenVerification.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Exp)!.Value);

            var expiryDateToken = UnixTimeStampToDateTime(utcExpiryDateToken);
            if (expiryDateToken > DateTime.UtcNow) throw new Exception("The Token hasn´t expired");

            //validation: Exist refresh token in DB
            var storedToken = await _context.RefreshTokens!
                .FirstOrDefaultAsync(x => x.Token == request.RefreshToken) ?? throw new Exception("The refresh Token does not exist");

            // validation to check if the refresh token was already used
            if (storedToken.IsUsed) throw new Exception("The refresh Token has already been used");

            // validation the refresh token was revoked?
            if (storedToken.IsRevoked) throw new Exception("The refresh Token has been revoked");

            // check refresh token id
            var jti = tokenVerification.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti)!.Value;

            if (storedToken.JwtId != jti) throw new Exception("Refresh Token does not match initial value");

            // validation for expiration date refresh token
            if (storedToken.ExpireDate < DateTime.UtcNow) throw new Exception("The refresh token has expired");

            storedToken.IsUsed = true;
            _context.RefreshTokens!.Update(storedToken);
            await _context.SaveChangesAsync(cancellationToken);

            var account = await _context.Accounts!
                .Include(a => a.Profiles)
                .FirstOrDefaultAsync(a => a.Id == storedToken.AccountId, cancellationToken) ?? throw new Exception("No account found for the given token");

            var tokenResponse = _tokenService.GenerateTokens(account);
            return tokenResponse;

        }
        catch (Exception)
        {
            throw;
        }
    }

    private static DateTime UnixTimeStampToDateTime(long unixTimeStamp)
    {
        var dateTimeval = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        dateTimeval = dateTimeval.AddSeconds(unixTimeStamp).ToUniversalTime();
        return dateTimeval;
    }
}
