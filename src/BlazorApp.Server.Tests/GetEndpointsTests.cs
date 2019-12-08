using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace BlazorApp.Server.Tests
{
    public class GetEndpointsTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _factory;

        public GetEndpointsTests(WebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Theory]
        [InlineData("/")]
        [InlineData("/Index")]
        [InlineData("/About")]
        [InlineData("/Privacy")]
        [InlineData("/Contact")]
        [InlineData("/DatabaseRecords")]
        [InlineData("/FetchData")]
        [InlineData("/Login")]
        [InlineData("/Logout")]
        [InlineData("/Register")]
        public async Task GetEndpointsReturnSuccessAndCorrectContentType(string url)
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync(url);

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            Assert.Equal("text/html", response.Content.Headers.ContentType.ToString());
        }

        [Theory]
        [InlineData("/api/user")]
        [InlineData("/api/weatherforcast")]
        public async Task GetApiEndpointsReturnSuccess(string url)
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync(url);

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
        }

        [Theory]
        [InlineData("/api/database")]
        [InlineData("/api/admin")]
        public async Task GetApiEndpointsReturnUnauthorized(string url)
        {

            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync(url);

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }
    }
}