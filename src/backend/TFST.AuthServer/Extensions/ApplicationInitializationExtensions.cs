using Microsoft.EntityFrameworkCore;
using TFST.AuthServer.Infrastructure.Workers;
using TFST.AuthServer.Persistence;

namespace TFST.AuthServer.Extensions;

public static class ApplicationInitializationExtensions
{
    public static async Task InitializeDatabaseAsync(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AuthDbContext>();

        await dbContext.Database.MigrateAsync();

        var worker = new Worker(app.ApplicationServices);
        await worker.StartAsync(default);
    }
}