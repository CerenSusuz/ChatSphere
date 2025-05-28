using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using ChatSphere.Client;
using ChatSphere.Client.Services;
using Microsoft.JSInterop;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped<AuthorizationMessageHandler>();

builder.Services.AddScoped(sp =>
{
    var js = sp.GetRequiredService<IJSRuntime>();
    var handler = new AuthorizationMessageHandler(js)
    {
        InnerHandler = new HttpClientHandler()
    };

    return new HttpClient(handler)
    {
        BaseAddress = new Uri("http://localhost:1234")
    };
});

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<RoomService>();

builder.Services.AddScoped<IChatService>(sp =>
{
    var httpClient = sp.GetRequiredService<HttpClient>();
    var hubUrl = "http://localhost:1234/chathub";
    return new ChatService(hubUrl, httpClient);
});

await builder.Build().RunAsync();
