using ChatSphere.Application.Features.Admin.Rooms.Commands;
using ChatSphere.Application.Features.Admin.Rooms.Queries;
using ChatSphere.Application.Features.Admin.Users.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChatSphere.Backend.Controllers;

[ApiController]
[Route("api/admin")]
[Authorize(Roles = "Admin")]
public class AdminController : ControllerBase
{
    private readonly IMediator _mediator;

    public AdminController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("rooms")]
    public async Task<IActionResult> GetRooms()
    {
        var result = await _mediator.Send(new GetAllRoomsQuery());
        return Ok(result);
    }

    [HttpGet("users")]
    public async Task<IActionResult> GetUsers()
    {
        var result = await _mediator.Send(new GetAllUsersQuery());
        return Ok(result);
    }

    [HttpPost("rooms")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> CreateRoom([FromBody] CreateRoomCommand command)
    {
        var roomId = await _mediator.Send(command);
        return Ok(new { roomId });
    }

    [HttpDelete("rooms/{roomId}")]
    public async Task<IActionResult> DeleteRoom(Guid roomId)
    {
        await _mediator.Send(new DeleteRoomCommand(roomId));
        return NoContent();
    }

    [HttpPut("rooms/{roomId}")]
    public async Task<IActionResult> EditRoom(Guid roomId, [FromBody] EditRoomCommand command)
    {
        if (roomId != command.Id)
            return BadRequest("RoomId mismatch");

        var result = await _mediator.Send(command);
        return result ? Ok() : NotFound();
    }



}
