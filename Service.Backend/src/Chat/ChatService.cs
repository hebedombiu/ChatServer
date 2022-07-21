using System.Collections.Concurrent;
using Service.Backend.Connections;
using Service.Backend.Model;
using Shared.Messages;

namespace Service.Backend.Chat;

public class ChatService {
    private readonly ChatRepository _repository;

    private readonly object _lock = new();

    private readonly ConcurrentDictionary<Connection, User> _connection2User = new();

    private readonly ConcurrentDictionary<string, IUser> _id2User = new();
    private readonly ConcurrentDictionary<string, bool> _id2State = new();

    public ChatService(
        ChatRepository repository
    ) {
        _repository = repository;

        var users = _repository.GetUsers();
        foreach (var user in users) {
            _id2User.TryAdd(user.Id.ToString(), user);
            _id2State.TryAdd(user.Id.ToString(), false);
        }
    }

    public void AddUser(Connection connection, string name, string color, Action<UserState[], IMessage[]> callback) {
        lock (_lock) {
            var userDb = _repository.GetUserOrCreate(name, color);

            var user = new User(connection, userDb.Id.ToString(), name, color);

            if (_connection2User.TryAdd(connection, user)) {
                _id2User.TryAdd(user.UserId, user);
                _id2State.AddOrUpdate(user.UserId, (_) => true, (_, _) => true);

                var us = _id2User.Values
                    .Select(u => new UserState {
                        User = u,
                        IsOnline = _id2State[u.UserId]
                    })
                    .ToArray();

                var lm = _repository.GetLastMessages(20);

                callback.Invoke(us, lm);

                foreach (var u in _connection2User.Values) {
                    u.Send(new UserStateNotify {
                        User = new ChatUser {UserId = user.UserId, Name = user.Name, Color = user.Color},
                        IsOnline = true
                    });
                }
            }
        }
    }

    public void RemoveUser(Connection connection) {
        lock (_lock) {
            if (_connection2User.TryRemove(connection, out var user)) {
                _id2State[user.UserId] = false;

                foreach (var u in _connection2User.Values) {
                    if (user.UserId == u.UserId) continue;
                    u.Send(new UserStateNotify {
                        User = new ChatUser {UserId = u.UserId, Name = u.Name, Color = u.Color},
                        IsOnline = false
                    });
                }
            }
        }
    }

    public void AddMessage(Connection connection, string text) {
        lock (_lock) {
            if (_connection2User.TryGetValue(connection, out var user)) {
                var message = _repository.CreateMessage(user.Name, user.Color, text);

                foreach (var u in _connection2User.Values) {
                    u.Send(new IncomeMessageNotify {
                        Message = new ChatMessage {
                            Name = message.Name,
                            Color = message.Color,
                            Text = message.Text,
                            Time = message.CreatedAt.ToUnixMilliseconds()
                        }
                    });
                }
            }
        }
    }
}