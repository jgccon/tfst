using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace TFST.AuthServer.Controllers;

[Route("[controller]")]
public class AccountController(
    ILogger<AccountController> logger,
    SignInManager<IdentityUser> signInManager,
    UserManager<IdentityUser> userManager
    ) : Controller
{
    private readonly SignInManager<IdentityUser> _signInManager = signInManager;
    private readonly UserManager<IdentityUser> _userManager = userManager;
    private readonly ILogger<AccountController> _logger = logger;

    [HttpGet]
    public IActionResult Register() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(object request)
    {
        throw new NotImplementedException();
    }

    [HttpGet]
    public IActionResult Login(string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(object request, string? returnUrl = null)
    {
        throw new NotImplementedException();
    }

    
}