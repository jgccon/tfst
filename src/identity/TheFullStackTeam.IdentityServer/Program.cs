using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TheFullStackTeam.IdentityServer.Data;
using TheFullStackTeam.IdentityServer.Domain.Entities;

var builder = WebApplication.CreateBuilder(args);

// Database Configuration
builder.Services.AddDbContext<IdentityDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityDB"));
    options.UseOpenIddict();
});

// Identity Configuration
builder.Services.AddIdentity<Account, IdentityRole>()
    .AddEntityFrameworkStores<IdentityDbContext>()
    .AddDefaultTokenProviders();

// OpenIddict Configuration
builder.Services.AddOpenIddict()
    .AddCore(options =>
    {
        options.UseEntityFrameworkCore()
               .UseDbContext<IdentityDbContext>();
    })
    .AddServer(options =>
    {
        options.SetTokenEndpointUris("/connect/token");
        options.AllowPasswordFlow();
        options.AcceptAnonymousClients();

        // Temporary encryption & signing keys for development purposes
        options.AddDevelopmentEncryptionCertificate(); // ✅ Required encryption key
        options.AddDevelopmentSigningCertificate();    // ✅ Required signing key

        options.UseAspNetCore()
               .EnableTokenEndpointPassthrough();
    })
    .AddValidation(options =>
    {
        options.UseLocalServer();
        options.UseAspNetCore();
    });

// Register essential services
builder.Services.AddControllers();
builder.Services.AddAuthentication();
builder.Services.AddAuthorization();

var app = builder.Build();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
