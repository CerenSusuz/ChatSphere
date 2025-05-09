using Microsoft.AspNetCore.SignalR.Client;
using ChatSphere.Domain.Entities;

namespace ChatSphere.Client.Services;

public class ChatService : IChatService
{
    private readonly string _hubUrl;
    private string _token;
    private HubConnection? _connection;

    public ChatService(string hubUrl)
    {
        _hubUrl = hubUrl;
        ChatMessages = new List<ChatMessage>();
    }

    public List<ChatMessage> ChatMessages { get; private set; }

    public bool IsConnected => _connection?.State == HubConnectionState.Connected;

    IEnumerable<ChatMessage> IChatService.ChatMessages => ChatMessages;

    public event Action? OnMessageReceived;

    public void SetToken(string token) => _token = token;

    public async Task ConnectAsync()
    {
        if (_connection == null)
        {
            _connection = new HubConnectionBuilder()
                .WithUrl(_hubUrl, options =>
                {
                    options.AccessTokenProvider = () => Task.FromResult(_token);
                })
                .Build();

            _connection.On<ChatMessage>("ReceiveMessage", message =>
            {
                ChatMessages.Add(message);
                OnMessageReceived?.Invoke();
            });

            await _connection.StartAsync();
        }
    }

    public async Task JoinRoomAsync(string roomId)
    {
        if (_connection != null)
        {
            await _connection.SendAsync("JoinRoom", roomId);
        }
    }

    public async Task SendMessageAsync(string roomId, string sender, string message)
    {
        if (_connection != null)
        {
            await _connection.SendAsync("SendMessage", roomId, sender, message);
            ChatMessages.Add(new ChatMessage
            {
                SenderUsername = sender,
                Content = message,
                RoomId = roomId,
                Timestamp = DateTime.UtcNow,
            });
            OnMessageReceived?.Invoke();
        }
    }

    public async Task LeaveRoomAsync(string roomId)
    {
        if (_connection != null)
        {
            await _connection.SendAsync("LeaveRoom", roomId);
            ChatMessages.Clear();
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (_connection != null)
        {
            await _connection.DisposeAsync();
            _connection = null;
        }
    }
}