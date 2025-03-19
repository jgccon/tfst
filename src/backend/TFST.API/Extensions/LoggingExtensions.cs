namespace TFST.API.Extensions;

public static class LoggingExtensions
{
    public static IServiceCollection AddCustomLogging(this IServiceCollection services)
    {
        services.AddLogging(loggingBuilder =>
        {
            loggingBuilder.ClearProviders();
            loggingBuilder.AddConsole();
            loggingBuilder.AddDebug();

            // Optional:
            // loggingBuilder.AddFile("logs/tfst-api.log"); // It needs Microsoft.Extensions.Logging.File
        });

        return services;
    }
}
