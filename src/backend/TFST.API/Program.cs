using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using OpenIddict.Validation.AspNetCore;
using System.Security.Claims;
using TFST.API.Extensions;
using TFST.Modules.Users.Presentation.Extensions;
using TFST.SharedKernel.Configuration;
using TFST.SharedKernel.Hosting;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
    .AddUserSecrets<Program>(optional: true)
    .AddEnvironmentVariables(prefix: "TFST_"); 

builder.Services.AddOpenIddict()
    .AddValidation(options =>
    {
        var issuer = builder.Configuration.GetSection("OpenIddict:Issuer").Get<string>() ?? throw new ArgumentNullException("OpenIddict:Issuer");
        var audience = builder.Configuration.GetSection("OpenIddict:Audience").Get<string>() ?? throw new ArgumentNullException("OpenIddict:Audience");
        options.SetIssuer(issuer);
        options.AddAudiences(audience);

        var encryptionKey = builder.Configuration["Security:EncryptionKey"] ?? throw new ArgumentNullException("Security:EncryptionKey");
        options.AddEncryptionKey(new SymmetricSecurityKey(
            Convert.FromBase64String(encryptionKey)));

        // Register the System.Net.Http integration.
        options.UseSystemNetHttp();

        // Register the ASP.NET Core host.
        options.UseAspNetCore();
    });

var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? Array.Empty<string>();
builder.Services.AddCors(options => options.AddDefaultPolicy(policy =>
policy.AllowAnyHeader()
      .AllowAnyMethod()
      .AllowCredentials()
      .WithOrigins(allowedOrigins)));

builder.Services.AddSettings(builder.Configuration);

builder.Services.AddControllers();
builder.Services.AddOpenApiConfiguration(builder.Environment);
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddUsersModule(builder.Configuration);

builder.Services.AddAuthentication(OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme);
builder.Services.AddAuthorization();
builder.Services.AddHealthChecks();
builder.WebHost.UseSmartPortConfiguration("http://*:5000", "https://*:5001");

var app = builder.Build();

app.UseCors();
if (app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection(); // Only for development
}
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.UseOpenApiConfiguration();

await app.UseUsersModuleAsync(app.Services, builder.Configuration);

app.MapHealthChecks("/health");
app.MapGet("api", [Authorize] (ClaimsPrincipal user) => $"Message: User {user.Identity!.Name} accessed the protected resource API.");

app.Run();
