using Microsoft.AspNetCore.SignalR.Client;
using ChatSphere.Domain.DTOs;

namespace ChatSphere.Client.Services;

public class ChatService : IAsyncDisposable
{
    private readonly HubConnection _hubConnection;
    public event Action<ChatMessageDto>? OnMessageReceived;

    public List<ChatMessageDto> ChatMessages { get; private set; } = new();

    public ChatService(string hubUrl, string? token)
    {
        _hubConnection = new HubConnectionBuilder()
            .WithUrl(hubUrl, options =>
            {
                options.AccessTokenProvider = () => Task.FromResult(token);
            })
            .Build();

        _hubConnection.On<string, string>("ReceiveMessage", (sender, content) =>
        {
            var isMine = sender == "MyUsername";
            var message = new ChatMessageDto(sender, content, isMine);
            ChatMessages.Add(message);
            OnMessageReceived?.Invoke(message);
        });
    }

    public async Task ConnectAsync()
    {
        await _hubConnection.StartAsync();
    }

    public async Task SendMessageAsync(string roomId, string username, string message)
    {
        await _hubConnection.SendAsync("SendMessage", roomId, username, message);
        ChatMessages.Add(new ChatMessageDto(username, message, true));
    }

    public async Task LeaveRoomAsync(string roomId)
    {
        await _hubConnection.SendAsync("LeaveRoom", roomId);
        ChatMessages.Clear();
    }

    public async ValueTask DisposeAsync()
    {
        await _hubConnection.DisposeAsync();
    }
}
