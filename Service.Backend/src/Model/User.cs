using Service.Backend.Chat;
using Service.Backend.Connections;
using IMessage = Shared.Messages.IMessage;

namespace Service.Backend.Model;

public class User : IUser {
    private readonly Connection _connection;

    public string UserId { get; }
    public string Name { get; }
    public string Color { get; }

    public User(Connection connection, string id, string name, string color) {
        _connection = connection;
        UserId = id;
        Name = name;
        Color = color;
    }

    public void Send(IMessage message) {
        _connection.Send(message);
    }
}