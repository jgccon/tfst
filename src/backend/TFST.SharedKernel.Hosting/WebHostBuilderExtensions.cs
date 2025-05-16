using Microsoft.AspNetCore.Hosting;

namespace TFST.SharedKernel.Hosting;

public static class WebHostBuilderExtensions
{
    /// <summary>
    /// Configures smart URL bindings based on the environment:
    /// 1. Uses ASPNETCORE_URLS if defined.
    /// 2. Falls back to Azure App Service PORT variable.
    /// 3. Otherwise uses the provided default URLs.
    /// </summary>
    /// <param name="builder">The web host builder.</param>
    /// <param name="defaultUrls">Optional fallback URLs.</param>
    public static void UseSmartPortConfiguration(this IWebHostBuilder builder, params string[] defaultUrls)
    {
        var aspnetUrls = Environment.GetEnvironmentVariable("ASPNETCORE_URLS");
        var azurePort = Environment.GetEnvironmentVariable("PORT");

        if (!string.IsNullOrWhiteSpace(aspnetUrls))
        {
            builder.UseUrls(aspnetUrls);
        }
        else if (!string.IsNullOrWhiteSpace(azurePort))
        {
            builder.UseUrls($"http://*:{azurePort}");
        }
        else if (defaultUrls is { Length: > 0 })
        {
            builder.UseUrls(defaultUrls);
        }
    }
}
