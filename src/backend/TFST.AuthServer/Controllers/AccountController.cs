using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Abstractions;
using System.Text;
using TFST.AuthServer.Models;
using TFST.AuthServer.Persistence;
using TFST.AuthServer.Services;

namespace TFST.AuthServer.Controllers;

[AllowAnonymous]
public class AccountController(
    ILogger<AccountController> logger,
    SignInManager<IdentityUser> signInManager,
    UserManager<IdentityUser> userManager,
    AuthDbContext context,
    IOpenIddictApplicationManager applicationManager,
    OpenIddictService openIddictService,
    PkceService pkceService) : Controller
{
    private readonly SignInManager<IdentityUser> _signInManager = signInManager;
    private readonly UserManager<IdentityUser> _userManager = userManager;
    private readonly ILogger<AccountController> _logger = logger;
    private readonly AuthDbContext _context = context;
    private readonly IOpenIddictApplicationManager _applicationManager = applicationManager;
    private readonly OpenIddictService _openIddictService = openIddictService;
    private readonly PkceService _pkceService = pkceService;

    [HttpGet]
    public IActionResult Register() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel viewModel)
    {
        if (!ModelState.IsValid) return View(viewModel);

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

            /* var addRoleResult = await _userManager.AddToRoleAsync(user, "USER");
            if (!addRoleResult.Succeeded)
            {
                HandleIdentityErrors(addRoleResult);
                await transaction.RollbackAsync();
                ModelState.AddModelError(string.Empty, "Can't add user to role.");
                return View(viewModel);
            } */

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
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var result = await _signInManager.PasswordSignInAsync(
            model.Email!, model.Password!, model.RememberMe, lockoutOnFailure: true);

        if (result.Succeeded)
        {
            _logger.LogInformation("User {Email} logged in.", model.Email);

            var application = await _openIddictService.GetApplicationAsync("angular_app");
            if (application is null)
            {
                ModelState.AddModelError(string.Empty, "Application configuration not found.");
                return View(model);
            }

            var redirectUri = application.RedirectUris.FirstOrDefault()?.ToString();
            if (string.IsNullOrEmpty(redirectUri))
            {
                ModelState.AddModelError(string.Empty, "No redirect URI configured.");
                return View(model);
            }

            var (codeChallenge, state) = await _pkceService.CreatePkceAsync();

            var routeValues = new
            {
                client_id = application.ClientId,
                redirect_uri = redirectUri,
                response_type = "code",
                scopes = string.Join(" ", application.Scopes.Concat(["api", "offline_access"])),
                state = state,
                code_challenge = codeChallenge,
                code_challenge_method = "S256"
            };

            return RedirectToAction("authorize", "authorization", routeValues);
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



}