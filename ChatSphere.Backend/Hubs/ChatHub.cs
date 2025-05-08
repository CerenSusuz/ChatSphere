using Microsoft.AspNetCore.SignalR;

namespace ChatSphere.Backend.Hubs;

public class ChatHub : Hub
{
    public async Task SendMessage(string roomId, string username, string message)
    {
        await Clients.Group(roomId).SendAsync("ReceiveMessage", username, message);
    }

    public async Task JoinRoom(string roomId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, roomId);
        await Clients.Group(roomId).SendAsync("ReceiveNotification", $"{Context.ConnectionId} joined the room.");
    }

    public async Task LeaveRoom(string roomId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomId);
        await Clients.Group(roomId).SendAsync("ReceiveNotification", $"{Context.ConnectionId} left the room.");
    }
}