using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using OctoBackend.Application.Abstractions.Storage.Local;
using OctoBackend.Infrastructure.Helpers;
using System;

namespace ETicaretAPI.Infrastructure.Services.Storage.Local
{
    public class LocalStorage : ILocalStorage
    {
        readonly IWebHostEnvironment _webHostEnvironment;

        public LocalStorage(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }
        public Task DeleteAsync(string path, string fileName)
        {
            File.Delete($"{path}\\{fileName}");
            return Task.CompletedTask;
        }

        public List<string> GetFiles(string path)
        {
            DirectoryInfo directory = new(path);
            return directory.GetFiles().Select(f => f.Name).ToList();
        }

        public bool HasFile(string path, string fileName)
            => File.Exists($"{path}\\{fileName}");


        static async Task<bool> CopyFileAsync(string path, byte[] fileBytes)
        {
            try
            {
                await using FileStream fileStream = new(path, FileMode.Create,
                    FileAccess.Write, FileShare.None, 1024 * 1024, useAsync: false);

                await fileStream.WriteAsync(fileBytes);
                await fileStream.FlushAsync();
                return true;
            }
            catch (Exception)
            {
                ///TODO: ERROR HANDLING
                throw;
            }
        }

        //static async Task<bool> CopyFileAsync(string path, IFormFile file)
        //{
        //    try
        //    {
        //        await using FileStream fileStream = new(path, FileMode.Create,
        //            FileAccess.Write, FileShare.None, 1024 * 1024, useAsync: false);

        //        await file.CopyToAsync(fileStream);
        //        await fileStream.FlushAsync();
        //        return true;
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }

        //}
        public async Task<List<(string fileName, string pathOrContainerName)>> UploadAsync(string path, IFormFileCollection files)
        {
            string uploadPath = Path.Combine(_webHostEnvironment.WebRootPath, path);

            if (!Directory.Exists(uploadPath))
                Directory.CreateDirectory(uploadPath);

            List<(string fileName, string path)> datas = new();
            foreach (IFormFile file in files)
            {
                string fileNewName = await FileHelper.FileRenameAsync(uploadPath, file.Name, HasFile);

                var fileBytes = await FileStreamReader.ToByteArrayAsync(file);

                await CopyFileAsync($"{uploadPath}\\{file.FileName}", fileBytes);
                datas.Add((file.FileName, $"{path}\\{file.FileName}"));
            }
            return datas;
        }

        ///TODO: BE ABLE TO ADD FILE EVEN THE DATA COMES FROM SWAGGER OR REAL APP
        public async Task<(string fileName, string pathOrContainerName)> UploadSingleAsync(string pathOrContainerName, IFormFile file)
        {
            string uploadPath = Path.Combine(_webHostEnvironment.WebRootPath, pathOrContainerName);

            if (!Directory.Exists(uploadPath))
                Directory.CreateDirectory(uploadPath);

            (string fileName, string path) datas = new();

            string fileNewName = await FileHelper.FileRenameAsync(uploadPath, file.Name, HasFile);

            var fileBytes = await FileStreamReader.ToByteArrayAsync(file);

            await CopyFileAsync($"{uploadPath}\\{file.FileName}", fileBytes);
            datas.fileName = file.FileName;
            datas.path = $"{pathOrContainerName}\\{file.FileName}";

            return datas;
        }
    }
}
