namespace ChatSphere.Client.Services;

public interface IAuthService
{
    Task<string?> LoginAsync(string email, string password);

    Task<bool> RegisterAsync(string email, string password, string username);

    Task<string?> GetTokenAsync();

    Task LogoutAsync();
}
