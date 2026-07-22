using System.Net.Http.Json;

namespace BorrowService.Application.Interface
{
    public interface IAuthServiceClient
    {
        Task<UserDto?> GetByUsernameAsync(string username, CancellationToken cancellationToken);
    }
    public record UserDto(
    Guid Id,
    string UserName
    );

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
            if (response.IsSuccessStatusCode)
            {
                var user = await response.Content.ReadFromJsonAsync<UserDto>();
                return user;
            }
            return null;
        }
    }
}
