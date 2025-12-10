using MediatR;
using SmartLibrary.Api.Application.Specs;
using SmartLibrary.Api.Domain.Entities;
using SmartLibrary.Api.Domain.Repositories;
using System.Security.Cryptography;

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

            string barcode = null!;
            const int maxAttempts = 10;
            int attempts = 0;

            while (attempts < maxAttempts)
            {
                var num = RandomNumberGenerator.GetInt32(10_000_000, 100_000_000); // 8 digits
                var candidate = num.ToString();
                var exists = (await _repo.ListAsync(new BookByBarcodeSpec(candidate))).FirstOrDefault();
                if (exists == null)
                {
                    barcode = candidate;
                    break;
                }
                attempts++;
            }

            if (barcode == null)
            {
                throw new InvalidOperationException("Unable to generate unique barcode for book.");
            }

            var book = new Book(request.Title, author.Id, request.Copies, request.DailyFine);
            book.SetBarcode(barcode);
            await _repo.AddAsync(book, ct);
            return book.Id;
        }
    }
}
