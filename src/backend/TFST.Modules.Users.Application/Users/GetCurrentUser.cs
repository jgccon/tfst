using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TFST.Modules.Users.Application.Models;
using TFST.Modules.Users.Application.Expressions;
using TFST.Modules.Users.Persistence;

namespace TFST.Modules.Users.Application.Users;

public record GetCurrentUserQuery(ClaimsPrincipal User) : IRequest<User>;

public class GetCurrentUserHandler : IRequestHandler<GetCurrentUserQuery, User>
{
    private readonly UsersDbContext _dbContext;

    public GetCurrentUserHandler(UsersDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<User> Handle(GetCurrentUserQuery request, CancellationToken cancellationToken)
    {
        var userIdClaim = request.User.FindFirst(ClaimTypes.NameIdentifier);

        if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
        {
            throw new UnauthorizedAccessException("Invalid user token.");
        }

        var user = await _dbContext.Users
            .Where(u => u.Id == userId)
            .Select(UserExpressions.Projection)
            .FirstOrDefaultAsync(cancellationToken);

        if (user is null)
            throw new KeyNotFoundException("User not found.");

        return user;
    }
}
