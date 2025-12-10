using MediatR;
using SmartLibrary.Api.Application.Specs;
using SmartLibrary.Api.Domain.Entities;
using SmartLibrary.Api.Domain.Repositories;

namespace SmartLibrary.Api.Application.Commands.BorrowBook
{
    public record BorrowBookCommand(
     string Barcode,
     string Username,
     string Pin
    )
    : IRequest<Guid>;

    public class BorrowBookHandler
        : IRequestHandler<BorrowBookCommand, Guid>
    {
        private readonly IRepository<AppUser> _userRepo;
        private readonly IRepository<Book> _bookRepo;
        private readonly IRepository<BorrowRecord> _borrowRepo;

        public BorrowBookHandler(
            IRepository<AppUser> userRepo,
            IRepository<Book> bookRepo,
            IRepository<BorrowRecord> borrowRepo)
        {
            _userRepo = userRepo;
            _bookRepo = bookRepo;
            _borrowRepo = borrowRepo;
        }

        public async Task<Guid> Handle(BorrowBookCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepo.FirstOrDefaultAsync(
            new UserByUsernameSpec(request.Username),
            cancellationToken
        );

            if (user is null)
                throw new Exception("Invalid username or pin");

            if (!user.VerifyPin(request.Pin))
                throw new UnauthorizedAccessException("Invalid username or pin.");

            var book = (await _bookRepo.ListAsync(new BookByBarcodeSpec(request.Barcode))).FirstOrDefault();
            if (book is null)
                throw new InvalidOperationException("Book not found.");

            var existing = await _borrowRepo.FirstOrDefaultAsync(
                new ActiveBorrowByUserAndBookSpec(user.Id, book.Id),
                cancellationToken
            );

            if (existing is not null)
                throw new InvalidOperationException("User already has an active borrow for this book.");

            if (book.CopiesAvailable <= 0) throw new InvalidOperationException("No copies available.");

            book.Borrow();

            await _bookRepo.UpdateAsync(book, cancellationToken);

            var borrowRecord = new BorrowRecord(book.Id, user.Id);

            var added = await _borrowRepo.AddAsync(borrowRecord, cancellationToken);

            return added.Id;
        }
    }

}
