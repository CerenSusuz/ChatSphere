namespace ChatSphere.Domain.Entities;

public class Room
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public bool IsActive { get; set; } = true;

    public ICollection<ChatUserRoom> Users { get; set; } = new List<ChatUserRoom>();

    public ICollection<ChatMessage> Messages { get; set; }

    public Room(string name, string description)
    {
        Id = Guid.NewGuid();
        Name = name;
        Description = description;
    }

    public Room() { }
}
