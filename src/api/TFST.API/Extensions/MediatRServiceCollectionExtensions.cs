using MediatR;
using TFST.API.Behaviors;

namespace TFST.API.Extensions;

public static class MediatRServiceCollectionExtensions
{
    public static IServiceCollection AddMediatRConfiguration(this IServiceCollection services)
    {
        // Configure MediatR using this assembly (NOTE: it should contain all our commands and handlers)
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(MediatRServiceCollectionExtensions).Assembly);
        });

        services.AddHttpContextAccessor();
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestLoggingPipelineBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestExceptionHandlerBehavior<,>));

        return services;
    }
}