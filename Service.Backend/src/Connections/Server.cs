using System.Collections.Concurrent;
using Lidgren.Network;

namespace Service.Backend.Connections;

public class Server {
    private readonly ILogger _logger;
    private readonly NetServer _server;

    private readonly ConcurrentDictionary<NetConnection, Connection> _connections = new();

    public event Action<Connection>? ConnectedEvent;
    public event Action<Connection>? DisconnectedEvent;
    public event Action<Connection, byte[]>? IncomingDataEvent;

    public Server(ILogger logger, string appIdentifier, int port) {
        _logger = logger;
        var configuration = new NetPeerConfiguration(appIdentifier) {
            Port = port
        };
        _server = new NetServer(configuration);
    }

    public void Start() {
        _server.Start();

        var thread = new Thread(Run) {
            Name = "Server incoming thread",
            IsBackground = true
        };
        thread.Start();
    }

    private void Run() {
        while (_server.Status != NetPeerStatus.NotRunning) {
            while (_server.ReadMessage(out var message)) {
                try {
                    HandleIncomingMessage(message);
                } finally {
                    _server.Recycle(message);
                }
            }

            _server.MessageReceivedEvent.WaitOne();
        }
    }

    private void HandleIncomingMessage(NetIncomingMessage message) {
        switch (message.MessageType) {
            case NetIncomingMessageType.StatusChanged:
                HandleStatusChanged(message);
                break;
            case NetIncomingMessageType.Data:
                HandleData(message);
                break;
        }
    }

    private void HandleStatusChanged(NetIncomingMessage message) {
        var status = (NetConnectionStatus) message.ReadByte();
        var reason = message.ReadString();

        switch (status) {
            case NetConnectionStatus.Connected:
                HandleConnected(message);
                break;
            case NetConnectionStatus.Disconnected:
                HandleDisconnected(message);
                break;
        }
    }

    private void HandleConnected(NetIncomingMessage message) {
        var connection = new Connection(message.SenderConnection);
        _connections.TryAdd(message.SenderConnection, connection);
        ConnectedEvent?.Invoke(connection);
    }

    private void HandleDisconnected(NetIncomingMessage message) {
        if (_connections.TryRemove(message.SenderConnection, out var connection)) {
            DisconnectedEvent?.Invoke(connection);
        }
    }

    private void HandleData(NetIncomingMessage message) {
        var length = message.ReadInt32();
        var bytes = message.ReadBytes(length);

        if (_connections.TryGetValue(message.SenderConnection, out var connection)) {
            IncomingDataEvent?.Invoke(connection, bytes);
        }
    }
}