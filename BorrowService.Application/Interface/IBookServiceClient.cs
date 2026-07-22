using System.Net.Http.Json;

namespace BorrowService.Application.Interface
{
    public interface IBookServiceClient
    {
        Task<BookDto?> GetBookAsync(Guid Id, CancellationToken cancellationToken);
    }
    public record BookDto(
    Guid Id,
    string Title
);

    public class BookServiceClient : IBookServiceClient
    {
        private readonly HttpClient _httpClient;
        public BookServiceClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<BookDto?> GetBookAsync(Guid Id, CancellationToken cancellationToken)
        {
            var response = await _httpClient.GetAsync($"/api/books/{Id}");
            if (response.IsSuccessStatusCode)
            {
                var book = await response.Content.ReadFromJsonAsync<BookDto>();
                return book;
            }
            return null;
        }
    }
}
