using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TFST.SharedKernel.Presentation;

namespace TFST.Modules.Users.Presentation.Controllers;

[Tags("Identity")]
[Authorize]
public class UsersController : ApiControllerBase
{
    private static readonly List<dynamic> Users = new()
    {
        new { Id = Guid.NewGuid(), Name = "Alice", Email = "alice@example.com" },
        new { Id = Guid.NewGuid(), Name = "Bob", Email = "bob@example.com" }
    };

    [HttpGet]
    public IActionResult GetUsers()
    {
        return Ok(Users);
    }

    [HttpGet("{id:guid}")]
    public IActionResult GetUserById(Guid id)
    {
        var user = Users.FirstOrDefault(u => u.Id == id);
        if (user == null)
        {
            return NotFound(new { Message = "User not found" });
        }
        return Ok(user);
    }

    [HttpPost]
    public IActionResult CreateUser([FromBody] dynamic newUser)
    {
        var user = new
        {
            Id = Guid.NewGuid(),
            newUser.Name,
            newUser.Email
        };
        Users.Add(user);
        return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, user);
    }

    [HttpPut("{id:guid}")]
    public IActionResult UpdateUser(Guid id, [FromBody] dynamic updatedUser)
    {
        var index = Users.FindIndex(u => u.Id == id);
        if (index == -1)
        {
            return NotFound(new { Message = "User not found" });
        }

        Users[index] = new
        {
            Id = id,
            updatedUser.Name,
            updatedUser.Email
        };

        return Ok(Users[index]);
    }

    [HttpDelete("{id:guid}")]
    public IActionResult DeleteUser(Guid id)
    {
        var user = Users.FirstOrDefault(u => u.Id == id);
        if (user == null)
        {
            return NotFound(new { Message = "User not found" });
        }

        Users.Remove(user);
        return NoContent();
    }
}
