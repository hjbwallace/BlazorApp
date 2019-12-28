using System.Threading.Tasks;

namespace BlazorApp.Client.Services
{
    public interface IFileService
    {
        Task SaveFileAsync(string fileName, byte[] fileContent);
    }
}