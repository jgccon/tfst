using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace TFST.SharedKernel.Configuration;

public static class ConfigurationServiceCollectionExtensions
{
    public static IServiceCollection AddSettings(this IServiceCollection services, IConfiguration configuration)
    {
        // Registrar todas las configuraciones en el contenedor de dependencias
        services.Configure<AdminSettings>(configuration.GetSection("AdminSettings").Bind);
        services.Configure<ApiSettings>(configuration.GetSection("ApiSettings").Bind);
        services.Configure<FeatureFlags>(configuration.GetSection("FeatureFlags").Bind);
        services.Configure<JwtSettings>(configuration.GetSection("JwtSettings").Bind);

        // Hacer que los objetos de configuración puedan ser inyectados directamente
        services.AddSingleton(sp => sp.GetRequiredService<IOptions<AdminSettings>>().Value);
        services.AddSingleton(sp => sp.GetRequiredService<IOptions<ApiSettings>>().Value);
        services.AddSingleton(sp => sp.GetRequiredService<IOptions<FeatureFlags>>().Value);
        services.AddSingleton(sp => sp.GetRequiredService<IOptions<JwtSettings>>().Value);

        return services;
    }
}

