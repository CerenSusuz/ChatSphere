using ChatSphere.Application.Features.Admin.Rooms.Queries;
using ChatSphere.Application.Features.Auth.Commands;
using ChatSphere.Backend.Hubs;
using ChatSphere.Domain.Entities;
using ChatSphere.Infrastructure.Database;
using Core.AI.Abstractions;
using Core.AI.Providers;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Core.AI.Config;
using Core.AI.Providers.Ollama;
using Core.AI.Providers.OpenRouter;
using Core.AI.Commands;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ChatSphereDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

builder.Services.AddMediatR(typeof(RegisterUserCommandHandler).Assembly);

builder.Services.AddMediatR(typeof(GetRoomsQuery).Assembly);

builder.Services.AddControllers();
builder.Services.AddSignalR();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// --- AI Services ---
builder.Services.AddOptions<AISettings>()
    .Bind(builder.Configuration.GetSection("AiSettings"))
    .Validate(settings => Enum.IsDefined(typeof(AIProvider), settings.Provider),
        "Invalid AI provider configured in AiSettings.Provider");

builder.Services.AddSingleton(sp =>
    sp.GetRequiredService<IOptions<AISettings>>().Value);

// Providers
builder.Services.AddScoped<OpenRouterAiService>();
builder.Services.AddScoped<OllamaAiService>();
builder.Services.AddScoped<IAIService, AIServiceResolver>();

// Model Providers
builder.Services.AddScoped<OpenRouterModelProvider>();
builder.Services.AddScoped<OllamaModelProvider>();
builder.Services.AddScoped<AIModelProviderResolver>();

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(PromptTextCommandHandler).Assembly);
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("ClientPolicy", policy =>
    {
        policy.WithOrigins("http://localhost:3456")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(
                builder.Configuration["Jwt:Secret"] ?? throw new Exception("JWT Secret not found."))),
            ValidateIssuer = false,
            ValidateAudience = false,
        };

        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var accessToken = context.Request.Query["access_token"];
                var path = context.HttpContext.Request.Path;

                if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/chathub"))
                {
                    context.Token = accessToken;
                }

                return Task.CompletedTask;
            }
        };
    });

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();
app.UseCors("ClientPolicy");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHub<ChatHub>("/chathub");

app.Run();
