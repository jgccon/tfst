using TheFullStackTeam.Domain.Repositories.Full;
using TheFullStackTeam.Domain.Views;
using TheFullStackTeam.Infrastructure.Repositories.MongoDB.Repositories;
using MongoDB.Driver;

namespace TheFullStackTeam.Infrastructure.Persistence.MongoDB.Repositories.Full
{
    public class AccountViewRepository : MongoRepository<AccountView>, IAccountViewRepository
    {
        public AccountViewRepository(IMongoDatabase database) : base(database, "Accounts")
        {
        }

        public async Task<AccountView?> GetByNameAsync(string name)
        {
            var filter = Builders<AccountView>.Filter.Eq(u => u.LoginName, name);
            return await _collection.Find(filter).FirstOrDefaultAsync();
        }
    }
}
