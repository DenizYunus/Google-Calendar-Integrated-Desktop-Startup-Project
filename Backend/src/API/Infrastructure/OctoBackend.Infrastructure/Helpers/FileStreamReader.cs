using Microsoft.AspNetCore.Http;

namespace OctoBackend.Infrastructure.Helpers
{
    public class FileStreamReader
    {
        public static async Task<MemoryStream> ToStreamAsync(IFormFile file)
        {
            var fileBytes = await ToByteArrayAsync(file);
            return new MemoryStream(fileBytes);
        }

        public static async Task<byte[]> ToByteArrayAsync(IFormFile file)
        {
            byte[] fileBytes;
            string base64string = await new StreamReader(file.OpenReadStream()).ReadToEndAsync();

            if (base64string.Contains(','))
            {
                fileBytes = Convert.FromBase64String(base64string.Split(',')[1]);
            }
            else
            {
                fileBytes = Convert.FromBase64String(base64string);
            }

            return fileBytes;
        }
    }
}
