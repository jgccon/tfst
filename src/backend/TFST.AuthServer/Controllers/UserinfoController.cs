using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;
using TFST.AuthServer.Infrastructure.Configuration;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace TFST.AuthServer.Controllers;

public class UserinfoController : Controller
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IOptions<AuthServerOptions> _options;
    private readonly ILogger<UserinfoController> _logger;

    public UserinfoController(
        UserManager<IdentityUser> userManager,
        IOptions<AuthServerOptions> options,
        ILogger<UserinfoController> logger)
    {
        _userManager = userManager;
        _options = options;
        _logger = logger;
    }

    [Authorize(AuthenticationSchemes = OpenIddictServerAspNetCoreDefaults.AuthenticationScheme)]
    [HttpGet("~/connect/userinfo"), HttpPost("~/connect/userinfo"), Produces("application/json")]
    public async Task<IActionResult> Userinfo()
    {
        try
        {
            var user = await _userManager.GetUserAsync(User);
            if (user is null)
            {
                _logger.LogWarning("Usuario no encontrado para el token proporcionado");
                return Challenge(
                    authenticationSchemes: OpenIddictServerAspNetCoreDefaults.AuthenticationScheme,
                    properties: new AuthenticationProperties(new Dictionary<string, string?>
                    {
                        [OpenIddictServerAspNetCoreConstants.Properties.Error] = Errors.InvalidToken,
                        [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] =
                            "El token de acceso no está asociado a un usuario válido."
                    }));
            }

            var claims = new Dictionary<string, object>(StringComparer.Ordinal)
            {
                [Claims.Subject] = await _userManager.GetUserIdAsync(user)
            };

            await AddStandardClaims(claims, user);
            AddCustomClaims(claims, user);

            return Ok(claims);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al procesar la solicitud userinfo");
            return StatusCode(500);
        }
    }

    private async Task AddStandardClaims(Dictionary<string, object> claims, IdentityUser user)
    {
        if (User.HasScope(Scopes.Email))
        {
            claims[Claims.Email] = await _userManager.GetEmailAsync(user) ?? string.Empty;
            claims[Claims.EmailVerified] = await _userManager.IsEmailConfirmedAsync(user);
        }

        if (User.HasScope(Scopes.Roles))
        {
            claims[Claims.Role] = await _userManager.GetRolesAsync(user);
        }

        if (User.HasScope(Scopes.Profile))
        {
            claims[Claims.Username] = await _userManager.GetUserNameAsync(user) ?? string.Empty;
        }
    }

    private void AddCustomClaims(Dictionary<string, object> claims, IdentityUser user)
    {
        foreach (var scope in _options.Value.ApiScopes)
        {
            claims[scope.Name] = scope.Resource;
        }
    }
}