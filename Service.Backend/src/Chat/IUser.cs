using MongoDB.Bson;

namespace Service.Backend.Chat;

public interface IUser {
    public string UserId { get; }
    public string Name { get; }
    public string Color { get; }
}