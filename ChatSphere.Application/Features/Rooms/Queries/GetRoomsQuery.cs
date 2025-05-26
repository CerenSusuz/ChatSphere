using ChatSphere.Domain.DTOs;
using MediatR;

namespace ChatSphere.Application.Features.Rooms.Queries
{
    public class GetRoomsQuery : IRequest<List<RoomDto>>
    {
    }
}
