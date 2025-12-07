using MediatR;
using SmartLibrary.Api.Application.DTOs;
using SmartLibrary.Api.Domain.Entities;
using SmartLibrary.Api.Domain.Repositories;

namespace SmartLibrary.Api.Application.Queries.GetAllBooks
{
    public record GetBookByIdQuery(Guid Id) : IRequest<BookDto>;

    public class GetBookByIdHandler : IRequestHandler<GetBookByIdQuery, BookDto>
    {
        private readonly IRepository<Book> _repo;

        public GetBookByIdHandler(IRepository<Book> repo)
        {
            _repo = repo;
        }

        public async Task<BookDto> Handle(GetBookByIdQuery request, CancellationToken ct)
        {
            var book = await _repo.GetByIdAsync(request.Id);
            if (book == null) return null;

            return new BookDto
            {
                Id = book.Id,
                Title = book.Title,
                CopiesAvailable = book.CopiesAvailable
            };
        }
    }

}
