namespace ChatSphere.Domain.Entities;

public class ChatMessage
{
    public Guid Id { get; set; }

    public string Content { get; set; }

    public string SenderUsername { get; set; }

    public string RoomId { get; set; }

    public DateTime Timestamp { get; set; }
}