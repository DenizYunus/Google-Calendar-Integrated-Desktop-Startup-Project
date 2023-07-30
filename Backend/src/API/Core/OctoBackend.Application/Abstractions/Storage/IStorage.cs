using Microsoft.AspNetCore.Http;

namespace OctoBackend.Application.Abstractions.Storage
{
    public interface IStorage
    {
        Task<List<(string fileName, string pathOrContainerName)>> UploadAsync(
           string pathOrContainerName, IFormFileCollection files);
        Task<(string fileName, string pathOrContainerName)> UploadSingleAsync(
        string pathOrContainerName, IFormFile file);
        Task DeleteAsync(string pathOrContainerName, string fileName);
        List<string> GetFiles(string pathOrContainerName);
        bool HasFile(string pathOrContainerName, string fileName);
    }
}
