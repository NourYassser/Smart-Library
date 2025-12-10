using MediatR;
using SmartLibrary.Api.Application.Specs;
using SmartLibrary.Api.Domain.Entities;
using SmartLibrary.Api.Domain.Repositories;

namespace SmartLibrary.Api.Application.Commands.BorrowBook
{
    public record ReturnBookCommand(string Barcode, string UserName, string Pin) : IRequest<bool>;

    public class ReturnBookHandler : IRequestHandler<ReturnBookCommand, bool>
    {
        private readonly IRepository<AppUser> _userRepo;
        private readonly IRepository<Book> _bookRepo;
        private readonly IRepository<BorrowRecord> _borrowRepo;

        public ReturnBookHandler(
            IRepository<AppUser> userRepo,
            IRepository<Book> bookRepo,
            IRepository<BorrowRecord> borrowRepo)
        {
            _userRepo = userRepo;
            _bookRepo = bookRepo;
            _borrowRepo = borrowRepo;
        }

        public async Task<bool> Handle(ReturnBookCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepo.FirstOrDefaultAsync(
                new UserByUsernameSpec(request.UserName),
                cancellationToken
            );

            if (user is null) return false;
            if (!user.VerifyPin(request.Pin)) return false;

            var book = (await _bookRepo.ListAsync(new BookByBarcodeSpec(request.Barcode))).FirstOrDefault();
            if (book is null) return false;

            var record = await _borrowRepo.FirstOrDefaultAsync(new ActiveBorrowByUserAndBookSpec(user.Id, book.Id), cancellationToken);
            if (record is null) return false;
            if (record.ReturnedAt != null) return false;
            if (record.UserId != user.Id) return false;


            var defaultLoanDays = 14;

            var elapsedDays = (int)Math.Floor((DateTime.UtcNow - record.BorrowedAt).TotalDays);
            var daysLate = Math.Max(0, elapsedDays - defaultLoanDays);

            decimal fine = daysLate * book.DailyFine;

            book.Return(daysLate);
            record.Return(book.DailyFine + fine);

            await _bookRepo.UpdateAsync(book, cancellationToken);
            await _borrowRepo.UpdateAsync(record, cancellationToken);

            return true;
        }
    }
}
