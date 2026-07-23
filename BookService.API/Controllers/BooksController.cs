using BookService.Application.Commands;
using BookService.Application.Queries;
using BookService.Application.Queries.GetBooks;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BookService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IMediator _mediator;
        public BooksController(IMediator mediator) => _mediator = mediator;

        //aa
        [HttpPost]
        public async Task<IActionResult> Add(AddBookCommand cmd)
        {
            var res = await _mediator.Send(cmd);
            return Ok(res);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var dto = await _mediator.Send(new GetBookByIdQuery(id));
            if (dto == null) return NotFound();
            return Ok(dto);
        }

        [HttpPut]
        public async Task<IActionResult> Update(UpdateBookCommand cmd)
        {
            await _mediator.Send(cmd);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _mediator.Send(new DeleteBookCommand(id));
            return NoContent();
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetTopRatedQuery());
            return Ok(result);
        }
    }
}
