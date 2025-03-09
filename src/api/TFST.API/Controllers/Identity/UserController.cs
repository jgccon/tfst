using Microsoft.AspNetCore.Mvc;
using TFST.API.Controllers.Base;

namespace TFST.API.Controllers.Identity;

[Tags("Identity")]
public class UsersController : ApiControllerBase
{
    private static readonly List<dynamic> Users = new()
    {
        new { Id = Guid.NewGuid(), Name = "Alice", Email = "alice@example.com" },
        new { Id = Guid.NewGuid(), Name = "Bob", Email = "bob@example.com" }
    };

    // GET /users → Listar todos los usuarios
    [HttpGet]
    public IActionResult GetUsers()
    {
        return Ok(Users);
    }

    // GET /users/{id} → Obtener un usuario por ID
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

    // POST /users → Crear un nuevo usuario
    [HttpPost]
    public IActionResult CreateUser([FromBody] dynamic newUser)
    {
        var user = new
        {
            Id = Guid.NewGuid(),
            Name = newUser.Name,
            Email = newUser.Email
        };
        Users.Add(user);
        return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, user);
    }

    // PUT /users/{id} → Actualizar un usuario existente
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
            Name = updatedUser.Name,
            Email = updatedUser.Email
        };

        return Ok(Users[index]);
    }

    // DELETE /users/{id} → Eliminar un usuario
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
