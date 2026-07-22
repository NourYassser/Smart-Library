using BookService.Application.Interface;
using Library.BuildingBlocks.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BookService.Application.Commands
{
    public record UpdateBookCommand(
    Guid Id,
    string Title,
    string Author,
    int Copies
) : IRequest<OperatingResult>;
    public class UpdateBookHandler : IRequestHandler<UpdateBookCommand, OperatingResult>
    {
        private readonly IBookDbContext _context;
        public UpdateBookHandler(IBookDbContext context)
        {
            _context = context;
        }
        public async Task<OperatingResult> Handle(UpdateBookCommand request, CancellationToken ct)
        {
            var book = await _context.Books.FirstOrDefaultAsync(x => x.Id == request.Id, ct);

            if (book == null) return new OperatingResult()
            {
                IsSuccess = false,
                Message = $"Book with Id {request.Id} not found."
            };
            book.Update(request.Title, request.Author, request.Copies);

            _context.Books.Update(book);

            await _context.SaveChangesAsync(ct);

            return new OperatingResult()
            {
                IsSuccess = true,
                Message = $"Book with Id {request.Id} updated successfully."
            };
        }
    }

}
