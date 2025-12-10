using Ardalis.Specification;
using MediatR;
using SmartLibrary.Api.Application.DTOs;
using SmartLibrary.Api.Application.Specs;
using SmartLibrary.Api.Domain.Entities;
using SmartLibrary.Api.Domain.Repositories;

namespace SmartLibrary.Api.Application.Queries.GetBooks
{
    public record GetTopRatedQuery() : IRequest<List<BookDto>>;
    public class GetAllBooksHandler
  : IRequestHandler<GetTopRatedQuery, List<BookDto>>
    {
        private readonly IRepository<Book> _repo;
        public GetAllBooksHandler(IRepository<Book> repo)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
        }
        public async Task<List<BookDto>> Handle(GetTopRatedQuery request, CancellationToken ct)
        {
            var books = await _repo.ListAsync(new TopRatedBooksSpec(1));

            return books.Select(b => new BookDto
            {
                Id = b.Id,
                BarCode = b.Barcode,
                Title = b.Title,
                AuthorName = b.Author.Name,
                CopiesAvailable = b.CopiesAvailable
            }).ToList();
        }
    }


}
