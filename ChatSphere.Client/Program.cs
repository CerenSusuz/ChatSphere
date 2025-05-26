using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using ChatSphere.Client;
using ChatSphere.Client.Services;
using Microsoft.AspNetCore.Components;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri("http://localhost:1234")
});

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<RoomService>();

builder.Services.AddScoped<IChatService>(sp =>
{
    var navigation = sp.GetRequiredService<NavigationManager>();
    var httpClient = sp.GetRequiredService<HttpClient>();
    var hubUrl = "http://localhost:1234/chathub";
    return new ChatService(hubUrl, httpClient);
});

await builder.Build().RunAsync();
