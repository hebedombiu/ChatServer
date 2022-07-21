using MongoDB.Bson.Serialization.Attributes;

namespace Service.Backend.Chat.Mongo;

public class Counter {
    [BsonId] public string Id { get; }
    [BsonElement] public long Value { get; }
    [BsonElement] public DateTime DateTime { get; }

    [BsonConstructor]
    private Counter(
        string id,
        long value,
        DateTime dateTime
    ) {
        Id = id;
        Value = value;
        DateTime = dateTime;
    }
}