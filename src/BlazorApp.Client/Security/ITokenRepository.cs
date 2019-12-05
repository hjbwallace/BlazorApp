using System.Threading.Tasks;

namespace BlazorApp.Client.Security
{
    public interface ITokenRepository
    {
        Task<string> GetTokenAsync();

        Task SaveTokenAsync(string token);
    }
}