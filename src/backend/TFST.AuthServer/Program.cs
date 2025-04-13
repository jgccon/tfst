using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;
using Quartz;
using System.Globalization;
using System.Security.Claims;
using TFST.AuthServer.Extensions;
using TFST.AuthServer.Infrastructure.Configuration;
using TFST.AuthServer.Persistence;
using TFST.AuthServer.Services;
using static OpenIddict.Abstractions.OpenIddictConstants;

var builder = WebApplication.CreateBuilder(args);

// OpenIddict offers native integration with Quartz.NET to perform scheduled tasks
// (like pruning orphaned authorizations/tokens from the database) at regular intervals.
builder.Services.AddQuartz(options =>
{
    options.UseSimpleTypeLoader();
    options.UseInMemoryStore();
});

// Register the Quartz.NET service and configure it to block shutdown until jobs are complete.
builder.Services.AddQuartzHostedService(options => options.WaitForJobsToComplete = true);

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
        options.SetTokenEndpointUris("token")
                .SetAuthorizationEndpointUris("authorize")
                .SetIntrospectionEndpointUris("introspect")
                .SetRevocationEndpointUris("revoke")
                .SetUserInfoEndpointUris("userinfo");

        options.AllowAuthorizationCodeFlow()
                .AllowRefreshTokenFlow();

        var encryptionKey = builder.Configuration["Security:EncryptionKey"] ?? throw new ArgumentNullException("Security:EncryptionKey");
        options.AddEncryptionKey(new SymmetricSecurityKey(
            Convert.FromBase64String(encryptionKey)));

        options.UseAspNetCore()
               .EnableAuthorizationEndpointPassthrough();

        options.SetAccessTokenLifetime(TimeSpan.FromHours(2))
               .SetRefreshTokenLifetime(TimeSpan.FromDays(7))
               .SetRefreshTokenReuseLeeway(TimeSpan.FromMinutes(2));

        options.AddDevelopmentEncryptionCertificate()
               .AddDevelopmentSigningCertificate();
    }
).AddValidation(options =>
    {
        options.UseLocalServer();
        options.UseAspNetCore();
    }
);

builder.Services.Configure<AuthServerOptions>(
    builder.Configuration.GetSection("AuthServer"));

//builder.Services.AddScoped<OpenIddictService>();
//builder.Services.AddScoped<PkceService>();
var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? Array.Empty<string>();
builder.Services.AddCors(options => options.AddDefaultPolicy(policy =>
    policy.AllowAnyHeader()
          .AllowAnyMethod()
          .WithOrigins(allowedOrigins)));

builder.Services.AddControllersWithViews(options =>
{
    var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
    options.Filters.Add(new AuthorizeFilter(policy));
});

builder.Services.AddAuthorization();

var app = builder.Build();

app.UseCors();
app.UseHttpsRedirection();
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

app.MapMethods("authorize", [HttpMethods.Get, HttpMethods.Post], async (HttpContext context, IOpenIddictScopeManager manager) =>
{
    // Retrieve the OpenIddict server request from the HTTP context.
    var request = context.GetOpenIddictServerRequest();

    var identifier = (int?)request["hardcoded_identity_id"];
    if (identifier is not (1 or 2))
    {
        return Results.Challenge(
            authenticationSchemes: [OpenIddictServerAspNetCoreDefaults.AuthenticationScheme],
            properties: new AuthenticationProperties(new Dictionary<string, string?>
            {
                [OpenIddictServerAspNetCoreConstants.Properties.Error] = Errors.InvalidRequest,
                [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] = "The specified hardcoded identity is invalid."
            }));
    }

    // Create the claims-based identity that will be used by OpenIddict to generate tokens.
    var identity = new ClaimsIdentity(
        authenticationType: TokenValidationParameters.DefaultAuthenticationType,
        nameType: Claims.Name,
        roleType: Claims.Role);

    // Add the claims that will be persisted in the tokens.
    identity.AddClaim(new Claim(Claims.Subject, identifier.Value.ToString(CultureInfo.InvariantCulture)));
    identity.AddClaim(new Claim(Claims.Name, identifier switch
    {
        1 => "Alice",
        2 => "Bob",
        _ => throw new InvalidOperationException()
    }));
    identity.AddClaim(new Claim(Claims.PreferredUsername, identifier switch
    {
        1 => "Alice",
        2 => "Bob",
        _ => throw new InvalidOperationException()
    }));

    // Note: in this sample, the client is granted all the requested scopes for the first identity (Alice)
    // but for the second one (Bob), only the "api1" scope can be granted, which will cause requests sent
    // to Zirku.Api2 on behalf of Bob to be automatically rejected by the OpenIddict validation handler,
    // as the access token representing Bob won't contain the "resource_server_2" audience required by Api2.
    identity.SetScopes(identifier switch
    {
        1 => request.GetScopes(),
        2 => new[] { Scopes.OpenId, "TFST_API" }.Intersect(request.GetScopes()),
        _ => throw new InvalidOperationException()
    });

    identity.SetResources(await manager.ListResourcesAsync(identity.GetScopes()).ToListAsync());

    // Allow all claims to be added in the access tokens.
    identity.SetDestinations(claim => [Destinations.AccessToken]);

    return Results.SignIn(new ClaimsPrincipal(identity), properties: null, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
});

app.Run();