using Scalar.AspNetCore;
using Microsoft.OpenApi.Models;
using TFST.API.Extensions;

namespace TFST.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddOpenApiConfiguration();
            builder.Services.AddJwtAuthentication(builder.Configuration); // Autenticación JWT

            var app = builder.Build();

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.UseOpenApiConfiguration();

            app.MapControllers();
            app.Run();
        }
    }
}
