using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TheFullStackTeam.IdentityServer.Data;
using TheFullStackTeam.IdentityServer.Domain.Entities;

var builder = WebApplication.CreateBuilder(args);

// Configuración de la BD
builder.Services.AddDbContext<IdentityDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityDB"));
    options.UseOpenIddict();
});

// Configuración de Identity
builder.Services.AddIdentity<Account, IdentityRole>()
    .AddEntityFrameworkStores<IdentityDbContext>()
    .AddDefaultTokenProviders();

// Configuración de OpenIddict
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
        options.UseAspNetCore();
    })
    .AddValidation(options =>
    {
        options.UseLocalServer();
        options.UseAspNetCore();
    });

var app = builder.Build();
app.UseRouting();
app.UseAuthorization();
app.MapControllers();
app.Run();
