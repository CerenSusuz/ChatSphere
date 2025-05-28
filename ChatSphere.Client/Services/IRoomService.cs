using ChatSphere.Domain.DTOs;

namespace ChatSphere.Client.Services;

public interface IRoomService
{
    Task<List<RoomDto>> GetRoomsAsync();
}
