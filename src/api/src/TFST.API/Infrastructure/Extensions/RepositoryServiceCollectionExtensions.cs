using TFST.Domain.Repositories.Command;
using TFST.Domain.Repositories.Full;
using TFST.Domain.Repositories.Query;
using TFST.Persistence.MongoDB.Repositories.Full;
using TFST.Persistence.MongoDB.Repositories.Queries;
using TFST.Persistence.Repositories;
using Microsoft.Extensions.DependencyInjection;
using TFST.Domain.Identity.Entities;

namespace TheFullStackTeam.Infrastructure.Extensions;

public static class RepositoryServiceCollectionExtensions
{
    public static IServiceCollection AddCommandRepositories(this IServiceCollection services)
    {
        // Command repositories based on SQL
        services.AddScoped<IAccountCommandRepository, AccountCommandRepository>();
        services.AddScoped<ICommandRepository<User>, UserProfileCommandRepository>();
        services.AddScoped<IRefreshTokenCommandRepository, RefreshTokenCommandRepository>();


        return services;
    }

    public static IServiceCollection AddQueryRepositories(this IServiceCollection services)
    {
        // Query repositories based on MongoDB (Account and Question views,containing domain aggregates)
        services.AddScoped<IAccountViewQueryRepository, AccountViewQueryRepository>();
        services.AddScoped<IUserProfileViewQueryRepository, UserProfileViewQueryRepository>();

        return services;
    }

    public static IServiceCollection AddSyncRepositories(this IServiceCollection services)
    {
        services.AddScoped<IAccountViewRepository, AccountViewRepository>();
        services.AddScoped<IUserProfileViewRepository, UserProfileViewRepository>();
        return services;
    }
}
