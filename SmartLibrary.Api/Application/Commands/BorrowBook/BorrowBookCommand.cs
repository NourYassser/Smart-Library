using MediatR;
using SmartLibrary.Api.Application.Specs;
using SmartLibrary.Api.Domain.Entities;
using SmartLibrary.Api.Domain.Repositories;

namespace SmartLibrary.Api.Application.Commands.BorrowBook
{
    public record BorrowBookCommand(
     Guid BookId,
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
            new UserByCredentialsSpec(request.Username, request.Pin),
            cancellationToken
        );

            if (user is null)
                throw new Exception("Invalid username or pin");

            var book = await _bookRepo.GetByIdAsync(request.BookId);
            if (book is null)
                throw new Exception("Book not found");

            book.Borrow();

            var borrowRecord = new BorrowRecord(book.Id, user.Id);
            await _borrowRepo.AddAsync(borrowRecord, cancellationToken);

            await _bookRepo.UpdateAsync(book, cancellationToken);

            return borrowRecord.Id;
        }
    }

}
