using MediatR;
using Microsoft.AspNetCore.Mvc;
using SmartLibrary.Api.Application.Commands.CreateUser;

namespace SmartLibrary.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(CreateUserCommand cmd)
        {
            var id = await _mediator.Send(cmd);
            return Ok(id);
        }
    }
}
