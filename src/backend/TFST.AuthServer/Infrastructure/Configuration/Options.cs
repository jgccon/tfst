namespace TFST.AuthServer.Infrastructure.Configuration;

public class AuthServerOptions
{
    public ClientOptions TfstApp { get; set; } = new();
    public ClientOptions ResourceServer { get; set; } = new();
    public List<ApiScopeOptions> ApiScopes { get; set; } = [];
}

public class ClientOptions
{
    public string ClientId { get; set; } = string.Empty;
    public string? ClientSecret { get; set; }
    public string DisplayName { get; set; } = string.Empty;
    public List<string> RedirectUris { get; set; } = [];
}

public class ApiScopeOptions
{
    public string Name { get; set; } = string.Empty;
    public string Resource { get; set; } = string.Empty;
}