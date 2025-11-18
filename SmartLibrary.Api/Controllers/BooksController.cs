using MediatR;
using Microsoft.AspNetCore.Mvc;
using SmartLibrary.Api.Application.Commands.AddBook;

namespace SmartLibrary.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IMediator _mediator;
        public BooksController(IMediator mediator) => _mediator = mediator;


        [HttpPost]
        public async Task<IActionResult> Add(AddBookCommand cmd)
        {
            var id = await _mediator.Send(cmd);
            return CreatedAtAction(nameof(GetById), new { id }, null);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var dto = await _mediator.Send(new GetBookByIdQuery(id));
            if (dto == null) return NotFound();
            return Ok(dto);
        }
    }
}
