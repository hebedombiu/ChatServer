using System.Text.Json;
using Service.Backend.Chat;
using Service.Backend.Connections;
using Service.Backend.Services;
using Shared.Messages;

namespace Service.Backend.Handlers;

public class ConnectionHandler : Handler<ConnectRequest> {
    private readonly ILogger<ConnectionHandler> _logger;
    private readonly StaticService _staticService;
    private readonly ChatService _chatService;

    public ConnectionHandler(
        ILogger<ConnectionHandler> logger,
        StaticService staticService,
        ChatService chatService
    ) {
        _logger = logger;
        _staticService = staticService;
        _chatService = chatService;
    }

    protected override void Handle(Connection connection, ConnectRequest message) {
        _logger.LogInformation(JsonSerializer.Serialize(message));

        if (!_staticService.HasColor($"#{message.Color}")) {
            connection.Send(new ConnectResponse {Parent = message.Id, IsSuccess = false, Message = "wrong color"});
            return;
        }

        _chatService.AddUser(connection, message.Name, $"#{message.Color}", (users, messages) => {
            connection.Send(new ConnectResponse {
                Parent = message.Id,
                IsSuccess = true,
                UserStates = users.Select(u => new UserStateNotify {
                    User = new ChatUser {
                        UserId = u.User.UserId,
                        Name = u.User.Name,
                        Color = u.User.Color
                    },
                    IsOnline = u.IsOnline
                }).ToArray(),
                Messages = messages.Select(m => new ChatMessage {
                    Name = m.Name,
                    Color = m.Color,
                    Text = m.Text,
                    Time = m.CreatedAt.ToUnixMilliseconds()
                }).ToArray()
            });
        });
    }
}