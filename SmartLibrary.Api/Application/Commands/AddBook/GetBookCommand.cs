using MediatR;
using SmartLibrary.Api.Application.DTOs;
using SmartLibrary.Api.Domain.Repositories;

namespace SmartLibrary.Api.Application.Commands.AddBook
{
    public record GetBookByIdQuery(Guid Id) : IRequest<BookDto>;

    public class GetBookByIdHandler : IRequestHandler<GetBookByIdQuery, BookDto>
    {
        private readonly IBookRepository _repo;

        public GetBookByIdHandler(IBookRepository repo)
        {
            _repo = repo;
        }

        public async Task<BookDto> Handle(GetBookByIdQuery request, CancellationToken ct)
        {
            var book = await _repo.GetByIdAsync(request.Id, ct);
            if (book == null) return null;

            return new BookDto
            {
                Id = book.Id,
                Title = book.Title,
                AuthorName = book.Author.Name,
                CopiesAvailable = book.CopiesAvailable
            };
        }
    }

}
