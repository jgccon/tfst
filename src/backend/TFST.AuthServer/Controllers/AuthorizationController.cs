using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;
using System.Security.Claims;
using System.Collections.Immutable;
using TFST.AuthServer.Persistence;
using static OpenIddict.Abstractions.OpenIddictConstants;
using Microsoft.AspNetCore.Authorization;
using TFST.AuthServer.Services;

namespace TFST.AuthServer.Controllers;

[ApiController]
[Route("connect")]
[AllowAnonymous]
public class AuthorizationController(
    IOpenIddictApplicationManager applicationManager,
    SignInManager<IdentityUser> signInManager,
    UserManager<IdentityUser> userManager,
    ILogger<AuthorizationController> logger,
    AuthDbContext context,
    PkceService pkceService
    ) : Controller
{
    private readonly IOpenIddictApplicationManager _applicationManager = applicationManager;
    private readonly SignInManager<IdentityUser> _signInManager = signInManager;
    private readonly UserManager<IdentityUser> _userManager = userManager;
    private readonly ILogger<AuthorizationController> _logger = logger;
    private readonly AuthDbContext _context = context;
    private readonly PkceService _pkceService = pkceService;

    [HttpPost("token"), Produces("application/json")]
    public async Task<IActionResult> Exchange()
    {
        var request = HttpContext.GetOpenIddictServerRequest() ?? throw new InvalidOperationException("Request is null");

        if (request.IsAuthorizationCodeGrantType())  // Add this condition
        {
            // Handle authorization code grant type
            return await HandleAuthorizationCodeGrantType(request);
        }
        else if (request.IsPasswordGrantType())
        {
            // password flow
            return await HandlePasswordGrantType(request);
        }
        else if (request.IsRefreshTokenGrantType())
        {
            // validate refresh token
            return await HandleRefreshTokenGrantType(request);
        }
        else if (request.IsClientCredentialsGrantType())
        {
            // Valide flow client credentials OIDC
            return await HandleClientCredentialsGrantType(request);
        }

        throw new NotImplementedException("The specified grant is not implemented.");
    }

    [HttpGet("authorize"), Produces("application/json")]
    [IgnoreAntiforgeryToken]
    public async Task<IActionResult> Authorize()
    {
        _logger.LogInformation("----------ENTRANDO---------");

        var request = HttpContext.GetOpenIddictServerRequest() ??
            throw new InvalidOperationException("The OpenID Connect request cannot be retrieved.");

        var result = await HttpContext.AuthenticateAsync();
        if (!result.Succeeded)
        {
            return Challenge();
        }

        var user = await _userManager.GetUserAsync(result.Principal) ??
            throw new InvalidOperationException("The user details cannot be retrieved.");

        var principal = await CreateClaimsPrincipalAsync(user, request.GetScopes());

        if (!principal.HasClaim(c => c.Type == Claims.Subject))
        {
            principal.AddIdentity(new ClaimsIdentity(new[]
            {
            new Claim(Claims.Subject, user.Id)
        }));
        }

        return SignIn(principal, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
    }

    [HttpGet("code-verifier"), Produces("application/json")]
    public async Task<IActionResult> GetCodeVerifier([FromQuery] string state)
    {
        var codeVerifier = await _pkceService.GetCodeVerifierAsync(state);
        if (codeVerifier == null)
        {
            return NotFound(new { error = "Code verifier not found or expired" });
        }

        return Ok(new { code_verifier = codeVerifier });
    }

    private async Task<IActionResult> HandleAuthorizationCodeGrantType(OpenIddictRequest request)
    {
        try
        {
            _logger.LogInformation("Iniciando intercambio de código de autorización");
            var result = await HttpContext.AuthenticateAsync(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
            if (!result.Succeeded)
            {
                _logger.LogWarning("Fallo en la autenticación del token");
                return ForbidWithError("invalid_grant", "The authorization code is invalid.");
            }

            // Loguear los claims disponibles para depuración
            _logger.LogInformation("Claims disponibles: {@Claims}",
                result.Principal?.Claims.Select(c => new { c.Type, c.Value }));

            // Obtener el email del principal
            var email = result.Principal?.Identity?.Name;
            if (string.IsNullOrEmpty(email))
            {
                _logger.LogWarning("No se encontró el email en los claims");
                return ForbidWithError("invalid_grant", "No se pudo identificar al usuario.");
            }

            // Buscar el usuario por email
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                _logger.LogWarning("No se encontró el usuario con email: {Email}", email);
                return ForbidWithError("invalid_grant", "Usuario no encontrado.");
            }

            // Create a new ClaimsPrincipal containing the claims that will be used to create the tokens
            var principal = await CreateClaimsPrincipalAsync(user, result.Principal!.GetScopes());

            return SignIn(principal, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while handling the authorization code grant: {Message}", ex.Message);
            return ForbidWithError("server_error", "An unexpected error occurred.");
        }
    }

    private async Task<IActionResult> HandlePasswordGrantType(OpenIddictRequest request)
    {
        try
        {
            if (string.IsNullOrEmpty(request.Username) || string.IsNullOrEmpty(request.Password))
            {
                return ForbidWithError("invalid_grant", "Username or password is missing.");
            }

            var user = await _userManager.FindByNameAsync(request.Username);
            if (user is null)
            {
                return ForbidWithError("invalid_grant", "Username or password is incorrect.");
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, lockoutOnFailure: true);
            if (!result.Succeeded)
            {
                if (result.IsLockedOut)
                {
                    _logger.LogWarning("User account is locked: {Email}", user.Email);
                    return ForbidWithError("account_locked", "The user account is locked.");
                }

                _logger.LogWarning("Invalid login attempt: {Email}", user.Email);
                return ForbidWithError("invalid_grant", "Username or password is incorrect.");
            }

            var principal = await CreateClaimsPrincipalAsync(user, request.GetScopes());

            return SignIn(principal, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred during password grant: {Message}", ex.Message);
            return ForbidWithError("server_error", "An unexpected error occurred.");
        }
    }

    private async Task<IActionResult> HandleRefreshTokenGrantType(OpenIddictRequest request)
    {
        try
        {
            var result = await HttpContext.AuthenticateAsync(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
            if (!result.Succeeded)
            {
                return ForbidWithError("invalid_grant", "Refresh token is missing.");
            }

            var user = await _userManager.GetUserAsync(result.Principal) ??
                throw new InvalidOperationException("The user principal cannot be resolved.");

            if (!await _userManager.IsEmailConfirmedAsync(user) ||
                await _userManager.IsLockedOutAsync(user))
            {
                return ForbidWithError("invalid_grant", "User is not active.");
            }

            var principal = await CreateClaimsPrincipalAsync(user, result.Principal.GetScopes());

            return SignIn(principal, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred during refresh token grant: {Message}", ex.Message);
            return ForbidWithError("server_error", "An unexpected error occurred.");
        }
    }

    private async Task<IActionResult> HandleClientCredentialsGrantType(OpenIddictRequest request)
    {
        if (string.IsNullOrEmpty(request.ClientId))
        {
            return ForbidWithError("invalid_grant", "ClientId is missing.");
        }

        var application = await _applicationManager.FindByClientIdAsync(request.ClientId);
        if (application is null)
        {
            return ForbidWithError("invalid_grant", "ClientId is invalid.");
        }

        var identity = new ClaimsIdentity(TokenValidationParameters.DefaultAuthenticationType, Claims.Name, Claims.Role);

        identity.SetClaim(Claims.Subject, await _applicationManager.GetClientIdAsync(application));
        identity.SetClaim(Claims.Name, await _applicationManager.GetDisplayNameAsync(application));

        identity.SetDestinations(static claim =>
        {
            if (claim.Subject is null)
            {
                return [Destinations.AccessToken];
            }

            return claim.Type switch
            {
                Claims.Name when claim.Subject.HasScope(Scopes.Profile)
                    => [Destinations.AccessToken, Destinations.IdentityToken],

                _ => [Destinations.AccessToken]
            };
        });

        return SignIn(new ClaimsPrincipal(identity), OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
    }

    private async Task<ClaimsPrincipal> CreateClaimsPrincipalAsync(IdentityUser user, ImmutableArray<string> scopes)
    {
        var identity = new ClaimsIdentity(
            OpenIddictServerAspNetCoreDefaults.AuthenticationScheme,
            Claims.Name,
            Claims.Role);

        identity.AddClaim(Claims.Subject, user.Id)
                .AddClaim(Claims.Name, user.UserName!)
                .AddClaim(Claims.Email, user.Email!);

        var roles = await _userManager.GetRolesAsync(user);
        foreach (var role in roles)
        {
            identity.AddClaim(Claims.Role, role);
        }

        var principal = new ClaimsPrincipal(identity);

        principal.SetScopes(scopes);
        principal.SetResources("api");

        return principal;
    }

    private IActionResult ForbidWithError(string error, string description)
    {
        var properties = new AuthenticationProperties(new Dictionary<string, string?>
        {
            [OpenIddictServerAspNetCoreConstants.Properties.Error] = error,
            [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] = description
        });

        return Forbid(properties, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
    }
}
