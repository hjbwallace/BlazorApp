using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;

namespace BlazorApp.Client.Security
{
    public class LocalStorageTokenRepository : ITokenRepository
    {
        private const string _authTokenKey = "authToken";
        private const string _authTokenExpiryKey = "authTokenExpiry";

        private readonly IJSRuntime _jsRuntime;

        public LocalStorageTokenRepository(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public async Task<string> GetTokenAsync()
        {
            var expiry = await _jsRuntime.InvokeAsync<object>("localStorage.getItem", _authTokenExpiryKey);

            if (expiry == null)
                return null;

            if (DateTime.Parse(expiry.ToString()) <= DateTime.Now)
            {
                await SaveTokenAsync(null);
                return null;
            }

            return await _jsRuntime.InvokeAsync<string>("localStorage.getItem", _authTokenKey);
        }

        public async Task SaveTokenAsync(string token, DateTime? tokenExpiry = null)
        {
            if (token == null)
            {
                await _jsRuntime.InvokeAsync<object>("localStorage.removeItem", _authTokenKey);
                await _jsRuntime.InvokeAsync<object>("localStorage.removeItem", _authTokenExpiryKey);
            }
            else
            {
                await _jsRuntime.InvokeAsync<object>("localStorage.setItem", "authToken", token);
                await _jsRuntime.InvokeAsync<object>("localStorage.setItem", "authTokenExpiry", tokenExpiry);
            }
        }
    }
}