using ChatSphere.Domain.DTOs;
using System.Net.Http.Json;

namespace ChatSphere.Client.Services;

public class RoomService : IRoomService
{
    private readonly HttpClient _http;

    public RoomService(HttpClient http)
    {
        _http = http;
    }

    public async Task<List<RoomDto>> GetRoomsAsync()
    {
        return await _http.GetFromJsonAsync<List<RoomDto>>("api/rooms") ?? new();
    }
}
