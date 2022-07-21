using Lidgren.Network;
using MessagePack;
using Shared.Messages;

namespace Service.Backend.Connections;

public static class Extensions {
    public static string HexId(this NetConnection connection) {
        return NetUtility.ToHexString(connection.RemoteUniqueIdentifier);
    }

    public static void Send(this Connection connection, IMessage message) {
        var bytes = MessagePackSerializer.Serialize(message);
        connection.Send(bytes);
    }
}