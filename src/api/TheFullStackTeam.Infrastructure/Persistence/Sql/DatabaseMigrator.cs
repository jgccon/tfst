using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;

namespace TheFullStackTeam.Infrastructure.Persistence.Sql;

public class DatabaseMigrator
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<DatabaseMigrator> _logger;
    private readonly AsyncRetryPolicy _retryPolicy;

    public DatabaseMigrator(ApplicationDbContext context, ILogger<DatabaseMigrator> logger)
    {
        _context = context;
        _logger = logger;
        _retryPolicy = CreateRetryPolicy();
    }

    public async Task MigrateDatabaseAsync()
    {
        _logger.LogInformation("Starting database migration...");

        try
        {
            await _retryPolicy.ExecuteAsync(async () =>
            {
                await _context.Database.MigrateAsync();
                _logger.LogInformation("Database migration completed successfully.");
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred during database migration.");
            throw;
        }
    }

    private AsyncRetryPolicy CreateRetryPolicy()
    {
        return Policy
            .Handle<SqlException>()
            .WaitAndRetryAsync(
                retryCount: 10,
                sleepDurationProvider: retryAttempt =>
                    TimeSpan.FromSeconds(Math.Pow(5, retryAttempt)), // Exponential backoff: a^x seconds
                onRetry: (exception, timeSpan, retryCount, context) =>
                {
                    _logger.LogWarning(
                        exception,
                        "Attempt {RetryCount} to migrate database failed. Waiting {TimeSpan} seconds before next retry.",
                        retryCount,
                        timeSpan.TotalSeconds);
                });
    }
}
