using System.Collections;
using Service.Backend.Connections;
using Service.Backend.Model;
using Shared.Messages;

namespace Service.Backend.Handlers;

public class HandlerSet : IEnumerable {
    private readonly Dictionary<Type, IHandler> _handlers = new();

    public void Add<T>(Handler<T> handler) where T : IMessage {
        _handlers.Add(typeof(T), handler);
    }

    public void Handle<T>(Connection connection, T message) where T : IMessage {
        if (_handlers.TryGetValue(message.GetType(), out var handler)) {
            handler.Handle(connection, message);
        } else {
            throw new Exception($"Handler for {typeof(T)} not found");
        }
    }

    public IEnumerator GetEnumerator() {
        return _handlers.GetEnumerator();
    }
}