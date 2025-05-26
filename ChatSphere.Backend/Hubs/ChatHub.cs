using ChatSphere.Domain.Entities;
using ChatSphere.Infrastructure.Database;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace ChatSphere.Backend.Hubs;

public class ChatHub(ChatSphereDbContext context) : Hub
{
    private readonly ChatSphereDbContext _context = context;

    public async Task JoinRoom(string roomId)
    {
        if (!Guid.TryParse(roomId, out var parsedRoomId))
            return;

        var username = Context.User?.Identity?.Name;
        if (string.IsNullOrWhiteSpace(username)) return;

        var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
        if (user == null) return;

        var alreadyJoined = await _context.ChatUserRooms
            .AnyAsync(x => x.RoomId == parsedRoomId && x.UserId == user.Id);

        if (!alreadyJoined)
        {
            _context.ChatUserRooms.Add(new ChatUserRoom
            {
                RoomId = parsedRoomId,
                UserId = user.Id
            });
            await _context.SaveChangesAsync();
        }

        await Groups.AddToGroupAsync(Context.ConnectionId, roomId);

        var usernames = await _context.ChatUserRooms
            .Where(x => x.RoomId == parsedRoomId)
            .Include(x => x.User)
            .Select(x => x.User.Username)
            .ToListAsync();

        await Clients.Group(roomId).SendAsync("UserListUpdated", usernames);
    }

    public async Task LeaveRoom(string roomId)
    {
        if (!Guid.TryParse(roomId, out var parsedRoomId))
            return;

        var username = Context.User?.Identity?.Name;
        if (string.IsNullOrWhiteSpace(username)) return;

        var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
        if (user == null) return;

        var entry = await _context.ChatUserRooms
            .FirstOrDefaultAsync(x => x.RoomId == parsedRoomId && x.UserId == user.Id);

        if (entry != null)
        {
            _context.ChatUserRooms.Remove(entry);
            await _context.SaveChangesAsync();
        }

        await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomId);

        var usernames = await _context.ChatUserRooms
            .Where(x => x.RoomId == parsedRoomId)
            .Include(x => x.User)
            .Select(x => x.User.Username)
            .ToListAsync();

        await Clients.Group(roomId).SendAsync("UserListUpdated", usernames);
    }

    public async Task SendMessage(Guid roomId, string sender, string message)
    {
        var chatMessage = new ChatMessage(sender, message, roomId);
        _context.ChatMessages.Add(chatMessage);
        await _context.SaveChangesAsync();

        await Clients.Group(roomId.ToString()).SendAsync("ReceiveMessage", chatMessage);
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var username = Context.User?.Identity?.Name;
        if (string.IsNullOrWhiteSpace(username)) return;

        var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
        if (user == null) return;

        var userRooms = await _context.ChatUserRooms
            .Where(x => x.UserId == user.Id)
            .ToListAsync();

        _context.ChatUserRooms.RemoveRange(userRooms);
        await _context.SaveChangesAsync();

        foreach (var room in userRooms)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, room.RoomId.ToString());

            var usernames = await _context.ChatUserRooms
                .Where(x => x.RoomId == room.RoomId)
                .Include(x => x.User)
                .Select(x => x.User.Username)
                .ToListAsync();

            await Clients.Group(room.RoomId.ToString()).SendAsync("UserListUpdated", usernames);
        }

        await base.OnDisconnectedAsync(exception);
    }
}
