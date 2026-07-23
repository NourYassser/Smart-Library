using BorrowService.Application.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BorrowService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BorrowController : ControllerBase
    {
        private readonly IMediator _mediator;
        public BorrowController(IMediator mediator) => _mediator = mediator;

        //borrow Services
        [HttpPost("borrow")]
        public async Task<IActionResult> BorrowBook(BorrowBookCommand cmd)
        {
            var result = await _mediator.Send(cmd);
            return Ok(result);
        }

        [HttpPost("renew-book-borrowing")]
        public async Task<IActionResult> Renew(RenewBorrowCommand cmd)
        {
            var result = await _mediator.Send(cmd);
            return Ok(result);
        }

        [HttpPost("return")]
        public async Task<IActionResult> Return(ReturnBookCommand cmd)
        {
            var x = await _mediator.Send(cmd);
            return Ok(x);
        }

        [HttpPost("return-batch")]
        public async Task<IActionResult> ReturnBatch(ReturnBatchByBarcodesCommand cmd)
        {
            var processed = await _mediator.Send(cmd);
            return Ok(new { Processed = processed });
        }
    }
}
