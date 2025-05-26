using MediatR;

namespace ChatSphere.Application.Features.Rooms.Commands
{
    public class CreateRoomCommand : IRequest<Guid>
    {
        public string Name { get; set; }

        public string Description { get; set; }
    }
}
