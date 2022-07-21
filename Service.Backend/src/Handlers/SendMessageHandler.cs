using Service.Backend.Chat;
using Service.Backend.Connections;
using Shared.Messages;

namespace Service.Backend.Handlers;

public class SendMessageHandler : Handler<SendMessageRequest> {
    private readonly ChatService _chatService;

    public SendMessageHandler(
        ChatService _chatService
    ) {
        this._chatService = _chatService;
    }

    protected override void Handle(Connection connection, SendMessageRequest message) {
        _chatService.AddMessage(connection, message.Text);
    }
}