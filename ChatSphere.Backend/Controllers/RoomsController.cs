using ChatSphere.Application.Features.Admin.Rooms.Commands;
using ChatSphere.Application.Features.Admin.Rooms.Queries;
using ChatSphere.Domain.DTOs;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChatSphere.Backend.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class RoomsController : ControllerBase
{
    private readonly IMediator _mediator;

    public RoomsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<List<RoomDto>>> GetRooms()
    {
        var rooms = await _mediator.Send(new GetRoomsQuery());
        return Ok(rooms);
    }

    [HttpPost]
    public async Task<ActionResult<Guid>> CreateRoom(CreateRoomCommand command)
    {
        var id = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetRooms), new { id }, id);
    }

}
