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

            // Add services to the container.

            builder.Services.AddControllers();

            builder.Services.AddOpenApiConfiguration();

            var app = builder.Build();

            //// Configure the HTTP request pipeline.
            //if (app.Environment.IsDevelopment())
            //{
            //    app.MapOpenApi();
            //}

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.UseOpenApiConfiguration();

            app.MapControllers();

            // Redirect root to Scalar UI
            app.MapGet("/", () => Results.Redirect("/scalar/v1"))
               .ExcludeFromDescription();
            app.Run();
        }
    }
}
