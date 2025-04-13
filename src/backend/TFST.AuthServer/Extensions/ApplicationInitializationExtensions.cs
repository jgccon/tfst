using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using TFST.AuthServer.Infrastructure.Configuration;
using TFST.AuthServer.Infrastructure.Workers;
using TFST.AuthServer.Persistence;

namespace TFST.AuthServer.Extensions;

public static class ApplicationInitializationExtensions
{
    public static async Task InitializeDatabaseAsync(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AuthDbContext>();
        var options = scope.ServiceProvider.GetRequiredService<IOptions<AuthServerOptions>>();
        await dbContext.Database.MigrateAsync();

        var worker = new Worker(app.ApplicationServices, options);
        await worker.StartAsync(default);
    }
}