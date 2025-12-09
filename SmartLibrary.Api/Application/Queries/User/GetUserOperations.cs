using MediatR;
using SmartLibrary.Api.Application.DTOs;
using SmartLibrary.Api.Application.Specs;
using SmartLibrary.Api.Domain.Entities;
using SmartLibrary.Api.Domain.Repositories;

namespace SmartLibrary.Api.Application.Queries.User
{
    public record GetUserOperationsQuery(string Username) : IRequest<UserOperationsDto>;
    public class GetUserOperationsHandler : IRequestHandler<GetUserOperationsQuery, UserOperationsDto>
    {
        private readonly IRepository<AppUser> _userRepo;
        private readonly IRepository<BorrowRecord> _borrowingRepo;

        public GetUserOperationsHandler(
            IRepository<AppUser> userRepo,
            IRepository<BorrowRecord> borrowingRepo)
        {
            _userRepo = userRepo;
            _borrowingRepo = borrowingRepo;
        }

        public async Task<UserOperationsDto> Handle(GetUserOperationsQuery request, CancellationToken cancellationToken)
        {
            var user = await _userRepo.FirstOrDefaultAsync(new UserByUsernameSpec(request.Username), cancellationToken);
            if (user == null)
            {
                return new UserOperationsDto(new List<BorrowingDto>(), new List<FineDto>());
            }

            var borrowings = await _borrowingRepo.ListAsync(new BorrowingsByUserSpec(user.Id));

            var borrowDtos = borrowings.Select(b => new BorrowingDto(
                b.Id,
                b.BookId,
                b.Book?.Title ?? string.Empty,
                b.BorrowedAt,
                b.ReturnedAt)).ToList();

            var fineDtos = borrowings.Select(a => new FineDto(
                a.BookId,
                a.FinePaid,
                a.BorrowedAt,
                a.ReturnedAt
                )
            ).ToList();

            return new UserOperationsDto(borrowDtos, fineDtos);
        }
    }

}
