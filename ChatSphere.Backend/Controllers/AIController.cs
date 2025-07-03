using Core.AI.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ChatSphere.Backend.Controllers
{
    public class AIController : Controller
    {
        private readonly IMediator _mediator;

        public AIController(IMediator mediator) => _mediator = mediator;

        [HttpPost("prompt")]
        public async Task<IActionResult> Prompt([FromBody] PromptTextCommand command)
        {
            var result = await _mediator.Send(command);

            return Ok(new { result });
        }
    }
}
