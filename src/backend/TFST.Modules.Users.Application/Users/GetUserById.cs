using MediatR;
using Microsoft.EntityFrameworkCore;
using TFST.Modules.Users.Application.Models;
using TFST.Modules.Users.Persistence;
using TFST.Persistence;

namespace TFST.Modules.Users.Application.Users;

public record GetUserByIdQuery(Guid UserId) : IRequest<UserProjection>;

public class GetUserByIdHandler : IRequestHandler<GetUserByIdQuery, UserProjection>
{
    private readonly UsersDbContext _dbContext;

    public GetUserByIdHandler(UsersDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<UserProjection> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await _dbContext.Users
            .Where(u => u.Id == request.UserId)
            .Select(u => new UserProjection(
                u.Id,
                u.Email,
                u.FirstName,
                u.LastName,
                u.Roles.Select(r => r.Name).ToList()
            ))
            .FirstOrDefaultAsync(cancellationToken);

        if (user is null) throw new KeyNotFoundException($"User with ID {request.UserId} not found.");

        return user;
    }
}
