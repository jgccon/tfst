using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using TheFullStackTeam.Common.Configuration;
using TheFullStackTeam.Domain.Views;

namespace TheFullStackTeam.Infrastructure.Persistence.MongoDB;
public class MongoDbWrapper
{
    private readonly Lazy<IMongoDatabase> _databaseLazy;
    private readonly ILogger<MongoDbWrapper> _logger;
    private readonly string _databaseName;
    private readonly MongoDbSettings _mongoSettings;

    public MongoDbWrapper(IConfiguration configuration, ILogger<MongoDbWrapper> logger)
    {
        _logger = logger;
        _mongoSettings = new MongoDbSettings();
        configuration.GetSection("MongoDbSettings").Bind(_mongoSettings);
        _databaseName = _mongoSettings.DatabaseName;

        _databaseLazy = new Lazy<IMongoDatabase>(() =>
        {
            try
            {
                _logger.LogInformation("Initializing MongoDB database...");
                var settings = MongoClientSettings.FromConnectionString(_mongoSettings.ConnectionString);
                var mongoClient = new MongoClient(settings);
                var database = mongoClient.GetDatabase(_databaseName);
                _logger.LogInformation("MongoDB database initialized.");
                return database;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to initialize MongoDB database.");
                return null;
            }
        });
    }

    public IMongoDatabase Database => _databaseLazy.Value;

    public bool IsDatabaseInitialized => _databaseLazy.IsValueCreated && _databaseLazy.Value != null;

    public async Task EnsureDatabaseIsReady()
    {
        if (!IsDatabaseInitialized)
        {
            _logger.LogWarning("MongoDB is not initialized. Skipping database readiness check.");
            return;
        }

        try
        {
            _logger.LogInformation("Pinging MongoDB...");
            var result = await Database.RunCommandAsync((Command<BsonDocument>)"{ping:1}");
            _logger.LogInformation("Successfully connected to MongoDB.");

            await InitializeIndexes();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to initialize MongoDB.");
        }
    }

    private async Task InitializeIndexes()
    {
        try
        {
            // Index AccountView
            var accountCollection = Database.GetCollection<AccountView>("Accounts");
            var accountIndexKeys = Builders<AccountView>.IndexKeys
                .Ascending(u => u.EntityId)
                .Ascending(u => u.Version);
            var accountIndexModel = new CreateIndexModel<AccountView>(accountIndexKeys);
            await accountCollection.Indexes.CreateOneAsync(accountIndexModel);

            // Index UserProfileView
            var userProfileCollection = Database.GetCollection<UserProfileView>("UserProfiles");
            var userProfileIndexKeys = Builders<UserProfileView>.IndexKeys
                .Ascending(up => up.EntityId)
                .Ascending(up => up.Version);
            var userProfileIndexModel = new CreateIndexModel<UserProfileView>(userProfileIndexKeys);
            await userProfileCollection.Indexes.CreateOneAsync(userProfileIndexModel);

            _logger.LogInformation("MongoDB indexes initialized successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to initialize MongoDB indexes.");
        }
    }
}

