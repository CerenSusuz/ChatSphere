namespace ChatSphere.Domain.DTOs;
public class JwtUserInfo
{
    public string Username { get; set; } = "";
    public string Email { get; set; } = "";
    public bool IsAdmin { get; set; } = false;

    public bool IsValid => !string.IsNullOrWhiteSpace(Username);
}
