using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace BlazorApp.Client.Security
{
    public class LocalStorageTokenRepository : ITokenRepository
    {
        private const string _authTokenKey = "authToken";

        private readonly IJSRuntime _jsRuntime;

        public LocalStorageTokenRepository(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public async Task<string> GetTokenAsync()
        {
            return await _jsRuntime.InvokeAsync<string>("localStorage.getItem", _authTokenKey);
        }

        public async Task SaveTokenAsync(string token)
        {
            if (token == null)
                await _jsRuntime.InvokeAsync<object>("localStorage.removeItem", _authTokenKey);
            else
                await _jsRuntime.InvokeAsync<object>("localStorage.setItem", _authTokenKey, token);
        }
    }
}