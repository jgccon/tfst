using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;
using System.Security.Claims;
using System.Collections.Immutable;
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
    PkceService pkceService
    ) : Controller
{
    private readonly IOpenIddictApplicationManager _applicationManager = applicationManager;
    private readonly SignInManager<IdentityUser> _signInManager = signInManager;
    private readonly UserManager<IdentityUser> _userManager = userManager;
    private readonly ILogger<AuthorizationController> _logger = logger;
    private readonly PkceService _pkceService = pkceService;

    [HttpPost("token"), Produces("application/json")]
    public async Task<IActionResult> Exchange()
    {
        var request = HttpContext.GetOpenIddictServerRequest() ?? throw new InvalidOperationException("Request is null");

        if (request.IsAuthorizationCodeGrantType())
        {
            // Handle authorization code grant type
            return await HandleAuthorizationCodeGrantType(request);
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
            var result = await HttpContext.AuthenticateAsync(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
            if (!result.Succeeded)
            {
                _logger.LogWarning("Failed to authenticate user.");
                return ForbidWithError("invalid_grant", "The authorization code is invalid.");
            }
           
            var email = result.Principal?.Identity?.Name;
            if (string.IsNullOrEmpty(email))
            {
                _logger.LogWarning("Don't find user email.");
                return ForbidWithError("invalid_grant", "Don't find user email.");
            }

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                _logger.LogWarning("User not found: {Email}", email);
                return ForbidWithError("invalid_grant", "User not found.");
            }

            var principal = await CreateClaimsPrincipalAsync(user, result.Principal!.GetScopes());

            return SignIn(principal, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while handling the authorization code grant: {Message}", ex.Message);
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
