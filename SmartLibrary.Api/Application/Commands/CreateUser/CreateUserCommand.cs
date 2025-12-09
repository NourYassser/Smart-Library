using MediatR;
using SmartLibrary.Api.Application.Specs;
using SmartLibrary.Api.Domain.Entities;
using SmartLibrary.Api.Domain.Repositories;
using System.ComponentModel.DataAnnotations;

namespace SmartLibrary.Api.Application.Commands.CreateUser
{
    public record CreateUserCommand(
         [Required, MaxLength(8)] string Username,
         [Required, MaxLength(6)] string Pin
    ) : IRequest<Guid>;

    public class CreateUserHandler : IRequestHandler<CreateUserCommand, Guid>
    {
        private readonly IRepository<AppUser> _repo;

        public CreateUserHandler(IRepository<AppUser> repo)
        {
            _repo = repo;
        }

        public async Task<Guid> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var existing = await _repo.FirstOrDefaultAsync(new UserByUsernameSpec(request.Username), cancellationToken);
            if (existing != null)
                throw new Exception("Username already exists");

            var user = new AppUser(request.Username, request.Pin);
            await _repo.AddAsync(user, cancellationToken);
            return user.Id;
        }
    }

}
