using BorrowService.Application.Interface;
using System.Net.Http.Json;

namespace BorrowService.Infrastructure.Service
{
    public class BookServiceClient : IBookServiceClient
    {
        private readonly HttpClient _httpClient;
        public BookServiceClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<BookDto?> GetBookAsync(string Id, CancellationToken cancellationToken)
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
