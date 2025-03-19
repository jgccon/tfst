using Microsoft.Azure.Amqp.Framing;
using Microsoft.OpenApi.Models;
using Scalar.AspNetCore;

namespace TFST.API.Extensions;

public static class OpenApiServiceCollectionExtensions
{
    public static IServiceCollection AddOpenApiConfiguration(this IServiceCollection services, IWebHostEnvironment env)
    {
        services.AddOpenApi(options =>
        {
            options.AddDocumentTransformer((document, context, _) =>
            {
                document.Info = new OpenApiInfo
                {
                    Title = Constants.Title,
                    Version = "v1",
                    Description = Constants.Description,
                    Contact = new OpenApiContact
                    {
                        Name = Constants.Contact["name"],
                        Url = new Uri(Constants.Contact["url"]),
                        Email = Constants.Contact["email"]
                    },
                    License = new OpenApiLicense
                    {
                        Name = Constants.LicenseInfo["name"],
                        Url = new Uri(Constants.LicenseInfo["url"])
                    }
                };

                var servers = new List<OpenApiServer>();

                if (env.IsDevelopment())
                {
                    servers.Add(new OpenApiServer { Url = "https://localhost:5000", Description = "Local Development" });
                }

                servers.Add(new OpenApiServer { Url = "https://api.tfst.xyz", Description = "Production" });
                servers.Add(new OpenApiServer { Url = "https://api.tfst.dev", Description = "Staging" });

                document.Servers = servers;

                document.Tags =
                [
                    new OpenApiTag { Name = "Admin", Description = "Protected Management Operations" },
                    new OpenApiTag { Name = "Users", Description = "User Operations" },
                    new OpenApiTag { Name = "Profiles", Description = "Professional Profiles" }
                ];

                return Task.CompletedTask;
            });

            //// OAuth2 Security Definition
            //options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
            //{
            //    Type = SecuritySchemeType.OAuth2,
            //    Flows = new OpenApiOAuthFlows
            //    {
            //        Password = new OpenApiOAuthFlow
            //        {
            //            TokenUrl = new Uri("/api/auth/token", UriKind.Relative)
            //        }
            //    }
            //});

            //// Security requirement globally
            //options.AddSecurityRequirement(new OpenApiSecurityRequirement
            //{
            //    {
            //        new OpenApiSecurityScheme
            //        {
            //            Reference = new OpenApiReference
            //            {
            //                Type = ReferenceType.SecurityScheme,
            //                Id = "oauth2"
            //            }
            //        },
            //        Array.Empty<string>()
            //    }
            //});
        });

        return services;
    }

    public static IApplicationBuilder UseOpenApiConfiguration(this WebApplication app)
    {
        app.MapOpenApi().CacheOutput();
        app.MapScalarApiReference();

        // Redirect root to Scalar UI
        app.MapGet("/", () => Results.Redirect("/scalar/v1")).ExcludeFromDescription();

        return app;
    }
}