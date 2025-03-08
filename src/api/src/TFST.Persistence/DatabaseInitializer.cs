using Microsoft.Extensions.Logging;

namespace TFST.Persistence.Initialization;

public class DatabaseInitializer
{
    private readonly ILogger<DatabaseInitializer> _logger;

    public DatabaseInitializer(ILogger<DatabaseInitializer> logger)
    {
        _logger = logger;
    }

    public void Seed()
    {
        _logger.LogInformation("Seeding Database...");
    }
}
