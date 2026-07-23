namespace BorrowService.Application.Interface
{
    public record BookDto(Guid Id, string Title);

    public interface IBookServiceClient
    {
        Task<BookDto?> GetBookAsync(string Id, CancellationToken cancellationToken);
    }
}
