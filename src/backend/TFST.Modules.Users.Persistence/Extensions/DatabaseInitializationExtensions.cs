using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using TFST.Modules.Users.Persistence;

namespace TFST.Persistence.Extensions;

public static class DatabaseInitializationExtensions
{
    public static async Task InitializeDatabaseAsync(this IServiceProvider services, IConfiguration configuration)
    {
        using var scope = services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<UsersDbContext>();
        var migrator = scope.ServiceProvider.GetRequiredService<DatabaseMigrator>();
        var seeder = scope.ServiceProvider.GetRequiredService<DatabaseSeeder>();

        if (configuration.GetValue<bool>("FeatureFlags:MigrateAtStartup"))
        {
            await migrator.MigrateDatabaseAsync();
        }

        await seeder.SeedAsync();
    }
}
