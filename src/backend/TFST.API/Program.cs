using TFST.API.Extensions;
using TFST.Modules.Users.Presentation.Extensions;


namespace TFST.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Configuration
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();

            builder.Services.AddControllers();
            builder.Services.AddOpenApiConfiguration();
            builder.Services.AddJwtAuthentication(builder.Configuration);
            builder.Services.AddUsersModule(builder.Configuration);

            var app = builder.Build();

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.UseOpenApiConfiguration();

            await app.UseUsersModuleAsync(app.Services, builder.Configuration);
            app.Run();
        }
    }
}
