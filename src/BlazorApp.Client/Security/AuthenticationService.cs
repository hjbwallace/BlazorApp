using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BlazorApp.Client.Security
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly HttpClient _httpClient;
        private readonly ServerAuthenticationStateProvider _authenticationStateProvider;

        public AuthenticationService(HttpClient httpClient, ServerAuthenticationStateProvider authenticationStateProvider)
        {
            _httpClient = httpClient;
            _authenticationStateProvider = authenticationStateProvider;
        }

        public async Task<RegisterResult> RegisterAsync(RegisterModel registerModel)
        {
            var result = await PostAsync<RegisterResult>("api/user/register", registerModel);
            return result;
        }

        public async Task<LoginResult> LoginAsync(LoginModel loginModel)
        {
            var result = await PostAsync<LoginResult>("api/user/login", loginModel);

            if (result.Successful)
                await _authenticationStateProvider.OnUserAuthenticated(result.Token, result.TokenExpiry.GetValueOrDefault());

            return result;
        }

        public async Task LogoutAsync()
        {
            await _authenticationStateProvider.OnUserAuthenticationRevoked();
        }

        private async Task<T> PostAsync<T>(string route, object content)
        {
            var requestContent = new StringContent(JsonSerializer.Serialize(content), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(route, requestContent);
            
            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
    }
}