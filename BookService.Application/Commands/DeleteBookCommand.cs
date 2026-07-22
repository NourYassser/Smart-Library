using BookService.Application.Interface;
using Library.BuildingBlocks.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BookService.Application.Commands
{
    public record DeleteBookCommand(Guid Id) : IRequest<OperatingResult>;
    public class DeleteBookHandler : IRequestHandler<DeleteBookCommand, OperatingResult>
    {
        private readonly IBookDbContext _context;
        public DeleteBookHandler(IBookDbContext context)
        {
            _context = context;
        }
        public async Task<OperatingResult> Handle(DeleteBookCommand request, CancellationToken ct)
        {
            var book = await _context.Books
                            .FirstOrDefaultAsync(x => x.Id == request.Id, ct);
            if (book == null)
                return new OperatingResult
                {
                    IsSuccess = false,
                    Message = $"Book with Id {request.Id} not found."
                };
            _context.Books.Remove(book);

            await _context.SaveChangesAsync(ct);

            return new OperatingResult
            {
                IsSuccess = true,
                Message = $"Book with Id {request.Id} deleted successfully."
            };
        }
    }

}
