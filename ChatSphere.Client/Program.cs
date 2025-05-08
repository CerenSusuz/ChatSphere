using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using ChatSphere.Client;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

var backendBaseAddress = "http://localhost:7081";

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(backendBaseAddress) });

await builder.Build().RunAsync();