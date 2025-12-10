using MediatR;
using SmartLibrary.Api.Application.DTOs;
using SmartLibrary.Api.Application.Specs;
using SmartLibrary.Api.Domain.Entities;
using SmartLibrary.Api.Domain.Repositories;

namespace SmartLibrary.Api.Application.Queries.GetBooks
{
    public record GetBooksByAuthorQuery(Guid AuthorId) : IRequest<List<BookDto>>;
    public class GetBooksByAuthorHandler : IRequestHandler<GetBooksByAuthorQuery, List<BookDto>>
    {
        private readonly IRepository<Book> _repo;

        public GetBooksByAuthorHandler(IRepository<Book> repo) => _repo = repo;

        public async Task<List<BookDto>> Handle(GetBooksByAuthorQuery request, CancellationToken cancellationToken)
        {
            var spec = new BooksByAuthorSpec(request.AuthorId);
            var books = await _repo.ListAsync(spec);
            return books.Select(b => new BookDto
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
