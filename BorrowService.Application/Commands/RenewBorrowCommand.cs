using BookService.Application.Interface;
using BorrowService.Application.Interface;
using Library.BuildingBlocks.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BorrowService.Application.Commands
{
    public record RenewBorrowCommand(Guid BorrowRecordId, string UserName, string Pin) : IRequest<OperatingResult>;

    public class RenewBorrowHandler : IRequestHandler<RenewBorrowCommand, OperatingResult?>
    {

        private readonly IAuthServiceClient _userClient;
        private readonly IBookServiceClient _bookClient;
        private readonly IBorrowDbContext _context;

        public RenewBorrowHandler(
            IBorrowDbContext context,
            IAuthServiceClient userClient,
            IBookServiceClient bookClient
        )
        {
            _userClient = userClient;
            _bookClient = bookClient;
            _context = context;
        }

        public async Task<OperatingResult?> Handle(RenewBorrowCommand request, CancellationToken cancellationToken)
        {
            var user = await _userClient.GetByUsernameAsync(
                request.UserName,
                cancellationToken);
            //if (user is null || !user.VerifyPin(request.Pin)) return null;

            var record = await _context.Borrow.FirstOrDefaultAsync(x => x.Id == request.BorrowRecordId, cancellationToken);
            if (record is null || !record.Borrowed || record.UserId != user.Id) return null;

            var maxRenewals = 2;
            if (record.RenewalsCount >= maxRenewals) return null;

            var defaultLoanDays = 14;

            record.Renew(defaultLoanDays);

            _context.Borrow.Update(record);

            await _context.SaveChangesAsync(cancellationToken);

            return new OperatingResult()
            {
                IsSuccess = true,
                Message = $"Borrow record renewed successfully. Untill date: {record.DueDate}"
            };
        }
    }
}
