using System.Threading.Tasks;

namespace BlazorApp.Client.Security
{
    public interface IAuthenticationService
    {
        Task<LoginResult> LoginAsync(LoginModel loginModel);

        Task LogoutAsync();

        Task<RegisterResult> RegisterAsync(RegisterModel registerModel);
    }
}