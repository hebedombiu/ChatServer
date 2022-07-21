using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Service.Backend.Options;

namespace Service.Backend.Services;

public class MongoService {
    private readonly IMongoDatabase _database;

    public MongoService(IOptions<MongoOptions> options) {
        var client = new MongoClient(options.Value.ConnectionString);
        _database = client.GetDatabase(options.Value.Database);
    }

    public IMongoCollection<T> GetCollection<T>(string name) {
        return _database.GetCollection<T>(name);
    }
}