namespace ChatSphere.Domain.DTOs;

public class ChatMessageDto
{
    public ChatMessageDto(string sender, string content, bool isMine)
    {
        Sender = sender;
        Content = content;
        IsMine = isMine;
    }

    public string Sender { get; }
    public string Content { get; }
    public bool IsMine { get; }
}

