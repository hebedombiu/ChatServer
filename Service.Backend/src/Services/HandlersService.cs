using Service.Backend.Connections;
using Service.Backend.Handlers;
using Shared.Messages;

namespace Service.Backend.Services;

public class HandlersService {
    private readonly HandlerSet _handlers;

    public HandlersService(
        ConnectionHandler connectionHandler,
        SendMessageHandler sendMessageHandler
    ) {
        _handlers = new HandlerSet {
            connectionHandler,
            sendMessageHandler
        };
    }

    public void Handle<T>(Connection connection, T message) where T : IMessage {
        _handlers.Handle(connection, message);
    }
}