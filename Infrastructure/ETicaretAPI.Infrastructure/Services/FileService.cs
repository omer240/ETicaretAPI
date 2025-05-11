using ETicaretAPI.Infrastructure.Operations;
using Microsoft.AspNetCore.Hosting;

namespace ETicaretAPI.Infrastructure.Services
{
    public class FileService
    {
        readonly IWebHostEnvironment _webHostEnvironment;

        public FileService(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        //protected delegate bool HasFile(string pathOrContainerName, string fileName);
        private async Task<string> FileRenameAsync(string pathOrContainerName, string fileName,  Func<string,string,bool> hasFileMethod)
        {
            return await Task.Run(() =>
            {
                string extension = Path.GetExtension(fileName);
                string baseName = NameOperation.CharacterRegulatory(Path.GetFileNameWithoutExtension(fileName));
                string newFileName = $"{baseName}{extension}";
                int counter = 1;

                // Dosya var mı kontrolü
                while (hasFileMethod(pathOrContainerName, newFileName))
                {
                    counter++;
                    newFileName = $"{baseName}-{counter}{extension}";
                }

                return newFileName;
            });
        }
    }
}
