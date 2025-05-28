using ChatSphere.Application.Features.Admin.Rooms.Commands;
using ChatSphere.Infrastructure.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatSphere.Application.Features.Admin.Rooms.Handlers
{
    public class EditRoomCommandHandler : IRequestHandler<EditRoomCommand, bool>
    {
        private readonly ChatSphereDbContext _context;

        public EditRoomCommandHandler(ChatSphereDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(EditRoomCommand request, CancellationToken cancellationToken)
        {
            var room = await _context.Rooms.FirstOrDefaultAsync(r => r.Id == request.Id, cancellationToken);

            if (room == null) return false;

            room.Name = request.Name;
            room.Description = request.Description;

            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
