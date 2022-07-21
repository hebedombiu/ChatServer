using MongoDB.Driver;
using Service.Backend.Chat.Mongo;
using Service.Backend.Services;

namespace Service.Backend.Chat;

public class ChatRepository {
    private readonly IMongoCollection<Counter> _counters;
    private readonly IMongoCollection<Message> _messages;
    private readonly IMongoCollection<User> _users;

    public ChatRepository(
        MongoService mongoService
    ) {
        _counters = mongoService.GetCollection<Counter>("counters");
        _messages = mongoService.GetCollection<Message>("messages");
        _users = mongoService.GetCollection<User>("users");

        _messages.Indexes.CreateOne(new CreateIndexModel<Message>(
            Builders<Message>.IndexKeys.Descending(m => m.CreatedAt)
        ));

        _users.Indexes.CreateOne(new CreateIndexModel<User>(
            Builders<User>.IndexKeys.Combine(
                Builders<User>.IndexKeys.Ascending(u => u.Name),
                Builders<User>.IndexKeys.Ascending(u => u.Color)
            ),
            new CreateIndexOptions {Unique = true}
        ));
    }

    private Counter IncrementCounter(string id) {
        return _counters.FindOneAndUpdate(
            Builders<Counter>.Filter.Eq(c => c.Id, id),
            Builders<Counter>.Update
                .Inc(c => c.Value, 1)
                .CurrentDate(c => c.DateTime)
            ,
            new FindOneAndUpdateOptions<Counter> {IsUpsert = true, ReturnDocument = ReturnDocument.After}
        );
    }

    public IMessage CreateMessage(string name, string color, string text) {
        var counter = IncrementCounter(_messages.CollectionNamespace.FullName);

        var message = new Message(
            counter.Value,
            name,
            color,
            text,
            counter.DateTime
        );

        _messages.InsertOne(message);

        return message;
    }

    public User GetUserOrCreate(string name, string color) {
        return _users.FindOneAndUpdate(
            Builders<User>.Filter.And(
                Builders<User>.Filter.Eq(u => u.Name, name),
                Builders<User>.Filter.Eq(u => u.Color, color)
            ),
            Builders<User>.Update
                .SetOnInsert(u => u.Name, name)
                .SetOnInsert(u => u.Color, color)
            ,
            new FindOneAndUpdateOptions<User> {IsUpsert = true, ReturnDocument = ReturnDocument.After}
        );
    }

    public List<User> GetUsers() {
        return _users.Find(_ => true).ToList();
    }

    public IMessage[] GetLastMessages(int count) {
        return _messages.Find(_ => true)
            .Sort(Builders<Message>.Sort.Descending(m => m.CreatedAt))
            .Limit(count)
            .ToList()
            .Select(m => (IMessage) m)
            .ToArray();
    }
}