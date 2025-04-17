using Microsoft.IdentityModel.Protocols.OpenIdConnect;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient();

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = "Cookies";
    options.DefaultChallengeScheme = "oidc";
})
.AddCookie("Cookies", options =>
{
    options.Cookie.Name = ".ClientWebAppAuth";
})
.AddOpenIdConnect("oidc", options =>
{
    options.Authority = "https://localhost:44319";

    options.ClientId = "tfst_clientwebapp";
    options.ResponseType = OpenIdConnectResponseType.Code;

    options.Scope.Add("TFST_API");
    options.Scope.Add("openid");
    options.Scope.Add("profile");
    options.Scope.Add("email");
    options.Scope.Add("roles");

    options.SaveTokens = true;
    options.GetClaimsFromUserInfoEndpoint = false;
    options.TokenValidationParameters.NameClaimType = "name";

    options.CallbackPath = new PathString("/index");
    options.SignedOutCallbackPath = new PathString("/index");
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets()
   .RequireAuthorization();

app.Run();
