using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Service.Backend.Chat.Mongo;

public class User : IUser {
    [BsonId] public ObjectId Id { get; }
    [BsonElement] public string Name { get; }
    [BsonElement] public string Color { get; }

    public string UserId => Id.ToString();

    [BsonConstructor]
    public User(
        ObjectId id,
        string name,
        string color
    ) {
        Id = id;
        Name = name;
        Color = color;
    }
}