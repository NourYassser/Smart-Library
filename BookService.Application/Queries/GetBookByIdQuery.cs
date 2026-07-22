using Ardalis.Specification.EntityFrameworkCore;
using BookService.Application.Interface;
using BookService.Application.Specs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BookService.Application.Queries
{
    public record GetBookByIdQuery(Guid Id) : IRequest<DTOs.BookDto>;

    public class GetBookByIdHandler : IRequestHandler<GetBookByIdQuery, DTOs.BookDto>
    {
        private readonly IBookDbContext _context;

        public GetBookByIdHandler(IBookDbContext context)
        {
            _context = context;
        }

        public async Task<DTOs.BookDto> Handle(GetBookByIdQuery request, CancellationToken ct)
        {
            var book = await _context.Books
                .WithSpecification(new GetByIdSpec(request.Id))
                .FirstOrDefaultAsync(ct);

            if (book == null) return null;

            return new DTOs.BookDto
            {
                Id = book.Id,
                BarCode = book.Barcode ?? "No available barcode",
                Title = book.Title,
                AuthorName = book.Author.Name,
                CopiesAvailable = book.CopiesAvailable
            };
        }
    }

}
