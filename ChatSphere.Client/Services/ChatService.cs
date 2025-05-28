using Microsoft.AspNetCore.SignalR.Client;
using System.Net.Http.Json;
using ChatSphere.Domain.DTOs;
using ChatSphere.Domain.Entities;

namespace ChatSphere.Client.Services;

public class ChatService : IChatService, IAsyncDisposable
{
    private readonly string _hubUrl;
    private string _token = string.Empty;
    private HubConnection? _connection;
    private readonly HttpClient _httpClient;

    public ChatService(string hubUrl, HttpClient httpClient)
    {
        _hubUrl = hubUrl;
        _httpClient = httpClient;
        ChatMessages = new List<ChatMessage>();
        UsersInCurrentRoom = new List<string>();
    }

    public List<ChatMessage> ChatMessages { get; private set; }
    public List<string> UsersInCurrentRoom { get; private set; }
    public bool IsConnected => _connection?.State == HubConnectionState.Connected;

    public event Action? OnMessageReceived;
    public event Action? OnUsersUpdated;

    public void SetToken(string token) => _token = token;

    public async Task ConnectAsync()
    {
        if (_connection != null && _connection.State == HubConnectionState.Connected)
            return;

        _connection = new HubConnectionBuilder()
            .WithUrl(_hubUrl, options =>
            {
                options.AccessTokenProvider = () => Task.FromResult(_token);
            })
            .WithAutomaticReconnect()
            .Build();

        _connection.On<ChatMessage>("ReceiveMessage", message =>
        {
            ChatMessages.Add(message);
            OnMessageReceived?.Invoke();
        });

        _connection.On<List<string>>("UserListUpdated", users =>
        {
            UsersInCurrentRoom = users;
            OnUsersUpdated?.Invoke();
        });

        await _connection.StartAsync();
    }

    public async Task JoinRoomAsync(Guid roomId)
    {
        if (_connection != null && _connection.State == HubConnectionState.Connected)
        {
            await _connection.SendAsync("JoinRoom", roomId.ToString());
        }
    }

    public async Task LeaveRoomAsync(Guid roomId)
    {
        if (_connection != null && _connection.State == HubConnectionState.Connected)
        {
            await _connection.SendAsync("LeaveRoom", roomId.ToString());
            ChatMessages.Clear();
            UsersInCurrentRoom.Clear();
        }
    }

    public async Task SendMessageAsync(Guid roomId, string sender, string message)
    {
        if (_connection != null && _connection.State == HubConnectionState.Connected)
        {
            await _connection.SendAsync("SendMessage", roomId, sender, message);
            ChatMessages.Add(new ChatMessage
            {
                SenderUsername = sender,
                Content = message,
                RoomId = roomId,
                Timestamp = DateTime.UtcNow
            });
            OnMessageReceived?.Invoke();
        }
    }

    public async Task<List<RoomDto>> GetAvailableRoomsAsync()
    {
        if (string.IsNullOrEmpty(_token))
            throw new InvalidOperationException("JWT token is not set.");

        _httpClient.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _token);

        var response = await _httpClient.GetFromJsonAsync<List<RoomDto>>("api/rooms");
        return response ?? new List<RoomDto>();
    }


    public async ValueTask DisposeAsync()
    {
        if (_connection != null)
        {
            await _connection.StopAsync();
            await _connection.DisposeAsync();
            _connection = null;
        }
    }

}
