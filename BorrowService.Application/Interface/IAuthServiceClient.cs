namespace BorrowService.Application.Interface
{
    public record UserDto(Guid Id, string UserName);

    public interface IAuthServiceClient
    {
        Task<UserDto?> GetByUsernameAsync(string username, CancellationToken cancellationToken);
    }
}
