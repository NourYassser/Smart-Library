using MediatR;
using SmartLibrary.Api.Application.Specs;
using SmartLibrary.Api.Domain.Entities;
using SmartLibrary.Api.Domain.Repositories;
using System.ComponentModel.DataAnnotations;

namespace SmartLibrary.Api.Application.Commands.Ratings
{
    public record CreateReviewCommand(
        Guid BookId,
        [Required, MaxLength(8)] string Username,
        [Required, MaxLength(6)] string Pin,
        int Rating,
        string? Text
    ) : IRequest<Guid>;

    public class CreateReviewHandler : IRequestHandler<CreateReviewCommand, Guid>
    {
        private readonly IRepository<AppUser> _userRepo;
        private readonly IRepository<Review> _repo;
        public CreateReviewHandler(IRepository<Review> repo, IRepository<AppUser> userRepo)
        {

            _repo = repo;
            _userRepo = userRepo;
        }

        public async Task<Guid> Handle(CreateReviewCommand request, CancellationToken cancellationToken)
        {
            var username = (request.Username ?? string.Empty).Trim();
            var user = await _userRepo.FirstOrDefaultAsync(new UserByUsernameSpec(username), cancellationToken);
            if (user == null || !user.VerifyPin(request.Pin))
                throw new UnauthorizedAccessException("Invalid username or pin.");

            if (request.Rating < 1 || request.Rating > 5)
                throw new ArgumentOutOfRangeException(nameof(request.Rating), "Rating must be between 1 and 5.");

            var review = new Review(request.BookId, user.Id, request.Rating, request.Text);
            var added = await _repo.AddAsync(review, cancellationToken);
            return added.Id;
        }
    }
}
