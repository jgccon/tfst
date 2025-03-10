using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TFST.Modules.Users.Application.Users;
using TFST.SharedKernel.Presentation;

namespace TFST.Modules.Users.Presentation.Controllers;

[Tags("Users")]
[Authorize]
[Route("users")]
public class UsersController : ApiControllerBase
{
    private readonly IMediator _mediator;

    public UsersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("me")]
    public async Task<IActionResult> GetCurrentUser()
    {
        var user = await _mediator.Send(new GetCurrentUserQuery(User));
        return Ok(user);
    }


    [HttpGet("{moniker}")]
    public async Task<IActionResult> GetUserByMoniker(string moniker)
    {
        var user = await _mediator.Send(new GetUserByMonikerQuery(moniker));
        return Ok(user);
    }

    [HttpPut("me")]
    public async Task<IActionResult> UpdateCurrentUser([FromBody] UpdateCurrentUserCommand command)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
        {
            return Unauthorized(new { Message = "Invalid user token." });
        }

        var updatedUser = await _mediator.Send(new UpdateCurrentUserCommand(userId, command.FirstName, command.LastName));
        return Ok(updatedUser);
    }

    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserCommand command)
    {
        var user = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetUserByIdQuery), new { id = user.Id }, user);
    }

    [HttpDelete("me")]
    public async Task<IActionResult> DeleteUser()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
        {
            return Unauthorized(new { Message = "Invalid user token." });
        }

        var result = await _mediator.Send(new DeleteCurrentUserCommand(userId));
        return result ? NoContent() : NotFound(new { Message = "User not found" });
    }
}
