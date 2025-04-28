using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using TFST.AuthServer.Infrastructure.Configuration;
using TFST.AuthServer.Models;

namespace TFST.AuthServer.Controllers;

public class ErrorController : Controller
{
    private readonly IOptions<AuthServerOptions> _options;

    public ErrorController(IOptions<AuthServerOptions> options)
    {
        _options = options;
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    [Route("~/error")]
    public IActionResult Error()
    {
        // Agregar la URL del cliente para el botón de "Volver al inicio"
        ViewData["ClientUri"] = _options.Value.TfstApp.PostLogoutRedirectUris;

        // Si el error se originó desde OpenIddict, mostrar los detalles del error
        var response = HttpContext.GetOpenIddictServerResponse();
        if (response is not null)
        {
            return View(new ErrorViewModel
            {
                Error = response.Error,
                ErrorDescription = response.ErrorDescription
            });
        }

        return View(new ErrorViewModel());
    }
}