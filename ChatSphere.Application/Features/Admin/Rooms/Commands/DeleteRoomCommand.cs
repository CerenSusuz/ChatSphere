using MediatR;

namespace ChatSphere.Application.Features.Admin.Rooms.Commands;

public class DeleteRoomCommand : IRequest
{
    public Guid RoomId { get; set; }

    public DeleteRoomCommand(Guid roomId)
    {
        RoomId = roomId;
    }
}
