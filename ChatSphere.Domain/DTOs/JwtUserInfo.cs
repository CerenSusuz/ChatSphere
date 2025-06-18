namespace ChatSphere.Domain.DTOs;
public class JwtUserInfo
{
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;

    public bool IsAdmin { get; set; }
}
