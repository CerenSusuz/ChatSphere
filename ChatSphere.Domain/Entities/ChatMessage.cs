namespace ChatSphere.Domain.Entities;

public class ChatMessage
{
    public Guid Id { get; set; }

    public string Content { get; set; }

    public string SenderUsername { get; set; }

    public Room Room { get; set; }

    public Guid RoomId { get; set; }

    public DateTime Timestamp { get; set; }

    public bool IsMine { get; set; }

    public ChatMessage(string senderUsername, string content, Guid roomId)
    {
        Id = Guid.NewGuid();
        SenderUsername = senderUsername;
        Content = content;
        RoomId = roomId;
        Timestamp = DateTime.UtcNow;
        IsMine = false;
    }

    public ChatMessage() { }
}
