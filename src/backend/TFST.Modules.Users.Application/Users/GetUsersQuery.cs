using MediatR;
using Microsoft.EntityFrameworkCore;
using TFST.Modules.Users.Application.Models;
using TFST.Modules.Users.Application.Expressions;
using TFST.Modules.Users.Persistence;

namespace TFST.Modules.Users.Application.Users;

public record GetUsersQuery() : IRequest<List<User>>;

public class GetUsersHandler : IRequestHandler<GetUsersQuery, List<User>>
{
    private readonly UsersDbContext _dbContext;

    public GetUsersHandler(UsersDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<User>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        return await _dbContext.Users
            .Where(u => !u.IsDeleted)
            .Select(UserExpressions.Projection)
            .ToListAsync(cancellationToken);
    }
}
