using Service.Backend.Connections;
using Shared.Messages;

namespace Service.Backend.Handlers;

public abstract class Handler<T> : IHandler where T : IMessage {
    public void Handle(Connection connection, IMessage message) {
        Handle(connection, (T) message);
    }

    protected abstract void Handle(Connection connection, T message);
}