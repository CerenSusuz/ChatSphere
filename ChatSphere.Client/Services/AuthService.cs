using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Json;
using System.Security.Claims;
using ChatSphere.Domain.DTOs;
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
            var loginData = new { Email = email, Password = password };
            var response = await _httpClient.PostAsJsonAsync("http://localhost:1234/api/auth/login", loginData);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>();

                if (result != null && result.TryGetValue("token", out var token))
                {
                    await _js.InvokeVoidAsync("sessionStorage.setItem", "jwtToken", token);

                    var handler = new JwtSecurityTokenHandler();
                    var jwt = handler.ReadJwtToken(token);
                    var isAdmin = jwt.Claims.Any(c =>
                        (c.Type == ClaimTypes.Role && c.Value == "Admin") ||
                        (c.Type == "isAdmin" && c.Value.Equals("true", StringComparison.OrdinalIgnoreCase)));

                    await _js.InvokeVoidAsync("sessionStorage.setItem", "isAdmin", isAdmin.ToString().ToLower());

                    return token;
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("LoginAsync failed: " + ex.Message);
        }

        return null;
    }


    public async Task LogoutAsync()
    {
        await _js.InvokeVoidAsync("sessionStorage.removeItem", "jwtToken");
        await _js.InvokeVoidAsync("sessionStorage.removeItem", "isAdmin");
    }


    public async Task<string?> GetTokenAsync()
    {
        return await _js.InvokeAsync<string?>("sessionStorage.getItem", "jwtToken");
    }

    public JwtUserInfo ParseToken(string token)
    {
        try
        {
            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(token);

            var username = jwt.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
            var email = jwt.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

            var isAdmin = jwt.Claims.Any(c =>
                (c.Type == ClaimTypes.Role && c.Value == "Admin") ||
                (c.Type == "isAdmin" && c.Value.Equals("true", StringComparison.OrdinalIgnoreCase)));

            return new JwtUserInfo
            {
                Username = username ?? "",
                Email = email ?? "",
                IsAdmin = isAdmin
            };
        }
        catch (Exception ex)
        {
            Console.WriteLine("ParseToken failed: " + ex.Message);
            return new JwtUserInfo();
        }
    }


    public async Task<bool> RegisterAsync(string email, string password, string username)
    {
        var response = await _httpClient.PostAsJsonAsync("api/auth/register", new { Email = email, Password = password, Username = username });

        return response.IsSuccessStatusCode;
    }
}
