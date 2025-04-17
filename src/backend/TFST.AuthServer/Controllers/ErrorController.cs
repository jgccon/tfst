using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using TFST.AuthServer.Models;

namespace TFST.AuthServer.Controllers;

public class ErrorController : Controller
{
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true), Route("~/error")]
    public IActionResult Error()
    {
        // If the error originated from the OpenIddict server, render the error details.
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