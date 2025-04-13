using Microsoft.Extensions.Options;
using OpenIddict.Abstractions;
using TFST.AuthServer.Infrastructure.Configuration;
using TFST.AuthServer.Persistence;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace TFST.AuthServer.Infrastructure.Workers;

public class Worker(IServiceProvider serviceProvider, IOptions<AuthServerOptions> options) : IHostedService
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;
    private readonly AuthServerOptions _options = options.Value;

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<AuthDbContext>();
        await context.Database.EnsureCreatedAsync(cancellationToken);

        var applicationManager = scope.ServiceProvider.GetRequiredService<IOpenIddictApplicationManager>();

        //Create SPA application
        if (await applicationManager.FindByClientIdAsync(_options.TfstApp.ClientId) is null)
        {
            var descriptor = new OpenIddictApplicationDescriptor
            {
                ClientId = _options.TfstApp.ClientId,
                ClientType = ClientTypes.Public,
                DisplayName = _options.TfstApp.DisplayName,
                Permissions =
                {
                    Permissions.Endpoints.Authorization,
                    Permissions.Endpoints.EndSession,
                    Permissions.Endpoints.Token,
                    Permissions.GrantTypes.AuthorizationCode,
                    Permissions.GrantTypes.RefreshToken,
                    Permissions.ResponseTypes.Code,
                    Permissions.Scopes.Email,
                    Permissions.Scopes.Profile,
                    Permissions.Scopes.Roles
                },
                Requirements =
                {
                    Requirements.Features.ProofKeyForCodeExchange,
                }
            };

            foreach (var redirectUri in _options.TfstApp.RedirectUris)
                descriptor.RedirectUris.Add(new Uri(redirectUri));

            foreach (var apiScope in _options.ApiScopes)
                descriptor.Permissions.Add(Permissions.Prefixes.Scope + apiScope.Name);

            await applicationManager.CreateAsync(descriptor);
        }

        /*
                //Create resource server application
                if (await applicationManager.FindByClientIdAsync(_options.ResourceServer.ClientId) is null)
                {
                    await applicationManager.CreateAsync(new OpenIddictApplicationDescriptor
                    {
                        ClientId = _options.ResourceServer.ClientId,
                        ClientSecret = _options.ResourceServer.ClientSecret,
                        DisplayName = _options.ResourceServer.DisplayName,
                        Permissions =
                        {
                            Permissions.Endpoints.Introspection
                        }
                    });
                }
        */

        //Create scopes
        var scopeManager = scope.ServiceProvider.GetRequiredService<IOpenIddictScopeManager>();

        foreach (var apiScope in _options.ApiScopes)
        {
            if (await scopeManager.FindByNameAsync(apiScope.Name) is null)
            {
                await scopeManager.CreateAsync(new OpenIddictScopeDescriptor
                {
                    Name = apiScope.Name,
                    Resources =
                    {
                        apiScope.Resource
                    }
                });
            }
        }
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}