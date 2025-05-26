using ChatSphere.Infrastructure.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ChatSphere.Domain.DTOs;

namespace ChatSphere.Application.Features.Rooms.Queries
{
    public class GetRoomsQueryHandler : IRequestHandler<GetRoomsQuery, List<RoomDto>>
    {
        private readonly ChatSphereDbContext _context;

        public GetRoomsQueryHandler(ChatSphereDbContext context)
        {
            _context = context;
        }

        public async Task<List<RoomDto>> Handle(GetRoomsQuery request, CancellationToken cancellationToken)
        {
            return await _context.Rooms
                .Select(r => new RoomDto
                {
                    Id = r.Id,
                    Name = r.Name
                })
                .ToListAsync(cancellationToken);
        }
    }
}
