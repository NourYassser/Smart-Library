using MediatR;
using SmartLibrary.Api.Domain.Entities;
using SmartLibrary.Api.Domain.Repositories;

namespace SmartLibrary.Api.Application.Commands.AddBook
{
    public record AddBookCommand(string Title, Guid AuthorId, int Copies, decimal DailyFine) : IRequest<Guid>;


    public class AddBookHandler : IRequestHandler<AddBookCommand, Guid>
    {
        private readonly IBookRepository _repo;
        public AddBookHandler(IBookRepository repo) => _repo = repo;


        public async Task<Guid> Handle(AddBookCommand request, CancellationToken ct)
        {
            var book = new Book(request.Title, request.AuthorId, request.Copies, request.DailyFine);
            await _repo.AddAsync(book, ct);
            return book.Id;
        }
    }
}
