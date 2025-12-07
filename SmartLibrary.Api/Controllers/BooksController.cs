using MediatR;
using Microsoft.AspNetCore.Mvc;
using SmartLibrary.Api.Application.Commands.AddBook;
using SmartLibrary.Api.Application.Commands.BorrowBook;
using SmartLibrary.Api.Application.Queries.GetAllBooks;

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
            return Ok(id);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
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

        [HttpPost("borrow")]
        public async Task<IActionResult> BorrowBook(BorrowBookCommand cmd)
        {
            var result = await _mediator.Send(cmd);
            return Ok(new { BorrowId = result });
        }


        [HttpPost("{id}/return")]
        public async Task<IActionResult> Return(ReturnBookCommand cmd)
        {
            await _mediator.Send(cmd);
            return NoContent();
        }
    }
}
