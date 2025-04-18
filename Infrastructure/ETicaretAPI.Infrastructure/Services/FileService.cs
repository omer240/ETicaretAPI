﻿using ETicaretAPI.Application.Services;
using ETicaretAPI.Infrastructure.Operations;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace ETicaretAPI.Infrastructure.Services
{
    public class FileService : IFileService
    {
        readonly IWebHostEnvironment _webHostEnvironment;

        public FileService(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<bool> CopyFileAsync(string path, IFormFile file)
        {
            try
            {
                await using FileStream fileStream = new(path, FileMode.Create, FileAccess.Write, FileShare.None, 1024 * 1024, useAsync: true);
                await file.CopyToAsync(fileStream);
                await fileStream.FlushAsync();
                return true;
            }
            catch (Exception ex)
            {
                //todo log!
                throw ex;
            }

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

        public bool HasFileTemp(string path, string fileName)
           => File.Exists($"{path}\\{fileName}");

        public async Task<List<(string fileName, string path)>> UploadAsync(string path, IFormFileCollection files)
        {
            string uploadPath = Path.Combine(_webHostEnvironment.WebRootPath, path);
            if(!Directory.Exists(uploadPath))
                Directory.CreateDirectory(uploadPath);

            List<(string fileName, string path)> datas = new();
            List<bool> results = new();
            foreach (var file in files)
            {
                string fileNewName = await FileRenameAsync(uploadPath, file.FileName, HasFileTemp);

                bool result = await CopyFileAsync($"{uploadPath}\\{fileNewName}",file);
                datas.Add((fileNewName, $"{uploadPath}\\{fileNewName}"));
                results.Add(result);
            }

            if (results.TrueForAll(r => r.Equals(true)))
                return datas;
             
            //todo eğer ki yukarıdaki if geçerli değilse burada dosyaların sunucuda yüklenirken hata alındığına dair uyarıcı bir exception fırlatılması gerekiyor.
            return null;
        }
    }
}
