using System.Text.Json;
using MessagePack;
using Microsoft.Extensions.Options;
using Service.Backend.Chat;
using Service.Backend.Handlers;
using Service.Backend.Options;
using Service.Backend.Services;
using IMessage = Shared.Messages.IMessage;

namespace Service.Backend.Connections;

public class ServerService {
    private readonly ILogger<ServerService> _logger;
    private readonly HandlersService _handlers;
    private readonly ChatService _chatService;
    private readonly Server _server;

    public ServerService(
        ILogger<ServerService> logger,
        IOptions<ServerOptions> options,
        HandlersService handlers,
        ChatService chatService
    ) {
        _logger = logger;
        _handlers = handlers;
        _chatService = chatService;
        _server = new Server(_logger, options.Value.AppIdentifier, options.Value.Port);

        _server.ConnectedEvent += ServerOnConnectedEvent;
        _server.DisconnectedEvent += ServerOnDisconnectedEvent;
        _server.IncomingDataEvent += ServerOnIncomingDataEvent;

        _server.Start();
        _logger.LogInformation($"Server started: {options.Value.AppIdentifier} {options.Value.Port}");
    }

    private void ServerOnConnectedEvent(Connection connection) { }

    private void ServerOnDisconnectedEvent(Connection connection) {
        _chatService.RemoveUser(connection);
    }

    private void ServerOnIncomingDataEvent(Connection connection, byte[] bytes) {
        var message = MessagePackSerializer.Deserialize<IMessage>(bytes);
        _handlers.Handle(connection, message);
    }
}