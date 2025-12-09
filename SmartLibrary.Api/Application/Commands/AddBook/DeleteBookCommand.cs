using MediatR;
using SmartLibrary.Api.Domain.Entities;
using SmartLibrary.Api.Domain.Repositories;

namespace SmartLibrary.Api.Application.Commands.AddBook
{
    public record DeleteBookCommand(Guid Id) : IRequest;
    public class DeleteBookHandler : IRequestHandler<DeleteBookCommand>
    {
        private readonly IRepository<Book> _repo;
        public DeleteBookHandler(IRepository<Book> repo)
        {
            _repo = repo;
        }
        public async Task Handle(DeleteBookCommand request, CancellationToken ct)
        {
            var book = await _repo.GetByIdAsync(request.Id);
            if (book == null) throw new Exception("Not found");

            await _repo.DeleteAsync(book, ct);
        }
    }

}
