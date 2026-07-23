using Ardalis.Specification.EntityFrameworkCore;
using AuthService.Application.Interface;
using AuthService.Application.Specs;
using AuthService.Domain;
using Library.BuildingBlocks.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace AuthService.Application.Commands
{
    public record CreateUserCommand(
         [Required, MaxLength(8)] string Username,
         [Required, MaxLength(6)] string Pin
    ) : IRequest<OperatingResult>;

    public class CreateUserHandler : IRequestHandler<CreateUserCommand, OperatingResult>
    {
        private readonly IAuthDbContext _context;

        public CreateUserHandler(IAuthDbContext context)
        {
            _context = context;
        }

        public async Task<OperatingResult> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var existing = await _context.Users
                .WithSpecification(new UserByUsernameSpec(request.Username))
                .FirstOrDefaultAsync(cancellationToken);
            if (existing != null)
                return new OperatingResult()
                {
                    IsSuccess = false,
                    Message = $"User with username {request.Username} already exists."
                };

            var user = new AppUser(request.Username, request.Pin);
            await _context.Users.AddAsync(user, cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);

            return new OperatingResult()
            {
                IsSuccess = true,
                Message = $"User with username {request.Username} created successfully."
            };
        }
    }

}
