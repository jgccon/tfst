using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using TFST.AuthServer.Models;
using TFST.AuthServer.Persistence;

namespace TFST.AuthServer.Controllers;

[Route("[controller]")]
public class AccountController : Controller
{
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly ILogger<AccountController> _logger;
    private readonly AuthDbContext _context; // Agregar esto

    public AccountController(
        ILogger<AccountController> logger,
        SignInManager<IdentityUser> signInManager,
        UserManager<IdentityUser> userManager,
        AuthDbContext context) // Agregar esto
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _logger = logger;
        _context = context;
    }

    [HttpGet]
    public IActionResult Register() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel viewModel)
    {
        if (!ModelState.IsValid) return View(viewModel);

        // Iniciar transacción
        await using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            var user = new IdentityUser { UserName = viewModel.Email, Email = viewModel.Email };
            var result = await _userManager.CreateAsync(user, viewModel.Password!);

            if (!result.Succeeded)
            {
                HandleIdentityErrors(result);
                return View(viewModel);
            }

            var addRoleResult = await _userManager.AddToRoleAsync(user, "USER");
            if (!addRoleResult.Succeeded)
            {
                HandleIdentityErrors(addRoleResult);
                await transaction.RollbackAsync();
                ModelState.AddModelError(string.Empty, "Can't add user to role.");
                return View(viewModel);
            }

            await transaction.CommitAsync();
            TempData["SuccessMessage"] = "Registered successfully!";
            return RedirectToAction(nameof(Login));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred: {Message}", ex.Message);
            await transaction.RollbackAsync();
            ModelState.AddModelError(string.Empty, "An unexpected error occurred. Please try again later.");
            return View(viewModel);
        }

    }

    [HttpGet]
    public IActionResult Login(string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
    {
        returnUrl ??= "https://tu-app-angular.com";
        ViewData["ReturnUrl"] = returnUrl;

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var result = await _signInManager.PasswordSignInAsync(
            model.Email!, model.Password!, model.RememberMe, lockoutOnFailure: true);

        if (result.Succeeded)
        {
            _logger.LogInformation("User {Email} logged in.", model.Email);

            return RedirectToAction("authorize", "authorization", new
            {
                client_id = "angular_app",
                redirect_uri = returnUrl,
                response_type = "code",
                scope = "openid profile email api offline_access",
                state = GenerateSecureRandomString(),
                code_challenge = GenerateCodeChallenge(),
                code_challenge_method = "S256"
            });
        }

        if (result.IsLockedOut)
        {
            _logger.LogWarning("User {Email} account locked.", model.Email);
            ModelState.AddModelError(string.Empty, "User account is locked.");
            return View(model);
        }

        ModelState.AddModelError(string.Empty, "Email or password is incorrect.");
        return View(model);
    }

    private void HandleIdentityErrors(IdentityResult result)
    {
        foreach (var error in result.Errors)
        {
            switch (error.Code)
            {
                case "DuplicateUserName":
                    ModelState.AddModelError(nameof(RegisterViewModel.Email), "El correo electrónico ya está registrado.");
                    break;
                case "DuplicateEmail":
                    ModelState.AddModelError(nameof(RegisterViewModel.Email), "El correo electrónico ya está registrado.");
                    break;
                default:
                    ModelState.AddModelError(string.Empty, error.Description);
                    break;
            }
        }
    }

    private string GenerateSecureRandomString()
    {
        var randomBytes = new byte[32];
        using var rng = System.Security.Cryptography.RandomNumberGenerator.Create();
        rng.GetBytes(randomBytes);
        return Convert.ToBase64String(randomBytes);
    }

    private string GenerateCodeChallenge()
    {        
        var codeVerifier = GenerateSecureRandomString();

        TempData["CodeVerifier"] = codeVerifier;

        using var sha256 = System.Security.Cryptography.SHA256.Create();
        var challengeBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(codeVerifier));

        return Base64UrlEncode(challengeBytes);
    }

    private string Base64UrlEncode(byte[] input)
    {
        var output = Convert.ToBase64String(input);
        output = output.Split('=')[0]; // Remove any trailing '='s
        output = output.Replace('+', '-'); // Replace '+' with '-'
        output = output.Replace('/', '_'); // Replace '/' with '_'
        return output;
    }

}