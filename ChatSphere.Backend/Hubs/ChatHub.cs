using ChatSphere.Domain.Entities;
using ChatSphere.Infrastructure.Database;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;

namespace ChatSphere.Backend.Hubs;

public class ChatHub : Hub
{
    private readonly IServiceProvider _serviceProvider;

    public ChatHub(IServiceProvider serviceProvider) => _serviceProvider = serviceProvider;

    public override Task OnConnectedAsync()
    {
        var userId = Context.User?.FindFirst("id")?.Value;

        if (userId == null)
        {
            Context.Abort();
        }
        else
        {
            Console.WriteLine($"User {userId} connected.");
        }

        return base.OnConnectedAsync();
    }


    public async Task SendMessage(string roomId, string username, string message)
    {
        var chatMessage = new ChatMessage
        {
            Id = Guid.NewGuid(),
            Content = message,
            SenderUsername = username,
            RoomId = roomId,
            Timestamp = DateTime.UtcNow
        };

        using (var scope = _serviceProvider.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<ChatSphereDbContext>();
            await dbContext.ChatMessages.AddAsync(chatMessage);
            await dbContext.SaveChangesAsync();
        }

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
