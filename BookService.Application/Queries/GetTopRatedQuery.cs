using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;
using BookService.Application.Interface;
using BookService.Application.Specs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BookService.Application.Queries.GetBooks
{
    public record GetTopRatedQuery() : IRequest<List<DTOs.BookDto>>;
    public class GetAllBooksHandler
  : IRequestHandler<GetTopRatedQuery, List<DTOs.BookDto>>
    {
        private readonly IBookDbContext _context;
        public GetAllBooksHandler(IBookDbContext context)
        {
            _context = context;
        }
        public async Task<List<DTOs.BookDto>> Handle(GetTopRatedQuery request, CancellationToken ct)
        {
            var books = await _context.Books
                .WithSpecification(new TopRatedBooksSpec(1))
                .ToListAsync();

            return books.Select(b => new DTOs.BookDto
            {
                Id = b.Id,
                BarCode = b.Barcode,
                Title = b.Title,
                AuthorName = b.Author.Name,
                CopiesAvailable = b.CopiesAvailable,
                /*Reviews = b.Reviews.Select(r => new ReviewDto
                {
                    Id = r.Id,
                    Username = r.User.Username,
                    Rating = r.Rating,
                    Comment = r.Text,
                    CreatedOn = r.CreatedOn

                })
                .OrderByDescending(r => r.CreatedOn)
                .ToList()*/
            }).ToList();
        }
    }


}
