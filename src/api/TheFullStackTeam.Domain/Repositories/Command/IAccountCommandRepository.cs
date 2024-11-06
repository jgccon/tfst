using TheFullStackTeam.Domain.Entities;

namespace TheFullStackTeam.Domain.Repositories.Command;

public interface IAccountCommandRepository : ICommandRepository<Account>
{
    Task<Account?> GetByEmailAsync(string username);
}
