using MediatR;
using SmartLibrary.Api.Application.Specs;
using SmartLibrary.Api.Domain.Entities;
using SmartLibrary.Api.Domain.Repositories;

namespace SmartLibrary.Api.Application.Commands.BorrowBook
{
    public record ReturnBatchByBarcodesCommand(List<string> Barcodes, string UserName, string Pin) : IRequest<int>;

    public class ReturnBatchByBarcodesHandler : IRequestHandler<ReturnBatchByBarcodesCommand, int>
    {
        private readonly IRepository<AppUser> _userRepo;
        private readonly IRepository<Book> _bookRepo;
        private readonly IRepository<BorrowRecord> _borrowRepo;
        private readonly IConfiguration _configuration;

        public ReturnBatchByBarcodesHandler(
            IRepository<AppUser> userRepo,
            IRepository<Book> bookRepo,
            IRepository<BorrowRecord> borrowRepo,
            IConfiguration configuration)
        {
            _userRepo = userRepo;
            _bookRepo = bookRepo;
            _borrowRepo = borrowRepo;
            _configuration = configuration;
        }

        public async Task<int> Handle(ReturnBatchByBarcodesCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepo.FirstOrDefaultAsync(new UserByUsernameSpec(request.UserName), cancellationToken);
            if (user is null || !user.VerifyPin(request.Pin)) return 0;

            var success = 0;
            foreach (var barcode in request.Barcodes.Distinct())
            {
                var book = (await _bookRepo.ListAsync(new BookByBarcodeSpec(barcode))).FirstOrDefault();
                if (book is null) continue;

                var record = await _borrowRepo.FirstOrDefaultAsync(new ActiveBorrowByUserAndBookSpec(user.Id, book.Id), cancellationToken);
                if (record is null) continue;

                var now = DateTime.UtcNow;
                var daysLate = Math.Max(0, (int)Math.Floor((now - record.DueDate).TotalDays));
                var fine = daysLate * book.DailyFine;

                book.Return(daysLate);
                record.Return(fine);

                await _bookRepo.UpdateAsync(book, cancellationToken);
                await _borrowRepo.UpdateAsync(record, cancellationToken);

                success++;
            }

            return success;
        }
    }
}
