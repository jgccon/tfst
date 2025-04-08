namespace TFST.AuthServer.Models;

public class OpenIddictApplicationModel
{
    public string ClientId { get; set; } = default!;
    public string DisplayName { get; set; } = default!;
    public List<Uri> RedirectUris { get; set; } = new();
    public List<Uri> PostLogoutRedirectUris { get; set; } = new();
    public List<string> Permissions { get; set; } = new();
    public List<string> Scopes { get; set; } = new();
}