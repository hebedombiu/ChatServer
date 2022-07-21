using Service.Backend.Connections;
using Shared.Messages;

namespace Service.Backend.Handlers;

public interface IHandler {
    public void Handle(Connection connection, IMessage message);
}