using MediatR;
using SmartLibrary.Api.Application.Specs;
using SmartLibrary.Api.Domain.Entities;
using SmartLibrary.Api.Domain.Repositories;

namespace SmartLibrary.Api.Application.Commands.CreateUser
{
    public record CreateUserCommand(string Username, string PinCode)
    : IRequest<Guid>;
    public class CreateUserHandler : IRequestHandler<CreateUserCommand, Guid>
    {
        private readonly IRepository<AppUser> _repo;

        public CreateUserHandler(IRepository<AppUser> repo)
        {
            _repo = repo;
        }

        public async Task<Guid> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            // Check if username is unique
            var existing = await _repo.FirstOrDefaultAsync(new UserByUsernameSpec(request.Username), cancellationToken);
            if (existing != null)
                throw new Exception("Username already exists");

            var user = new AppUser
            {
                Username = request.Username,
                PinCode = request.PinCode
            };

            await _repo.AddAsync(user, cancellationToken);

            return user.Id;
        }
    }

}
