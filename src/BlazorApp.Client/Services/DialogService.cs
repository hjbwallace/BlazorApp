using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;

namespace BlazorApp.Client.Services
{
    public class DialogService : IDialogService
    {
        private const string _showAlertMethod = "ShowAlert";
        private const string _showConfirmMethod = "ShowConfirm";
        private const string _showPromptMethod = "ShowPrompt";

        private readonly IJSRuntime _js;

        public DialogService(IJSRuntime js)
        {
            _js = js;
        }

        public async Task ShowAlertAsync(string message)
        {
            await _js.InvokeAsync<object>(_showAlertMethod, message);
        }

        public async Task<bool> ShowConfirmAsync(string message)
        {
            var result = await _js.InvokeAsync<object>(_showConfirmMethod, message);
            Console.WriteLine(result);
            return result?.ToString() == "True";
        }

        public async Task<string> ShowPromptAsync(string message)
        {
            var result = await _js.InvokeAsync<object>(_showPromptMethod, message);
            return result?.ToString();
        }
    }
}