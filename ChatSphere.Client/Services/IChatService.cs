using ChatSphere.Domain.DTOs;
using ChatSphere.Domain.Entities;

namespace ChatSphere.Client.Services;

public interface IChatService : IAsyncDisposable
{
    List<ChatMessage> ChatMessages { get; }
    List<string> UsersInCurrentRoom { get; }

    bool IsConnected { get; }

    event Action OnMessageReceived;
    event Action OnUsersUpdated;

    void SetToken(string token);
    Task ConnectAsync();
    Task JoinRoomAsync(Guid roomId);
    Task LeaveRoomAsync(Guid roomId);
    Task SendMessageAsync(Guid roomId, string sender, string message);
    Task<List<RoomDto>> GetAvailableRoomsAsync();

}
