namespace Service.Backend.Chat;

public interface IMessage {
    public string Name { get; }
    public string Color { get; }
    public string Text { get; }
    public DateTime CreatedAt { get; }
}