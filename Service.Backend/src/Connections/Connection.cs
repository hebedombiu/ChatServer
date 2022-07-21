using Lidgren.Network;

namespace Service.Backend.Connections;

public class Connection {
    private readonly NetConnection _connection;

    public Connection(NetConnection connection) {
        _connection = connection;
    }

    public string Id => _connection.HexId();
    public bool IsConnected => _connection.Status == NetConnectionStatus.Connected;

    public void Send(byte[] bytes) {
        var message = _connection.Peer.CreateMessage();

        message.Write(bytes.Length);
        message.Write(bytes);

        _connection.SendMessage(message, NetDeliveryMethod.ReliableOrdered, 0);
    }
}