using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;

namespace TFST.Modules.Users.Presentation.Extensions;

public static class UsersModuleExtensions
{
    public static IServiceCollection AddUsersModule(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(
            typeof(UsersModuleExtensions).Assembly));

        return services;
    }

    public static IApplicationBuilder UseUsersModule(this IApplicationBuilder app)
    {
        return app;
    }
}
