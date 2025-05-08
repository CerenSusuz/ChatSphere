using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using ChatSphere.Client;
using ChatSphere.Client.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://localhost:5080") });
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://localhost:5080") });
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<ChatService>(provider =>
{
    var authService = provider.GetRequiredService<AuthService>();
    var token = authService.GetTokenAsync().Result;

    return new ChatService("http://localhost:5080/chathub", token);
});


await builder.Build().RunAsync();