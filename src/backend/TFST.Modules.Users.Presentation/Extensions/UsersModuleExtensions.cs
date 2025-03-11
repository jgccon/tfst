using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using TFST.Modules.Users.Persistence;
using Microsoft.EntityFrameworkCore;

namespace TFST.Modules.Users.Presentation.Extensions;

public static class UsersModuleExtensions
{
    public static IServiceCollection AddUsersModule(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<UsersDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
        services.AddScoped<DatabaseMigrator>();
        services.AddScoped<DatabaseSeeder>();

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(UsersModuleExtensions).Assembly));

        return services;
    }

    public static async Task UseUsersModuleAsync(this IApplicationBuilder app, IServiceProvider serviceProvider, IConfiguration configuration)
    {
        using var scope = serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<UsersDbContext>();
        var migrator = scope.ServiceProvider.GetRequiredService<DatabaseMigrator>();
        var seeder = scope.ServiceProvider.GetRequiredService<DatabaseSeeder>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<DatabaseSeeder>>();

        if (configuration.GetValue<bool>("FeatureFlags:MigrateAtStartup"))
        {
            await migrator.MigrateDatabaseAsync();
        }

        await seeder.SeedAsync();
        logger.LogInformation("Users module initialized.");
    }
}