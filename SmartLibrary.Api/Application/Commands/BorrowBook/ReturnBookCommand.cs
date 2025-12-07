using MediatR;
using SmartLibrary.Api.Application.Specs;
using SmartLibrary.Api.Domain.Entities;
using SmartLibrary.Api.Domain.Repositories;

namespace SmartLibrary.Api.Application.Commands.BorrowBook
{
    public record ReturnBookCommand(Guid BorrowRecordId, string UserName, string Pin, decimal Fine, int DaysLate)
        : IRequest<bool>;

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
                new UserByCredentialsSpec(request.UserName, request.Pin),
                cancellationToken
            );

            if (user is null)
                return false;

            var record = await _borrowRepo.GetByIdAsync(request.BorrowRecordId);
            if (record is null)
                return false;

            if (record.ReturnedAt != null)
                return false;

            if (record.UserId != user.Id)
                return false;

            record.Return(request.Fine);
            await _borrowRepo.UpdateAsync(record, cancellationToken);

            var book = await _bookRepo.GetByIdAsync(record.BookId);
            if (book is null)
                return false;
            int daysLate = request.DaysLate;
            decimal fine = daysLate * book.DailyFine;

            book.Return(daysLate);
            record.Return(fine);

            await _bookRepo.UpdateAsync(book, cancellationToken);
            await _borrowRepo.UpdateAsync(record, cancellationToken);

            return true;
        }
    }
}
