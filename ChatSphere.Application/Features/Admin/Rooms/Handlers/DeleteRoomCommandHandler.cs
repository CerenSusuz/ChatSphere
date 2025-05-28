using ChatSphere.Application.Features.Admin.Rooms.Commands;
using ChatSphere.Infrastructure.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ChatSphere.Application.Features.Admin.Rooms.Handlers;

public class DeleteRoomCommandHandler : IRequestHandler<DeleteRoomCommand>
{
    private readonly ChatSphereDbContext _context;

    public DeleteRoomCommandHandler(ChatSphereDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteRoomCommand request, CancellationToken cancellationToken)
    {
        var room = await _context.Rooms
            .Include(r => r.Messages)
            .Include(r => r.Users)
            .FirstOrDefaultAsync(r => r.Id == request.RoomId, cancellationToken);

        if (room != null)
        {
            _context.Rooms.Remove(room);
            await _context.SaveChangesAsync(cancellationToken);
        }

        return Unit.Value;
    }
}
