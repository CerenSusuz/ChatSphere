using System.Net.Http.Json;
using Microsoft.JSInterop;

namespace ChatSphere.Client.Services;

public class AuthService(HttpClient httpClient, IJSRuntime js)
{
    private readonly HttpClient _httpClient = httpClient;
    private readonly IJSRuntime _js = js;

    public async Task<bool> LoginAsync(string email, string password)
    {
        var response = await _httpClient.PostAsJsonAsync("api/auth/login", new { Email = email, Password = password });

        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<dynamic>();
            string token = result?.Token;

            await _js.InvokeVoidAsync("localStorage.setItem", "jwtToken", token);

            return true;
        }

        return false;
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

