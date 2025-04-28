using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using TFST.AuthServer.Infrastructure.Configuration;
using TFST.AuthServer.Models;

namespace TFST.AuthServer.Controllers;

public class AccountController(
    UserManager<IdentityUser> userManager,
    SignInManager<IdentityUser> signInManager,
    IOptions<AuthServerOptions> options,
    ILogger<AccountController> logger
    ) : Controller
{
    private readonly UserManager<IdentityUser> _userManager = userManager;
    private readonly SignInManager<IdentityUser> _signInManager = signInManager;
    private readonly AuthServerOptions _options = options.Value;
    private readonly ILogger<AccountController> _logger = logger;

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (ModelState.IsValid)
        {
            var user = new IdentityUser
            {
                UserName = model.Email,
                Email = model.Email
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                return RedirectToAction(nameof(Login));
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        return View(model);
    }

    [HttpGet]
    public IActionResult Login(string? returnUrl = null)
    {
        if (string.IsNullOrEmpty(returnUrl))
        {
            _logger.LogWarning("Intento de acceso directo al login");
            return Redirect(_options.TfstApp.PostLogoutRedirectUris);
        }

        try
        {
            if (!Url.IsLocalUrl(returnUrl) ||
                !returnUrl.StartsWith("/connect/authorize", StringComparison.OrdinalIgnoreCase))
            {
                _logger.LogWarning("URL de retorno inválida: {ReturnUrl}", returnUrl);
                return Redirect(_options.TfstApp.PostLogoutRedirectUris);
            }

            bool isValidUrl = returnUrl.Contains("client_id=") &&
                          returnUrl.Contains("response_type=") &&
                          returnUrl.Contains("scope=") &&
                          returnUrl.Contains("code_challenge=");

            if (!isValidUrl)
            {
                _logger.LogWarning("URL de retorno no contiene los parámetros requeridos: {ReturnUrl}", returnUrl);
                return Redirect(_options.TfstApp.PostLogoutRedirectUris);
            }

            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }
        catch
        {
            _logger.LogWarning("Error procesando returnUrl");
            return Redirect(_options.TfstApp.PostLogoutRedirectUris);
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;

        if (ModelState.IsValid)
        {
            var result = await _signInManager.PasswordSignInAsync(
                model.Email,
                model.Password,
                model.RememberMe,
                lockoutOnFailure: false);

            if (result.Succeeded)
            {
                if (string.IsNullOrEmpty(returnUrl))
                {
                    _logger.LogWarning("Redirección nula");
                    ModelState.AddModelError(string.Empty, "Error en la redirección de autenticación.");
                    return View(model);
                }

                if (Url.IsLocalUrl(returnUrl))
                {
                    _logger.LogWarning("Redirección local");
                    _logger.LogWarning("Redirección: {ReturnUrl}", returnUrl);
                    return Redirect(returnUrl);
                }
                else
                {
                    _logger.LogWarning("Redirección no local");
                    ModelState.AddModelError(string.Empty, "Error en la redirección de autenticación.");
                    return View(model);
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Intento de inicio de sesión no válido.");
            }
        }

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout(string? returnUrl = null)
    {
        await _signInManager.SignOutAsync();

        return SignOut(
            authenticationSchemes: new[]
            {
                OpenIddict.Validation.AspNetCore.OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme,
                OpenIddict.Server.AspNetCore.OpenIddictServerAspNetCoreDefaults.AuthenticationScheme
            });
    }

    [HttpGet]
    public IActionResult AccessDenied()
    {
        return View();
    }
}