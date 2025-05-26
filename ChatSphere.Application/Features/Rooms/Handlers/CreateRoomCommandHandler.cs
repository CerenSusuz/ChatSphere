using ChatSphere.Application.Features.Rooms.Commands;
using ChatSphere.Domain.Entities;
using ChatSphere.Infrastructure.Database;
using MediatR;

namespace ChatSphere.Application.Features.Rooms.Handlers
{
    public class CreateRoomCommandHandler : IRequestHandler<CreateRoomCommand, Guid>
    {
        private readonly ChatSphereDbContext _context;

        public CreateRoomCommandHandler(ChatSphereDbContext context)
        {
            _context = context;
        }

        public async Task<Guid> Handle(CreateRoomCommand request, CancellationToken cancellationToken)
        {
            var room = new Room(request.Name, request.Description);
            _context.Rooms.Add(room);
            await _context.SaveChangesAsync(cancellationToken);

            return room.Id;
        }
    }
}
