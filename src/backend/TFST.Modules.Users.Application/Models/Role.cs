namespace TFST.Modules.Users.Application.Models;

public record Role(Guid Id, string Name)
{
    public Role() : this(Guid.Empty, string.Empty) { }
}
