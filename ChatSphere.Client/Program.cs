using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using ChatSphere.Client;
using ChatSphere.Client.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://localhost:5080") });

builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddScoped<IChatService>(provider =>
{
    return new ChatService("http://localhost:5080/chathub");
});

await builder.Build().RunAsync();