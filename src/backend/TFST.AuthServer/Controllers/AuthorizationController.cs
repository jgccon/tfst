using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;
using System.Security.Claims;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace TFST.AuthServer.Controllers;

[ApiController]
[Route("connect")]
public class AuthorizationController(
    IOpenIddictApplicationManager applicationManager
    ) : ControllerBase
{
    private readonly IOpenIddictApplicationManager _applicationManager = applicationManager;

    [HttpPost("token"), Produces("application/json")]
    public async Task<IActionResult> Exchange()
    {
        var request = HttpContext.GetOpenIddictServerRequest() ?? throw new Exception("Request is null");

        if (request.IsClientCredentialsGrantType())
        {
            if (string.IsNullOrEmpty(request.ClientId))
            {
                return BadRequest("ClientId is missing.");
            }

            var application = await _applicationManager.FindByClientIdAsync(request.ClientId)
                ?? throw new InvalidOperationException("The application cannot be found.");

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

        throw new NotImplementedException("The specified grant is not implemented.");
    }
}
