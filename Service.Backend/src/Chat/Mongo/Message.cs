using MongoDB.Bson.Serialization.Attributes;

namespace Service.Backend.Chat.Mongo;

public class Message : IMessage {
    [BsonId] public long Id { get; }
    [BsonElement] public string Name { get; }
    [BsonElement] public string Color { get; }
    [BsonElement] public string Text { get; }
    [BsonElement] public DateTime CreatedAt { get; }

    [BsonConstructor]
    public Message(
        long id,
        string name,
        string color,
        string text,
        DateTime createdAt
    ) {
        Id = id;
        Name = name;
        Color = color;
        Text = text;
        CreatedAt = createdAt;
    }
}