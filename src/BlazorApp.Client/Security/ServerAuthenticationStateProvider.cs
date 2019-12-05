using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;

namespace BlazorApp.Client.Security
{
    public class ServerAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly HttpClient _httpClient;
        private readonly ITokenRepository _tokenRepository;

        public ServerAuthenticationStateProvider(HttpClient httpClient, ITokenRepository tokenRepository)
        {
            _httpClient = httpClient;
            _tokenRepository = tokenRepository;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var token = await _tokenRepository.GetTokenAsync();

            return await GetAuthenticationStateAsync(token);
        }

        public void OnUserAuthenticated(string token) => OnAuthenticationStateChanged(token);

        public void OnUserAuthenticationRevoked() => OnAuthenticationStateChanged(null);

        private async Task<AuthenticationState> GetAuthenticationStateAsync(string token)
        {
            await _tokenRepository.SaveTokenAsync(token);

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);

            var identity = string.IsNullOrWhiteSpace(token)
                ? new ClaimsIdentity()
                : new ClaimsIdentity(ParseClaimsFromJwt(token), "jwt");

            return await Task.FromResult(new AuthenticationState(new ClaimsPrincipal(identity)));
        }

        private void OnAuthenticationStateChanged(string token)
        {
            var authenticationState = GetAuthenticationStateAsync(token);
            NotifyAuthenticationStateChanged(authenticationState);
        }

        private IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
        {
            var claims = new List<Claim>();
            var payload = jwt.Split('.')[1];
            var jsonBytes = ParseBase64WithoutPadding(payload);
            var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);

            keyValuePairs.TryGetValue(ClaimTypes.Role, out object roles);

            if (roles != null)
            {
                if (roles.ToString().Trim().StartsWith("["))
                {
                    var parsedRoles = JsonSerializer.Deserialize<string[]>(roles.ToString());

                    foreach (var parsedRole in parsedRoles)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, parsedRole));
                    }
                }
                else
                {
                    claims.Add(new Claim(ClaimTypes.Role, roles.ToString()));
                }

                keyValuePairs.Remove(ClaimTypes.Role);
            }

            claims.AddRange(keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString())));

            return claims;
        }

        private byte[] ParseBase64WithoutPadding(string base64)
        {
            switch (base64.Length % 4)
            {
                case 2: base64 += "=="; break;
                case 3: base64 += "="; break;
            }
            return Convert.FromBase64String(base64);
        }
    }
}