using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TFST.Modules.Users.Application.Models;
using TFST.Modules.Users.Application.Users;
using TFST.SharedKernel.Presentation;

namespace TFST.Modules.Users.Presentation.Controllers.Admin;

[Tags("Admin")]
[Authorize(Roles = "admin")]
[Route("admin/users")]
public class AdminUsersController : ApiControllerBase
{
    private readonly IMediator _mediator;

    public AdminUsersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetUsers()
    {
        var users = await _mediator.Send(new GetUsersQuery());
        return Ok(users);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetUserById(Guid id)
    {
        var user = await _mediator.Send(new GetUserByIdQuery(id));
        return Ok(user);
    }

    [HttpGet("by-moniker/{moniker}")]
    public async Task<IActionResult> GetUserByMoniker(string moniker)
    {
        var user = await _mediator.Send(new GetUserByMonikerQuery(moniker));
        return Ok(user);
    }

    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserModel model)
    {
        var user = await _mediator.Send(new CreateUserCommand(model));
        return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, user);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteUser(Guid id)
    {
        var result = await _mediator.Send(new DeleteUserCommand(id));
        return result ? NoContent() : NotFound(new { Message = "User not found" });
    }

    [HttpPost("{id:guid}/roles/{roleId:guid}")]
    public async Task<IActionResult> AssignRole(Guid id, Guid roleId)
    {
        var result = await _mediator.Send(new AssignRoleCommand(id, roleId));
        return result ? Ok() : NotFound(new { Message = "User or Role not found" });
    }
}
