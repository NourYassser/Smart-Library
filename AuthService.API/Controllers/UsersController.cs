using AuthService.Application.Commands;
using AuthService.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

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

        /*[HttpPost("add-author")]
        public async Task<IActionResult> Add(AddAuthorCommand cmd)
        {
            var id = await _mediator.Send(cmd);
            return Ok(id);
        }*/

        [HttpPost("add-user")]
        public async Task<IActionResult> CreateUser(CreateUserCommand cmd)
        {
            var id = await _mediator.Send(cmd);
            return Ok();
        }

        /*[HttpGet("author/{authorId:guid}/books")]
        public async Task<IActionResult> GetBooksByAuthor(Guid authorId)
        {
            var books = await _mediator.Send(new GetBooksByAuthorQuery(authorId));
            return Ok(books);
        }*/

        [HttpGet("user/{username}")]
        public async Task<IActionResult> GetUser(string username)
        {
            var books = await _mediator.Send(new GetUserByUserNameQuery(username));
            return Ok(books);
        }

        /*[HttpPost("reviews")]
        public async Task<IActionResult> CreateReview(CreateReviewCommand cmd)
        {
            var id = await _mediator.Send(cmd);
            return Ok(id);
        }

        [HttpGet("operations/{username}")]
        public async Task<IActionResult> GetUserOperations(string username)
        {
            var ops = await _mediator.Send(new GetUserOperationsQuery(username));
            return Ok(ops);
        }*/
    }
}
