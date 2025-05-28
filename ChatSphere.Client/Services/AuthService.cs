using System.Net.Http.Json;
using Microsoft.JSInterop;

namespace ChatSphere.Client.Services;

public class AuthService : IAuthService
{
    private readonly HttpClient _httpClient;
    private readonly IJSRuntime _js;

    public AuthService(HttpClient httpClient, IJSRuntime js)
    {
        _httpClient = httpClient;
        _js = js;
    }

    public async Task<string?> LoginAsync(string email, string password)
    {
        try
        {
            var loginData = new
            {
                Email = email,
                Password = password
            };

            var response = await _httpClient.PostAsJsonAsync("http://localhost:1234/api/auth/login", loginData);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>();

                if (result != null && result.TryGetValue("token", out var token))
                {
                    await _js.InvokeVoidAsync("localStorage.setItem", "jwtToken", token);
                    return token;
                }
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Login failed: {response.StatusCode}, {errorContent}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception during login: {ex.Message}");
        }

        return null;
    }


    public async Task<bool> RegisterAsync(string email, string password, string username)
    {
        var response = await _httpClient.PostAsJsonAsync("api/auth/register", new { Email = email, Password = password, Username = username });

        return response.IsSuccessStatusCode;
    }

    public async Task<string?> GetTokenAsync()
    {
        return await _js.InvokeAsync<string?>("localStorage.getItem", "jwtToken");
    }

    public async Task LogoutAsync()
    {
        await _js.InvokeVoidAsync("localStorage.removeItem", "jwtToken");
    }
}
