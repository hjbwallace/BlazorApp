using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace BlazorApp.Client.Services
{
    public class FileService : IFileService
    {
        private const string _saveFileMethod = "saveAsFile";
        private readonly IJSRuntime _js;

        public FileService(IJSRuntime js)
        {
            _js = js;
        }

        public async Task SaveFileAsync(string fileName, byte[] fileContent)
        {
            await _js.InvokeAsync<object>(
                _saveFileMethod,
                fileName,
                fileContent);
        }
    }
}