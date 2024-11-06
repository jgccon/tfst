using TheFullStackTeam.Domain.Repositories.Full;
using TheFullStackTeam.Domain.Views;
using TheFullStackTeam.Infrastructure.Repositories.MongoDB.Repositories;
using MongoDB.Driver;

namespace TheFullStackTeam.Infrastructure.Persistence.MongoDB.Repositories.Full
{
    public class UserProfileViewRepository : MongoRepository<UserProfileView>, IUserProfileViewRepository
    {
        public UserProfileViewRepository(IMongoDatabase database) : base(database, "UserProfiles")
        {
        }
    }
}
