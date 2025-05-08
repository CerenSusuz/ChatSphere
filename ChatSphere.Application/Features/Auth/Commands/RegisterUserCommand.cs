using MediatR;

namespace ChatSphere.Application.Features.Auth.Commands;

public class RegisterUserCommand : IRequest
{
    public string Username { get; set; }

    public string Email { get; set; }

    public string Password { get; set; }
}