using System.Threading.Tasks;

namespace BlazorApp.Client.Services
{
    public interface IDialogService
    {
        Task ShowAlertAsync(string message);
        Task<bool> ShowConfirmAsync(string message);
        Task<string> ShowPromptAsync(string message);
    }
}