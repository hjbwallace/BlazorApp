using BlazorApp.Client.Security;
using BlazorApp.Client.Services;
using Microsoft.AspNetCore.Blazor.Hosting;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace BlazorApp.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);

            builder.Services.AddSingleton<IAuthenticationService, AuthenticationService>();
            builder.Services.AddSingleton<ITokenRepository, LocalStorageTokenRepository>();

            // Make the same instance accessible as both AuthenticationStateProvider and ApiAuthenticationStateProvider
            builder.Services.AddSingleton<ServerAuthenticationStateProvider>();
            builder.Services.AddSingleton<AuthenticationStateProvider>(provider => provider.GetRequiredService<ServerAuthenticationStateProvider>());

            builder.Services.AddAuthorizationCore(config =>
            {
                config.AddPolicy(Policies.IsAdmin, Policies.IsAdminPolicy());
            });

            builder.Services.AddSingleton<IDialogService, DialogService>();
            builder.Services.AddSingleton<IFileService, FileService>();

            builder.RootComponents.Add<App>("app");

            await builder.Build().RunAsync();
        }
    }
}