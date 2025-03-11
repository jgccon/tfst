using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TFST.Modules.Users.Persistence;

namespace TFST.Persistence.Extensions;

public static class DatabaseServiceCollectionExtensions
{
    public static IServiceCollection AddDatabaseConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<UsersDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        // Register database migrator
        services.AddScoped<DatabaseMigrator>();
        services.AddScoped<DatabaseSeeder>();
        return services;
    }
}
