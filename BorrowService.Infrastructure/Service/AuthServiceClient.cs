using BorrowService.Application.Interface;
using System.Net.Http.Json;

namespace BorrowService.Infrastructure.Service
{
    public class AuthServiceClient : IAuthServiceClient
    {
        private readonly HttpClient _httpClient;
        public AuthServiceClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<UserDto?> GetByUsernameAsync(string username, CancellationToken cancellationToken)
        {
            var response = await _httpClient.GetAsync($"/api/users/{username}");

            // var user = await response.Content.ReadFromJsonAsync<UserDto>();
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception(
                    $"AuthService returned {(int)response.StatusCode}: {error}");
            }

            return await response.Content.ReadFromJsonAsync<UserDto>(cancellationToken: cancellationToken);
        }
    }
}
