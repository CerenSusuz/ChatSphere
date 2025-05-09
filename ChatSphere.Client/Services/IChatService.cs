using ChatSphere.Domain.Entities;

namespace ChatSphere.Client.Services;

public interface IChatService
{
    Task ConnectAsync();

    Task JoinRoomAsync(string roomId);

    Task SendMessageAsync(string roomId, string sender, string message);

    IEnumerable<ChatMessage> ChatMessages { get; }

    bool IsConnected { get; }

    event Action OnMessageReceived;

    Task LeaveRoomAsync(string roomId);

    void SetToken(string token);
}
