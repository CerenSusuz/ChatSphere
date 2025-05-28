using ChatSphere.Application.Features.Admin.Users.Queries;
using ChatSphere.Domain.DTOs;
using ChatSphere.Infrastructure.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ChatSphere.Application.Features.Admin.Users.Handlers;

public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, List<UserDto>>
{
    private readonly ChatSphereDbContext _context;

    public GetAllUsersQueryHandler(ChatSphereDbContext context)
    {
        _context = context;
    }

    public async Task<List<UserDto>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        return await _context.Users
            .Select(u => new UserDto
            {
                Id = u.Id,
                Username = u.Username,
                Email = u.Email
            }).ToListAsync(cancellationToken);
    }
}