using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using OpenIddict.Abstractions;
using TFST.AuthServer.Extensions;
using TFST.AuthServer.Persistence;
using TFST.AuthServer.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables();

builder.Services.AddDbContext<AuthDbContext>(options =>
{
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlOptions => sqlOptions.MigrationsAssembly(typeof(AuthDbContext).Assembly.GetName().Name)
    );

    options.UseOpenIddict();
});

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<AuthDbContext>()
    .AddDefaultTokenProviders();

builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 8;
    options.Password.RequiredUniqueChars = 1;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(60);
    options.Lockout.MaxFailedAccessAttempts = 3;
    options.Lockout.AllowedForNewUsers = true;
    options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
    options.User.RequireUniqueEmail = true;
    //options.SignIn.RequireConfirmedEmail = true;
});

builder.Services.AddOpenIddict()

    // Register the OpenIddict core components.
    .AddCore(options =>
    {
        options.UseEntityFrameworkCore()
               .UseDbContext<AuthDbContext>();
    })
    // Register the OpenIddict server components.
    .AddServer(options =>
    {
        // Enable the token endpoint.
        options.SetTokenEndpointUris("connect/token")
                .SetAuthorizationEndpointUris("connect/authorize")
                .SetUserInfoEndpointUris("connect/userinfo");

        // Configura los flujos de OAuth 2.0/OpenID Connect permitidos:
        // - Password flow: permite autenticación directa con usuario/contraseña
        // - Refresh token flow: permite renovar tokens de acceso
        // - Client credentials flow: para autenticación de aplicaciones cliente
        // - Authorization code flow: para aplicaciones web que requieren autorización del usuario
        options.AllowPasswordFlow()
               .AllowRefreshTokenFlow()
               .AllowClientCredentialsFlow()
               .AllowAuthorizationCodeFlow();

        // Configurations tokens
        options.AcceptAnonymousClients()
               .UseAspNetCore()
               .EnableTokenEndpointPassthrough()
               .EnableAuthorizationEndpointPassthrough()
               .EnableUserInfoEndpointPassthrough();

        // Configurations of lifetime and properties tokens
        options.SetAccessTokenLifetime(TimeSpan.FromHours(2))
               .SetRefreshTokenLifetime(TimeSpan.FromDays(7))
               .SetRefreshTokenReuseLeeway(TimeSpan.FromMinutes(2));

        // Register the signing and encryption credentials.
        options.AddDevelopmentEncryptionCertificate()
               .AddDevelopmentSigningCertificate();

        // Register scopes soported by the application.
        options.RegisterScopes("api", "offline_access");

        // Register the ASP.NET Core host and configure the ASP.NET Core options.
        options.UseAspNetCore()
               .EnableTokenEndpointPassthrough();

        // Register scopes supported by the application
        options.RegisterScopes(
            "api",
            "offline_access",  // Agregar offline_access
            OpenIddictConstants.Scopes.Email,
            OpenIddictConstants.Scopes.Profile,
            OpenIddictConstants.Scopes.Roles
        );
    }
).AddValidation(options =>
    {
        // Import the configuration from the local OpenIddict server instance.
        options.UseLocalServer();

        // Register the ASP.NET Core host.
        options.UseAspNetCore();
    }
);

builder.Services.AddScoped<OpenIddictService>();
builder.Services.AddScoped<PkceService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.Services.AddControllersWithViews(options =>
{
    var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
    options.Filters.Add(new AuthorizeFilter(policy));
});

var app = builder.Build();

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

if (builder.Configuration.GetValue<bool>("FeatureFlags:MigrateAtStartup"))
{
    await app.InitializeDatabaseAsync();
}

app.Run();