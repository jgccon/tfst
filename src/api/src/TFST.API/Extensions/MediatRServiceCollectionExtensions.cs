using TheFullStackTeam.Application.UserProfiles.Commands;
using MediatR;
using TheFullStackTeam.API.Behaviors;

namespace TFST.API.Extensions;

public static class MediatRServiceCollectionExtensions
{
    public static IServiceCollection AddMediatRConfiguration(this IServiceCollection services)
    {
        // Configure MediatR using the assembly that contains commands and handlers
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(UpdateUserProfileCommand).Assembly);
        });

        services.AddHttpContextAccessor();
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestLoggingPipelineBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestExceptionHandlerBehavior<,>));

        return services;
    }
}