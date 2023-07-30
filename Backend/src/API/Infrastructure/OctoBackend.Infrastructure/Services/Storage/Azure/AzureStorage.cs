using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using OctoBackend.Application.Abstractions.Storage.Azure;
using OctoBackend.Infrastructure.Helpers;

namespace OctoBackend.Infrastructure.Services.Storage.Azure
{
    public class AzureStorage : IAzureStorage
    {
        readonly BlobServiceClient _blobServiceClient;
        BlobContainerClient? _blobContainerClient;

        public AzureStorage(IConfiguration configuration)
        {
            _blobServiceClient = new(configuration["AzureStorage:AzureConnectionString"]);
        }
        public async Task DeleteAsync(string ContainerName, string fileName)
        {
            _blobContainerClient = _blobServiceClient.GetBlobContainerClient(ContainerName);
            BlobClient blobClient = _blobContainerClient.GetBlobClient(fileName);
            await blobClient.DeleteAsync();
        }

        public List<string> GetFiles(string ContainerName)
        {
            _blobContainerClient = _blobServiceClient.GetBlobContainerClient(ContainerName);
            return _blobContainerClient.GetBlobs().Select(b => b.Name).ToList();
        }

        public bool HasFile(string ContainerName, string fileName)
        {
            _blobContainerClient = _blobServiceClient.GetBlobContainerClient(ContainerName);
            return _blobContainerClient.GetBlobs().Any(b => b.Name == fileName);
        }

        public async Task<List<(string fileName, string pathOrContainerName)>> UploadAsync(string containerName, IFormFileCollection files)
        {
            var _blobContainerClient = await GetBlobClientAsync(containerName);

            List<(string fileName, string pathOrContainerName)> datas = new();
            foreach (IFormFile file in files)
            {
                string fileNewName = await FileHelper.FileRenameAsync(containerName, file.FileName, HasFile);

                BlobClient blobClient = _blobContainerClient.GetBlobClient(fileNewName);
                await blobClient.UploadAsync(file.OpenReadStream());
                datas.Add((fileNewName, $"{containerName}/{fileNewName}"));
            }
            return datas;
        }

        public async Task<(string fileName, string pathOrContainerName)> UploadSingleAsync(string containerName, IFormFile file)
        {
            var _blobContainerClient = await GetBlobClientAsync(containerName);

            string fileNewName = await FileHelper.FileRenameAsync(containerName, file.FileName, HasFile);            

            BlobClient blobClient = _blobContainerClient.GetBlobClient(fileNewName);

            var stream = await FileStreamReader.ToStreamAsync(file);

            await blobClient.UploadAsync(stream);
            return (fileNewName, $"{containerName}/{fileNewName}");

        }

        private async Task<BlobContainerClient> GetBlobClientAsync(string containerName)
        {
            _blobContainerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            await _blobContainerClient.CreateIfNotExistsAsync();
            await _blobContainerClient.SetAccessPolicyAsync(PublicAccessType.BlobContainer);
            return _blobContainerClient;
        }

    }
}
