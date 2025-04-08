using OpenIddict.Abstractions;
using TFST.AuthServer.Models;

namespace TFST.AuthServer.Services;

public class OpenIddictService(IOpenIddictApplicationManager applicationManager)
{
    private readonly IOpenIddictApplicationManager _applicationManager = applicationManager;

    public async Task<OpenIddictApplicationModel?> GetApplicationAsync(string clientId)
    {
        var application = await _applicationManager.FindByClientIdAsync(clientId);
        if (application == null) return null;

        var descriptor = new OpenIddictApplicationDescriptor();
        await _applicationManager.PopulateAsync(descriptor, application);
        if (descriptor == null) return null;

        var model = new OpenIddictApplicationModel
        {
            ClientId = descriptor.ClientId!,
            DisplayName = descriptor.DisplayName!,
            RedirectUris = descriptor.RedirectUris.ToList(),
            PostLogoutRedirectUris = descriptor.PostLogoutRedirectUris.ToList(),
            Permissions = descriptor.Permissions.ToList(),
            Scopes = descriptor.Permissions
                .Where(p => p.StartsWith(OpenIddictConstants.Permissions.Prefixes.Scope))
                .Select(p => p.Replace(OpenIddictConstants.Permissions.Prefixes.Scope, ""))
                .ToList()
        };

        return model;
    }
}