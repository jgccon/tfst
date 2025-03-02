using Microsoft.Data.SqlClient;

namespace TheFullStackTeam.Infrastructure.Persistence.Sql.Extensions;

public static class DatabaseConnectionRetryExtension
{
    public static async Task RetryDatabaseOperationAsync(Func<Task> operation, int maxRetries = 5)
    {
        for (int i = 0; i < maxRetries; i++)
        {
            try
            {
                await operation();
                return;
            }
            catch (SqlException)
            {
                if (i == maxRetries - 1) throw;
                
                await Task.Delay(TimeSpan.FromSeconds(5 * (i + 1)));
                Console.WriteLine($"Retrying database operation. Attempt {i + 1} of {maxRetries}");
            }
        }
    }
}