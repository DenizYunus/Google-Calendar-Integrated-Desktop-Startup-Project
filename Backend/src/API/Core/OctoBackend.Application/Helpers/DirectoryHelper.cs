
namespace OctoBackend.Application.Helpers
{
    public static class DirectoryHelper
    {
        public static string ReadDirectoryContent(string content)
        {
            string currentDirectory = Directory.GetCurrentDirectory();
            string htmlDirectory = Path.Combine(currentDirectory, content);
            Console.WriteLine(htmlDirectory);
            if (File.Exists(htmlDirectory))//burayı nsaı yapcaz
            {
                return File.ReadAllText(htmlDirectory);
            }
            else
            {
                Console.WriteLine("No any path matches");
                return string.Empty;
            }
        }
    }
}
