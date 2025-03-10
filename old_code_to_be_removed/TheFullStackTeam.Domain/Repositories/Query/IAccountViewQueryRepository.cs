using TheFullStackTeam.Domain.Views;

namespace TheFullStackTeam.Domain.Repositories.Query;

public interface IAccountViewQueryRepository : IQueryRepository<AccountView>
{
    Task<AccountView?> GetByNameAsync(string name);
}
