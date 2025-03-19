using MediatR;
using Microsoft.EntityFrameworkCore;
using TFST.Modules.Users.Application.Models;
using TFST.Modules.Users.Application.Expressions;
using TFST.Modules.Users.Persistence;

namespace TFST.Modules.Users.Application.Users;

public record GetUserByMonikerQuery(string Moniker) : IRequest<User>;

public class GetUserByMonikerHandler : IRequestHandler<GetUserByMonikerQuery, User>
{
    private readonly UsersDbContext _dbContext;

    public GetUserByMonikerHandler(UsersDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<User> Handle(GetUserByMonikerQuery request, CancellationToken cancellationToken)
    {
        var user = await _dbContext.Users
            .Where(u => u.Moniker == request.Moniker)
            .Select(UserExpressions.Projection)
            .FirstOrDefaultAsync(cancellationToken);

        if (user is null)
            throw new KeyNotFoundException($"User with moniker '{request.Moniker}' not found.");

        return user;
    }
}
