using ChatSphere.Domain.DTOs;
using ChatSphere.Infrastructure.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ChatSphere.Application.Features.Admin.Rooms.Queries;

public class GetAllRoomsQuery : IRequest<List<RoomDto>> { }

public class GetAllRoomsQueryHandler : IRequestHandler<GetAllRoomsQuery, List<RoomDto>>
{
    private readonly ChatSphereDbContext _context;
    public GetAllRoomsQueryHandler(ChatSphereDbContext context)
    {
        _context = context;
    }

    public async Task<List<RoomDto>> Handle(GetAllRoomsQuery request, CancellationToken cancellationToken)
    {
        return await _context.Rooms
            .Select(r => new RoomDto { Id = r.Id, Name = r.Name, Description = r.Description, IsActive = r.IsActive })
            .ToListAsync(cancellationToken);
    }
}
