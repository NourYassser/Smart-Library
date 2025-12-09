using MediatR;
using SmartLibrary.Api.Application.Specs;
using SmartLibrary.Api.Domain.Entities;
using SmartLibrary.Api.Domain.Repositories;

namespace SmartLibrary.Api.Application.Commands.AddBook
{
    public record AddBookCommand(
        string Title,
        string AuthorName,
        int Copies,
        decimal DailyFine
        ) : IRequest<Guid>;


    public class AddBookHandler : IRequestHandler<AddBookCommand, Guid>
    {
        private readonly IRepository<Book> _repo;
        private readonly IRepository<Author> _authorRepo;
        public AddBookHandler(IRepository<Book> repo, IRepository<Author> authorRepo)
        {
            _authorRepo = authorRepo;
            _repo = repo;
        }

        public async Task<Guid> Handle(AddBookCommand request, CancellationToken ct)
        {
            var authorName = (request.AuthorName ?? string.Empty).Trim();

            var existing = await _authorRepo.FirstOrDefaultAsync(new AuthorByNameSpec(authorName), ct);

            Author author;
            if (existing != null)
            {
                author = existing;
            }
            else
            {
                author = new Author(authorName);
                await _authorRepo.AddAsync(author, ct);
            }

            var book = new Book(request.Title, author.Id, request.Copies, request.DailyFine);
            await _repo.AddAsync(book, ct);
            return book.Id;
        }
    }
}
