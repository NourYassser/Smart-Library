using Ardalis.Specification;
using MediatR;
using SmartLibrary.Api.Application.DTOs;
using SmartLibrary.Api.Application.Specs;
using SmartLibrary.Api.Domain.Entities;
using SmartLibrary.Api.Domain.Repositories;

namespace SmartLibrary.Api.Application.Queries.GetAllBooks
{
    public record GetTopRatedQuery() : IRequest<List<BookDto>>;
    public class GetAllBooksHandler
  : IRequestHandler<GetTopRatedQuery, List<BookDto>>
    {
        private readonly IRepository<Book> _repo;

        public async Task<List<BookDto>> Handle(GetTopRatedQuery request, CancellationToken ct)
        {
            var books = await _repo.ListAsync(new TopRatedBooksSpec(10));

            return books.Select(b => new BookDto
            {
                Id = b.Id,
                Title = b.Title,
                AuthorName = b.Author.Name,
                CopiesAvailable = b.CopiesAvailable
            }).ToList();
        }
    }


}
