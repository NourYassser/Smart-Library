using MediatR;
using SmartLibrary.Api.Domain.Entities;
using SmartLibrary.Api.Domain.Repositories;

namespace SmartLibrary.Api.Application.Commands.AddBook
{
    public record UpdateBookCommand(
    Guid Id,
    string Title,
    string Author,
    int Copies
) : IRequest;
    public class UpdateBookHandler : IRequestHandler<UpdateBookCommand>
    {
        private readonly IRepository<Book> _repo;
        public UpdateBookHandler(IRepository<Book> repo)
        {
            _repo = repo;
        }
        public async Task Handle(UpdateBookCommand request, CancellationToken ct)
        {
            var book = await _repo.GetByIdAsync(request.Id);
            if (book == null) throw new Exception("Book not found");

            book.Update(request.Title, request.Author, request.Copies);
            await _repo.UpdateAsync(book, ct);
        }
    }

}
