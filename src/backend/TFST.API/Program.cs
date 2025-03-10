using TFST.API.Extensions;
using TFST.Modules.Users.Presentation.Extensions;


namespace TFST.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddOpenApiConfiguration();
            builder.Services.AddJwtAuthentication(builder.Configuration);

            builder.Services.AddUsersModule();

            var app = builder.Build();

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.UseOpenApiConfiguration();

            app.UseUsersModule();
            app.Run();
        }
    }
}
