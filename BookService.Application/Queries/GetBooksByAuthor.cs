using Ardalis.Specification.EntityFrameworkCore;
using BookService.Application.Interface;
using BookService.Application.Specs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BookService.Application.Queries.GetBooks
{
    public record GetBooksByAuthorQuery(Guid AuthorId) : IRequest<List<DTOs.BookDto>>;
    public class GetBooksByAuthorHandler : IRequestHandler<GetBooksByAuthorQuery, List<DTOs.BookDto>>
    {
        private readonly IBookDbContext _context;

        public GetBooksByAuthorHandler(IBookDbContext context) => _context = context;

        public async Task<List<DTOs.BookDto>> Handle(GetBooksByAuthorQuery request, CancellationToken cancellationToken)
        {
            var spec = new BooksByAuthorSpec(request.AuthorId);
            var books = await _context.Books.WithSpecification(spec).ToListAsync();
            return books.Select(b => new DTOs.BookDto
            {
                Id = b.Id,
                BarCode = b.Barcode,
                Title = b.Title,
                AuthorName = b.Author?.Name,
                CopiesAvailable = b.CopiesAvailable
            }).ToList();
        }
    }
}
