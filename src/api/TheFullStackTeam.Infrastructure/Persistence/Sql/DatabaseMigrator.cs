using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TheFullStackTeam.Infrastructure.Persistence.Sql.Extensions;

namespace TheFullStackTeam.Infrastructure.Persistence.Sql;

public class DatabaseMigrator
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<DatabaseMigrator> _logger;

    public DatabaseMigrator(ApplicationDbContext context, ILogger<DatabaseMigrator> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task MigrateDatabaseAsync()
    {
        await DatabaseConnectionRetryExtension.RetryDatabaseOperationAsync(async () =>
        {
            _logger.LogInformation("Starting database migration...");
            await _context.Database.MigrateAsync();
            _logger.LogInformation("Database migration completed.");
        });
    }
}
