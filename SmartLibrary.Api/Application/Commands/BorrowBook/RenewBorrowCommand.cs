using MediatR;
using SmartLibrary.Api.Application.Specs;
using SmartLibrary.Api.Domain.Entities;
using SmartLibrary.Api.Domain.Repositories;

namespace SmartLibrary.Api.Application.Commands.BorrowBook
{
    public record RenewBorrowCommand(Guid BorrowRecordId, string UserName, string Pin) : IRequest<DateTime?>;

    public class RenewBorrowHandler : IRequestHandler<RenewBorrowCommand, DateTime?>
    {
        private readonly IRepository<AppUser> _userRepo;
        private readonly IRepository<BorrowRecord> _borrowRepo;
        private readonly IRepository<Book> _bookRepo;

        public RenewBorrowHandler(
            IRepository<AppUser> userRepo,
            IRepository<BorrowRecord> borrowRepo,
            IRepository<Book> bookRepo)
        {
            _userRepo = userRepo;
            _borrowRepo = borrowRepo;
            _bookRepo = bookRepo;
        }

        public async Task<DateTime?> Handle(RenewBorrowCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepo.FirstOrDefaultAsync(
                new UserByUsernameSpec(request.UserName),
                cancellationToken);
            if (user is null || !user.VerifyPin(request.Pin)) return null;

            var record = await _borrowRepo.GetByIdAsync(request.BorrowRecordId);
            if (record is null || !record.Borrowed || record.UserId != user.Id) return null;

            var maxRenewals = 2;
            if (record.RenewalsCount >= maxRenewals) return null;

            var defaultLoanDays = 14;

            record.Renew(defaultLoanDays);

            await _borrowRepo.UpdateAsync(record, cancellationToken);

            return record.DueDate;
        }
    }
}
