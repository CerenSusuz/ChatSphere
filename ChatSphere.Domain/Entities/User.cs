namespace ChatSphere.Domain.Entities;

public class User
{
    public Guid Id { get; set; }

    public string Username { get; set; }

    public string Email { get; set; }

    public string PasswordHash { get; set; }

    public bool IsBanned { get; set; } = false;

    public string Role { get; set; } = "User";

    public bool IsAdmin { get; set; }

    public ICollection<ChatUserRoom> Rooms { get; set; } = new List<ChatUserRoom>();

}
