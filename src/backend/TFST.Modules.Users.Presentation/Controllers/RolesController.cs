using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TFST.Modules.Users.Application.Roles;
using TFST.SharedKernel.Presentation;

namespace TFST.Modules.Users.Presentation.Controllers;

[Tags("Users")]
[Authorize(Roles = "admin")]
[Route("roles")]
public class RolesController : ApiControllerBase
{
    private readonly IMediator _mediator;

    public RolesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllRoles()
    {
        var roles = await _mediator.Send(new GetRolesQuery());
        return Ok(roles);
    }
}